namespace RMB.Core.ValuesObjects.Logradouro
{
    /// <summary>
    /// Represents a Brazilian address structure, typically returned from services like ViaCEP.
    /// </summary>
    public record Endereco
    {
        /// <summary>
        /// Brazilian postal code (CEP), formatted as "00000-000".
        /// </summary>
        public string Cep { get; set; }

        /// <summary>
        /// Street name or public place (e.g., avenue, square).
        /// </summary>
        public string Logradouro { get; set; }

        /// <summary>
        /// Street number or building number.
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Address complement (e.g., apartment, block).
        /// </summary>
        public string Complemento { get; set; }

        /// <summary>
        /// Neighborhood or district of the address.
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// City or municipality.
        /// </summary>
        public string Localidade { get; set; }

        /// <summary>
        /// Federative unit (state), represented by its two-letter abbreviation (e.g., "SP", "RJ").
        /// </summary>
        public string Uf { get; set; }

        /// <summary>
        /// IBGE code corresponding to the municipality.
        /// </summary>
        public string Ibge { get; set; }
    }
}
