using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_NET_HW_11.Models.Database {
    [Table("Session")]
    public class Session {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public DateTime StartDateTime { get; set; }

        public double Price { get; set; }

        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

        public ICollection<Schedule> Schedules = new List<Schedule>();
    }
}