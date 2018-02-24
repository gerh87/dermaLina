using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;

// ReSharper disable once CheckNamespace
namespace Mia.Razor.Helpers.Html
{
    public static class HtmlHelpers
    {
        public class HtmlHelperObject
        {
            public string @Id { get; set; }
            public string @Class { get; set; }
            public string @OnFocus { get; set; }
            public string @Url{ get; set; }
            public int @Limit { get; set; }
            public int @MaxChars { get; set; }
        }

        public static MvcHtmlString MyTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            HtmlHelperObject properties)
        {
            //dynamic param = new ExpandoObject();
            //param.id = "test";
            //param.onfocus = "startAutoComplete('test')";
            return null; // helper.TextBoxFor(expression, null, properties);
        }

    }
}