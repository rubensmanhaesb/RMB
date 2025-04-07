using System.Text.Json;

namespace RMB.Core.ValuesObjects.Logradouro
{
    /// <summary>
    /// Service responsible for querying address data from the ViaCEP API using a given Brazilian postal code (CEP).
    /// </summary>
    public class ViaCepService : IViaCepService
    {
        public async Task<Endereco?> GetCepAsync(string cep, CancellationToken cancellationToken = default)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<Endereco>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

}
