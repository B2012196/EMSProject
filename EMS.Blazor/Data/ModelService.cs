using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
namespace EMS.Blazor.Data
{
    public class ModelService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ModelService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<List<Models>> GetAllModels()
        {
            return await _httpClient.GetFromJsonAsync<List<Models>>("https://localhost:7008/api/Models");
        }

        public async Task<(string, string)> GetById(int id)
        {
            try
            {

                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Models/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Model với Id {id}.");
                }
                else
                {
                    return (null, $"An error occurred while getting the equipment by Id.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, $"An error occurred while getting the equipment by Id.");
            }

        }

        public async Task<(string, string)> GetByFieldId(string field, string id)
        {
            try
            {

                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Models/{field}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Model với {id}.");
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

        public async Task<(bool, string)> CreateNewModel(ModelDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Models", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Thêm model mới thành công");
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
                    return (false, "Không thể thêm model");
                }
            }
            catch
            {
                return (false, "Không thể thêm model");
            }
        }

        public async Task<(bool, string)> UpdateModel(Models obj)
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
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/Models", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Cập nhật model thành công");
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
                    return (false, "Không thể thêm model");
                }
            }
            catch
            {
                return (false, "Không thể thêm model");
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
