#pragma checksum "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a5f75519a7e763350526d33ad5c013367a0ece40"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_UserType_Index), @"mvc.1.0.view", @"/Views/UserType/Index.cshtml")]
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
#nullable restore
#line 1 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\_ViewImports.cshtml"
using SensorWeb;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\_ViewImports.cshtml"
using SensorWeb.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a5f75519a7e763350526d33ad5c013367a0ece40", @"/Views/UserType/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc3edcab5c525076ddb9767b99a274e7be94ac3b", @"/Views/_ViewImports.cshtml")]
    public class Views_UserType_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<UserTypeModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral(@"

<div class=""page-title"">
    <h3 class=""breadcrumb-header"">Cadastros</h3>
</div>


<div id=""main-wrapper"">
    <div class=""row"">

        <div class=""col-md-12"">

            <div class=""card card-white"">

                <div class=""card-heading clearfix"">

                    <table style=""width: 100%;"">
                        <tr>
                            <td><h4 class=""card-title"">");
#nullable restore
#line 22 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                  Write(localizer.Get("UserTypeTitle"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4></td>\r\n                            <td style=\"text-align:right\">\r\n\r\n                                <a");
            BeginWriteAttribute("href", " href=\"", 652, "\"", 693, 1);
#nullable restore
#line 25 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 659, Url.Action("Create", "UserType" ), 659, 34, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 694, "\"", 727, 1);
#nullable restore
#line 25 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 702, localizer.Get("Add new"), 702, 25, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
                                    <button type=""button"" class=""btn btn-success btn-sm""><i class=""fas fa-plus""></i> </button>
                                </a>
                              

                            </td>
                        </tr>

                    </table>

                </div>

                <div class=""card-body"">

                    <div class=""table-responsive"">
                        <table id=""example"" class=""display table"" style=""width:100%;text-align:center"">
                            <thead>
                                <tr>
                                    <th>
                                        ");
#nullable restore
#line 44 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(Html.DisplayNameFor(modelItem => modelItem.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 47 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(Html.DisplayNameFor(modelItem => modelItem.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n                                    <th class=\"sccentralize\">\r\n                                        ");
#nullable restore
#line 50 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(Html.DisplayNameFor(modelItem => modelItem.UpdatedAt));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n\r\n                                    <th style=\"text-align:center\">\r\n                                        ");
#nullable restore
#line 54 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(localizer.Get("Actions"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n                                    <th></th>\r\n                                </tr>\r\n                            </thead>\r\n\r\n                            <tbody>\r\n");
#nullable restore
#line 61 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <tr>\r\n                                    <td>\r\n                                        ");
#nullable restore
#line 65 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(Html.DisplayFor(modelItem => item.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n                                    <td>\r\n                                        ");
#nullable restore
#line 68 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(Html.DisplayFor(modelItem => item.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n                                    <td class=\"sccentralize\">\r\n                                        ");
#nullable restore
#line 71 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                   Write(Html.DisplayFor(modelItem => item.UpdatedAtSt));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                    </td>


                                    <td>
                                        <table style=""width:100%"">
                                            <tr>
                                                <td style=""padding:0 4px""><a");
            BeginWriteAttribute("href", " href=\"", 3129, "\"", 3192, 1);
#nullable restore
#line 78 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 3136, Url.Action("Details", "UserType", new { id = item.Id }), 3136, 56, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 3193, "\"", 3223, 1);
#nullable restore
#line 78 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 3201, localizer.Get("View"), 3201, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-eye\"></i></a></td>\r\n                                                <td style=\"padding:0 4px\"><a");
            BeginWriteAttribute("href", " href=\"", 3338, "\"", 3398, 1);
#nullable restore
#line 79 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 3345, Url.Action("Edit", "UserType", new { id = item.Id }), 3345, 53, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 3399, "\"", 3429, 1);
#nullable restore
#line 79 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 3407, localizer.Get("Edit"), 3407, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\" style=\"color:#ff6a00\"></i></a></td>\r\n                                                <td style=\"padding:0 4px\"><a");
            BeginWriteAttribute("href", " href=\"", 3567, "\"", 3629, 1);
#nullable restore
#line 80 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 3574, Url.Action("Delete", "UserType", new { id = item.Id }), 3574, 55, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 3630, "\"", 3662, 1);
#nullable restore
#line 80 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
WriteAttributeValue("", 3638, localizer.Get("Delete"), 3638, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" style=""color:#ff0000""><i class=""fas fa-window-close""></i></a></td>
                                            </tr>
                                        </table>
                                    </td>

                                    <!--<td style=""text-align:center"">-->

");
            WriteLiteral("\r\n                                        <!--<a href=\"");
#nullable restore
#line 89 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                Write(Url.Action("Details", "UserType", new { id = item.Id }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" title=\"");
#nullable restore
#line 89 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                                                                                 Write(localizer.Get("View"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" class=\"links\">\r\n                                            <img alt=\"List View\" style=\"height:16px;width:23px;margin-right:15px\" src=\"");
#nullable restore
#line 90 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                                                                                  Write(Url.Content("~/Resources/visual.png"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                                        </a>\r\n                                        <a href=\"");
#nullable restore
#line 92 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                            Write(Url.Action("Edit", "UserType", new { id = item.Id }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" title=\"");
#nullable restore
#line 92 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                                                                          Write(localizer.Get("Edit"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" class=\"links\">\r\n                                            <img alt=\"List View\" style=\"margin-right: 15px\" src=\"");
#nullable restore
#line 93 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                                                            Write(Url.Content("~/Resources/lapis_16x16.png"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                                        </a>\r\n\r\n                                        <a href=\"");
#nullable restore
#line 96 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                            Write(Url.Action("Delete", "UserType", new { id = item.Id }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" title=\"");
#nullable restore
#line 96 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                                                                            Write(localizer.Get("Delete"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" class=\"links\">\r\n                                            <img alt=\"List View\" src=\"");
#nullable restore
#line 97 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                                                 Write(Url.Content("~/Resources/exc_16x16.png"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                                        </a>\r\n                                    </td>\r\n                                    <td></td>-->\r\n                                </tr>\r\n");
#nullable restore
#line 102 "C:\Users\hreis\source\repos\SensorWeb\SensorWeb\Views\UserType\Index.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
            WriteLiteral(@"


                            </tbody>

                        </table>
                    </div>
                </div>






            </div>
        </div>
    </div>

</div>

<!-- end page main wrapper -->
<div class=""page-footer"">
    <p>Iot Nest/Vibração &copy; <span class=""current-year""></span>. Conteúdo de uso exclusivo.</p>
</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SensorWeb.Resources.CommonLocalizationService localizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<UserTypeModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
