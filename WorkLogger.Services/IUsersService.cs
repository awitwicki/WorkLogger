﻿using Microsoft.AspNetCore.Identity;
using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services.Services;

public interface IUsersService
{
    Task<List<UserViewModel>> GetUsers();
    Task AddRoleToUser(string userId, string role);
    Task<IEnumerable<string>> GetUserRoles(string userId);
    Task CleanUserRoles(string userId);
    Task<IEnumerable<IdentityRole>> GetAllRoles();
}