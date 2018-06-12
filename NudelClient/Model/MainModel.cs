using Nudel.BusinessObjects;
using System;
using System.Windows;

namespace Nudel.Client.Model
{
    public delegate void ModelChangedHandler(string fieldName, object field);

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

                Invoke("SessionToken", sessionToken);
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
                Invoke("User", user);
            }
        }

        private static void Invoke(string fieldName, object field)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                ModelChanged?.Invoke(fieldName, field);
            }));
        }

        public static event ModelChangedHandler ModelChanged;
    }
}
