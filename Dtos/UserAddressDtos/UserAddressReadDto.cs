// Dtos/UserAddressDtos/UserAddressReadDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.UserAddressDtos
{
    public class UserAddressReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullAddress { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}