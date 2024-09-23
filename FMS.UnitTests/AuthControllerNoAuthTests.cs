using FluentAssertions;
using FMS.API.Entities;
using FMS.API.Models;
using FMS.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.UnitTests;

public class AuthControllerNoAuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public AuthControllerNoAuthTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FMSDbContext>));

                    services.Remove(dbContextOptions);

                    services.AddDbContext<FMSDbContext>(options => options.UseInMemoryDatabase("AuthControllerNoAuthTests"));
                });
            });

        _client = _factory.CreateClient();
    }

    private void SeedUser(User user)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<FMSDbContext>();

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("test1@test.com", "test1", "test1@!#!123")]
    [InlineData("test2@test.com", "test2", "123oLMn$")]
    public async Task Post_Login_ReturnsOk(string email, string fullName, string password)
    {
        // arrange
        User newUser = new() { Email = email, FullName = fullName };
        newUser.PasswordHash = password.HashedPassword(newUser);
        SeedUser(newUser);

        UserLoginDto userLoginDto = new() { Email = email, Password = password };
        var httpContent = userLoginDto.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/api/auth/login", httpContent);

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_LoginWithInvalidPassword_ReturnsBadRequest()
    {
        // arrange
        string email = "test3@test.com";
        string fullName = "test3";
        string password = "tDest3@!#!123";
        string anotherPassword = "asd1425";

        User newUser = new() { Email = email, FullName = fullName };
        newUser.PasswordHash = password.HashedPassword(newUser);
        SeedUser(newUser);

        UserLoginDto userLoginDto = new() { Email = email, Password = anotherPassword };
        var httpContent = userLoginDto.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/api/auth/login", httpContent);

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("test1", "")]
    [InlineData("test1@test512", "")]
    [InlineData("test1@test512.com", "")]
    [InlineData("", "test1@!#!123")]
    [InlineData("", "")]
    public async Task Post_LoginWithInvalidValues_ReturnsBadRequest(string email, string password)
    {
        // arrange
        string emailCreated = "test1@test512.com";
        string fullNameCreated = "test1";
        string passwordCreated = "tDest3@!#!123";

        User newUser = new() { Email = emailCreated, FullName = fullNameCreated };
        newUser.PasswordHash = passwordCreated.HashedPassword(newUser);
        SeedUser(newUser);

        UserLoginDto userLoginDto = new() { Email = email, Password = password };
        var httpContent = userLoginDto.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/api/auth/login", httpContent);

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("test1@test1.com", "test11", "tes5t1@!#!123")]
    [InlineData("test2@test1.com", "test21", "1523oLMn$")]
    public async Task Post_Signup_ReturnsOk(string email, string fullName, string password)
    {
        // arrange
        UserSignupDto userSignupDto = new() { Email = email, FullName = fullName, Password = password, ConfirmPassword = password };
        var httpContent = userSignupDto.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/api/auth/signup", httpContent);

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("", "test11", "tes5t1@!#!123", "tes5t1@!#!123")]
    [InlineData("test41@test1.com", "", "tes5t1@!#!123", "tes5t1@!#!123")]
    [InlineData("test41@test1.com", "test11", "", "tes5t1@!#!123")]
    [InlineData("test41@test1.com", "test11", "tes5t1@!#!123", "")]
    [InlineData("test41@test1.com", "test11", "", "")]
    [InlineData("test41@test1.com", "", "", "")]
    [InlineData("", "", "", "")]
    [InlineData("test41@test1.com", "test11", "tes5t1@!#!123", "dIFF123D1#")]
    public async Task Post_SignupWithInvalidValues_ReturnsBadRequest(string email, string fullName, string password, string confirmPassword)
    {
        // arrange
        UserSignupDto userSignupDto = new() { Email = email, FullName = fullName, Password = password, ConfirmPassword = confirmPassword };
        var httpContent = userSignupDto.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/api/auth/signup", httpContent);

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}

