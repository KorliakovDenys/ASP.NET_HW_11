using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_NET_HW_11.Models.Database {
    [Table("Schedule")]
    public class Schedule {
        [Key]
        public int Id { get; set; }

        public int SessionId { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("SessionId")]
        public Session? Session { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
    }
}