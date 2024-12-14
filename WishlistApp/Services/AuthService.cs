using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WishlistApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:5063/api/User/")
            };
        }

        public async Task<string> RegisterAsync(string name, string email, string password)
        {
            var userData = new
            {
                name = name,
                email = email,
                password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("register", content);

            if (response.IsSuccessStatusCode)
            {
                return "Registration successful";
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return $"Registration failed: errorResponse";
        }

        public async Task<(string Message, string? Token, string? UserId)> LoginAsync(string email, string password)
        {
            var loginData = new
            {
                email = email,
                password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResult = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return ("Login successful", loginResult?.Token, loginResult?.User?.Id);
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return ($"Login failed: errorResponse", null, null);
        }

        public async Task<User?> GetUserInfoAsync(string userId)
        {
            var response = await _httpClient.GetAsync(userId);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<User>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }
    }

    public class LoginResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("user")]
        public User? User { get; set; }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}
