using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquadMatching.Models
{
    public class TurmaModel
    {   public String CD_Turma { get; set; }
        public String CD_Materia { get; set; }
        public String Nome_Turma { get; set; }
        public String Nome_Materia { get; set; }
        public int Horario { get; set; }
        public int dia { get; set; }
        public String Turno { get; set; }
        public String Unidade { get; set; }
        
    }
    }

