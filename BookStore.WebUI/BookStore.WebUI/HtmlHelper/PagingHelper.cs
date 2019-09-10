using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.HtmlHelper
{
    public static class PagingHelper
    {
        //this hat5ly pageLinks aknha goz2 aw emtdad mn HtmlHelper
        public static MvcHtmlString PageLinks(
            this System.Web.Mvc.HtmlHelper html ,PagingInfo pageInfo,
            //hata5od rakm w trag3 nas
            Func<int,string> pageUrl
            )
        {
            //string bmsa7at kbera w feh function zyada
            StringBuilder result = new StringBuilder();
            for(int i=1 ;i<=pageInfo.TotalPages;i++)
            {
                //bgm3 mn 5lalh tagat el html
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                //ben <a>  w </a>
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                    tag.AddCssClass("btn btn-default");
                    result.Append(tag.ToString());

            }
            return MvcHtmlString.Create( result.ToString());
        }
    }
}