using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBUI.Models.FigureStore
{
    public class FigureStoreCreatingAndEditingView
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Figure store name")]
        [StringLength(10, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
    }
}