using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace EMS.Blazor.Data
{
    public class UsageHistoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public UsageHistoryService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<List<UsageHistoryModel>> GetAllUsageHistories()
        {
            return await _httpClient.GetFromJsonAsync<List<UsageHistoryModel>>("https://localhost:7008/api/UsageHistories");
        }

        public async Task<(string, string)> GetByFieldId(string field, string name)
        {
            try
            {
             
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/UsageHistories/{field}/{name}");         
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"No Usage Histories found with {name}.");
                }
                else
                {
                    return (null, $"An error occurred while getting the equipment by {field}.");
                }

            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, $"An error occurred while getting the equipment by {field}.");
            }

        }


        public async Task<(bool, string)> CreateNewUsageHistory(UsageHistoryDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/UsageHistories", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Sử dụng thành công");
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
                    return (false, "Không thể sử dụng thiết bị");
                }
            }
            catch
            {
                return (false, "Không thể sử dụng thiết bị");
            }
        }

        public async Task<(bool, string)> CompleteUsageHistory(int id)
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
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7008/api/UsageHistories/complete", id);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Ngừng sử dụng thành công");
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
                    return (false, "Không thể ngừng sử dụng thiết bị");
                }
            }
            catch
            {
                return (false, "Không thể sử dụng thiết bị");
            }
        }
       
        public async Task<(string, string)> GetEquipmentUsageStatistics()
        {
            try
            {

                var response = await _httpClient.GetAsync($"https://localhost:7008/api/UsageHistories/equipment-usage-statistics");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"No Usage Histories found.");
                }
                else
                {
                    return (null, $"Error");
                }

            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, $"Error");
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
