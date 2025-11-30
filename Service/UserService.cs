using Microsoft.AspNetCore.Identity;

namespace PoseDatabaseWebApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUserStore<IdentityUser> _userStore;
        public UserService(IUserStore<IdentityUser> store) { _userStore = store; }

        public async Task<bool> CreateNewUserAsync(string username, string password, string email)
        {
            var user = new IdentityUser
            {
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                // You might want to set other properties as needed
            };
            var result = await _userStore.CreateAsync(user, CancellationToken.None);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            // Note: Password handling is not implemented here.
            // In a real application, you would use a UserManager to handle password hashing and storage.

            return result.Succeeded;
        }
    }
}
