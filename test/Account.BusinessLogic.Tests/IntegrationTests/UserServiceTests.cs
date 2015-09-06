using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.DataModel;
using Account.DataRepository;
using NUnit.Framework;
using Generic.BusinessLogic.User;
using Should;

namespace Account.BusinessLogic.Tests.IntegrationTests
{
    [TestFixture, Ignore]
    public class UserServiceTests
    {
        [Test]
        public void AddUser_should_save()
        {
            string username = "save" + Guid.NewGuid();
            
            var user = new User
            {
                 Email = "save@gmail.com",
                 FirstName = "john",
                 IsActive = true,
                 LastName = "haigh",
                 PasswordHash = "asdf",
                 Username = username
            };

            var svc = new UserService();
            svc.AddUser(user);

            var repo = new UserRepository();

            var repoUser = repo.Find(u => u.Username == username).FirstOrDefault();

            // Test
            repoUser.ShouldNotBeNull();

            // Cleanup user 
            repo.Delete(repoUser);
            repo.Save();

            var results2 = repo.Find(u => u.Username == username);
            var userFound2 = results2.FirstOrDefault();

            Assert.IsNull(userFound2, "Tried to get user but the user was found");
        }

        [Test]
        public void AddUser_should_save_with_user_properties()
        {
            string username = "save_with_user_propertie" + Guid.NewGuid();

            var user = new User
            {
                Email = "save_with_user_properties@gmail.com",
                FirstName = "john",
                IsActive = true,
                LastName = "haigh",
                PasswordHash = "asdf",
                Username = username
            };

            var svc = new UserService();
            svc.AddUser(user);

            var repo = new UserRepository();

            var repoUser = repo.Find(u => u.Username == username).FirstOrDefault();

            // Test
            repoUser.Email.ShouldEqual(user.Email);
            repoUser.FirstName.ShouldEqual(user.FirstName);
            repoUser.LastName.ShouldEqual(user.LastName);
            repoUser.IsActive.ShouldEqual(user.IsActive);
            repoUser.PasswordHash.ShouldEqual(user.PasswordHash);
            repoUser.Username.ShouldEqual(user.Username);
            
            // Cleanup user 
            repo.Delete(repoUser);
            repo.Save();

            var results2 = repo.Find(u => u.Username == username);
            var userFound2 = results2.FirstOrDefault();

            userFound2.ShouldBeNull();
        }

        [Test]
        public void GetUserByUsername_should_not_getuser()
        {
            var svc = new UserService();
         
            // main test

            var userAssert = svc.GetUserByUsername("badusername");

            userAssert.ShouldBeNull();
        }

        [Test]
        public void GetUserByUsername_should_getuser()
        {
            string username = "getbyusername" + Guid.NewGuid();

            var user = new User
            {
                Email = "getbyusername@gmail.com",
                FirstName = "john",
                IsActive = true,
                LastName = "haigh",
                PasswordHash = "asdf",
                Username = username
            };

            var svc = new UserService();
            svc.AddUser(user);

            // main test
            var userAssert = svc.GetUserByUsername(username);

            userAssert.ShouldNotBeNull();

            // Assert
            var repo = new UserRepository();

            var repoUser = repo.Find(u => u.Username == username).FirstOrDefault();

            // Cleanup user 
            repo.Delete(repoUser);
            repo.Save();

            var results2 = repo.Find(u => u.Username == username);
            var userFound2 = results2.FirstOrDefault();

            userFound2.ShouldBeNull();
        }

        [Test]
        public void UpdateUserNameEmailFields_should_update_fields()
        {
        }

        [Test]
        public void DeleteUser_should_delete_user()
        {
        }

        [Test]
        public void DisableUser_should_disable_user()
        {
        }

        [Test]
        public void DoesUserExist_should_user_exist()
        {
        }
    }
}
