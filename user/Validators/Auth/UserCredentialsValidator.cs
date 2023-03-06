using FluentValidation;
using user.Interfaces.Operations;
using user.Models.Auth;

namespace user.Validators.Auth
{
    public class UserCredentialsValidator : AbstractValidator<UserCredentialsModel>
    {
        private readonly IIdentityOperation identityOperation;

        public UserCredentialsValidator(IIdentityOperation identityOperation) 
        {
            this.identityOperation = identityOperation;

            RuleFor(model => model)
                .Custom(ValidateIdentity);
        }

        private void ValidateIdentity(UserCredentialsModel model, ValidationContext<UserCredentialsModel> context)
        {
            var identity = identityOperation.Get(model.Name, model.Password);
            if (identity == null)
                context.AddFailure(string.Empty, "Cannot find a user with specified credentials");
        }
    }
}
