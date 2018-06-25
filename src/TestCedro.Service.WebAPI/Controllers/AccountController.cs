using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Services;
using TestCedro.Infra.CrossCutting.Helpers;
using TestCedro.Infra.CrossCutting.Helpers.Abstractions.Implementations;
using TestCedro.Service.WebAPI.Helpers;
using TestCedro.Service.WebAPI.ViewModels;

namespace TestCedro.Service.WebAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("signout")]
        public dynamic Signout()
        {
            //Authentication.Signout();
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        public async Task<dynamic> Signup(RegisterVm model)
        {   
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(EntityState.GetErrors(ModelState));

                const bool requireEmailConfirmation = true;
                var token = _userService.Create(model.Email, model.Password, model.ConfirmPassword,
                    model.FirstName, model.LastName, requireEmailConfirmation);
                var confirmationUrl = $"{Request.Localhost()}/api/v1/account/confirm/{HttpUtility.UrlEncode(token)}";
                var emailFactory = await new EmailFactory().EmailSignup(model.Email, confirmationUrl);
                await new MessengerService().SendAsync(emailFactory.FromAddress, emailFactory.ToAddress, emailFactory.Subject, emailFactory.Body, emailFactory.IsBodyHtml);

                return Ok(
                    "Obrigado pelo registro, mas você ainda não terminou! Um e-mail com instruções sobre como ativar sua conta está a caminho de você");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("confirm/{*confirmationToken}")]
        public dynamic Confirm(string confirmationToken)
        {
            try
            {
                //Authentication.Signout();
                if (string.IsNullOrEmpty(confirmationToken)) return BadRequest("Token não encontrado.");
                return _userService.ConfirmAccount(confirmationToken)
                    ? (dynamic)Ok(
                        "Registo confirmado!")
                    : BadRequest("Não foi possível confirmar suas informações de registro.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("changepassword")]
        public dynamic ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
                return _userService.ChangePassword(User.Identity.Name, model.Input.OldPassword, model.Input.NewPassword)
                    ? (dynamic)Ok("Sua senha foi alterada com sucesso.")
                    : BadRequest("A senha atual está incorreta ou a nova senha é inválida.");

            return BadRequest(EntityState.GetErrors(ModelState));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgotpassword")]
        public async Task<dynamic> ForgotPassword(ForgotPasswordVm model)
        {
            if (ModelState.IsValid)
            {
                var resetToken = _userService.GeneratePasswordResetToken(model.UserName);
                if (!string.IsNullOrEmpty(resetToken))
                {
                    var resetUrl = $"{Request.Localhost()}/api/v1/account/passwordreset/{HttpUtility.UrlEncode(resetToken)}";
                    var emailFactory = await new EmailFactory().EmailForgotPassword(model.Email, resetToken, resetUrl);
                    await new MessengerService().SendAsync(emailFactory.FromAddress, emailFactory.ToAddress, emailFactory.Subject, emailFactory.Body, emailFactory.IsBodyHtml);
                }
                return Ok("Um e-mail com instruções sobre como redefinir sua senha está a caminho de você.");
            }
            return BadRequest(EntityState.GetErrors(ModelState));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("passwordreset")]
        public dynamic PasswordReset(PasswordResetVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(EntityState.GetErrors(ModelState));
            return Ok(_userService.ResetPassword(model.ResetToken, model.NewPassword)
                ? "Sua senha foi alterada com sucesso."
                : "O token de redefinição de senha é inválido.");
        }

        [Route("details")]
        [HttpGet]
        public dynamic Details()
        {
            return _userService.GetById(Account.CurrentUserId);
        }

        [HttpPost]
        [Route("edit")]
        public dynamic Edit([FromBody] User user)
        {
            try
            {
                _userService.Update(user);
                return Ok("Operação realizada com sucesso.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}