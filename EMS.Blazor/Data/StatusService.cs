
using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;

namespace EMS.Blazor.Data
{
    public class StatusService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public StatusService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<List<StatusModel>> GetAllStatus()
        {
            return await _httpClient.GetFromJsonAsync<List<StatusModel>>("https://localhost:7008/api/Status");
        }

        public async Task<(bool, string)> CreateNewStatus(StatusDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Status", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Thêm Location mới thành công");
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
                    return (false, "Không thể thêm Location");
                }
            }
            catch (Exception ex)
            {
                return (false, "Không thể thêm model" + ex.ToString());
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
