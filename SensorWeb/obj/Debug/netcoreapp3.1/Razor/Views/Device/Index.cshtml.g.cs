#pragma checksum "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "621c95e1c229336f21f952da8ac71786e98d061b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Device_Index), @"mvc.1.0.view", @"/Views/Device/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"621c95e1c229336f21f952da8ac71786e98d061b", @"/Views/Device/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc3edcab5c525076ddb9767b99a274e7be94ac3b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Device_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<DeviceModel>>
    #nullable disable
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
#line 25 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                                  Write(localizer.Get("DeviceTitle"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4></td>\r\n                            <td style=\"text-align:right\">\r\n\r\n                                <a");
            BeginWriteAttribute("href", " href=\"", 654, "\"", 693, 1);
#nullable restore
#line 28 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 661, Url.Action("Create", "Device" ), 661, 32, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 694, "\"", 727, 1);
#nullable restore
#line 28 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
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
#line 47 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(Html.DisplayNameFor(modelItem => modelItem.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 50 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(Html.DisplayNameFor(modelItem => modelItem.Tag));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n                                    <th class=\"sccentralize\">\r\n                                        ");
#nullable restore
#line 53 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(Html.DisplayNameFor(modelItem => modelItem.Code));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n\r\n");
            WriteLiteral("\r\n                                    <th style=\"text-align:center\">\r\n                                        ");
#nullable restore
#line 61 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(localizer.Get("QrCode"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n\r\n                                    <th style=\"text-align:center\">\r\n                                        ");
#nullable restore
#line 65 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(localizer.Get("Actions"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </th>\r\n                                    <th></th>\r\n                                </tr>\r\n                            </thead>\r\n\r\n                            <tbody>\r\n");
#nullable restore
#line 72 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <tr>\r\n                                    <td>\r\n                                        ");
#nullable restore
#line 76 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(Html.DisplayFor(modelItem => item.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n                                    <td>\r\n                                        ");
#nullable restore
#line 79 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(Html.DisplayFor(modelItem => item.Tag));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n                                    <td class=\"sccentralize\">\r\n                                        ");
#nullable restore
#line 82 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                   Write(Html.DisplayFor(modelItem => item.Code));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n");
            WriteLiteral("                                    <td style=\"text-align:center\">\r\n                                        <img style=\"height:50px;width:50px;cursor:pointer\"");
            BeginWriteAttribute("id", " id=\"", 3603, "\"", 3620, 2);
            WriteAttributeValue("", 3608, "img_", 3608, 4, true);
#nullable restore
#line 88 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 3612, item.Id, 3612, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("src", " src=\"", 3621, "\"", 3711, 1);
#nullable restore
#line 88 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 3627, String.Format("data:image/png;base64,{0}", Convert.ToBase64String( item.QrCodeImg)), 3627, 84, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                                    </td>
                                    <td>
                                        <table style=""width:100%"">
                                            <tr>
                                                <td style=""padding:0 4px""><a");
            BeginWriteAttribute("href", " href=\"", 3996, "\"", 4057, 1);
#nullable restore
#line 93 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 4003, Url.Action("Details", "Device", new { id = item.Id }), 4003, 54, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 4058, "\"", 4088, 1);
#nullable restore
#line 93 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 4066, localizer.Get("View"), 4066, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-eye\"></i></a></td>\r\n                                                <td style=\"padding:0 4px\"><a");
            BeginWriteAttribute("href", " href=\"", 4203, "\"", 4261, 1);
#nullable restore
#line 94 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 4210, Url.Action("Edit", "Device", new { id = item.Id }), 4210, 51, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 4262, "\"", 4292, 1);
#nullable restore
#line 94 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 4270, localizer.Get("Edit"), 4270, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\" style=\"color:#ff6a00\"></i></a></td>\r\n                                                <td style=\"padding:0 4px\"><a");
            BeginWriteAttribute("href", " href=\"", 4430, "\"", 4490, 1);
#nullable restore
#line 95 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 4437, Url.Action("Delete", "Device", new { id = item.Id }), 4437, 53, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 4491, "\"", 4523, 1);
#nullable restore
#line 95 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
WriteAttributeValue("", 4499, localizer.Get("Delete"), 4499, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"color:#ff0000\"><i class=\"fas fa-window-close\"></i></a></td>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n");
#nullable restore
#line 144 "D:\Dev\repos\SensorWeb\SensorWeb\Views\Device\Index.cshtml"
                                               

                                }

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<DeviceModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591