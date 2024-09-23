using FMS.API.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.UnitTests.Helpers;

public static class PasswordHasherHelper
{
    public static string HashedPassword(this object obj, User newUser)
    {
        var _passwordHasher = new PasswordHasher<User>();
        var hashedPasword = _passwordHasher.HashPassword(newUser, (string)obj);

        return hashedPasword;
    }
}