#pragma checksum "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cab8e992244b684abb4f4d02f485c92b00756c07"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_UserType_IndexOriginal), @"mvc.1.0.view", @"/Views/UserType/IndexOriginal.cshtml")]
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
#line 1 "D:\Dev\repos\SensorWeb\SensorWeb\Views\_ViewImports.cshtml"
using SensorWeb;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Dev\repos\SensorWeb\SensorWeb\Views\_ViewImports.cshtml"
using SensorWeb.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cab8e992244b684abb4f4d02f485c92b00756c07", @"/Views/UserType/IndexOriginal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc3edcab5c525076ddb9767b99a274e7be94ac3b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_UserType_IndexOriginal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<UserTypeModel>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n\r\n<h4 style=\"color:white; text-align:center\">");
#nullable restore
#line 6 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
                                      Write(localizer.Get("UserTypeTitle"));

#line default
#line hidden
#nullable disable
            WriteLiteral(" </h4>\r\n<hr />\r\n<p>\r\n    <a");
            BeginWriteAttribute("href", " href=\"", 207, "\"", 248, 1);
#nullable restore
#line 9 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 214, Url.Action("Create", "UserType" ), 214, 34, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 249, "\"", 282, 1);
#nullable restore
#line 9 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 257, localizer.Get("Add new"), 257, 25, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <img");
            BeginWriteAttribute("src", " src=\"", 298, "\"", 349, 1);
#nullable restore
#line 10 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 304, Url.Content("~/Resources/new_rec_32x32.png"), 304, 45, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    </a>\r\n</p>\r\n<table class=\"table sclistTable\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 17 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
           Write(Html.DisplayNameFor(modelItem => modelItem.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 20 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
           Write(Html.DisplayNameFor(modelItem => modelItem.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th class=\"sccentralize\">\r\n                ");
#nullable restore
#line 23 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
           Write(Html.DisplayNameFor(modelItem => modelItem.UpdatedAt));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n\r\n            <th style=\"text-align:center\">\r\n                ");
#nullable restore
#line 27 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
           Write(localizer.Get("Actions"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody class=\"sctbodyresult\">\r\n");
#nullable restore
#line 33 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 37 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
               Write(Html.DisplayFor(modelItem => item.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 40 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
               Write(Html.DisplayFor(modelItem => item.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td class=\"sccentralize\">\r\n                    ");
#nullable restore
#line 43 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
               Write(Html.DisplayFor(modelItem => item.UpdatedAtSt));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td style=\"text-align:center\">\r\n\r\n");
            WriteLiteral("\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 1517, "\"", 1580, 1);
#nullable restore
#line 49 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 1524, Url.Action("Details", "UserType", new { id = item.Id }), 1524, 56, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 1581, "\"", 1611, 1);
#nullable restore
#line 49 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 1589, localizer.Get("View"), 1589, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"links\">\r\n                        <img alt=\"List View\" style=\"height:16px;width:23px;margin-right:15px\"");
            BeginWriteAttribute("src", " src=\"", 1722, "\"", 1766, 1);
#nullable restore
#line 50 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 1728, Url.Content("~/Resources/visual.png"), 1728, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    </a>\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 1818, "\"", 1878, 1);
#nullable restore
#line 52 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 1825, Url.Action("Edit", "UserType", new { id = item.Id }), 1825, 53, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 1879, "\"", 1909, 1);
#nullable restore
#line 52 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 1887, localizer.Get("Edit"), 1887, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"links\">\r\n                        <img alt=\"List View\" style=\"margin-right: 15px\"");
            BeginWriteAttribute("src", " src=\"", 1998, "\"", 2047, 1);
#nullable restore
#line 53 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 2004, Url.Content("~/Resources/lapis_16x16.png"), 2004, 43, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    </a>\r\n\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 2101, "\"", 2163, 1);
#nullable restore
#line 56 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 2108, Url.Action("Delete", "UserType", new { id = item.Id }), 2108, 55, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 2164, "\"", 2196, 1);
#nullable restore
#line 56 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 2172, localizer.Get("Delete"), 2172, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"links\">\r\n                        <img alt=\"List View\"");
            BeginWriteAttribute("src", " src=\"", 2258, "\"", 2305, 1);
#nullable restore
#line 57 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
WriteAttributeValue("", 2264, Url.Content("~/Resources/exc_16x16.png"), 2264, 41, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    </a>\r\n                </td>\r\n                <td></td>\r\n            </tr>\r\n");
#nullable restore
#line 62 "D:\Dev\repos\SensorWeb\SensorWeb\Views\UserType\IndexOriginal.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SensorWeb.Resources.CommonLocalizationService localizer { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<UserTypeModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591