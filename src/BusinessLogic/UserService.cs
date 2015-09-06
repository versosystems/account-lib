using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Account.DataRepository;
using UsersDataModel = Account.DataModel;
using TableDataModel = Account.TableModel;

namespace Generic.BusinessLogic.User
{
    public interface IUserService
    {
        void AddUser(UsersDataModel.User user);

        void UpdateUserNameEmailFields(UsersDataModel.User user);

        void DisableUser(string username);

        bool DoesUserExist(string username);

        void DeleteUser(string username);

        UsersDataModel.User GetUserByUsername(string username);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService()
        { 
            _userRepository = new UserRepository();
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UsersDataModel.User GetUserByUsername(string username)
        {
            var user = _userRepository.Find(a => a.Username == username);

            if (user != null && user.AsEnumerable().Count() > 0)
            {
                var foundUser = user.First();

                return new UsersDataModel.User
                {
                    Email = foundUser.Email,
                    FirstName = foundUser.FirstName,
                    IsActive = foundUser.IsActive,
                    LastName = foundUser.LastName,
                    Username = foundUser.Username,
                    PasswordHash = foundUser.PasswordHash
                };
            }

            return null;
        }

        public void AddUser(UsersDataModel.User user)
        {
            _userRepository.Add(new TableDataModel.User(user.Username,user.FirstName,user.LastName,user.PasswordHash,user.Email,true));
            _userRepository.Save();
        }

        public void UpdateUserNameEmailFields(UsersDataModel.User user)
        {
            var existUser = GetTableModelUser(user.Username);

            if (existUser != null)
            {
                existUser.Email = user.Email;
                existUser.FirstName = user.FirstName;
                existUser.LastName = user.LastName;
                
                _userRepository.Update(existUser);
                _userRepository.Save();
            }
        }

        public void DeleteUser(string username)
        {
            var user = GetTableModelUser(username);

            if (user != null)
            {
                _userRepository.Delete(user);
                _userRepository.Save();
            }
        }

        public void DisableUser(string username)
        {
            var user = GetTableModelUser(username);

            if (user != null)
            {
                // Disable user
                user.IsActive = false;

                _userRepository.Update(user);
                _userRepository.Save();
            }
        }

        public bool DoesUserExist(string username)
        {
            var user = GetDataModelUser(username);
            return user == null ? false : true;
        }

        private TableDataModel.User GetTableModelUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            return _userRepository.Find(a => a.Username == username).FirstOrDefault();
        }

        private UsersDataModel.User GetDataModelUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            return GetUserByUsername(username);
        }
    }
}
