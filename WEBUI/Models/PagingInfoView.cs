using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models
{
    public class PagingInfoView
    {
         public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages()
        {
           return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); 
        }
        // @Html.PageLinks(Model.PagingInfo, x => Url.Action("List", new { page = x+1 }))
    }
}