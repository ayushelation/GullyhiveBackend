using Dapper;
using GullyHive.Auth.Models;
using GullyHive.Auth.Repositories;
using Npgsql;
using System;

namespace GullyHive.Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr")!;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);


        public async Task<long> AddUserAsync(RegisterRequest user)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();

            //try
            //{
            // ===============================
            // 0. SECURITY: HASH PASSWORD
            // ===============================
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // ===============================
            // 1. CITY (GET OR CREATE)
            // ===============================
            var defaultTier = "X"; // PostgreSQL enum-safe

            var cityId = await conn.ExecuteScalarAsync<long?>(@"
            SELECT id FROM cities
            WHERE name = @City AND state = @State
            LIMIT 1;",
            new { user.City, user.State }, tx);

            if (cityId == null)
            {
                cityId = await conn.ExecuteScalarAsync<long>(@"
                INSERT INTO cities (name, state, tier)
                VALUES (@City, @State, @Tier)
                RETURNING id;",
                new
                {
                    user.City,
                    user.State,
                    Tier = defaultTier
                }, tx);
            }

            // ===============================
            // 2. USERS
            // ===============================
            var userId = await conn.ExecuteScalarAsync<long>(@"
            INSERT INTO users (phone,email,display_name,password_hash, default_city_id)
            VALUES (@Mobile,@Email,@FullName,@PasswordHash,@CityId)
            RETURNING id;",
            new
            {
                user.Mobile,
                user.Email,
                user.FullName,
                PasswordHash = passwordHash,
                CityId = cityId
            }, tx);

            // ===============================
            // 3. USER ROLES (DYNAMIC & SAFE)
            // ===============================
           
            string NormalizeRole(string role) => role?.Trim().ToLower() switch
            {
                "admin" => "Admin",
                "seller" => "Seller",
                "buyer" => "Buyer",
                _ => throw new ArgumentException("Invalid role")
            };

            var roleName = NormalizeRole(user.Role);

            // Get role_id from roles table
            var roleId = await conn.ExecuteScalarAsync<long>(
                "SELECT id FROM roles WHERE name = @RoleName",
                new { RoleName = "Seller" },
                tx
            );

            // Now insert directly using role_id
            await conn.ExecuteAsync(@"
              INSERT INTO user_roles (user_id, role_id)
                   VALUES (@UserId, @RoleId)
                  ON CONFLICT (user_id, role_id) DO NOTHING; ",
                  new
            {
                UserId = userId,
                RoleId = roleId
            }, tx);


            // ===============================
            // 4. ADDRESS
            // ===============================
            var addressId = await conn.ExecuteScalarAsync<long>(@"
            INSERT INTO addresses ( user_id,label,line1, city_id, state,pincode,is_primary)
            VALUES (@UserId,'Business',@BusinessAddress,@CityId, @State,@PinCode,TRUE)
            RETURNING id;", 
            new
            {
                UserId = userId,
                user.BusinessAddress,
                CityId = cityId,
                user.State,
                user.PinCode
            }, tx);

            // ===============================
            // 5. BUSINESS
            // ===============================
            var businessId = await conn.ExecuteScalarAsync<long>(@"
            INSERT INTO businesses (owner_user_id,legal_name,primary_city_id)
            VALUES (@UserId, @BusinessName, @CityId )
            RETURNING id;",
            new
            {
                UserId = userId,
                user.BusinessName,
                CityId = cityId
            }, tx);

            // ===============================
            // 6. BUSINESS SITE
            // ===============================
            await conn.ExecuteAsync(@"
            INSERT INTO business_sites (business_id,name,address_id,city_id)
            VALUES (@BusinessId,'Primary Site',@AddressId,@CityId);",
            new
            {
                BusinessId = businessId,
                AddressId = addressId,
                CityId = cityId
            }, tx);

            // ===============================
            // 7. PROVIDER PROFILE
            // ===============================
           

            ProviderTypeEnum MapProviderType(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new ApplicationException("Professional type is required");

                return input.Trim().ToLower() switch
                {
                    "individual" => ProviderTypeEnum.individual,
                    "msme" => ProviderTypeEnum.msme,
                    "company" => ProviderTypeEnum.company,
                    _ => throw new ApplicationException($"Invalid ProfessionalType: {input}")
                };
            }

            var providerType = MapProviderType(user.ProfessionalType);

            long providerId;

            try
            {
                providerId = await conn.ExecuteScalarAsync<long>(@"
                     INSERT INTO provider_profiles ( user_id, provider_type, legal_name, base_city_id, base_address_id, description )
                    VALUES (@UserId, @ProviderType::provider_type_enum, @BusinessName, @CityId, @AddressId, @Description)
                    RETURNING id;", 
                new
                  {
                    UserId = userId,
                    ProviderType = providerType.ToString(), // "individual", "msme", "company"
                    BusinessName = user.BusinessName,
                    CityId = cityId,
                    AddressId = addressId,
                    Description = string.IsNullOrWhiteSpace(user.SelfOverview)
               ? "Professional service provider"
               : user.SelfOverview
                }, tx);

            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine("Postgres Exception!");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"SQL State: {ex.SqlState}");
                Console.WriteLine($"Detail: {ex.Detail}");
                Console.WriteLine($"Constraint: {ex.ConstraintName}");
                throw; // rethrow so you can see it in the debugger
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Exception!");
                Console.WriteLine(ex);
                throw;
            }

            // ===============================
            // 8. PROVIDER DOCUMENTS
            // ===============================
            try
            {

                byte[] FileToBytes(IFormFile file)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    return ms.ToArray();
                }

                var registrationBytes = FileToBytes(user.RegistrationDocument);
                var addressProofBytes = FileToBytes(user.AddressProof);

                await conn.ExecuteAsync(@"
              INSERT INTO provider_documents (provider_id, doc_type, file_url)
              VALUES
             (@ProviderId, 'registration', @RegistrationDoc),
             (@ProviderId, 'address_proof', @AddressProof);",
             new
                {
                    ProviderId = providerId,
                    RegistrationDoc = registrationBytes,
                    AddressProof = addressProofBytes
                }, tx);

            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine("Postgres Exception in provider_documents!");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"SQL State: {ex.SqlState}");
                Console.WriteLine($"Detail: {ex.Detail}");
                Console.WriteLine($"Constraint: {ex.ConstraintName}");
                Console.WriteLine($"providerId: {providerId}");
                Console.WriteLine($"RegistrationDoc: {user.RegistrationDocument}");
                Console.WriteLine($"AddressProof: {user.AddressProof}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Exception in provider_documents!");
                Console.WriteLine(ex);
                throw;
            }


            // ===============================
            // 9. PROVIDER SERVICE AREA
            // ===============================
            await conn.ExecuteAsync(@"
            INSERT INTO provider_service_areas (provider_id,type,city_id)
            VALUES (@ProviderId, @Type::service_area_type_enum,@CityId);",
            new
            {
                ProviderId = providerId,
                Type = "city_radius", // <-- must match enum exactly!
                CityId = cityId
            }, tx);

            // ===============================
            // 10. PROVIDER SERVICE CATEGORIES
            // ===============================
            if (user.SubCategoryIds == null || user.SubCategoryIds.Count == 0)
                throw new ApplicationException("At least one subcategory is required");

            try
            {
                await conn.ExecuteAsync(@"
        INSERT INTO public.provider_service_categories
              ( provider_id, category_id,sub_category_id)
         SELECT
              @ProviderId,
              scm.category_id,
              scm.id
           FROM india_leadgen.sub_category_master scm
           WHERE scm.id = ANY(@SubCategoryIds::BIGINT[])
           AND scm.category_id = @CategoryId
           ON CONFLICT (provider_id, category_id, sub_category_id)
            DO NOTHING;", 
            new
                {
                    ProviderId = providerId,
                    CategoryId = user.ServiceCategoryId,
                    SubCategoryIds = user.SubCategoryIds.ToArray()
                }, tx);
            }
            catch (PostgresException ex)
            {
                Console.WriteLine("❌ POSTGRES ERROR (provider_service_categories)");
                Console.WriteLine($"Message     : {ex.Message}");
                Console.WriteLine($"Detail      : {ex.Detail}");
                Console.WriteLine($"SQL State   : {ex.SqlState}");
                Console.WriteLine($"Constraint  : {ex.ConstraintName}");
                throw;
            }

            if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
            {
                // Define the uploads folder path (relative or absolute)
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "profiles");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                // Create a unique filename
                var fileName = $"profile_{providerId}{Path.GetExtension(user.ProfilePicture.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                // Save the file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await user.ProfilePicture.CopyToAsync(stream);
                }

                // Save the relative or absolute URL/path in DB
                var fileUrl = $"/uploads/profiles/{fileName}"; // For web access

                await conn.ExecuteAsync(@"
        UPDATE provider_profiles
        SET profile_picture_url = @ProfileUrl
        WHERE id = @ProviderId
    ", new
                {
                    ProviderId = providerId,
                    ProfileUrl = fileUrl
                }, tx);
            }

            // ===============================
            // COMMIT
            // ===============================
            tx.Commit();
            return userId;
        }

        public enum ProviderTypeEnum
        {
            individual,
            msme,
            company
        }



    }

}


