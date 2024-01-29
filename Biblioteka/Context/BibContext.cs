using Biblioteka.Areas.Identity.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteka.Context
{
    public class BibContext : IdentityDbContext<BibUser>
    {
        public BibContext(DbContextOptions<BibContext> options) : base(options)
        {

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<RentalBook> RentalBook { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AdminSettings> AdminSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Konfiguracja danych nowych użytkowników
            modelBuilder.ApplyConfiguration(new BibUserEntityConfiguration());


            modelBuilder.Entity<Author>().ToTable("Authors");      // <- nie działa idk why
        }


        public DbSet<Biblioteka.Models.Category> Category { get; set; } = default!;
    }

}

internal class BibUserEntityConfiguration : IEntityTypeConfiguration<BibUser>
{
    public void Configure(EntityTypeBuilder<BibUser> builder)
    {
        builder.Property(u => u.name).IsRequired().HasMaxLength(20);
        builder.Property(u => u.surname).IsRequired().HasMaxLength(40);
        builder.Property(u => u.Email).HasMaxLength(40);
    }
}