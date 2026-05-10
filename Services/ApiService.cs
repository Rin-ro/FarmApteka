using AptekaLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Apteka.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public ApiService()
        {
            _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/api";
            _http = new HttpClient { BaseAddress = new System.Uri(_baseUrl) };
            _http.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // ==========================================
        // 🔐 АВТОРИЗАЦИЯ
        // ==========================================

        public async Task<AuthResult?> LoginAsync(string login, string password)
        {
            try
            {
                var payload = new { login, password };
                var response = await _http.PostAsJsonAsync("users/login", payload);
                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<AuthResult>()
                    : null;
            }
            catch { return null; }
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("users/register", user);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ==========================================
        // 👤 ПОЛЬЗОВАТЕЛИ
        // ==========================================

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try { return await _http.GetFromJsonAsync<User>($"users/{id}"); }
            catch { return null; }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try { return await _http.GetFromJsonAsync<List<User>>("users") ?? new List<User>(); }
            catch { return new List<User>(); }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"users/{user.Id}", user);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ==========================================
        // 📂 КАТЕГОРИИ
        // ==========================================

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try { return await _http.GetFromJsonAsync<List<Category>>("categories") ?? new List<Category>(); }
            catch { return new List<Category>(); }
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("categories", category);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"categories/{category.Id}", category);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"categories/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ==========================================
        // 💊 ЛЕКАРСТВА
        // ==========================================

        public async Task<List<Medicine>> GetMedicinesAsync()
        {
            try { return await _http.GetFromJsonAsync<List<Medicine>>("medicines") ?? new List<Medicine>(); }
            catch { return new List<Medicine>(); }
        }

        public async Task<Medicine?> GetMedicineByIdAsync(int id)
        {
            try { return await _http.GetFromJsonAsync<Medicine>($"medicines/{id}"); }
            catch { return null; }
        }

        public async Task<List<Medicine>> GetMedicinesByCategoryAsync(int categoryId)
        {
            try { return await _http.GetFromJsonAsync<List<Medicine>>($"medicines/category/{categoryId}") ?? new List<Medicine>(); }
            catch { return new List<Medicine>(); }
        }

        public async Task<List<Medicine>> SearchMedicinesAsync(string query)
        {
            try { return await _http.GetFromJsonAsync<List<Medicine>>($"medicines/search?query={query}") ?? new List<Medicine>(); }
            catch { return new List<Medicine>(); }
        }

        public async Task<List<Medicine>> GetLowStockMedicinesAsync(int threshold = 10)
        {
            try { return await _http.GetFromJsonAsync<List<Medicine>>($"medicines/low-stock?threshold={threshold}") ?? new List<Medicine>(); }
            catch { return new List<Medicine>(); }
        }

        public async Task<bool> AddMedicineAsync(Medicine medicine)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("medicines", medicine);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateMedicineAsync(Medicine medicine)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"medicines/{medicine.Id}", medicine);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteMedicineAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"medicines/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ==========================================
        // 🛒 КОРЗИНА (по ТЗ: Element of Cart)
        // ==========================================

        public async Task<List<CartItem>> GetCartAsync(int userId)
        {
            try { return await _http.GetFromJsonAsync<List<CartItem>>($"cart/{userId}") ?? new List<CartItem>(); }
            catch { return new List<CartItem>(); }
        }

        public async Task<bool> AddToCartAsync(int userId, int medicineId, int quantity = 1)
        {
            try
            {
                var payload = new { UserId = userId, MedicineId = medicineId, Quantity = quantity };
                var response = await _http.PostAsJsonAsync("cart/add", payload);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateCartItemAsync(int userId, int medicineId, int quantity)
        {
            try
            {
                var payload = new { UserId = userId, MedicineId = medicineId, Quantity = quantity };
                var response = await _http.PutAsJsonAsync("cart/update", payload);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int medicineId)
        {
            try
            {
                var response = await _http.DeleteAsync($"cart/remove/{userId}/{medicineId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            try
            {
                var response = await _http.DeleteAsync($"cart/clear/{userId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ==========================================
        // 📦 ЗАКАЗЫ
        // ==========================================

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            try { return await _http.GetFromJsonAsync<List<Order>>($"orders/user/{userId}") ?? new List<Order>(); }
            catch { return new List<Order>(); }
        }

        public async Task<OrderDetails?> GetOrderDetailsAsync(int orderId)
        {
            try { return await _http.GetFromJsonAsync<OrderDetails>($"orders/{orderId}/details"); }
            catch { return null; }
        }

        public async Task<Order?> CreateOrderAsync(CreateOrderRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("orders", request);
                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<Order>()
                    : null;
            }
            catch { return null; }
        }

        public async Task<bool> RepeatOrderAsync(int orderId)
        {
            try
            {
                var response = await _http.PostAsync($"orders/{orderId}/repeat", null);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            try
            {
                var response = await _http.DeleteAsync($"orders/{orderId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ==========================================
        // 🔔 УВЕДОМЛЕНИЯ (по ТЗ: read_date вместо is_read)
        // ==========================================

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            try
            {
                var url = unreadOnly
                    ? $"notifications/user/{userId}?unreadOnly=true"
                    : $"notifications/user/{userId}";
                return await _http.GetFromJsonAsync<List<Notification>>(url) ?? new List<Notification>();
            }
            catch { return new List<Notification>(); }
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                var response = await _http.PutAsync($"notifications/{notificationId}/read", null);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> MarkNotificationAsUnreadAsync(int notificationId)
        {
            try
            {
                var response = await _http.PutAsync($"notifications/{notificationId}/unread", null);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var response = await _http.DeleteAsync($"notifications/{notificationId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }

    // ==========================================
    // 📦 DTO (классы для передачи данных)
    // ==========================================

    public class AuthResult
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }

    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public string PaymentMethod { get; set; } = "Наличные";
        public string DeliveryMethod { get; set; } = "Самовывоз";
        public DateTime? DeliveryDate { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime DateOfOrder { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public TimeSpan? DeliveryTime { get; set; }
        public decimal Sum { get; set; }
        public List<OrderItemDetails> Items { get; set; } = new();
    }

    public class OrderItemDetails
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}