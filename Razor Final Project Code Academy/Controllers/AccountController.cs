using System;
using Razor_Final_Project_Code_Academy.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.ViewModel;


namespace Final_Project_Razor.Controllers
{
	public class AccountController:Controller
	{
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
            _userManager = userManager;
            _signInManager = signInManager;
        }


		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM account)
        {
            if (!ModelState.IsValid) return View();
            User user = new()
            {
                Fullname = string.Concat(account.FirstName, " ", account.LastNAme),
                Email = account.Email,
                UserName = account.UserName
            };
            if (_userManager.Users.Any(x => x.NormalizedEmail == account.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Bu e-poçtda istifadəçi mövcuddur");
                return View();
            }
            IdentityResult result = await _userManager.CreateAsync(user, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return  View();
            }
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("razor.familysites@gmail.com", "Welcome To Razor!");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Verify Email";
            mail.Body = string.Empty;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{userFullName}}", user.Fullname);
            mail.Body = body.Replace("{{link}}", link);
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("razor.familysites@gmail.com", "lrzxazlzxwtshywn");
            smtp.Send(mail);
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Welcome");
        }

        public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            User user = await _userManager.FindByNameAsync(login.Username);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "you blocked for 5 minutes");
                    return View();
                }
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgot(ForgotVM account)
        {
            if (account.User.Email is null) return View() ;
            User user = await _userManager.FindByEmailAsync(account.User.Email);
            if (user is null) return View();
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action(nameof(RestartPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("razor.familysites@gmail.com", "Welcome To Razor!");
            mail.To.Add(new MailAddress(user.Email));

            mail.Subject = "Reset Password";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/ForgotPassword.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{userFullName}}", user.Fullname);
            mail.Body = body.Replace("{{link}}", link);
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("razor.familysites@gmail.com", "lrzxazlzxwtshywn");
            smtp.Send(mail);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RestartPassword(string email, string token)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) BadRequest();
            ForgotVM model = new()
            {
                User = user,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestartPassword(ForgotVM forgot)
        {
            User user = await _userManager.FindByEmailAsync(forgot.User.Email);
            ForgotVM model = new()
            {
                User = user,
                Token = forgot.Token
            };
            await _userManager.ResetPasswordAsync(user, forgot.Token, forgot.Password);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccountSetting()
        {
            return View();
        }

    }
}

