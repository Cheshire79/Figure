using System.ComponentModel.DataAnnotations;


namespace WEBUI.Models.Figures.Square
{
    public class SquareCreatingAndEditingView
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Square name")]
        [StringLength(10, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [RegularExpression(@"^\d*\,?\d*", ErrorMessage = "Invalid height value")]
        public //float 
         string Side { get; set; }

    }
}