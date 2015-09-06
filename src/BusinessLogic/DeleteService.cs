namespace Generic.BusinessLogic.User
{
    public interface IDeleteService
    {
        void DeleteAccount(string username);
    }

    public class DeleteService : IDeleteService
    {
        private readonly IUserService _userService;

        public DeleteService(IUserService userService)
        {
            _userService = userService;
        }

        public void DeleteAccount(string username)
        {
            _userService.DeleteUser(username);
        }
    }
}
