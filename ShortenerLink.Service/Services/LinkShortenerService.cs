using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ShortenerLink.Service.Services
{
    public class LinkShortenerService : ILinkShortenerService
    {
        #region Field
        private readonly IDistributedCache _cache;

        public LinkShortenerService(IDistributedCache cache)
        {
            _cache = cache;
        }
        #endregion
        public async Task<string> CreateShortUrlAsync(string longUrl, string baseUrl)
        {
            // تولید کد کوتاه بر اساس هش MD5
            string shortCode = BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(longUrl)))
                                .Replace("-", "").Substring(0, 6).ToLower();

            // تنظیم زمان انقضا (مثلاً ۷ روز)
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(7));

            await _cache.SetStringAsync(shortCode, longUrl, options);

            return $"{baseUrl}/{shortCode}";
        }

        public async Task<string?> GetLongUrlAsync(string shortCode)
        {
            return await _cache.GetStringAsync(shortCode);
        }
    }
}
