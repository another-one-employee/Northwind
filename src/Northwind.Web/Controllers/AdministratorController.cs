using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Infrastructure.Annotations;
using System.Linq;

namespace Northwind.Web.Controllers
{
    [Authorize(Roles=nameof(Roles.admin))]
    public class AdministratorController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
 
        public AdministratorController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
 
        public IActionResult Index() => View(_userManager.Users.ToList());
    }
}
