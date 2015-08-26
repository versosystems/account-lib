using Generic.BusinessLogic.User;
using UsersDataModel = Account.DataModel;

namespace Account.BusinessLogic
{
    public interface IRegistrationService
    {
        void Register(string username, string firstname, string lastname, string password, string email);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _userService;
        public RegistrationService()
        {
            _userService = new UserService();
        }

        public void Register(string username, string firstname, string lastname, string password, string email)
        {
            // Hash the password
            var hashedPassword = Hasher.CreateHash(password);

            var existUser = new UsersDataModel.User
            {
                Email = email,
                LastName = lastname,
                FirstName = firstname,
                PasswordHash = hashedPassword,
                Username = username
            };

            // fail safe in case someone has registered this account after the register check user has been called.
            if (_userService.GetUser(existUser) == null)
            {
                _userService.AddUser(existUser);
            }
        }
    }
}
