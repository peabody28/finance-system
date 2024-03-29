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

        private readonly IUserRepository userRepository;

        private readonly IRoleRepository roleRepository;

        #endregion

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        [HttpPost]
        public UserModel Create(UserCreateModel model)
        {
            var role = roleRepository.Get(model.RoleCode);
            var passwordHash = MD5Helper.Hash(model.Password);

            var user = userRepository.Create(model.Name, passwordHash, role);

            return new UserModel { Name = user.Name, RoleCode = role.Code };
        }
    }
}