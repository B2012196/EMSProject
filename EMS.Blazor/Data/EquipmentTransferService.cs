using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;

namespace EMS.Blazor.Data
{
    public class EquipmentTransferService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public EquipmentTransferService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<List<EquipmentTransferModel>> GetAllEquipmentTransfers()
        {
            return await _httpClient.GetFromJsonAsync<List<EquipmentTransferModel>>("https://localhost:7008/api/EquipmentTransfers");
        }

        public async Task<(bool, string)> TransferEquipment(EquipmentTransferDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/EquipmentTransfers", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Luân chuyển thiết bị thành công");
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
                    return (false, "Không thể chuyển thiết bị");
                }
            }
            catch
            {
                return (false, "Không thể chuyển thiết bị");
            }
        }

        public async Task<(bool, string)> CompleteTransfer(int id)
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
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7008/api/EquipmentTransfers", id);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Nhận thiết bị thành công");
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
                    return (false, "Không thể nhận thiết bị");
                }
            }
            catch
            {
                return (false, "Không thể nhận thiết bị");
            }
        }

        public async Task<(string, string)> GetById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/EquipmentTransfers/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Equipment Transfer với Id {id}.");
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
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/EquipmentTransfers/{field}/{value}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Equipment Transfer với value {value}.");
                }
                else
                {
                    return (null, $"An error occurred while getting the Equipment Transfer by {field}.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "An error occurred while getting the equipment.");
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
