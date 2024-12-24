using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WishlistApp.Services
{
    public class PresentCommandsService
    {
        private readonly HttpClient _httpClient;

        public PresentCommandsService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5063/api/PresentCommands/")
            };
        }

        private void AddAuthorizationHeader()
        {
            // Берем токен из UserService и добавляем его в заголовки
            if (!string.IsNullOrEmpty(UserService._jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserService._jwtToken);
            }
        }

        /// <summary>
        /// Добавление нового подарка.
        /// </summary>
        public async Task AddPresentAsync(string name, string description, string reserverId, string wishlistId, CancellationToken token = default)
        {
            AddAuthorizationHeader();

            var request = new
            {
                Name = name,
                Description = description,
                ReserverId = reserverId,
                WishlistId = wishlistId
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("add", content, token);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(token);
                throw new Exception($"Error adding present: {response.StatusCode} - {errorMessage}");
            }
        }

        /// <summary>
        /// Удаление подарка по ID.
        /// </summary>
        public async Task DeletePresentAsync(string presentId, CancellationToken token = default)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.DeleteAsync($"delete/{presentId}", token);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(token);
                throw new Exception($"Error deleting present: {response.StatusCode} - {errorMessage}");
            }
        }

        /// <summary>
        /// Резервирование подарка.
        /// </summary>
        public async Task ReservePresentAsync(string presentId, string reserverId, CancellationToken token = default)
        {
            AddAuthorizationHeader();

            var request = new
            {
                PresentId = presentId,
                ReserverId = reserverId
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("reserve", content, token);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(token);
                throw new Exception($"Error reserving present: {response.StatusCode} - {errorMessage}");
            }
        }
    }
}
