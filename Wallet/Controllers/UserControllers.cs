using Businesslogic.DTO;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Wallet.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UserControllers : ControllerBase
    {
        private readonly WalletDbContext _context;
        private readonly IConfiguration _config;

        public UserControllers(WalletDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Usuarios/Listar
        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserDto>> Listar()
        {
            var usuario = await _context.Users.Include(u => u.rol).ToListAsync();

            return usuario.Select(u => new UserDto
            {
                idusuario = u.idusuario,
                idrol = u.idrol,
                rol = u.rol.nombre,
                nombre = u.nombre,               
                email = u.email,          
                condicion = u.condicion
            });

        }

        // POST: api/Usuarios/Crear
        // [Authorize(Roles = "Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CreateUseDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = model.email.ToLower();

            if (await _context.Users.AnyAsync(u => u.email == email))
            {
                return BadRequest("El email ya existe");
            }

            CrearPasswordHash(model.password, out byte[] passwordHash, out byte[] passwordSalt);

            User usuario = new User
            {
                idrol = model.idrol,
                nombre = model.nombre,               
                email = model.email.ToLower(),
                password_hash = passwordHash,
                password_salt = passwordSalt,
                condicion = true
            };

            _context.Users.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }


        // PUT: api/Articulos/Actualizar
        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idusuario <= 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.idusuario == model.idusuario);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.idrol = model.idrol;
            usuario.nombre = model.nombre;        
            usuario.email = model.email.ToLower();

            if (model.act_password == true)
            {
                CrearPasswordHash(model.password, out byte[] passwordHash, out byte[] passwordSalt);
                usuario.password_hash = passwordHash;
                usuario.password_salt = passwordSalt;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        
        // DELETE: api/Categorias/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Usuarios = await _context.Users.FindAsync(id);
            if (Usuarios == null)
            {
                return NotFound();
            }

            _context.Users.Remove(Usuarios);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(Usuarios);
        }

        [HttpPost("CrearRol")]
        public async Task<IActionResult> CrearRol([FromBody] string nombre)
        {
            if (await _context.Roles.AnyAsync(r => r.nombre == nombre))
                return BadRequest("El rol ya existe");

            _context.Roles.Add(new Rol { nombre = nombre });
            await _context.SaveChangesAsync();

            return Ok("Rol creado correctamente");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto model)
        {

            var email = model.email.ToLower();

            var usuario = await _context.Users.Where(u => u.condicion == true).Include(u => u.rol).FirstOrDefaultAsync(u => u.email == email);

            if (usuario == null)
            {
                return NotFound();
            }

            if (!VerificarPasswordHash(model.password, usuario.password_hash, usuario.password_salt))
            {
                return NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.idusuario.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, usuario.rol.nombre ),
                new Claim("idusuario", usuario.idusuario.ToString() ),
                new Claim("rol", usuario.rol.nombre ),
                new Claim("nombre", usuario.nombre )
            };

            return Ok(
                    new { token = GenerarToken(claims) }
                );

        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo));
            }
        }

        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Users.Any(e => e.idusuario == id);
        }
    }
}
