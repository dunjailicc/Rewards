using FluentValidation;
using Rewards.DataAccess.Models;

namespace Rewards.Business.Validators
{
    public class RewardValidator : AbstractValidator<Reward>
    {
        public RewardValidator() {

            RuleFor(r => r.CampaignId)
               .NotEmpty().WithMessage("CampaignId is required.");

            RuleFor(r => r.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(r => r.ValidFrom)
                .NotEmpty()
                .WithMessage("ValidFrom is required.");

            RuleFor(r => r.ValidFrom)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("ValidFrom cannot be in the past.");

            RuleFor(r => r.ValidTo)
                .NotEmpty().WithMessage("ValidTo is required.");

            RuleFor(r => r.ValidTo)
                .GreaterThanOrEqualTo(r => r.ValidFrom)
                .WithMessage("ValidFrom must be earlier than ValidTo");

            RuleFor(r => r.DiscountPercentage)
                .NotEmpty().WithMessage("DiscountPercentage is required.");

            RuleFor(r => r.DiscountPercentage)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");
        }
    }
}
