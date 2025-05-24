using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace noticias.Models
{
    [Table("t_feedback")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdPost { get; set; }       

        public string? Titulo { get; set; }
    
        public string? Sentimiento { get; set; } // "like" o "dislike" 

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? Username { get; set; }


    }
}