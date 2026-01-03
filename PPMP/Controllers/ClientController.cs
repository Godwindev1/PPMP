using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPMP.Models;
using PPMP.Repo;

namespace PPMP.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientRepo ClientManager;

        public ClientController(ClientRepo clientRepo)
        {
            ClientManager = clientRepo;
        } 

        [HttpGet("clientsetup")]
        public async Task<IActionResult> UpdatePassword([FromQuery]string ?Token )
        {
            if(Token != null)
            {
                ViewData["Token"] = Token;
                ViewData["HasToken"] = true;
            }
            else
            {
                ViewData["HasToken"] = false;
            }

            return View("Updatepassword");
        }


        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword(ClientUpdateViewModel model)
        {
            var result = await ClientManager.FindByIdAsync(model.Token);

            if(result != null)
            {
                if(result.HasPassword)
                {
                    ModelState.AddModelError("", "Password has Already Been Included Go to Signin Page To Reset Password ");
                    return LocalRedirect("~/client/login");
                }

                if(await ClientManager.SetPasswordAsync(result, model.Password))
                {
                    return LocalRedirect("~/client/login");
                }
                else
                {
                    ModelState.AddModelError("", "Password Addition Failed Please Contact Support On recurring failures ");
                    return View("Updatepassword");
                }
            }

            ModelState.AddModelError("", "Incorrect User ID ");
            return View("Updatepassword");

        }

        [HttpGet("client/login")]
        public async Task<IActionResult> LoginClient()
        {
            return View("Login");
        }

        [HttpPost("Client/LoginModel")]
        public async Task<IActionResult> Login(ClientLoginViewModel clientLogin)
        {
            if(!ModelState.IsValid)
            {
                return View("Login");
            }

            var Client  = await ClientManager.FindByIdAsync(clientLogin.Token);
            if(Client == null)
            {
                ModelState.AddModelError("", "This Id Does Not Exist Please Contact The Developer To Create Your Session");
                return View("Login");
            }
            else
            {
                var result = await ClientManager.SignInPasswordAsync(Client, clientLogin.Password, clientLogin.RememberMe);

                if(result.Succeeded)
                {
                   return View("Dashboard");
                    //TODO: Implement Client Dashboard View 
                }
                else if(result.PasswordNotSet)
                {
                    ModelState.AddModelError("", "Set User Password");
                    return LocalRedirect("~/clientsetup");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Details");
                    return View("Login");
                }
            }
        }
    }
}
