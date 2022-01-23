using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SquadMatching.Models
{
    public class UsuarioModel
    {
        [Key]
        public int CD_aluno { get; set; }
        public String Nome { get; set; }
        public String Matricula { get; set; }
        public List<HabilidadeModel> Habilidades { get; set; }
        public Boolean CD_Professor { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }
        public double Media { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public String Senha { get; set; }
        public String imgPath { get; set; }


    }



    }

