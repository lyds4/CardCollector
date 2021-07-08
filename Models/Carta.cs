using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardCollector.Models
{
    public class Carta
    {
        [Key]
        public int Id { get; set; } // prop + tabtab
        public int IdUser { get; set; }
        [Required]
        public String Nombre { get; set; }
        public String Descripcion { get; set; }
        [Range(1,50)]
        public int Nivel { get; set; }

        public Carta()  // ctror + tabtab
        {

        }
    }
}
