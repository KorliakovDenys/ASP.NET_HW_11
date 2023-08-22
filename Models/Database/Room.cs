using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_NET_HW_11.Models.Database {
    [Table("Room")]
    public class Room {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Difficulty { get; set; }

        public int FareRate { get; set; }

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}