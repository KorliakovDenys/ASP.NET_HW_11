using ASP_NET_HW_11.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_HW_11.Data {
    public class DataContext : DbContext {
        public DbSet<Customer>? Customers { get; set; }

        public DbSet<Room>? Rooms { get; set; }

        public DbSet<Schedule>? Schedules { get; set; }

        public DbSet<Session>? Sessions { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Session)
                .WithMany(sess => sess.Schedules)
                .HasForeignKey(s => s.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(sess => sess.Room)
                .WithMany(room => room.Sessions)
                .HasForeignKey(sess => sess.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}