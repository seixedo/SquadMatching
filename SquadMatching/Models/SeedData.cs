using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SquadMatching.Models;

namespace SquadMatching.Models
{
    public static class SeedData
    {
        /*public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SquadMatchingContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SquadMatchingContext>>()))
            {
                // Look for any UsuarioModels.
                if (context.UsuarioModel.Any())
                {
                    return;   // DB has been seeded
                }

                context.UsuarioModel.AddRange(
                    new UsuarioModel
                    {
                        Nome = "Joao",
                        CD_Habilidade= 1,
                        CD_Habilidade2 = 2,
                        Funcao = "Front-End",
                        Matricula = "2015103297",
                        CD_Professor = false,
                        
                    }
                );
                context.SaveChanges();
            }
        }
    }*/
    }
    }