using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EMS.Blazor.Data
{
    public class InventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public InventoryService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<(IEnumerable<InventoryModel>, string)> GetAllInventories()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7008/api/Inventories");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, "No Content available");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var inventories  = JsonSerializer.Deserialize<IEnumerable<InventoryModel>>(jsonResponse);
                    return (inventories, null);
                }
                else
                {
                    return (null, "An error occurred while getting all inventories");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while getting all inventories: {ex.Message}");
                return (null, "An error occurred while getting all inventories.");
            }
        }

        public async Task<(string, string)> GetById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Inventories/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Inventories với Id {id}.");
                }
                else
                {
                    return (null, "An error occurred while getting the inventory by Id.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "An error occurred while getting the inventory by Id.");
            }
        }

        public async Task<(string, string)> GetByFieldId(string field, string value)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Inventories/{field}/{value}");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, "Not found");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Location với value {value}.");
                }
                else
                {
                    return (null, $"An error occurred while getting the location by {field}.");
                }
            }
            catch (Exception ex ) 
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, $"An error occurred while getting the location by {field}.");
            }
        }

        public async Task<(bool, string)> CreateNewInventory(InventoryDto obj)
        {
            try
            {
                var token = await getToken();

                if (token == "Token is not available." || token == "Username is not available")
                {
                    return (false, $"An error occurred." + token);
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Inventories", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Thêm Inventory mới thành công");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (false, "Vui lòng đăng nhập để tiếp tục");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return (false, "Không có quyền thực hiện chức năng này");
                }
                else
                {
                    return (false, "Không thể thêm Inventory");
                }
            }
            catch (Exception ex)
            {
                return (false, "Không thể thêm Inventory" + ex.ToString());
            }
        }

        public async Task<string> getToken()
        {
            var username = await _localStorage.GetItemAsync<string>("username");
            if (string.IsNullOrEmpty(username))
            {
                return ("Username is not available");
            }

            string tokenKey = username;
            var token = await _localStorage.GetItemAsync<string>(tokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return ("Token is not available.");
            }
            return token;
        }
    }
}
