using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenerLink.Service.Services
{
    public interface ILinkShortenerService
    {
        Task<string> CreateShortUrlAsync(string longUrl, string baseUrl);
        Task<string?> GetLongUrlAsync(string shortCode);
    }
}
