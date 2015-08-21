using Microsoft.WindowsAzure.StorageClient;

namespace Account.TableModel
{
    /// <summary>
    /// User 
    /// Partition Key - This field contains the username of the user. john@haigh.com - username 
    /// Row Key - This field contains the partition key of the user concatenated with the fullname of the user. johnhaigh@haigh.com_johnhaigh - username_fullname
    /// </summary>
    public class User : TableServiceEntity
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public User()
        {
        }
        
        /// <summary>
        /// User for an account. 
        /// Partition Key - Lowercase username.
        /// Row Key - A combination of username_firstnamelastname
        /// </summary>
        /// <param name="username">The Username</param>
        /// <param name="firstName">The Firstname</param>
        /// <param name="lastName">The Lastname</param>
        /// <param name="passwordHash">The Password Hash</param>
        /// <param name="email">The Email Address</param>
        /// <param name="isActive">True if the user is active, otherwise false for not active.</param>
        public User(string username, string firstName, string lastName, string passwordHash, string email, bool isActive)
        {
            var usernameClean = username.ToLower().Trim();
            var firstnameClean = firstName.ToLower().Trim();
            var lastnameClean = lastName.ToLower().Trim();

            this.PartitionKey = usernameClean;
            this.RowKey = string.Format("{0}_{1}{2}", usernameClean, firstnameClean, lastnameClean);

            this.Username = usernameClean;
            this.PasswordHash = passwordHash;
            this.Email = email;

            this.IsActive = isActive;
        }
    }
}
