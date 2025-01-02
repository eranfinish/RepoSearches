using FluentValidation;
using RepoSearches.Models.DTOs;


public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        // Validate that the username is not empty and has at least 3 characters
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .MinimumLength(3).WithMessage("User name must be at least 3 characters long.")
  .When(u => u.IsRegistering);
        // Validate that the password is not empty, has at least 6 characters, and contains uppercase, lowercase, and numbers
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
        .When(u => u.IsRegistering);

        // Validate that the email is not empty and has a valid email format
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
