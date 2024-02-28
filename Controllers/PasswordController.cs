using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordValidator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        [HttpPost]
        public ActionResult ValidarSenha(string senha)
        {
            try
            {
                if (senha.Length < 9)
                    return BadRequest(false);

                else if (!Regex.IsMatch(senha, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()\-+]).+$"))
                    return BadRequest(false);

                else if (Regex.IsMatch(senha, @"(.).*\1"))
                    return BadRequest(false);

                else
                    return Ok(true);
            }
            catch (NullReferenceException nrex)
            {
                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
