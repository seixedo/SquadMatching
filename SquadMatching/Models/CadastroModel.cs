using System;
using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace SquadMatching.Models
{
    public class CadastroModel
    {
        [Required]
        public String Nome { get; set; }
        public int CD_Habilidade { get; set; }

        public int CD_Habilidade2 { get; set; }
        public IEnumerable<HabilidadeModel> Hab2 { get; set; }
        public Boolean CD_Professor { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        public String Matricula { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public String Senha { get; set; }

        [Key]
        public List<HabilidadeModel> Habilidades { get; set; }

        public string ImagePath { get; set; }

     public IFormFile image { get; set; }
       
            
    }
}
