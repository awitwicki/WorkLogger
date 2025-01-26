using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.ViewModels;
using WorkLogger.Services;

namespace WorkLogger.Api.Controllers.Admin;

[Route("[controller]")]
[Authorize(Roles = ApplicationRoles.Admin)]
public class UserManageController: ControllerBase
{
    private readonly IUsersService _usersService;

    public UserManageController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _usersService.GetUsers();
        
        return Ok(result);
    }
    
    [HttpPost]
    [Route("AddRoleToUser")]
    public async Task<IActionResult> AddRoleToUser(Guid userId, string roleName)
    {
        await _usersService.AddRoleToUser(userId, roleName);
        
        return Ok();
    }
    
    [HttpPost]
    [Route("CleanUserRoles")]
    public async Task<IActionResult> CleanUserRoles(Guid userId)
    {
        await _usersService.CleanUserRoles(userId);
        
        return Ok();
    }
    
    [HttpPost]
    [Route("RemoveUser")]
    public async Task<IActionResult> RemoveUser(Guid userId)
    {
        await _usersService.RemoveUser(userId);
        
        return Ok();
    }
}
