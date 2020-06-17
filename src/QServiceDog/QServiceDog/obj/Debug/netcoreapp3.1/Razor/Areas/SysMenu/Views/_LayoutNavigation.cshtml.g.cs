#pragma checksum "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "084468aa0d1561af9cc692cc0936827fec706705"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_SysMenu_Views__LayoutNavigation), @"mvc.1.0.view", @"/Areas/SysMenu/Views/_LayoutNavigation.cshtml")]
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
#line 1 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_ViewImports.cshtml"
using Q.DevExtreme.Tpl;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_ViewImports.cshtml"
using Q.DevExtreme.Tpl.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"084468aa0d1561af9cc692cc0936827fec706705", @"/Areas/SysMenu/Views/_LayoutNavigation.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1d438cfecb3c675ed605e17ecc5c4af8f8e18ee6", @"/Areas/SysMenu/Views/_ViewImports.cshtml")]
    public class Areas_SysMenu_Views__LayoutNavigation : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("dx-viewport"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "084468aa0d1561af9cc692cc0936827fec7067053780", async() => {
                WriteLiteral("\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"robots\" content=\"noindex\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>");
#nullable restore
#line 20 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
      Write(meta.GetBreadcrumb(meta.CurrentDemo, "{0}～{1}"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</title>\r\n    ");
#nullable restore
#line 21 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
Write(await Html.PartialAsync("~/Areas/SysMenu/Views/_Resources.cshtml"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n\r\n    ");
#nullable restore
#line 24 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
Write(RenderSection("HeadExtras", false));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"


    <script>
        $('#document').ready(function () {
            if (getLocale() != ""zh"") {
                setLocale(""zh"");
                document.location.reload();
            }
        });
        var locale = getLocale();
        //Globalize.locale(locale);
        DevExpress.localization.locale(locale);
        function changeLocale(data) {
            setLocale(data.value);
            document.location.reload();
        }

        function getLocale() {
            var locale = sessionStorage.getItem(""locale"");
            return locale != null ? locale : ""en"";
        }

        function setLocale(locale) {
            sessionStorage.setItem(""locale"", locale);
        }
        function hideMenu() {
            //var x = $('#demo-menu').attr('class');
            if ($('#demo-menu').css('width').replace('px', '') * 1 < 3) {
                $('#demo-menu').width(200);
                $('#demo-wrapper').css('margin-left', '200px');
            } else {
          ");
                WriteLiteral("      $(\'#demo-menu\').width(0);\r\n                $(\'#demo-wrapper\').css(\'margin-left\', \'0px\');\r\n            }\r\n            //alert(x);\r\n        }\r\n\r\n    </script>\r\n    \r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "084468aa0d1561af9cc692cc0936827fec7067057083", async() => {
                WriteLiteral("\r\n    <header class=\"header\">\r\n        <div class=\"header-container\">\r\n            <a class=\"logo\"");
                BeginWriteAttribute("href", " href=\"", 1954, "\"", 1979, 1);
#nullable restore
#line 68 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
WriteAttributeValue("", 1961, Url.Content("~/"), 1961, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                <span class=\"dx\"></span> ");
#nullable restore
#line 69 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
                                    Write(Utils.SUITE_NAME);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n            </a>\r\n            <div class=\"header-menu\">\r\n                <a class=\"dx-icon-menu\" href=\"javascript:hideMenu()\"></a>\r\n            </div>\r\n\r\n        </div>\r\n    </header>\r\n    <div class=\"body\">\r\n        ");
#nullable restore
#line 78 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
   Write(await Html.PartialAsync("~/Areas/SysMenu/Views/Menu.cshtml"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n        <div class=\"demo-wrapper\" id=\"demo-wrapper\">\r\n            <h4>");
#nullable restore
#line 81 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
           Write(meta.GetBreadcrumb(meta.CurrentDemo, "{0} ▸ {1}"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</h4>\r\n\r\n");
                WriteLiteral("\r\n            <div");
                BeginWriteAttribute("class", " class=\"", 2748, "\"", 2779, 1);
#nullable restore
#line 90 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
WriteAttributeValue("", 2756, GetSimulatorCssClass(), 2756, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                <div class=\"demo-device\">\r\n                    <div class=\"demo-display\">\r\n                        <div class=\"demo-container\">\r\n                            ");
#nullable restore
#line 94 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
                       Write(RenderBody());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n\r\n    <footer class=\"footer\">\r\n        <div class=\"footer-container\">\r\n            Copyright © 2016-");
#nullable restore
#line 105 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
                        Write(DateTime.Now.Year);

#line default
#line hidden
#nullable disable
                WriteLiteral(" <a");
                BeginWriteAttribute("href", " href=\"", 3226, "\"", 3243, 1);
#nullable restore
#line 105 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
WriteAttributeValue("", 3233, Utils.Web, 3233, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" target=\"_blank\"> ");
#nullable restore
#line 105 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
                                                                                 Write(Utils.Copyright);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a>\r\n        </div>\r\n    </footer>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n</html>\r\n");
        }
        #pragma warning restore 1998
#nullable restore
#line 2 "D:\Code\github\qinqoushui\QServiceDog\src\QServiceDog\QServiceDog\Areas\SysMenu\Views\_LayoutNavigation.cshtml"
            
    string GetSimulatorCssClass()
    {
        var result = "simulator";
        if (Utils.SIMULATOR_NO_BORDER)
        {
            result += " simulator-no-border";
        }
        return result;
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public MenuMeta meta { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
