using FMS.API.Entities;
using FMS.UnitTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.UnitTests;

public class AuthControllerAuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public AuthControllerAuthTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FMSDbContext>));

                    services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    services.AddDbContext<FMSDbContext>(options => options.UseInMemoryDatabase("AuthControllerAuthTests"));
                });
            });

        _client = _factory.CreateClient();
    }

    private void SeedUser(User user)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>()!;
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<FMSDbContext>()!;

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("/api/auth/account")]
    // checks with hard coded claimes
    public async Task Get_Endpoints_ReturnSuccessAndCorrectContentType(string url)
    {
        // arrange
        Guid id = new Guid("389c9463-05d4-4bb9-95c9-ccf0239c21bc");
        string email = "test1@test.com";
        string fullName = "test1";
        string password = "tDest3@!#!123";

        User newUser = new() { Id = id, Email = email, FullName = fullName };
        newUser.PasswordHash = password.HashedPassword(newUser);
        SeedUser(newUser);

        // act
        var response = await _client.GetAsync(url);

        // assert
        response.EnsureSuccessStatusCode();
    }
}
