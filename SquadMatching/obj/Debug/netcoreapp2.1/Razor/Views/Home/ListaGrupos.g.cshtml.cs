#pragma checksum "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7434bff076f58724ad718297346c5ed65231870a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ListaGrupos), @"mvc.1.0.view", @"/Views/Home/ListaGrupos.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/ListaGrupos.cshtml", typeof(AspNetCore.Views_Home_ListaGrupos))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\_ViewImports.cshtml"
using SquadMatching;

#line default
#line hidden
#line 2 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\_ViewImports.cshtml"
using SquadMatching.Models;

#line default
#line hidden
#line 1 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7434bff076f58724ad718297346c5ed65231870a", @"/Views/Home/ListaGrupos.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c4e2924f02d40c6d954fc29d9baeebf9848eb28", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ListaGrupos : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IList<SquadMatching.Models.GrupoModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(131, 34, true);
            WriteLiteral("\r\n    <div id=\"CorpoListaGrupo\">\r\n");
            EndContext();
#line 6 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
          

            foreach (var Grupos in Model)
            {




#line default
#line hidden
            BeginContext(243, 144, true);
            WriteLiteral("                <div class=\"AlunosCard\">\r\n                            <header class=\"TituloGrupo\">\r\n\r\n                                        \r\n");
            EndContext();
#line 17 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                                   
                                    if (Grupos.Alunos.Count < 5)
                                    {

#line default
#line hidden
            BeginContext(529, 244, true);
            WriteLiteral("                                                                <div>\r\n                                                                    <h5 style=\"text-align:center;\">\r\n                                                                        ");
            EndContext();
            BeginContext(774, 11, false);
#line 22 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                                                                   Write(Grupos.Nome);

#line default
#line hidden
            EndContext();
            BeginContext(785, 25, true);
            WriteLiteral(" <a class=\"btn btn-block\"");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 810, "\"", 891, 7);
            WriteAttributeValue("", 820, "location.href", 820, 13, true);
            WriteAttributeValue(" ", 833, "=", 834, 2, true);
            WriteAttributeValue(" ", 835, "\'", 836, 2, true);
#line 22 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
WriteAttributeValue("", 837, Url.Action("ParticiparGrupo", "Home"), 837, 38, false);

#line default
#line hidden
            WriteAttributeValue("", 875, "?idg=", 875, 5, true);
#line 22 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
WriteAttributeValue("", 880, Grupos.id, 880, 10, false);

#line default
#line hidden
            WriteAttributeValue("", 890, "\'", 890, 1, true);
            EndWriteAttribute();
            BeginContext(892, 349, true);
            WriteLiteral(@">
                                                                        Participar <i class=""fa fa-arrow-alt-circle-right""></i>
                                                                    </a>
                                                                </h5>
                                                                </div>
");
            EndContext();
#line 27 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                                    }
                                

#line default
#line hidden
            BeginContext(1315, 177, true);
            WriteLiteral("                                <hr style=\"border-width:1px;border-style:inset\" />\r\n                            </header>\r\n                    \r\n\r\n                    <footer>\r\n");
            EndContext();
#line 34 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                         foreach (var Alunos in Grupos.Alunos)
                        {


#line default
#line hidden
            BeginContext(1585, 32, true);
            WriteLiteral("                            <h6>");
            EndContext();
            BeginContext(1618, 11, false);
#line 37 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                           Write(Alunos.Nome);

#line default
#line hidden
            EndContext();
            BeginContext(1629, 87, true);
            WriteLiteral("</h6>\r\n                            <hr style=\"border-width:1px;border-style:inset\" />\r\n");
            EndContext();
#line 39 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                            
                        }

#line default
#line hidden
            BeginContext(1773, 81, true);
            WriteLiteral("                    </footer>\r\n\r\n                </div>\r\n                <br />\r\n");
            EndContext();
#line 45 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\ListaGrupos.cshtml"
                
            }


        

#line default
#line hidden
            BeginContext(1902, 16, true);
            WriteLiteral("\r\n    </div>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(1935, 429, true);
                WriteLiteral(@"
    <link rel=""stylesheet"" href=""//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"">
    <script src=""//code.jquery.com/jquery-1.10.2.js""></script>
    <script src=""//code.jquery.com/ui/1.11.4/jquery-ui.js""></script>
    <script>
        $(document).ready(function () {
            $('.MeuButton').on('click', function () {
                $('.collapse').collapse('hide');
            })
        })
    </script>
");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHttpContextAccessor HttpContextAccessor { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IList<SquadMatching.Models.GrupoModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
