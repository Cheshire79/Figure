using System.ComponentModel.DataAnnotations;

namespace WEBUI.Models.Figures.Circle
{
    public class CircleCreatingAndEditingView
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Circle name")]
        [StringLength(10, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d*\,?\d*", ErrorMessage = "Invalid radious value")]
        public //float 
         string  Radius { get; set; }

    }
}