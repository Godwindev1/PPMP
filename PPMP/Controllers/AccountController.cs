using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PPMP.Data;
using PPMP.Models;
using System.Net;
using System.Security.Claims;

namespace PPMP.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> RoleManager;

        private readonly EmailService emailService;
        public AccountController(UserManager<User> UserManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, EmailService emailService)
            : base()
        {
            this.userManager = UserManager;
            this.signInManager = signInManager;
            RoleManager = roleManager;
            this.emailService = emailService;
        }

        [HttpGet("Account/login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginModel(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return LocalRedirect(returnUrl);

                    else
                        return LocalRedirect("/");
                }
                else if(result.IsNotAllowed)
                {
                    ViewBag.LoginError = @$"<p style='color: red'>Please verify your email.</p> <a href='ResendVerification?email={model.Email}' 
                                            style='color: #3B4CB8; text-decoration: underline ' >resend email</a>";
                    return View("login");

                }
                else
                {
                    ModelState.AddModelError("", "Incorrect email or password");
                    return View("login");
                }
            }
            else
            {
              ModelState.AddModelError("", "User Does Not Exist");
              return LocalRedirect($"/Register?email={model.Email}");
            }

        }


        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return LocalRedirect("/");
        }



        [HttpGet("Register")]
        public ViewResult Register([FromQuery]string? email = null)
        {
            ViewData["ReadyEmail"] = false;

            if(email != null)
            {
                ViewData["ReadyEmail"] = true;
                ViewBag.Email = email;
            }

            return View();
        }


        public async Task<IActionResult> Registermodel(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var Role = await RoleManager.FindByNameAsync("Developer");

            if (Role == null)
            {
                await RoleManager.CreateAsync(new Role { Name = "Developer", NormalizedName = "DEVELOPER" });

                Role = await RoleManager.FindByNameAsync("Developer");

                if(Role != null)
                {
                    await RoleManager.AddClaimAsync(Role, new Claim(ClaimTypes.Role, Role.NormalizedName ?? "Developer"));
                }
            }

            if(await userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("", "This already user exist");
                return View("Register");
            }


            User user = new User();
            user.Email = model.Email;
            user.UserName = model.Username;

            var result  = await userManager.CreateAsync(user, model.Password);
           
            if(result.Succeeded)
            {
                var _user = await userManager.FindByEmailAsync(user.Email);
                await userManager.AddToRoleAsync(_user, "Developer");

                await SendVerificationEmail(_user);
                
                return LocalRedirect($"~/WaitForEmailVerification?email={_user.Email}");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                return View("Register");

            }
        
        }

        [HttpGet("WaitForEmailVerification")]
        public async Task<ActionResult> WaitForEmailVerification([FromQuery]string email)
        {
            if(!ModelState.IsValid)
            {
                return View("error");
            }

            try
            {
                if( (await userManager.FindByEmailAsync(email)).EmailConfirmed)
                {
                    ViewBag.IsConfirmed = true;
                     ViewBag.Message = "Your email is verified!";
                }
                else
                {
                    ViewBag.IsConfirmed = false;
                     ViewBag.Message = "Please verify your email.";
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View();
        }

        
        public async Task  SendVerificationEmail(User user)
        {
               var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                
               var baseUrl = Url.RouteUrl(
                    routeName: "ConfirmEmailRoute",
                    values: new { },
                    protocol: HttpContext.Request.Scheme
                );

                var confirmationLink =
                    $"{baseUrl}?userId={WebUtility.UrlEncode(user.Id)}&token={WebUtility.UrlEncode(token)}";

                //Send Email
                emailService.SendLinkEmail(
                    to: user.Email,
                    subject: "Verify Your Email",
                    message: "Please verify your email by clicking the button below:",
                    buttonText: "Verify Email",
                    linkUrl: confirmationLink
                );
        }

        [HttpGet("Account/ResendVerification")]
        public async Task<IActionResult?> ResendVerification([FromQuery]string email)
        {
            var user =  await userManager.FindByEmailAsync(email);
            if(user != null)
            {
                await SendVerificationEmail(user);  
            }

            return Redirect("login");
        }

        [HttpGet("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, [FromQuery]string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return View("ConfirmEmailSuccess");

            return View("ConfirmEmailFailed");
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromQuery]string? Email)
        {
            if(Email == null)
            {
                ViewData["IsEmailNull"] = true;
            }
            else
            {
                ViewData["IsEmailNull"] = false;
                ViewData["Email"] = Email;
            }

            return View("ForgotPassword");
        }


        [HttpPost(template: "SendPasswordReset",  Name = "PasswordRequest")]
        public async Task<IActionResult> SendPasswordResetToken(ForgotPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                
                if(user == null)
                {
                    return View("ResetPasswordLink");    
                }

               var token  =  await userManager.GeneratePasswordResetTokenAsync(user);
                 
               var baseUrl = Url.RouteUrl(
                    routeName: "ResetPassword",
                    values: new { },
                    protocol: HttpContext.Request.Scheme
                );

                var ResetLink =
                    $"{baseUrl}?userId={WebUtility.UrlEncode(user.Id)}&token={WebUtility.UrlEncode(token)}";

                  emailService.SendLinkEmail(
                    to: user.Email,
                    subject: "Password Reset",
                    message: "Reset Your Password by clicking the button below:",
                    buttonText: "Reset",
                    linkUrl: ResetLink
                );

                return View("ResetPasswordLink");
            }

            ModelState.AddModelError("", "invalid ");
            return View("ResetPasswordLink");
        }

        [HttpGet(template:"ResetPassword", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery]string userId, [FromQuery]string token)
        {
            ViewBag.userID = userId;
            ViewBag.token = token;

            return View("ResetPassword");
        }

        [HttpPost("UpdatePassword", Name = "UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View("ResetPasswordFailed");
            }

            var user = await  userManager.FindByIdAsync(resetPasswordViewModel.UserID);
            var result = await userManager.ResetPasswordAsync(user, resetPasswordViewModel.token, resetPasswordViewModel.Password);

            if(result.Succeeded)
            {
                return View("ResetPasswordSucces");
            }
            else
            {
                return View("ResetPasswordFailed");
            }
        }


    }
}
