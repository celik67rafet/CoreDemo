using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class Context: IdentityDbContext<AppUser,AppRole, int> // aspnet user tablosuyla ilişkilendirmek için
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=94.73.170.52;Database=u1933038_CoreDb;User Id=u1933038_user599;Password=L7@B2mzk_O:9o=Z1;TrustServerCertificate=True;");
            //trustservercertificate biz ekledik true, migration esnasında gelen hatayı engelledi.
            
        }
        protected override void OnModelCreating( ModelBuilder modelBuilder ) 
        {
            modelBuilder.Entity<Match>()
               .HasOne( x => x.HomeTeam )
               .WithMany( y => y.HomeMatches )
               .HasForeignKey( z => z.HomeTeamID )
               .OnDelete( DeleteBehavior.ClientSetNull );

            modelBuilder.Entity<Match>()
                .HasOne( x => x.GuestTeam )
                .WithMany( y => y.AwayMatches )
                .HasForeignKey( z => z.GuestTeamID )
                .OnDelete( DeleteBehavior.ClientSetNull );

            modelBuilder.Entity<Message2>()
                .HasOne( x => x.SenderUser )
                .WithMany( y => y.WriterSender )
                .HasForeignKey( z => z.SenderID )
                .OnDelete( DeleteBehavior.ClientSetNull );

            modelBuilder.Entity<Message2>()
                .HasOne( x => x.ReceiverUser )
                .WithMany( y => y.WriterReceiver )
                .HasForeignKey( z => z.ReceiverID )
                .OnDelete( DeleteBehavior.ClientSetNull );

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.BlogRayting) // Blog bir BlogRayting'e sahiptir
                .WithOne(br => br.Blog)      // BlogRayting bir Blog'a sahiptir
                .HasForeignKey<BlogRayting>(br => br.BlogID); // BlogRayting'teki foreign key BlogID'dir

            base.OnModelCreating(modelBuilder); // identity ekledikten sonra migration'da hata aldık ve bunu ekledik.

            // tablolara trigger eklendikten sonra OUTPUT hatası alırsan her bir trigger için aşağıdakileri yaz:

            //============================ TRIGGER TANITMA BAŞLANGIÇ =================================

            modelBuilder.Entity<Blog>(entry =>
            {
                entry.ToTable("Blogs", tb => tb.HasTrigger("AddBlogInRaytingTable"));
            });

            modelBuilder.Entity<Comment>( entry =>
            {

                entry.ToTable("Comments", tb => tb.HasTrigger("AddScoreInComment"));

            } );

            modelBuilder.Entity<Blog>(entry =>
            {
                entry.ToTable("Blogs", tb => tb.HasTrigger("AddBlogInRaytingTable"));
            });

            // ===================== TRIGGER TANITMA BİTİŞ =========================

            // HomeMatches --> WriterSender
            // AwayMatches --> WriterReceiver

            //HomeTeam --> SenderUser
            //AwayTeam --> ReceiverUser
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Writer>  Writers{ get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet <NewsLetter> NewsLetters { get; set; }
        public DbSet <BlogRayting> BlogRaytings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Message2> Messages2 { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
