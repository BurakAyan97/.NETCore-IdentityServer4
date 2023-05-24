using Microsoft.EntityFrameworkCore;

namespace UdemyIdentityServer.AuthServer.Models
{
    //Custom üyelik sistemi konusu anlatırken oluşturuldu.Başlarda böyle bir class yok.
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> opts) : base(opts)
        {

        }
        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser() { Id = 1, Email = "burakayan@outlook.com", Password = "password", UserName = "burak61", City = "İstanbul" },
                new CustomUser() { Id = 2, Email = "ahmet@outlook.com", Password = "password", UserName = "ahmet61", City = "Ankara" },
                new CustomUser() { Id = 3, Email = "mehmet@outlook.com", Password = "password", UserName = "mehmet61", City = "Konya" }
                );



            base.OnModelCreating(modelBuilder);
        }
    }
}
