using WebScrapping.Dto.Users;

namespace WebScrapping.Application.Interfaces
{
    public interface IHomeApplication
    {
        bool Login(UserDto user);
        string CreateToken(UserDto user);
    }
}
