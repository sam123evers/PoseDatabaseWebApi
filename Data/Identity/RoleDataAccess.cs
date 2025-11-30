using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace PoseDatabaseWebApi.Data.Identity
{
    //public class RoleDataAccess : IRoleStore<IdentityRole>, IRoleClaimStore<IdentityRole>
    //{
//        private readonly string _connectionString;

//        public RoleDataAccess(string connectionString)
//        {
//            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
//        }

//        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));

//            if (string.IsNullOrEmpty(role.Id))
//                role.Id = Guid.NewGuid().ToString();

//            const string sql = @"
//INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
//VALUES (@Id, @Name, @NormalizedName, @ConcurrencyStamp)";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("Id", role.Id);
//            cmd.Parameters.AddWithValue("Name", (object?)role.Name ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("NormalizedName", (object?)role.NormalizedName ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("ConcurrencyStamp", (object?)role.ConcurrencyStamp ?? DBNull.Value);

//            try
//            {
//                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
//                return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to insert role." });
//            }
//            catch (PostgresException pgEx) when (pgEx.SqlState == "23505") // unique_violation
//            {
//                return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
//            }
//        }

//        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));

//            const string sql = @"
//UPDATE AspNetRoles SET
//    Name = @Name,
//    NormalizedName = @NormalizedName,
//    ConcurrencyStamp = @ConcurrencyStamp
//WHERE Id = @Id";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("Id", role.Id);
//            cmd.Parameters.AddWithValue("Name", (object?)role.Name ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("NormalizedName", (object?)role.NormalizedName ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("ConcurrencyStamp", (object?)role.ConcurrencyStamp ?? DBNull.Value);

//            var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
//            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "No role found to update." });
//        }

//        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));

//            const string sql = "DELETE FROM AspNetRoles WHERE Id = @Id";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("Id", role.Id);

//            var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
//            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "No role found to delete." });
//        }

//        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
//        {
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            cancellationToken.ThrowIfCancellationRequested();
//            return Task.FromResult(role.Id);
//        }

//        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
//        {
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            cancellationToken.ThrowIfCancellationRequested();
//            return Task.FromResult(role.Name);
//        }

//        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
//        {
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            cancellationToken.ThrowIfCancellationRequested();
//            role.Name = roleName;
//            return Task.CompletedTask;
//        }

//        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
//        {
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            cancellationToken.ThrowIfCancellationRequested();
//            return Task.FromResult(role.NormalizedName);
//        }

//        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
//        {
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            cancellationToken.ThrowIfCancellationRequested();
//            role.NormalizedName = normalizedName;
//            return Task.CompletedTask;
//        }

//        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (string.IsNullOrEmpty(roleId)) return null;

//            const string sql = "SELECT * FROM AspNetRoles WHERE Id = @Id LIMIT 1";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("Id", roleId);

//            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
//            if (!await reader.ReadAsync(cancellationToken)) return null;

//            return MapReaderToIdentityRole(reader);
//        }

//        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (string.IsNullOrEmpty(normalizedRoleName)) return null;

//            const string sql = "SELECT * FROM AspNetRoles WHERE NormalizedName = @NormalizedName LIMIT 1";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("NormalizedName", normalizedRoleName);

//            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
//            if (!await reader.ReadAsync(cancellationToken)) return null;

//            return MapReaderToIdentityRole(reader);
//        }

//        // IRoleClaimStore<IdentityRole> implementation
//        public async Task<IList<Claim>> GetClaimsAsync(IdentityRole role, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));

//            const string sql = "SELECT ClaimType, ClaimValue FROM AspNetRoleClaims WHERE RoleId = @RoleId";

//            var claims = new List<Claim>();
//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("RoleId", role.Id);

//            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
//            while (await reader.ReadAsync(cancellationToken))
//            {
//                var type = reader["ClaimType"] as string;
//                var value = reader["ClaimValue"] as string;
//                if (type != null)
//                    claims.Add(new Claim(type, value ?? string.Empty));
//            }

//            return claims;
//        }

//        public async Task AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            if (claim is null) throw new ArgumentNullException(nameof(claim));

//            const string sql = "INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) VALUES (@RoleId, @ClaimType, @ClaimValue)";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("RoleId", role.Id);
//            cmd.Parameters.AddWithValue("ClaimType", (object?)claim.Type ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("ClaimValue", (object?)claim.Value ?? DBNull.Value);

//            await cmd.ExecuteNonQueryAsync(cancellationToken);
//        }

//        public async Task RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            if (claim is null) throw new ArgumentNullException(nameof(claim));

//            const string sql = @"
//DELETE FROM AspNetRoleClaims
//WHERE RoleId = @RoleId AND ClaimType = @ClaimType AND (ClaimValue = @ClaimValue OR (@ClaimValue IS NULL AND ClaimValue IS NULL))";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("RoleId", role.Id);
//            cmd.Parameters.AddWithValue("ClaimType", (object?)claim.Type ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("ClaimValue", (object?)claim.Value ?? DBNull.Value);

//            await cmd.ExecuteNonQueryAsync(cancellationToken);
//        }

//        public async Task ReplaceClaimAsync(IdentityRole role, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (role is null) throw new ArgumentNullException(nameof(role));
//            if (claim is null) throw new ArgumentNullException(nameof(claim));
//            if (newClaim is null) throw new ArgumentNullException(nameof(newClaim));

//            const string sql = @"
//UPDATE AspNetRoleClaims
//SET ClaimType = @NewClaimType, ClaimValue = @NewClaimValue
//WHERE RoleId = @RoleId AND ClaimType = @OldClaimType AND (ClaimValue = @OldClaimValue OR (@OldClaimValue IS NULL AND ClaimValue IS NULL))";

//            await using var conn = new NpgsqlConnection(_connectionString);
//            await conn.OpenAsync(cancellationToken);

//            await using var cmd = new NpgsqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("RoleId", role.Id);
//            cmd.Parameters.AddWithValue("NewClaimType", (object?)newClaim.Type ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("NewClaimValue", (object?)newClaim.Value ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("OldClaimType", (object?)claim.Type ?? DBNull.Value);
//            cmd.Parameters.AddWithValue("OldClaimValue", (object?)claim.Value ?? DBNull.Value);

//            await cmd.ExecuteNonQueryAsync(cancellationToken);
//        }

//        public void Dispose()
//        {
//            // no unmanaged resources to dispose
//        }

//        private static IdentityRole MapReaderToIdentityRole(NpgsqlDataReader reader)
//        {
//            var role = new IdentityRole
//            {
//                Id = reader["Id"]?.ToString(),
//                Name = reader["Name"] as string,
//                NormalizedName = reader["NormalizedName"] as string,
//                ConcurrencyStamp = reader["ConcurrencyStamp"] as string
//            };

//            return role;
//        }
//    }
}
