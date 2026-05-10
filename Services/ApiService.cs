using AptekaLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Apteka.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new TimeSpanConverter() }
        };

        public ApiService()
        {
            string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/api/";
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _http.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // ────────────── Вспомогательные методы ──────────────
        private async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
                var response = await _http.GetAsync(url);
                if (!response.IsSuccessStatusCode) return new List<T>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
            }
            catch { return new List<T>(); }
        }

        private async Task<bool> DeleteAsync(string url)
        {
            try { var r = await _http.DeleteAsync(url); return r.IsSuccessStatusCode; }
            catch { return false; }
        }

        // ────────────── ПОЛЬЗОВАТЕЛИ ──────────────
        public Task<List<User>> GetUsersAsync() => GetListAsync<User>("users");
        public async Task<User?> GetUserByIdAsync(int id) { try { return await _http.GetFromJsonAsync<User>($"users/{id}", _jsonOptions); } catch { return null; } }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                var payload = new { login = user.Login, password = user.PasswordHash, emailOrPhone = user.EmailOrPhone, fio = user.FIO };
                var resp = await _http.PostAsJsonAsync("users/register", payload);
                return resp.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var payload = new { fio = user.FIO, emailOrPhone = user.EmailOrPhone, newPassword = user.PasswordHash };
                var resp = await _http.PutAsJsonAsync($"users/{user.Id}", payload);
                return resp.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public Task<bool> DeleteUserAsync(int id) => DeleteAsync($"users/{id}");

        // ────────────── КАТЕГОРИИ ──────────────
        public Task<List<Category>> GetCategoriesAsync() => GetListAsync<Category>("categories");
        public async Task<bool> AddCategoryAsync(Category cat) { try { var r = await _http.PostAsJsonAsync("categories", cat); return r.IsSuccessStatusCode; } catch { return false; } }
        public async Task<bool> UpdateCategoryAsync(Category cat) { try { var r = await _http.PutAsJsonAsync($"categories/{cat.Id}", cat); return r.IsSuccessStatusCode; } catch { return false; } }
        public Task<bool> DeleteCategoryAsync(int id) => DeleteAsync($"categories/{id}");

        // ────────────── ЛЕКАРСТВА ──────────────
        public Task<List<Medicine>> GetMedicinesAsync() => GetListAsync<Medicine>("medicines");
        public async Task<bool> AddMedicineAsync(Medicine med) { try { var r = await _http.PostAsJsonAsync("medicines", med); return r.IsSuccessStatusCode; } catch { return false; } }
        public async Task<bool> UpdateMedicineAsync(Medicine med) { try { var r = await _http.PutAsJsonAsync($"medicines/{med.Id}", med); return r.IsSuccessStatusCode; } catch { return false; } }
        public Task<bool> DeleteMedicineAsync(int id) => DeleteAsync($"medicines/{id}");

        // ────────────── ЗАКАЗЫ ──────────────
        public Task<List<Order>> GetAllOrdersAsync() => GetListAsync<Order>("orders");
        public Task<List<Order>> GetUserOrdersAsync(int userId) => GetListAsync<Order>($"orders/user/{userId}");
        public async Task<Order?> CreateOrderAsync(CreateOrderRequest req) { try { var r = await _http.PostAsJsonAsync("orders", req); return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<Order>(_jsonOptions) : null; } catch { return null; } }
        public Task<bool> CancelOrderAsync(int id) => DeleteAsync($"orders/{id}");
        public async Task<bool> UpdateOrderStatusAsync(int id, string status) { try { var r = await _http.PutAsJsonAsync($"orders/{id}/status", new { status }); return r.IsSuccessStatusCode; } catch { return false; } }

        // ────────────── ПОЗИЦИИ ЗАКАЗОВ ──────────────
        public Task<List<OrderPosition>> GetOrderPositionsAsync() => GetListAsync<OrderPosition>("orderpositions");
        public async Task<bool> AddOrderPositionAsync(OrderPosition pos) { try { var r = await _http.PostAsJsonAsync("orderpositions", pos); return r.IsSuccessStatusCode; } catch { return false; } }
        public async Task<bool> UpdateOrderPositionAsync(OrderPosition pos) { try { var r = await _http.PutAsJsonAsync($"orderpositions/{pos.Id}", pos); return r.IsSuccessStatusCode; } catch { return false; } }
        public Task<bool> DeleteOrderPositionAsync(int id) => DeleteAsync($"orderpositions/{id}");

        // ────────────── УВЕДОМЛЕНИЯ ──────────────
        public Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false) =>
            GetListAsync<Notification>($"notifications/user/{userId}?unreadOnly={unreadOnly}");
        public async Task<bool> AddNotificationAsync(Notification n) { try { var r = await _http.PostAsJsonAsync("notifications", n); return r.IsSuccessStatusCode; } catch { return false; } }
        public async Task<bool> UpdateNotificationAsync(Notification n) { try { var r = await _http.PutAsJsonAsync($"notifications/{n.Id}", n); return r.IsSuccessStatusCode; } catch { return false; } }
        public Task<bool> DeleteNotificationAsync(int id) => DeleteAsync($"notifications/{id}");

        public async Task<bool> MarkNotificationReadAsync(int id) { try { var r = await _http.PutAsync($"notifications/{id}/read", null); return r.IsSuccessStatusCode; } catch { return false; } }
        public async Task<bool> MarkNotificationUnreadAsync(int id) { try { var r = await _http.PutAsync($"notifications/{id}/unread", null); return r.IsSuccessStatusCode; } catch { return false; } }
    }

    public class TimeSpanConverter : System.Text.Json.Serialization.JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => TimeSpan.Parse(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
    }

    public class CreateOrderRequest { public int UserId; public string PaymentMethod = "Наличные"; public string DeliveryMethod = "Самовывоз"; public DateTime? DeliveryDate; public List<OrderItemRequest> Items = new(); }
    public class OrderItemRequest { public int MedicineId; public int Quantity; }
}