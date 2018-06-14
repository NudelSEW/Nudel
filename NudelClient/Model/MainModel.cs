using Nudel.BusinessObjects;
using System;
using System.Windows;

namespace Nudel.Client.Model
{
    public delegate void ModelChangedHandler(string fieldName, object field);

    /// <summary>
    /// the basic definition for the sessionToken, User, Invoke, ModelChanged(event)
    /// </summary>
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

        /// <summary>
        /// invoking current dispatcher and changing the model through a new action
        /// </summary>
        /// <param name="fieldName"> the button or field name </param>
        /// <param name="field">the type of the object </param>
        private static void Invoke(string fieldName, object field)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                ModelChanged?.Invoke(fieldName, field);
            }));
        }

        public static event ModelChangedHandler ModelChanged;
    }
}
