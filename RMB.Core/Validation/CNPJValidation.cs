using System.Text.RegularExpressions;


namespace RMB.Core.Validation
{
    public static class CNPJValidation
    {
        public static bool ValidarCNPJ(string? cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Remove caracteres não numéricos
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            if (cnpj.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais (exemplo: 11111111111111)
            if (new string(cnpj[0], cnpj.Length) == cnpj)
                return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;

            return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
        }

    }
}
