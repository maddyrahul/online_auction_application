using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(int id);
        Task<bool> EmailExists(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByEmailAndPassword(string email, string password);

        Task AddUserAsync(User user);
    }
}
