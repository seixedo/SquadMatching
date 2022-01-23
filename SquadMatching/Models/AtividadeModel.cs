using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquadMatching.Models
{
    public class AtividadeModel
    {
        public String CD_Atividade { get; set; }
        public String CD_Turma { get; set; }
        public String CD_Materia { get; set; }
        public String CD_Status { get; set; }
        public Double Nota { get; set; }
        public String Descricao { get; set; } 
        public String Habilidade { get; set; }
        public List<String> Habilidades { get; set; }
        public List<UsuarioModel> Alunos { get; set; }
    }
}
