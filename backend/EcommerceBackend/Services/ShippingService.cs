using EcommerceBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EcommerceBackend.Services
{
    public class ShippingService
    {
        private readonly ShopDbContext _context;
        private readonly HttpClient _httpClient;

        public ShippingService(ShopDbContext context, HttpClient httpClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public IEnumerable<Shipping> GetAllShippings() => _context.Shippings.ToList();
        public Shipping? GetShippingByOrderId(int orderId)
        {
            return _context.Shippings.SingleOrDefault(s => s.OrderId == orderId);
        }


        public void AddShipping(Shipping shipping)
        {
            _context.Shippings.Add(shipping);
            _context.SaveChanges();
        }

        public void UpdateShipping(Shipping shipping)
        {
            _context.Shippings.Update(shipping);
            _context.SaveChanges();
        }

        public void DeleteShipping(int id)
        {
            var shipping = _context.Shippings.Find(id);
            if (shipping != null)
            {
                _context.Shippings.Remove(shipping);
                _context.SaveChanges();
            }
        }

        public async Task<string> GetFedExRatesAsync(string origin, string destination)
        {
            var response = await _httpClient.GetAsync($"https://api.fedex.com/rates?origin={origin}&destination={destination}");
            return await response.Content.ReadAsStringAsync();
        }

    }
}
