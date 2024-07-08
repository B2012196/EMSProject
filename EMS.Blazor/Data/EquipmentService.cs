using EMS.Blazor.Model;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace EMS.Blazor.Data
{
    public class EquipmentService
    {

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public EquipmentService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<(IEnumerable<Equipment>, string)> GetAllEquipments()
        {
            try
            {
                var response =  await _httpClient.GetAsync("https://localhost:7008/api/Equipments");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, "No Content available");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var equipments = JsonSerializer.Deserialize<IEnumerable<Equipment>>(jsonResponse);
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

        public async Task<Equipment> GetById(int id)
        {
            try
            {
                var username = await _localStorage.GetItemAsync<string>("username");
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Username is not available");
                    return null;
                }

                string tokenKey = username;
                var token = await _localStorage.GetItemAsync<string>(tokenKey);

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token is not available.");
                    return null;
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine(id);
                var equipment = await _httpClient.GetFromJsonAsync<Equipment>($"https://localhost:7008/api/Equipments/{id}");
                return equipment;
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi HTTP
                Console.WriteLine($"An error occurred while sending the request: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }

        public async Task<(string, string)> GetByFieldId(string field, string id)
        {
            try
            {
                
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/Equipments/{field}/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"No equipment found with {field} ID {id}."); 
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

        public async Task<string> CreateNewEquipment(EquipmentDto obj)
        {
            try
            {
                var token = await getToken();
                
                if (token == "Token is not available." || token == "Username is not available")
                {
                    return ($"An error occurred." + token);
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/Equipments", obj);
                if(response.IsSuccessStatusCode)
                {
                    return "Thêm thiết bị mới thành công";
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "Vui lòng đăng nhập để tiếp tục";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return "Không có quyền thực hiện chức năng này";
                }
                else
                {
                    return "Không thể tạo thiết bị";
                }
            }
            catch 
            {
                return "Không thể tạo thiết bị";
            }
        }

        public async Task<string> UpdateEquipment(EquipmentDtoUser obj)
        {
            try
            {
                var token = await getToken();

                if (token == "Token is not available." || token == "Username is not available")
                {
                    return ($"An error occurred." + token);
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/Equipments/Update", obj);
                if (response.IsSuccessStatusCode)
                {
                    return "Cập nhật thiết bị thành công";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "Vui lòng đăng nhập để tiếp tục";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return "Không có quyền thực hiện chức năng này";
                }
                else
                {
                    return "Không thể tạo thiết bị";
                }
            }
            catch
            {
                return "Không thể cập nhật thiết bị";
            }
        }

        public async Task<(bool,string)> DeleteEquipment(int Id)
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
                var response = await _httpClient.DeleteAsync($"https://localhost:7008/api/Equipments/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Deleted Successfully");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (false, $"No equipment found with ID {Id}.");
                }
                else
                {
                    return (false, $"An error occurred while delete the equipment by {Id}.");
                }
            }
            catch
            {
                return (false, $"An error occurred while delete the equipment by {Id}.");
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
                return("Token is not available.");
            }
            return token;
        }

    }
}
