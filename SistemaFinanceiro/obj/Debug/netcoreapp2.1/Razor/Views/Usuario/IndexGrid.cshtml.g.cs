#pragma checksum "C:\Projetos\Projeto asp.net core\Finaceiro-Construtora-asp.core\SistemaFinanceiro\Views\Usuario\IndexGrid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5b066f1ce71ca7a074ef3a0906e46d9bd1a68bdd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Usuario_IndexGrid), @"mvc.1.0.view", @"/Views/Usuario/IndexGrid.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Usuario/IndexGrid.cshtml", typeof(AspNetCore.Views_Usuario_IndexGrid))]
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
#line 1 "C:\Projetos\Projeto asp.net core\Finaceiro-Construtora-asp.core\SistemaFinanceiro\Views\_ViewImports.cshtml"
using SistemaFinanceiro;

#line default
#line hidden
#line 2 "C:\Projetos\Projeto asp.net core\Finaceiro-Construtora-asp.core\SistemaFinanceiro\Views\_ViewImports.cshtml"
using SistemaFinanceiro.Models;

#line default
#line hidden
#line 3 "C:\Projetos\Projeto asp.net core\Finaceiro-Construtora-asp.core\SistemaFinanceiro\Views\_ViewImports.cshtml"
using NonFactors.Mvc.Grid;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5b066f1ce71ca7a074ef3a0906e46d9bd1a68bdd", @"/Views/Usuario/IndexGrid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68eccfb344492a0da8f411ca996302faaade2f82", @"/Views/_ViewImports.cshtml")]
    public class Views_Usuario_IndexGrid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Usuario>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(29, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(33, 660, false);
#line 3 "C:\Projetos\Projeto asp.net core\Finaceiro-Construtora-asp.core\SistemaFinanceiro\Views\Usuario\IndexGrid.cshtml"
Write(Html
        .Grid(Model)
        .Build(columns =>
        {
            columns.Add(model => model.Id).Css("collapse");
            columns.Add(model => model.Nome).Titled("Nome").Encoded(false).RenderedAs(model => "<a href='/Painel/UsuarioEditar/" + model.Id + "'>" + model.Nome + "</a>").InitialSort(GridSortOrder.Asc);
            columns.Add(model => model.Email).Titled("E-mail");
        })
        .Css("table table-hover")
        .Filterable()
        .Sortable()
        .Pageable(pager =>
        {
            pager.PagesToDisplay = 5;
            pager.RowsPerPage = 10;
        })
        .Empty("Nenhum resultado encontrado")
);

#line default
#line hidden
            EndContext();
            BeginContext(694, 2, true);
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Usuario>> Html { get; private set; }
    }
}
#pragma warning restore 1591