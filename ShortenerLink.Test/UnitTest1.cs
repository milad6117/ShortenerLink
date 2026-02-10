using Microsoft.Extensions.Caching.Distributed;
using Moq;
using ShortenerLink.Service.Services;
using System.Text;

namespace ShortenerLink.Test
{
    public class LinkShortenerServiceTest
    {
        #region Field
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly LinkShortenerService _linkShortenerService;

        public LinkShortenerServiceTest(Mock<IDistributedCache> cacheMock, LinkShortenerService linkShortenerService)
        {
            _cacheMock = cacheMock;
            _linkShortenerService = linkShortenerService;
        }
        #endregion

        [Fact]
        public async Task CreateShortUrlAsync_ShouldReturnValidUrl()

        {
            // Arrange
            var longUrl = "https://google.com";
            var baseUrl = "http://localhost:8081";

            // Act
            var result = await _linkShortenerService.CreateShortUrlAsync(longUrl, baseUrl);

            // Assert
            Assert.StartsWith(baseUrl, result);
            Assert.True(result.Length > baseUrl.Length);

            // بررسی اینکه آیا متد Set در کش صدا زده شده است یا خیر
            _cacheMock.Verify(x => x.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                default), Times.Once);
        }


        [Fact]
        public async Task GetLongUrlAsync_WhenKeyExists_ShouldReturnUrl()
        {
            // Arrange
            var shortCode = "abc123";
            var expectedUrl = "https://google.com";
            var bytes = Encoding.UTF8.GetBytes(expectedUrl);

            _cacheMock.Setup(x => x.GetAsync(shortCode, default))
                      .ReturnsAsync(bytes);

            // Act
            var result = await _linkShortenerService.GetLongUrlAsync(shortCode);

            // Assert
            Assert.Equal(expectedUrl, result);
        }
    }
}
