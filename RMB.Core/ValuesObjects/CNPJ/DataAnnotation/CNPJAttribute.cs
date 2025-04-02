using RMB.Core.ValuesObjects.CNPJ.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RMB.Core.ValuesObjects.CNPJ.DataAnnotation
{
    /// <summary>
    /// Custom Data Annotation for CNPJ validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CNPJAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates whether the provided value is a valid CNPJ.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <param name="validationContext">Validation context.</param>
        /// <returns>ValidationResult.Success if valid; otherwise, a validation error message.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success; // Permite valores nulos (caso seja obrigatório, adicione [Required] junto ao atributo)

            string cnpj = value.ToString()!;

            if (!CNPJValidation.Validation(cnpj))
                return new ValidationResult("CNPJ inválido.");

            return ValidationResult.Success;
        }
    }
}
