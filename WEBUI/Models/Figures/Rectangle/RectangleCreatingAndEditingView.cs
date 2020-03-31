using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures.Rectangle
{
    public class RectangleCreatingAndEditingView
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rectangle name")]
        [StringLength(10, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [RegularExpression(@"^\d*\,?\d*", ErrorMessage = "Invalid height value")]
        public //float 
         string Height { get; set; }

        [Required]
        [RegularExpression(@"^\d*\,?\d*", ErrorMessage = "Invalid width value")]
        public //float 
         string Width { get; set; }
    }
}