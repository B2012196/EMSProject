using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EMS.Blazor.Data
{
    public class ReplacementRecordService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ReplacementRecordService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<(IEnumerable<ReplacementRecordModel>, string)> GetAllEquipments()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7008/api/ReplacementRecords");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, "No Content available");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var equipments = JsonSerializer.Deserialize<IEnumerable<ReplacementRecordModel>>(jsonResponse);
                    return (equipments, null);
                }
                else
                {
                    return (null, "An error occurred while getting all equpments");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while getting all equipment: {ex.Message}");
                return (null, "An error occurred while getting all equipment.");
            }
        }


        public async Task<(bool, string)> CreateReplaceRecord(ReplacementRecordDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/ReplacementRecords", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Thêm Replacement Record thành công");
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
                    return (false, "Số lượng phụ tùng trong kho không đủ");
                }
            }
            catch
            {
                return (false, "Error");
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
