﻿namespace Account.DataModel
{
    public class User 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
