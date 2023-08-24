using Blog.Data;
using Blog.Models;
using BlogAPI.Extensions;
using BlogAPI.Services;
using BlogAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogAPI.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] BlogDataContext ctx, [FromServices] EmailService emailService)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            };
            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);
            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();
                emailService.Send(user.Name, user.Email, "Bem vindo ao BlogAPI by Gustavo Monteiro", $"Sua senha é: <strong> {password} </strong>");
                return Ok(new ResultViewModel<dynamic>(new { user = user.Email, password = "Senha enviada por e-mail", senhaReal = user.PasswordHash }));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ResultViewModel<string>("AC0002 - Email já cadastrado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>("AC0001 - Erro ao cadastrar usuário"));
            }
        }

        [HttpPost("v1/login")]
        public async Task<IActionResult> Login([FromServices] TokenService tokenService, [FromBody] LoginViewModel model, [FromServices] BlogDataContext ctx)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            var user = await ctx.Users.AsNoTracking().Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null)
            {
                return StatusCode(401, new ResultViewModel<string>("AC0003 - Usuário ou senha inválidos"));
            }
            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("AC0003 - Usuário ou senha inválidos"));
            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<dynamic>(token, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>("AC0004 - Falha interna no servidor"));
            }
        }
    }
}