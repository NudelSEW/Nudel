using Nudel.BusinessObjects;
using System;

namespace Nudel.Client
{
    public delegate void ModelChangedHandler(string fieldName, Object field);

    public class MainModel
    {
        private static string sessionToken;
        public static string SessionToken
        {
            get
            {
                return sessionToken;
            }
            set
            {
                sessionToken = value;
                ModelChanged?.Invoke("SessionToken", sessionToken);
            }
        }

        private static User user;
        public static User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                ModelChanged?.Invoke("User", user);
            }
        }

        public static event ModelChangedHandler ModelChanged;
    }
}
