using EMS.Blazor.Model;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
namespace EMS.Blazor.Data
{
    public class ManufacturerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public ManufacturerService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<Manufacturers>> GetAllManufacturers()
        {
            return await _httpClient.GetFromJsonAsync<List<Manufacturers>>("https://localhost:7008/api/Manufacturers");
        }

        public async Task<(string, string)> GetById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Manufacturers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Manufacturer với Id {id}.");
                }
                else
                {
                    return (null, "An error occurred while getting the manufacturer by Id.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "An error occurred while getting the manufacturer by Id.");
            }

        }

        public async Task<(string, string)> GetByFieldValue(string field, string value)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Manufacturers/{field}/{value}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Manufacturer với {value}.");
                }
                else
                {
                    return (null, $"An error occurred while getting the manufacturer by {field}.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, $"An error occurred while getting the manufacturer by {field}.");
            }
        }

        public async Task<(bool, string)> CreateNewManufacturer(ManufacturerDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Manufacturers", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Thêm nhà sản xuât thành công");
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
                    return (false, "Không thể thêm Nhà sản xuất");
                }
            }
            catch
            {
                return (false, "Không thể thêm Nhà sản xuất");
            }
        }

        public async Task<(bool, string)> UpdateManufacturer(Manufacturers obj)
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
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/Manufacturers", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Cập nhật Manufacturer thành công");
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
                    return (false, "Không thể cập nhật Manufacturer");
                }
            }
            catch
            {
                return (false, "Không thể cập nhật Manufacturer");
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
