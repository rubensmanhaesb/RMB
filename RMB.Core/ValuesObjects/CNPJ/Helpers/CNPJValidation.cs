using System.Text.RegularExpressions;

namespace RMB.Core.ValuesObjects.CNPJ.Helpers
{
    /// <summary>
    /// Provides validation logic for Brazilian CNPJ numbers.
    /// </summary>
    public static class CNPJValidation
    {
        /// <summary>
        /// Validates whether a given CNPJ string is structurally valid.
        /// </summary>
        /// <param name="cnpj">The CNPJ to validate.</param>
        /// <returns>True if the CNPJ is valid; otherwise, false.</returns>
        public static bool Validation(string? cnpj)
        {

            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            if (cnpj.Length != 14)
                return false;

            if (new string(cnpj[0], cnpj.Length) == cnpj)
                return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}
