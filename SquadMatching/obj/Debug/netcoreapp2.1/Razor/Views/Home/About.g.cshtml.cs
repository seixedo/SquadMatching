#pragma checksum "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\About.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a56527fa70de636d19cfc7f170109a09b83d4694"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_About), @"mvc.1.0.view", @"/Views/Home/About.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/About.cshtml", typeof(AspNetCore.Views_Home_About))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a56527fa70de636d19cfc7f170109a09b83d4694", @"/Views/Home/About.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c4e2924f02d40c6d954fc29d9baeebf9848eb28", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_About : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SquadMatching.Models.UsuarioModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\About.cshtml"
  
    ViewData["Title"] = "About";

#line default
#line hidden
            BeginContext(84, 4, true);
            WriteLiteral("<h2>");
            EndContext();
            BeginContext(89, 17, false);
#line 5 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\About.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(106, 11, true);
            WriteLiteral("</h2>\r\n<h3>");
            EndContext();
            BeginContext(118, 19, false);
#line 6 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\About.cshtml"
Write(ViewData["Message"]);

#line default
#line hidden
            EndContext();
            BeginContext(137, 74, true);
            WriteLiteral("</h3>\r\n\r\n<p>Use this area to provide additional information.</p>\r\n<button>");
            EndContext();
            BeginContext(212, 12, false);
#line 9 "C:\Users\T-Gamer\source\repos\SquadMatching\Views\Home\About.cshtml"
   Write(ViewBag.Nome);

#line default
#line hidden
            EndContext();
            BeginContext(224, 9, true);
            WriteLiteral("</button>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SquadMatching.Models.UsuarioModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
