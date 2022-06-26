using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam._7.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Work { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
