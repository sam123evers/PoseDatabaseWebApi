using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace PoseDatabaseWebApi.Stores
{
    public class PgUserAuthenticationTokenStore : IUserAuthenticationTokenStore<IdentityUser>
    {
        private readonly string _connectionString;

        public PgUserAuthenticationTokenStore(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // IUserAuthenticationTokenStore (Npgsql-backed skeletons)
        public async Task SetTokenAsync(IdentityUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"
INSERT INTO AspNetUserTokens (UserId, LoginProvider, Name, Value)
VALUES (@UserId, @LoginProvider, @Name, @Value)
ON CONFLICT (UserId, LoginProvider, Name)
DO UPDATE SET Value = EXCLUDED.Value";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("UserId", user.Id ?? string.Empty);
            cmd.Parameters.AddWithValue("LoginProvider", loginProvider ?? string.Empty);
            cmd.Parameters.AddWithValue("Name", name ?? string.Empty);
            cmd.Parameters.AddWithValue("Value", (object?)value ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"
DELETE FROM AspNetUserTokens
WHERE UserId = @UserId AND LoginProvider = @LoginProvider AND Name = @Name";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("UserId", user.Id ?? string.Empty);
            cmd.Parameters.AddWithValue("LoginProvider", loginProvider ?? string.Empty);
            cmd.Parameters.AddWithValue("Name", name ?? string.Empty);

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<string> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"
SELECT Value
FROM AspNetUserTokens
WHERE UserId = @UserId AND LoginProvider = @LoginProvider AND Name = @Name
LIMIT 1";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("UserId", user.Id ?? string.Empty);
            cmd.Parameters.AddWithValue("LoginProvider", loginProvider ?? string.Empty);
            cmd.Parameters.AddWithValue("Name", name ?? string.Empty);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken)) return null;
            return reader["Value"] as string;
        }

        Task<string> IUserStore<IdentityUser>.GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string?> IUserStore<IdentityUser>.GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<IdentityUser>.SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string?> IUserStore<IdentityUser>.GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<IdentityUser>.SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserStore<IdentityUser>.CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserStore<IdentityUser>.UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserStore<IdentityUser>.DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityUser?> IUserStore<IdentityUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityUser?> IUserStore<IdentityUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
