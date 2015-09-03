namespace Generic.BusinessLogic.User
{
    public class AuthHelper
    {
        /// <summary>
        /// Compare Paswords - Compares a source password and the user supplied password.
        /// </summary>
        /// <param name="password">The password - database password</param>
        /// <param name="suppliedPassword">The user supplied password</param>
        /// <returns>True if the passwords are equal, false if not equal.</returns>
        public static bool ComparePasswords(string password, string suppliedPassword)
        {
            return password == suppliedPassword;
        }
    }
}
