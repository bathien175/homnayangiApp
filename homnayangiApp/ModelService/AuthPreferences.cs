using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ModelService
{
    public static class AuthPreferences
    {
        private const string PhoneKey = "Phone";
        private const string PasswordKey = "Password";
        private const string RememberLoginKey = "RememberLogin";

        public static string Phone
        {
            get => Preferences.Get(PhoneKey, string.Empty);
            set => Preferences.Set(PhoneKey, value);
        }

        public static string Password
        {
            get => Preferences.Get(PasswordKey, string.Empty);
            set => Preferences.Set(PasswordKey, value);
        }

        public static bool RememberLogin
        {
            get => Preferences.Get(RememberLoginKey, false);
            set => Preferences.Set(RememberLoginKey, value);
        }
    }
}
