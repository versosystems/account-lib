using Generic.BusinessLogic.User;

namespace Generic.BusinessLogic.Authentication
{
    public interface IAuthenticationService
    {
        bool AuthenticateUser(string username, string userSuppliedPassword);
    }

    public class AuthenticateService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticateService()
        {
            _userService = new UserService();
        }

        public bool AuthenticateUser(string username, string userSuppliedPassword)
        {
            var user = _userService.GetUserByUsername(username);

            var isAuth = false;

            if (user != null)
            {
                var hashedPassword = Hasher.GetHash(userSuppliedPassword, user.PasswordHash);
                // todo validate islocked out, 
                // todo validate is temp lockout due to login from location not in user typical region
                isAuth = user.IsActive && AuthHelper.ComparePasswords(user.PasswordHash, hashedPassword);
            }

            return isAuth;
        }
    }
}
