using Account.TableModel;
using Microsoft.WindowsAzure;
using VersoSystems.Storage.TableStorage;
using VersoSystems.Storage.TableStorage.Account;

namespace Account.DataRepository
{
    public class UserRepository : TableStorageRepository<User>
    {
        public UserRepository()
            : base(new AccountConnection<User>(CloudConfigurationManager.GetSetting("StorageConnectionString")))
        {
            base.CreateTableIfNotExists();
        }

        public UserRepository(AccountConnection<User> connection) : base(connection)
        {
            base.CreateTableIfNotExists();
        }
    }
}
