using Business_Layer.Sevices.Interfaces;
using Data_Access_Layer.DTOs;
using Data_Access_Layer.Models;
using Data_Access_Layer.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business_Layer.Sevices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration= configuration;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return user != null ? MapToDto(user) : null;
        }

     

        public async Task<UserDto> UpdateUser(int id, UserDto userDto)
        {
            var existingUser = await _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }

            existingUser.Username = userDto.Username;
            existingUser.Email = userDto.Email;
            existingUser.Password = userDto.Password;
            existingUser.Role = userDto.Role;
            existingUser.IsBanned = userDto.IsBanned;

            var updatedUser = await _userRepository.UpdateUser(existingUser);
            return MapToDto(updatedUser);
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                IsBanned = user.IsBanned,
            };
        }


        public async Task<string> RegisterAsync(UserDto userRegisterDto)
        {
            // Check if a user with the same email already exists
            if (await _userRepository.GetUserByEmailAsync(userRegisterDto.Email) != null)
            {
                return null; // User with the same email already exists
            }

            string hashedPassword = HashPassword(userRegisterDto.Password);

            // Create a new user entity with the provided information
            User newUser = new User
            {
                
                Email = userRegisterDto.Email,
                Password = hashedPassword,
                Username = userRegisterDto.Username,
                Role = userRegisterDto.Role,
                IsBanned=userRegisterDto.IsBanned,
            };

            // Save the new user to the database
            await _userRepository.AddUserAsync(newUser);

            // Generate a JWT token for the newly registered user
            return GenerateJwtToken(newUser);
        }

        /*public async Task<string> AuthenticateAsync(LoginDto userLoginDto)
        {
            // Validate user credentials (e.g., check username and password against the database)
            User user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email);

            if (user == null || !VerifyPassword(userLoginDto.Password, user.Password))
            {
                return null; // Invalid email or password
            }

            return GenerateJwtToken(user);
        }*/

        public async Task<string> AuthenticateAsync(LoginDto userLoginDto)
        {
            // Validate user credentials (e.g., check username and password against the database)
            User user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email);

            if (user == null || !VerifyPassword(userLoginDto.Password, user.Password))
            {
                return null; // Invalid email or password
            }

            if (user.IsBanned)
            {
                throw new Exception("User is banned.");
            }

            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(User user)
        {
            var secret = _configuration["JWT:Secret"];
            var issuer = _configuration["JWT:ValidIssuer"];
            var audience = _configuration["JWT:ValidAudience"];

            // Log or debug these values to ensure they are not null
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new Exception("JWT configuration values are not set properly.");
            }

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1), // Adjust expiration time as needed
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }


    }
}
