using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;
using Xunit;

namespace WDPR_2024.server.Tests.Services
{
    public class VerhuurAanvraagServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly VerhuurAanvraagService _service;

        public VerhuurAanvraagServiceTests()
        {
            var options = new DbContextOptions<AppDbContext>();
            _mockContext = new Mock<AppDbContext>(options);
            _service = new VerhuurAanvraagService(_mockContext.Object);
        }

        [Fact]
        public async Task GetAanvraagByIdAsync_MoetAanvraagTeruggeven_AlsAanvraagBestaat()
        {
            // Arrange
            var aanvraagId = 1;
            var verwachtAanvraag = new VerhuurAanvraag { VerhuurAanvraagID = aanvraagId };
            var mockSet = new Mock<DbSet<VerhuurAanvraag>>();
            mockSet.Setup(m => m.FindAsync(aanvraagId)).ReturnsAsync(verwachtAanvraag);
            _mockContext.Setup(c => c.VerhuurAanvragen).Returns(mockSet.Object);

            // Act
            var result = await _service.GetAanvraagByIdAsync(aanvraagId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(verwachtAanvraag, result);
        }

        [Fact]
        public async Task GetAanvraagByIdAsync_MoetNullTeruggeven_AlsAanvraagNietBestaat()
        {
            // Arrange
            var aanvraagId = 1;
            var mockSet = new Mock<DbSet<VerhuurAanvraag>>();
            mockSet.Setup(m => m.FindAsync(aanvraagId)).ReturnsAsync((VerhuurAanvraag)null);
            _mockContext.Setup(c => c.VerhuurAanvragen).Returns(mockSet.Object);

            // Act
            var result = await _service.GetAanvraagByIdAsync(aanvraagId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAanvraagAsync_MoetContextAddMethodeAanroepen_AlsVoertuigBeschikbaarIs()
        {
            // Arrange
            var nieuwAanvraag = new VerhuurAanvraag { VerhuurAanvraagID = 1, VoertuigID = 1, Status = "In Behandeling" };
            var mockSet = new Mock<DbSet<VerhuurAanvraag>>();
            var mockVoertuig = new Voertuig { VoertuigID = 1, Status = "Beschikbaar" };
            var mockVoertuigSet = new Mock<DbSet<Voertuig>>();
            _mockContext.Setup(c => c.VerhuurAanvragen).Returns(mockSet.Object);
            _mockContext.Setup(c => c.Voertuigen).Returns(mockVoertuigSet.Object);
            mockVoertuigSet.Setup(m => m.FindAsync(1)).ReturnsAsync(mockVoertuig);

            // Act
            await _service.AddAanvraagAsync(nieuwAanvraag);

            // Assert
            mockSet.Verify(m => m.AddAsync(nieuwAanvraag, default), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AddAanvraagAsync_MoetExceptionWerpen_AlsVoertuigNietBeschikbaarIs()
        {
            // Arrange
            var nieuwAanvraag = new VerhuurAanvraag { VerhuurAanvraagID = 1, VoertuigID = 1, Status = "In Behandeling" };
            var mockSet = new Mock<DbSet<VerhuurAanvraag>>();
            var mockVoertuig = new Voertuig { VoertuigID = 1, Status = "In Behandeling" };
            var mockVoertuigSet = new Mock<DbSet<Voertuig>>();
            _mockContext.Setup(c => c.VerhuurAanvragen).Returns(mockSet.Object);
            _mockContext.Setup(c => c.Voertuigen).Returns(mockVoertuigSet.Object);
            mockVoertuigSet.Setup(m => m.FindAsync(1)).ReturnsAsync(mockVoertuig);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await _service.AddAanvraagAsync(nieuwAanvraag));
        }

        [Fact]
        public async Task GetAanvragenByStatusAsync_MoetGefilterdeAanvragenTeruggeven_AlsValidStatus()
        {
            // Arrange
            var aanvragen = new List<VerhuurAanvraag>
            {
                new VerhuurAanvraag { VerhuurAanvraagID = 1, Status = "Goedgekeurd" },
                new VerhuurAanvraag { VerhuurAanvraagID = 2, Status = "In Behandeling" }
            };
            var mockSet = new Mock<DbSet<VerhuurAanvraag>>();
            mockSet.As<IQueryable<VerhuurAanvraag>>().Setup(m => m.Provider).Returns(aanvragen.AsQueryable().Provider);
            mockSet.As<IQueryable<VerhuurAanvraag>>().Setup(m => m.Expression).Returns(aanvragen.AsQueryable().Expression);
            mockSet.As<IQueryable<VerhuurAanvraag>>().Setup(m => m.ElementType).Returns(aanvragen.AsQueryable().ElementType);
            mockSet.As<IQueryable<VerhuurAanvraag>>().Setup(m => m.GetEnumerator()).Returns(aanvragen.AsQueryable().GetEnumerator());
            _mockContext.Setup(c => c.VerhuurAanvragen).Returns(mockSet.Object);

            // Act
            var result = await _service.GetAanvragenByStatusAsync("Goedgekeurd");

            // Assert
            Assert.Single(result);
            Assert.Equal("Goedgekeurd", result.First().Status);
        }

        [Fact]
        public async Task GetBeschikbareVoertuigenAsync_MoetBeschikbareVoertuigenTeruggeven()
        {
            // Arrange
            var voertuigen = new List<Voertuig>
            {
                new Voertuig { VoertuigID = 1, Status = "Beschikbaar" },
                new Voertuig { VoertuigID = 2, Status = "In Behandeling" }
            };
            var aanvragen = new List<VerhuurAanvraag>
            {
                new VerhuurAanvraag { VoertuigID = 1, Status = "Verhuurd", StartDatum = DateTime.Now.AddDays(-1), EindDatum = DateTime.Now.AddDays(1) }
            };
            var mockVoertuigenSet = new Mock<DbSet<Voertuig>>();
            var mockAanvragenSet = new Mock<DbSet<VerhuurAanvraag>>();
            _mockContext.Setup(c => c.Voertuigen).Returns(mockVoertuigenSet.Object);
            _mockContext.Setup(c => c.VerhuurAanvragen).Returns(mockAanvragenSet.Object);
            mockVoertuigenSet.Setup(m => m.Where(It.IsAny<Func<Voertuig, bool>>())).Returns(voertuigen.AsQueryable());
            mockAanvragenSet.Setup(m => m.Where(It.IsAny<Func<VerhuurAanvraag, bool>>())).Returns(aanvragen.AsQueryable());

            // Act
            var result = await _service.GetBeschikbareVoertuigenAsync("SUV", DateTime.Now, DateTime.Now.AddDays(2));

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result.First().VoertuigID); // Voertuig 2 is beschikbaar
        }
    }
}
