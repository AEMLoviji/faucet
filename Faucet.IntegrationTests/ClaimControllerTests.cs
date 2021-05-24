using Faucet.API;
using Faucet.API.Data;
using Faucet.API.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Faucet.IntegrationTests
{
    public class ClaimControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly IBalanceRepository _balanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        private string RandomEmail => $"{Guid.NewGuid()}@gmail.com";

        public ClaimControllerTests(WebApplicationFactory<Startup> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

            _client = factory.CreateClient();

            _balanceRepository = (IBalanceRepository)factory.Services.GetService(typeof(IBalanceRepository));
            _unitOfWork = (IUnitOfWork)factory.Services.GetService(typeof(IUnitOfWork));
        }

        [Fact]
        public async Task Claim_WithoutEmail_ReturnsBadRequest()
        {
            //Act
            var httpResponse = await _client.PostAsync("/api/claim", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Claim_WhenBtcIsNotAvailable_ReturnsForbidded()
        {
            //Arrange 
            await SetZeroBalance();

            //Act
            var httpResponse = await _client.PostAsync("/api/claim?email=test@mail.com", null);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Claim_MultipleTimeWithSameEmail_ReturnsTooManyRequests()
        {
            //Arrange      
            await AddBtcToStock();
            var email = RandomEmail;

            //Act
            var httpResponseFirst = await _client.PostAsync($"/api/claim?email={email}", null);
            var httpResponseSecond = await _client.PostAsync($"/api/claim?email={email}", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseFirst.StatusCode);
            Assert.Equal(HttpStatusCode.TooManyRequests, httpResponseSecond.StatusCode);
        }

        [Fact]
        public async Task Claim_WithEmail_ReturnsOk()
        {
            //Arrange          
            await AddBtcToStock();

            //Act
            var httpResponse = await _client.PostAsync($"/api/claim?email={RandomEmail}", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        private async Task AddBtcToStock()
        {
            _balanceRepository.Change(1); //Add BTC to stock
            await _unitOfWork.Commit();
        }

        private async Task SetZeroBalance()
        {
            var currentBalance = _balanceRepository.Get().BitcoinsAmount;
            _balanceRepository.Change(-1 * currentBalance);
            await _unitOfWork.Commit();
        }
    }
}
