using Account.TableModel;
using Microsoft.WindowsAzure;
using VersoSystems.Storage.TableStorage;
using VersoSystems.Storage.TableStorage.Account;

namespace Account.DataRepository
{

    public interface IUserRepository : ITableStorageRepository<User>
    {
    }

    public class UserRepository : TableStorageRepository<User>, IUserRepository
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
