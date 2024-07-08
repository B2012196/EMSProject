using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EMS.Blazor.Data
{
    public class MaintenanceScheduleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public MaintenanceScheduleService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<(string, string)> GetAllMaintenanceSchedules()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7008/api/MaintenanceSchedules");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, $"Dữ liệu rỗng");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else
                {
                    return (null, "Error.");
                }
            }
            catch
            {
                return (null, "Error.");
            }
        }

        public async Task<(string, string)> GetById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/MaintenanceSchedules/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Location với Id {id}.");
                }
                else
                {
                    return (null, "An error occurred while getting the location by Id.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "An error occurred while getting the equipment by Id.");
            }
        }

        public async Task<(string, string)> GetByFieldId(string field, string value)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/MaintenanceSchedules/{field}/{value}");
                if (response.IsSuccessStatusCode)
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
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, $"An error occurred while getting the location by {field}.");
            }
        
        }

        public async Task<(bool, string)> CreateNewMaintenance(MaintenanceScheduleDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/MaintenanceSchedules", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Thêm Maintenance Schedule mới thành công");
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
                    return (false, "Không thể thêm Maintenance Schedule");
                }
            }
            catch (Exception ex)
            {
                return (false, "Không thể thêm Maintenance Schedule" + ex.ToString());
            }
        }

        public async Task<(bool, string)> CompleteMaintenance(int id)
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
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/MaintenanceSchedules/complete", id);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Đã sửa thiết bị");
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
                    return (false, "Không thể cập nhật Maintenance Schedule");
                }
            }
            catch
            {
                return (false, "Không thể cập nhật Maintenance Schedule");
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
