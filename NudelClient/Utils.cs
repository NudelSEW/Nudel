using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nudel.Client
{
    /// <summary>
    /// checking the entered email of the user
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// comparing the new users email if it already exists
        /// </summary>
        /// <param name="email"> the entered email of the registration </param>
        /// <returns> returns if the email is conforming the requirements </returns>
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
