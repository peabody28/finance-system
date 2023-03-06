using Microsoft.AspNetCore.Mvc;
using user.Helper;
using user.Interfaces.Repositories;
using user.Models.User;

namespace user.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        #region [ Dependency -> Repositories ]

        private readonly IUserRepository UserRepository;

        private readonly IRoleRepository RoleRepository;

        #endregion

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            UserRepository = userRepository;
            RoleRepository = roleRepository;
        }

        [HttpPost]
        public UserModel Create(UserCreateModel model)
        {
            var role = RoleRepository.Get(model.RoleCode);
            var passwordHash = MD5Helper.Hash(model.Password);

            var user = UserRepository.Create(model.Name, passwordHash, role);

            return new UserModel { Name = user.Name, RoleCode = role.Code };
        }
    }
}