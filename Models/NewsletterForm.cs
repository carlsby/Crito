using System.ComponentModel.DataAnnotations;

namespace Crito.Models
{
    public class NewsletterForm
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}
