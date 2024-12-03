using System.ComponentModel.DataAnnotations;

namespace JustShop2.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string ALLOWEDDOMAIN;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            ALLOWEDDOMAIN = allowedDomain;
        }

    }
}
