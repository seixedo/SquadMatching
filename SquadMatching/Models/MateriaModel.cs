using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace   SquadMatching.Models
{
    public class MateriaModel
    {
        public int CD_Materia { get; }
        public String Curso { get; set; }
        public int Carga_H { get; set; }
        public string Materia { get; set; }

      

    }


}
