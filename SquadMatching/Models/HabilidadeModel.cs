using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquadMatching.Models
{
    public class HabilidadeModel
    {
        //public List<HabilidadeModel> Habilidades { get; set; }
        public int CD_Habilidade { get; set; }
        public String Descricao { get; set; }

        public double Rating { get; set; }
        public String OBS { get; set; }



    }
  
}
