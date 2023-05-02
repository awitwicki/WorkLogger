using WorkLogger.Domain.ViewModels;

namespace WorkLogger.Services.Services;

public interface IUsersService
{
    public Task<List<UserViewModel>> GetUsers();
}
