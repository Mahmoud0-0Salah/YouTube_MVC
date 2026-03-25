using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class SubscribeValidator : AbstractValidator<SubscribeViewModel>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubscribeValidator(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Channel user ID is required.")
            .Must(NotBeSelf).WithMessage("You cannot subscribe to yourself.");
    }

    private bool NotBeSelf(string userId)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
        return currentUserId != userId;
    }
}