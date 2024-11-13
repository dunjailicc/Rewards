using FluentValidation;
using Rewards.Application.Pagination;
using Rewards.Business.DTO;
using Rewards.Business.Helper;
using Rewards.DataAccess.Models;
using Rewards.DataAccess.Repositories;

namespace Rewards.Business.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IValidator<Campaign> _validator;   
        public CampaignService(ICampaignRepository campaignRepository, IValidator<Campaign> validator) {

            _campaignRepository = campaignRepository;
            _validator = validator;
        }
        public async Task<Campaign> CreateCampaignAsync(CampaignDto campaignDto)
        {   
            var campaignFromDto = Mapper.Map(campaignDto);
            var validationResult = _validator.Validate(campaignFromDto);
            if (!validationResult.IsValid)
            {
                //throw new ValidationException(validationResult.Errors);
            }
            return await _campaignRepository.CreateCampaignAsync(campaignFromDto);
        }

        public Task DeleteCampaignAsync(int campaignId)
        {
            throw new NotImplementedException();
        }

        public async Task<Campaign> GetCampaignByIdAsync(int campaignId)
        {   
            if(campaignId <= 0)
            {
                // throw new Exception()
            }
            
            var campaign = await _campaignRepository.GetCampaignByIdAsync(campaignId);

            if(campaign == null)
            {
                // throw new Exception()
            }

            return campaign;
        }

        public async Task<PaginatedResult<Campaign>> GetCampaignsAsync(DateTime? date, int? adminId, int? pageNumber, int? itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public async Task<Campaign> UpdateCampaignAsync(int campaignId, CampaignDto campaignDto)
        {
            throw new NotImplementedException();
        }
    }
}
