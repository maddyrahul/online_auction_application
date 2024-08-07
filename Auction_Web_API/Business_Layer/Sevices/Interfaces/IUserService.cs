using Data_Access_Layer.DTOs;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Sevices.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<string> RegisterAsync(UserDto userRegisterDto);
        Task<string> AuthenticateAsync(LoginDto userLoginDto);
        Task<UserDto> UpdateUser(int id, UserDto userDto);
        Task DeleteUser(int id);

       
    }
}
