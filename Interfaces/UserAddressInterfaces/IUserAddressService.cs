// Interfaces/IUserAddressService.cs
using drinking_be.Dtos.UserAddressDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IUserAddressService
    {
        Task<IEnumerable<UserAddressReadDto>> GetAllMyAddressesAsync(int userId);
        Task<UserAddressReadDto> CreateAddressAsync(int userId, UserAddressCreateDto dto);
        Task<UserAddressReadDto?> UpdateAddressAsync(int addressId, int userId, UserAddressCreateDto dto);
        Task<bool> DeleteAddressAsync(int addressId, int userId);
    }
}