using FluentValidation;
using Rewards.Application.DTO;
using Rewards.Application.Interfaces;
using Rewards.Application.Pagination;
using Rewards.Business.Exceptions;
using Rewards.Business.Helper;
using Rewards.DataAccess.Models;
using Rewards.DataAccess.Repositories;

namespace Rewards.Business.Services
{
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _rewardRepository;
        private readonly IValidator<Reward> _validator;
        private readonly ICampaignService _campaignService;

        public RewardService(IRewardRepository rewardRepository, IValidator<Reward> validator, ICampaignService campaignService)
        {
            _rewardRepository = rewardRepository;
            _validator = validator;
            _campaignService = campaignService;
        }

        public async Task<Reward> CreateRewardAsync(RewardDto rewardDto) {

            var rewardFromDto = Mapper.Map(rewardDto);
            
            var validationResult = _validator.Validate(rewardFromDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("Input is not valid.");
            }

            var campaignById = await _campaignService.GetCampaignByIdAsync(rewardFromDto.Campaign.Id);

            if(campaignById is null) 
            {
                throw new NotFoundException("Campaign not found.");
            }

            return await _rewardRepository.CreateRewardAsync(rewardFromDto);
        }

        public async Task<PaginatedResult<Reward>> GetRewardsAsync(DateTime? date, int? agentId, int? pageNumber, int? itemsPerPage)
        {
            var rewards = await _rewardRepository.GetRewardsAsync(date, agentId, pageNumber, itemsPerPage);
            if(rewards is null)
            {
                throw new NotFoundException("Rewards not found.");
            }
            return rewards;
        }

        public async Task<Reward> UpdateRewardAsync(int id, RewardDto rewardDto)
        {
            var existingReward = await _rewardRepository.GetRewardByIdAsync(id);
            
            if (existingReward is null)
                throw new NotFoundException("Reward not found.");
            
            var rewardFromDto = Mapper.Map(rewardDto);
            var updatedReward = await _rewardRepository.UpdateRewardAsync(id, rewardFromDto);

            return updatedReward;
        }

        public async Task DeleteRewardAsync(int rewardId)
        {
            var existingReward = await _rewardRepository.GetRewardByIdAsync(rewardId);

            if (existingReward is null)
                throw new NotFoundException("Reward not found.");

            await _rewardRepository.DeleteRewardAsync(rewardId);
        }

    }
}
