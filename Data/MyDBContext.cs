    using CarritoDeComprasMVC.Models;
    using Microsoft.EntityFrameworkCore;

    namespace CarritoDeComprasMVC.Data;

    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
        
        public DbSet<User> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(60)
                    .IsRequired();

                e.Property(p => p.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(p => p.Password)
                    .IsRequired();
            });
        }

    }
