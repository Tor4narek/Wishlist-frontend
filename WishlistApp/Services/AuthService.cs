using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WishlistApp.Models;

namespace WishlistApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private string? _jwtToken; // Хранение JWT токена

        public AuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5063/api/User/")
            };
        }

        // Метод для сохранения JWT токена после авторизации
        public void SetJwtToken(string token)
        {
            _jwtToken = token;
        }

        // Установка заголовка Authorization для аутентификации
        private void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);
            }
        }

        // Метод регистрации пользователя
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
            return $"Registration failed: {errorResponse}";
        }

        // Метод для логина пользователя и получения токена
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
            return ($"Login failed: {errorResponse}", null, null);
        }

        // Метод для получения информации о пользователе по ID (без токена)
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

        // Новый метод для получения пользователя по email (с токеном)
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            // Устанавливаем заголовок с токеном
            SetAuthorizationHeader();

            var requestUrl = $"email/{Uri.EscapeDataString(email)}";
            Console.WriteLine($"Requesting URL: {requestUrl}");

            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {responseContent}");

            return JsonSerializer.Deserialize<User>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    // Модель для ответа при логине
    public class LoginResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("user")]
        public User? User { get; set; }
    }
}
