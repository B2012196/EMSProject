using Blazored.LocalStorage;
using EMS.Blazor.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EMS.Blazor.Data
{
    public class InventoryOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public InventoryOrderService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<(IEnumerable<InventoryOrderModel>, string)> GetAllOrders()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7008/api/InventoryOrders");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (null, "No Content available");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var inventories = JsonSerializer.Deserialize<IEnumerable<InventoryOrderModel>>(jsonResponse);
                    return (inventories, null);
                }
                else
                {
                    return (null, "An error occurred while getting all inventories");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while getting all inventoriy orders: {ex.Message}");
                return (null, "An error occurred while getting all inventory orders.");
            }
        }

        public async Task<(string, string)> GetById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7008/api/InventoryOrders/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return (jsonResponse, null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (null, $"Không tìm thấy Inventory order với Id {id}.");
                }
                else
                {
                    return (null, "An error occurred while getting the inventory order by Id.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, "An error occurred while getting the inventory order by Id.");
            }
        }


        public async Task<(bool, string, int)> CreateNewOrder(int quantity)
        {
            try
            {
                var token = await getToken();

                if (token == "Token is not available." || token == "Username is not available")
                {
                    return (false, $"An error occurred." + token, 0);
                }
                //Thiet lap Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7008/api/InventoryOrders", quantity);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<ResponseModel>();
                    if (responseData != null)
                    {
                        return (true, "Thêm đơn hàng thành công", responseData.Id);
                    }
                    else return (false, "Error", 0);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (false, "Vui lòng đăng nhập để tiếp tục", 0);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return (false, "Không có quyền thực hiện chức năng này", 0);
                }
                else
                {
                    return (false, "Không thể thêm đơn hàng", 0);
                }
            }
            catch
            {
                return (false, "Không thể thêm đơn hàng", 0);
            }
        }

        public async Task<(bool, string)> UpdateQuatityOrder(InventoryOrderDto obj)
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
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7008/api/InventoryOrders", obj);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Updated successfully");
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
                    return (false, "Không thể thêm đơn hàng");
                }
            }
            catch
            {
                return (false, "Không thể thêm đơn hàng");
            }

        }

        public async Task<(bool, string)> DeleteOrder(int id)
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
                var response = await _httpClient.DeleteAsync($"https://localhost:7008/api/InventoryOrders/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Deleted successfully");
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
                    return (false, "Không thể xóa đơn hàng");
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
