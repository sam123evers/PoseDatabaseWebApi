using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Identity.User;

namespace PoseDatabaseWebApi.Data.Identity
{

    public class UserDataAccess : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
    {
        //private readonly string _connectionString;
        private readonly NpgsqlDataSource dataSource;

        public UserDataAccess(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
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

        //public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            // open NpgsqlConnection, INSERT into AspNetUsers, return IdentityResult.Success
            //throw new NotImplementedException();
            // you might need IdentityUser model for something

            var sql = @"
                INSERT INTO identity.asp_net_users (
                  id
                , username
                , normalized_username
                , email
                , normalized_email
                , email_confirmed
                , password_hash
                , security_stamp
                , concurrency_stamp
                , phone_number
                , phone_number_confirmed
                , two_factor_enabled
                , lockout_end
                , lockout_enabled
                , access_failed_count ) 
                VALUES (
                  @id
                , @username
                , @norm_username
                , @email
                , @norm_email
                , @email_confirmed
                , @pass_hash
                , @sec_stamp
                , @concur_stamp
                , @phone_num
                , @phone_num_conf
                , @two_fact_en
                , @lock_end
                , @lock_en
                , @acc_fail_count ) RETURNING id;".Trim();

            await using var cmd = dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("id", user.Id);
            cmd.Parameters.AddWithValue("username", user.UserName);
            cmd.Parameters.AddWithValue("norm_username", user.NormalizedUserName);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("norm_email", user.NormalizedEmail);
            cmd.Parameters.AddWithValue("email_confirmed", user.EmailConfirmed);
            //cmd.Parameters.AddWithValue("pass_hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("pass_hash", (object?)user.PasswordHash ?? DBNull.Value);
            cmd.Parameters.AddWithValue("sec_stamp", user.SecurityStamp);
            cmd.Parameters.AddWithValue("concur_stamp", user.ConcurrencyStamp);
            //cmd.Parameters.AddWithValue("phone_num", user.PhoneNumber);
            cmd.Parameters.AddWithValue("phone_num", (object?)user.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("phone_num_conf", user.PhoneNumberConfirmed);
            cmd.Parameters.AddWithValue("two_fact_en", user.TwoFactorEnabled);
            cmd.Parameters.AddWithValue("lock_end", (object?)user.LockoutEnd ?? DBNull.Value);
            cmd.Parameters.AddWithValue("lock_en", user.LockoutEnabled);
            cmd.Parameters.AddWithValue("acc_fail_count", user.AccessFailedCount);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            //if (result == null || result == DBNull.Value)
            //{
            //    return -1;
            //}

            //return Convert.ToInt32(result);

            return IdentityResult.Success;
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // IUserPasswordStore
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public void Dispose() { /* dispose connections if needed */ }
    }
}

