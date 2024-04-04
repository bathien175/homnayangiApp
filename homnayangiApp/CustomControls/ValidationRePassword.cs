using InputKit.Shared.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class ValidationRePassword : IValidation
    {
        private readonly string password;

        public ValidationRePassword(string password)
        {
            this.password = password;
        }

        public string Message => "Mật khẩu không khớp!";

        public bool Validate(object value)
        {
            if (value == null || !(value is string confirmPassword))
                return false;

            return confirmPassword == password;
        }
    }
}
