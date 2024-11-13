using FluentValidation;
using Rewards.DataAccess.Models;

namespace Rewards.Business.Validators
{
    public class CampaignValidator : AbstractValidator<Campaign>
    {
        public CampaignValidator() 
        {   
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(c => c.ValidFrom)
                .NotEmpty()
                .WithMessage("ValidFrom is required.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("ValidFrom must be today or a future date.");

            RuleFor(c => c.ValidTo)
                .NotEmpty()
                .WithMessage("ValidTo is required.")
                .GreaterThan(c => c.ValidFrom)
                .WithMessage("ValidTo must be later than ValidFrom.");
        }
    }
}
