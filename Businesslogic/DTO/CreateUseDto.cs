using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesslogic.DTO
{
    public class CreateUseDto
    {
        public int idusuario { get; set; }
        [Required]
        public int idrol { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 100 caracteres, ni menos de 3 caracteres.")]
        public string nombre { get; set; }

        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }

        public bool condicion { get; set; }

    }
}
