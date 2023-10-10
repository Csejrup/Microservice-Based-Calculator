using CalculatorAPI.Aggregates;
using OpenTelemetry.Trace;
using System.Diagnostics;
using Serilog;

namespace CalculatorAPI.Services
{
    public class CalculatorService
    {
        private readonly HttpClient _httpClient;
        private readonly Tracer _tracer;

        public CalculatorService(HttpClient httpClient, TracerProvider tracerProvider)
        {
            _httpClient = httpClient;
            _tracer = tracerProvider.GetTracer("CalculatorAPI");
        }

        public async Task<double> Add(double a, double b)
        {
            var span = _tracer.StartSpan("Add");
            try
            {
                span.SetAttribute("operand1", a);
                span.SetAttribute("operand2", b);

                var response = await _httpClient.GetAsync($"http://addition-service:80/add/{a}/{b}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<double>();

                span.SetAttribute("result", result);
                span.End();

                return result;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                span.End();
                Log.Error($"An error occurred while adding numbers: {ex.Message}");
                throw;
            }
        }

        public async Task<double> Subtract(double a, double b)
        {
            var span = _tracer.StartSpan("Subtract");
            try
            {
                span.SetAttribute("operand1", a);
                span.SetAttribute("operand2", b);

                var response = await _httpClient.GetAsync($"http://subtraction-service:80/sub/{a}/{b}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<double>();

                span.SetAttribute("result", result);
                span.End();

                return result;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                span.End();
                Log.Error($"An error occurred while subtracting numbers: {ex.Message}");
                throw;
            }
        }

        public async Task AddToHistory(string operation)
        {
            var span = _tracer.StartSpan("AddToHistory");
            try
            {
                span.SetAttribute("operation", operation);

                var response = await _httpClient.PostAsJsonAsync("http://history-service:80/history/add", operation);
                response.EnsureSuccessStatusCode();

                span.End();
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                span.End();
                Log.Error($"An error occurred while adding to history: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CalculationHistory>> GetCalculationHistory()
        {
            var span = _tracer.StartSpan("GetCalculationHistory");
            try
            {
                var response = await _httpClient.GetAsync("http://history-service:80/history/get-all");
                response.EnsureSuccessStatusCode();
                var history = await response.Content.ReadFromJsonAsync<IEnumerable<CalculationHistory>>();

                span.End();

                return history;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                span.End();
                Log.Error($"An error occurred while fetching the calculation history: {ex.Message}");
                throw;
            }
        }
    }
}
