using Account.DataRepository;
using UsersDataModel = Account.DataModel;
using TableDataModel = Account.TableModel;

namespace Generic.BusinessLogic.User
{
    public interface IUserService
    {
        UsersDataModel.User GetUser(UsersDataModel.User user);
        void AddUser(UsersDataModel.User user);

        void UpdateUser(UsersDataModel.User user);

        void DisableUser(UsersDataModel.User user);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        public UserService()
        { 
            _userRepository = new UserRepository();
        }

        public UsersDataModel.User GetUser(UsersDataModel.User user)
        {
            var rowKey = string.Format("{0}_{1}{2}", 
                user.Username.ToLower().Trim(), 
                user.FirstName.ToLower().Trim(), 
                user.LastName.ToLower().Trim());

            var existUser = _userRepository.GetByPartitionKeyAndRowKey(user.Username, rowKey);

            if (existUser != null)
            {
                return new UsersDataModel.User
                {
                    Email = existUser.Email,
                    FirstName = existUser.Email,
                    IsActive = existUser.IsActive,
                    LastName = existUser.LastName,
                    PasswordHash = existUser.PasswordHash,
                    Username = existUser.Username
                };
            }

            return null;
        }

        public void AddUser(UsersDataModel.User user)
        {
            _userRepository.Add(new TableDataModel.User(user.Username,user.FirstName,user.LastName,user.PasswordHash,user.Email,true));
            _userRepository.Save();
        }

        public void UpdateUser(UsersDataModel.User user)
        {
            //_userRepository.Update();
            //_userRepository.Save();
        }

        public void DisableUser(UsersDataModel.User user)
        {
            // Disable user
            user.IsActive = false;

            //_userRepository.Update();
            _userRepository.Save();
        }
    }
}
