using FluentValidation;
using user.Interfaces.Repositories;
using user.Models.User;

namespace user.Validators.User
{
    public class UserCreateValidator : AbstractValidator<UserCreateModel>
    {
        private readonly IUserRepository userRepository;

        private readonly IRoleRepository roleRepository;

        public UserCreateValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;

            RuleFor(model => model)
                .Custom(CheckDuplicates)
                .Custom(ValidateRoleCode)
                .Custom(ValidatePassword);
        }

        private void CheckDuplicates(UserCreateModel model, ValidationContext<UserCreateModel> context)
        {
            var existingUser = userRepository.Get(model.Name);
            if (existingUser != null)
                context.AddFailure(nameof(model.Name), "User with specified name already exists");
        }

        private void ValidateRoleCode(UserCreateModel model, ValidationContext<UserCreateModel> context)
        {
            var role = roleRepository.Get(model.RoleCode);
            if(role == null)
                context.AddFailure(nameof(model.RoleCode), "Cannot find a role with specified code");
        }

        private void ValidatePassword(UserCreateModel model, ValidationContext<UserCreateModel> context)
        {
            if(string.IsNullOrWhiteSpace(model.Password))
                context.AddFailure(nameof(model.Password), "Password required");
        }
    }
}
