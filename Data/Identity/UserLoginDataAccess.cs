using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace PoseDatabaseWebApi.Data.Identity
{
    public class UserLoginDataAccess : IUserLoginStore<IdentityUser>
    {
        private readonly string _connectionString;

        public UserLoginDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // IUserLoginStore (Npgsql-backed skeletons)
        public async Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (login is null) throw new ArgumentNullException(nameof(login));
            cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"
INSERT INTO AspNetUserLogins (LoginProvider, ProviderKey, ProviderDisplayName, UserId)
VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId)";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("LoginProvider", login.LoginProvider ?? string.Empty);
            cmd.Parameters.AddWithValue("ProviderKey", login.ProviderKey ?? string.Empty);
            cmd.Parameters.AddWithValue("ProviderDisplayName", (object?)login.ProviderDisplayName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("UserId", user.Id ?? string.Empty);

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"
DELETE FROM AspNetUserLogins
WHERE LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey AND UserId = @UserId";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("LoginProvider", loginProvider ?? string.Empty);
            cmd.Parameters.AddWithValue("ProviderKey", providerKey ?? string.Empty);
            cmd.Parameters.AddWithValue("UserId", user.Id ?? string.Empty);

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            const string sql = @"
SELECT LoginProvider, ProviderKey, ProviderDisplayName
FROM AspNetUserLogins
WHERE UserId = @UserId";

            var result = new List<UserLoginInfo>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("UserId", user.Id ?? string.Empty);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var provider = reader["LoginProvider"] as string ?? string.Empty;
                var key = reader["ProviderKey"] as string ?? string.Empty;
                var display = reader["ProviderDisplayName"] as string;
                result.Add(new UserLoginInfo(provider, key, display));
            }

            return result;
        }
        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
//        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            if (string.IsNullOrEmpty(loginProvider) || string.IsNullOrEmpty(providerKey)) return null;

        //            const string sql = @"
        //SELECT u.*
        //FROM AspNetUserLogins l
        //JOIN AspNetUsers u ON u.Id = l.UserId
        //WHERE l.LoginProvider = @LoginProvider AND l.ProviderKey = @ProviderKey
        //LIMIT 1";

        //            await using var conn = new NpgsqlConnection(_connectionString);
        //            await conn.OpenAsync(cancellationToken);

        //            await using var cmd = new NpgsqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("LoginProvider", loginProvider);
        //            cmd.Parameters.AddWithValue("ProviderKey", providerKey);

        //            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        //            if (!await reader.ReadAsync(cancellationToken)) return null;

        //            return MapReaderToIdentityUser(reader);
        //        }

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

