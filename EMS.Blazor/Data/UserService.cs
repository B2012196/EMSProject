using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EMS.Blazor.Data
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<(IEnumerable<UserModel>, string)> GetAllUsers()
        {

            try
            {
                var username = await _localStorage.GetItemAsync<string>("username");
                if (string.IsNullOrEmpty(username))
                {
                    throw new UnauthorizedAccessException("Username is not available");
                }

                string tokenKey = username;
                var token = await _localStorage.GetItemAsync<string>(tokenKey);

                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("Token is not available.");
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("https://localhost:7008/api/Users");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<IEnumerable<UserModel>>(jsonResponse);
                    return (users, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent){
                    return (null, "Không tồn tại users");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (null, "Đăng nhập để tiếp tục");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return (null, "Không có quyền thực hiện chức năng này");
                }
                else 
                {
                    return (null, "Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "Đăng nhập để tiếp tục");
            }
        }

        public async Task<(IEnumerable<UserModel>, string)> GetAllFullname()
        {

            try
            {
                var username = await _localStorage.GetItemAsync<string>("username");
                if (string.IsNullOrEmpty(username))
                {
                    throw new UnauthorizedAccessException("Username is not available");
                }

                string tokenKey = username;
                var token = await _localStorage.GetItemAsync<string>(tokenKey);

                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("Token is not available.");
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("https://localhost:7008/api/Users/Fullname");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<IEnumerable<UserModel>>(jsonResponse);
                    return (users, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, "Không tồn tại users");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (null, "Đăng nhập để tiếp tục");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return (null, "Không có quyền thực hiện chức năng này");
                }
                else
                {
                    return (null, "Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "Đăng nhập để tiếp tục");
            }
        }


        public async Task<(UserModel, string)> GetUserById(int id)
        {
            try
            {
                var username = await _localStorage.GetItemAsync<string>("username");
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Username is not available");
                    return (null, "Vui lòng đăng nhập để tiếp tục");
                }

                string tokenKey = username;
                var token = await _localStorage.GetItemAsync<string>(tokenKey);

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token is not available");
                    return (null, "Vui lòng đăng nhập để tiếp tục");
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Users{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserModel>(jsonResponse);
                    return (user, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, "Không tồn tại users");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (null, "Đăng nhập để tiếp tục");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return (null, "Không có quyền thực hiện chức năng này");
                }
                else
                {
                    return (null, "Error");
                }
            }
            catch (Exception ex)
            {
                return (null, "Error" + ex.ToString());
            }
        }

        public async Task<(UserModel, string)> GetUserByName()
        {

            try
            {
                var username = await _localStorage.GetItemAsync<string>("username");
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Username is not available");
                    return (null, "Không tồn tại user trong localstorage");
                }

                string tokenKey = username;
                var token = await _localStorage.GetItemAsync<string>(tokenKey);

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token is not available");
                    return (null, "Không tồn tại token trong localstorage");
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Users/username/{tokenKey}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<UserModel>(jsonResponse);
                    return (users, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, "Không tồn tại users");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (null, "Đăng nhập để tiếp tục");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return (null, "Không có quyền thực hiện chức năng này");
                }
                else
                {
                    return (null, "Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "Đăng nhập để tiếp tục");
            }
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var username = await _localStorage.GetItemAsync<string>("username");
            return !string.IsNullOrEmpty(username);
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var username = await _localStorage.GetItemAsStringAsync("username");
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Không tìm thấy tên người dùng trong Local Storage.");
                    return false;
                }
                username = username.Replace("/", "").Replace("\"", "");
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Users/Logout", username);
                if (response.IsSuccessStatusCode)
                {
                    await _localStorage.RemoveItemAsync(username);
                    await _localStorage.RemoveItemAsync("username");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Login failed: {response}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<(string, string)> GetById (int id)
        {
            try
            {
                var token = await getToken();

                if (token == "Token is not available." || token == "Username is not available")
                {
                    return (null, $"An error occurred." + token);
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy User với Id {id}.");
                }
                else
                {
                    return (null, "An error occurred while getting the User by Id.");
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
                var token = await getToken();

                if (token == "Token is not available." || token == "Username is not available")
                {
                    return (null, $"An error occurred." + token);
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Users/{field}/{value}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy User với value {value}.");
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

        public async Task<(bool, string)> CreateNewUser(UserDto obj)
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Users", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Tạo User mới thành công");
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
                    return (false, "Không thể tạo mới User");
                }
            }
            catch (Exception ex)
            {
                return (false, "Không thể thêm model" + ex.ToString());
            }
        }

        public async Task<(bool, string)> UnlockUser(int id)
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
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/Users/unlock", id);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Tạo User mới thành công");
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
                    return (false, "Không thể tạo mới User");
                }
            }
            catch
            {
                return (false, "Không thể cập nhật Location");
            }
        }


        public async Task<(bool, string)> UpdateUser(UserDto obj)
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
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/Users/update", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Update User thành công");
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
                    return (false, "Không thể update User");
                }
            }
            catch (Exception ex)
            {
                return (false, "Không thể update user" + ex.ToString());
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
