using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Serilog;

namespace Calculator.Web.Pages
{
    public class CalculatorModel : PageModel
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [BindProperty]
        public double Number1 { get; set; }

        [BindProperty]
        public double Number2 { get; set; }
        public double Result { get; set; }
        public List<string> History { get; set; } = new List<string>();

        public async Task<IActionResult> OnPostAddAsync()
        {
            try
            {
                await Add();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding numbers");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSubtractAsync()
        {
            try
            {
                await Subtract();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while subtracting numbers");
            }
            return Page();
        }

        public async Task Add()
        {
            var response = await _httpClient.GetAsync($"http://localhost:5003/api/calculator/add/{Number1}/{Number2}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    Result = JsonConvert.DeserializeObject<double>(content);
                    await LoadHistory();
                }
                else
                {
                    Log.Warning("The response content is empty during addition operation.");
                }
            }
            else
            {
                Log.Error($"Failed to add numbers: {response.StatusCode}");
            }
        }

        public async Task Subtract()
        {
            var response = await _httpClient.GetAsync($"http://localhost:5003/api/calculator/subtract/{Number1}/{Number2}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    Result = JsonConvert.DeserializeObject<double>(content);
                    await LoadHistory();
                }
                else
                {
                    Log.Warning("The response content is empty during subtraction operation.");
                }
            }
            else
            {
                Log.Error($"Failed to subtract numbers: {response.StatusCode}");
            }
        }

        private async Task LoadHistory()
        {
            var response = await _httpClient.GetAsync("http://localhost:5003/api/calculator/history");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    History = JsonConvert.DeserializeObject<List<string>>(content);
                }
                else
                {
                    Log.Warning("The history content is empty.");
                }
            }
            else
            {
                Log.Error($"Failed to load history: {response.StatusCode}");
            }
        }

        public async Task OnGetAsync()
        {
            try
            {
                await LoadHistory();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while loading history");
            }
        }
    }
}
