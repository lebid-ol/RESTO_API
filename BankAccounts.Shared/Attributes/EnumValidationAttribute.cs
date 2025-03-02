using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Shared.Attributes
{
    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType; // AccountType

        public EnumValidationAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !Enum.IsDefined(_enumType, value))
            {
                return new ValidationResult($"The value '{value}' is not valid for {_enumType.Name}.");
            }

            return ValidationResult.Success;
        }
    }
}
