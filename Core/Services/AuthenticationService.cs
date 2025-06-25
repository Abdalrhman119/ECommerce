using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServicesAbstraction;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AuthenticationService(UserManager<ApplicationUser> _userManager,
                                         IOptions<JWTOptions> _jwtOptions,
                                         IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
            //Map From AddressDto Address
            var user = await _userManager.Users
                                       .Include(u => u.Address)
                                       .FirstOrDefaultAsync(u => u.Email == email)
                                       ?? throw new UserNotFoundException(email);

            //if (user.Address is not null)
            //throw new AddressNotFoundException(user.UserName);

            return _mapper.Map<AddressDto>(user.Address);

        }

        public async Task<UserResponse> GetUserByEmail(string email)
        {
            var user = await _userManager.Users
                             .Include(u => u.Address)
                             .FirstOrDefaultAsync(u => u.Email == email)
                             ?? throw new UserNotFoundException(email);


            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateToken(user)
            };
        }
        public async Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string email)
        {

            var user = await _userManager.Users
                           .Include(u => u.Address)
                           .FirstOrDefaultAsync(u => u.Email == email)
                           ?? throw new UserNotFoundException(email);

            if (user.Address is not null)
            {//update
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Country = addressDto.Country;
                user.Address.City = addressDto.City;
                user.Address.Street = addressDto.Street;

            }
            else//Create
            {
                user.Address = _mapper.Map<Address>(addressDto);
            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }


        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            //step 01 : Find User Using Email
            var user = await _userManager.FindByEmailAsync(loginRequest.Email) ??
                    throw new UserNotFoundException(loginRequest.Email);
            //step 02 : Check Password For This User -> Compare password with hashed password
            var isValidPass = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (isValidPass)
                return new UserResponse()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await GenerateToken(user)     //step 03 : generate token
                };

            throw new UnAuthorizedException();

        }
        public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
                DisplayName = registerRequest.DisplayName,
                PhoneNumber = registerRequest.PhoneNumber
            };

            var createdUser = await _userManager.CreateAsync(user, registerRequest.Password);
            if (createdUser.Succeeded) return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateToken(user)//Generate Token
            };
            var errors = createdUser.Errors.Select(e => e.Description).ToList();
            throw new BadRequestException(errors);
        }


        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtOpt = _jwtOptions.Value;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Name,user.UserName!),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            string secretKey = jwtOpt.SecretKey;//
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOpt.Issuer,//
                audience: jwtOpt.Audience,//
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtOpt.DurationInDays),
                signingCredentials: credintials
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }
    }
}
