using System.ComponentModel.DataAnnotations;

namespace LiteraturePlatform.Models
{
    public class Composition
    {
        public int CompositionId { get; set; }
        public string Name { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public DateTime Date { get; set; }
        public double Rating { get; set; }

        public int TextId { get; set; }
        public Text Text { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public byte[]? Image { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
