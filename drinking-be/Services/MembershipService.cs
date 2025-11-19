// Services/MembershipService.cs
using AutoMapper;
using drinking_be.Dtos.MembershipDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.MembershipInterfaces;
using drinking_be.Models;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepo;
        private readonly IMapper _mapper;

        public MembershipService(IMembershipRepository membershipRepo, IMapper mapper)
        {
            _membershipRepo = membershipRepo;
            _mapper = mapper;
        }

        public async Task<MembershipReadDto?> GetMyMembershipAsync(int userId)
        {
            var membership = await _membershipRepo.GetByUserIdWithLevelAsync(userId);

            if (membership == null)
            {
                // Có thể xảy ra nếu user được tạo trước khi logic này được thêm vào
                return null;
            }

            return _mapper.Map<MembershipReadDto>(membership);
        }
    }
}