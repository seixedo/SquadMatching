using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquadMatching.Models
{
    public class GrupoModel
    {
        public String id { get; set; }
        public string Nome { get; set; }
        public double Nota { get; set; }
        public string LinkAtividade { get; set; }
        public List<UsuarioModel> Alunos { get; set; }

    }
}
