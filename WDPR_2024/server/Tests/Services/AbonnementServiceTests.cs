using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;
using Xunit;

namespace WDPR_2024.server.Tests.Services
{
    public class AbonnementServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly AbonnementService _service;

        public AbonnementServiceTests()
        {
            var options = new DbContextOptions<AppDbContext>();
            _mockContext = new Mock<AppDbContext>(options);
            _service = new AbonnementService(_mockContext.Object);
        }

        [Fact]
        public async Task HaalAbonnementOpViaIdAsync_MoetAbonnementTeruggeven_AlsAbonnementBestaat()
        {
            // Arrange
            var abonnementId = 1;
            var verwachtAbonnement = new Abonnement { AbonnementID = abonnementId };
            var mockSet = new Mock<DbSet<Abonnement>>();
            mockSet.Setup(m => m.FindAsync(abonnementId)).ReturnsAsync(verwachtAbonnement);
            _mockContext.Setup(c => c.Abonnementen).Returns(mockSet.Object);

            // Act
            var result = await _service.GetAbonnementByIdAsync(abonnementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(verwachtAbonnement, result);
        }

        [Fact]
        public async Task HaalAbonnementOpViaIdAsync_MoetNullTeruggeven_AlsAbonnementNietBestaat()
        {
            // Arrange
            var abonnementId = 1;
            var mockSet = new Mock<DbSet<Abonnement>>();
            mockSet.Setup(m => m.FindAsync(abonnementId)).ReturnsAsync((Abonnement)null);
            _mockContext.Setup(c => c.Abonnementen).Returns(mockSet.Object);

            // Act
            var result = await _service.GetAbonnementByIdAsync(abonnementId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task VoegAbonnementToeAsync_MoetContextAddMethodeAanroepen()
        {
            // Arrange
            var nieuwAbonnement = new Abonnement { AbonnementID = 1 };
            var mockSet = new Mock<DbSet<Abonnement>>();
            _mockContext.Setup(c => c.Abonnementen).Returns(mockSet.Object);

            // Act
            await _service.AddAbonnementAsync(nieuwAbonnement);

            // Assert
            mockSet.Verify(m => m.AddAsync(nieuwAbonnement, default), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task HaalAlleAbonnementenOpAsync_MoetAlleAbonnementenTeruggeven()
        {
            // Arrange
            var verwachteAbonnementen = new List<Abonnement>
            {
                new Abonnement { AbonnementID = 1 },
                new Abonnement { AbonnementID = 2 }
            };
            var mockSet = new Mock<DbSet<Abonnement>>();
            mockSet.As<IQueryable<Abonnement>>().Setup(m => m.Provider).Returns(verwachteAbonnementen.AsQueryable().Provider);
            mockSet.As<IQueryable<Abonnement>>().Setup(m => m.Expression).Returns(verwachteAbonnementen.AsQueryable().Expression);
            mockSet.As<IQueryable<Abonnement>>().Setup(m => m.ElementType).Returns(verwachteAbonnementen.AsQueryable().ElementType);
            mockSet.As<IQueryable<Abonnement>>().Setup(m => m.GetEnumerator()).Returns(verwachteAbonnementen.AsQueryable().GetEnumerator());
            _mockContext.Setup(c => c.Abonnementen).Returns(mockSet.Object);

            // Act
            var result = await _service.GetAlleAbonnementenAsync();

            // Assert
            Assert.Equal(verwachteAbonnementen.Count, result.Count);
            Assert.Equal(verwachteAbonnementen, result);
        }
    }
}
