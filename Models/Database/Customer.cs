using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_NET_HW_11.Models.Database {
    [Table("Customer")]
    public class Customer {
        public int Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}