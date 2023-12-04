using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
    public class UpdateCategoryDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(2500)]
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
