using BootCamp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BootCamp.Core.DTOs;
using Microsoft.Win32;

namespace BootCamp.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]

    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        public HomeController(IAccountService accountService)
        {
            _accountService=accountService;
        }
      

        public IActionResult Index()
        {
            ViewBag.ListWallet = _accountService.GetWalletForUser(User.Identity.Name);
            return View(_accountService.GetInformationForUserPanel(User.Identity.Name));

        }
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListWallet = _accountService.GetWalletForUser(User.Identity.Name);
                return View(charge);
            }

            int walletId = _accountService.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ حساب");
            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(charge.Amount);

            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44349/OnlinePayment/" + walletId, "Info@topLearn.Com", "09197070750");

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion
            return null;

        }

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_accountService.GetDataForEditProfileUser(User.Identity.Name));
        }

        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);
           
            if (profile.Email == null && profile.UserName == null&&profile.UserAvatar == null&&profile.OldPassword != null)
            {
                if (profile.OldPassword != null && _accountService.ComparePassword(profile.UserName, profile.OldPassword))
                {
                    ModelState.AddModelError("UserName", "رمز عبور فعلی درست نمی باشد");
                    return View(profile);
                }
                _accountService.EditPassword(User.Identity.Name, profile);

                //Log Out User
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect("/Login?EditProfile=true");
 
            }
            else
            {
                _accountService.EditProfile(User.Identity.Name, profile);

                //Log Out User
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Redirect("/Login?EditProfile=true");
            }
          

        }


        
    }
}
