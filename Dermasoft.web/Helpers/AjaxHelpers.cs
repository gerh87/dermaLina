using System;
using System.IO;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

// ReSharper disable once CheckNamespace
namespace Mia.Razor.Helpers.Ajax
{
    public class AjaxHelperObject
    {
        public string Id { get; set; }
        public string Link { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string Title { get; set; }
        public string Width { get; set; }
        public Object Params { get; set; }
        public Object Attrs { get; set; }
        
    }

    public static class AjaxHelpers 
    {
        public static MvcHtmlString PopUp(this AjaxHelper ajaxHelper, AjaxHelperObject properties)
        {
            return new MvcHtmlString(ajaxHelper.ActionLink(
                String.Format(" {0} ", properties.Link ?? ""), 
                properties.Action ?? "Index", 
                properties.Controller ?? "Home", 
                properties.Params,
                PopUpAjaxOptions(
                    properties.Method ?? "GET", 
                    properties.Title ?? "Title", 
                    properties.Width ?? "200"), 
                properties.Attrs
                ).ToHtmlString());
        }

   
        public static AjaxOptions PopUpAjaxOptions(string method, string title, string width)
        {
            return new AjaxOptions
            {
                HttpMethod = method,
                InsertionMode = InsertionMode.Replace,
                OnComplete = "loadPopup('" + title + "', " + width + ")",
                LoadingElementId = "loading",
                UpdateTargetId = "poprender"
            };
        }

        public static MvcHtmlString PopIn(this AjaxHelper ajaxHelper, AjaxHelperObject properties)
        {
            return new MvcHtmlString(ajaxHelper.ActionLink(
                String.Format(" {0} ", properties.Link ?? ""),
                properties.Action ?? "Index",
                properties.Controller ?? "Home",
                properties.Params,
                PopInAjaxOptions(
                    properties.Method ?? "GET",
                    properties.Title ?? "Title",
                    properties.Id ?? "content"),
                properties.Attrs
                ).ToHtmlString());
        }

        public static AjaxOptions PopInAjaxOptions(string method, string title, string id)
        {
            return new AjaxOptions
            {
                HttpMethod = method,
                InsertionMode = InsertionMode.Replace,
                OnComplete = "loadPopin('" + title + "')",
                LoadingElementId = "loading",
                UpdateTargetId = id
            };
        }

        public static MvcHtmlString AutoComplete(this MvcHtmlString element, string id, string url, int length = 1, int top = 10)
        {
            var result = element.ToHtmlString().Replace(" ", " onfocus = \"loadComplete('" + id +"', '" + url + "', " + length + ", " + top + ");\" ");
            return new MvcHtmlString(result);
        }


        
    }
}