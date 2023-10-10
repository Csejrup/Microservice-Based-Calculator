using CalculatorAPI.Aggregates;

namespace CalculatorAPI.Services;

public class CalculatorService
{
    private readonly HttpClient _httpClient;

    public CalculatorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> Add(double a, double b)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5000/addition/{a}/{b}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<double>();
    }

    public async Task<double> Subtract(double a, double b)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5001/subtraction/{a}/{b}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<double>();
    }

    public async Task AddToHistory(string operation)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5002/history/add", operation);
        response.EnsureSuccessStatusCode();
    }
    public async Task<IEnumerable<CalculationHistory>> GetCalculationHistory()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:5002/history/get-all");
            response.EnsureSuccessStatusCode();
            var history = await response.Content.ReadFromJsonAsync<IEnumerable<CalculationHistory>>();
            return history;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching the calculation history: {ex.Message}");
            throw;
        }
    }

}