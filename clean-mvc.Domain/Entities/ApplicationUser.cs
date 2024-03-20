using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace clean_mvc.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string EmplCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}


