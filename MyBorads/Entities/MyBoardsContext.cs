using Microsoft.EntityFrameworkCore;

namespace MyBorads.Entities
{
    public class MyBoardsContext : DbContext   //ta klasa reprezentuje cala baze danych
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }
        //teraz zdefiniujemy tabele ktore beda sie w bazie danych znajdowac

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           

            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.State).IsRequired();
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.Efford).HasColumnType("decimal(5,2)");
                eb.Property(wi => wi.EndDate).HasPrecision(3);
                eb.Property(wi => wi.Activity).HasMaxLength(200);
                eb.Property(wi => wi.RemaningWork).HasPrecision(14, 2);
                eb.Property(wi => wi.Priority).HasDefaultValue(1);
                eb.HasMany(w => w.Comments) //relacja 1:wielu gdzie workitem ma wiele komentarzy , a te komentarze maja jeden workitem
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c =>c.WorkItemId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();

            });

            modelBuilder.Entity<User>() //konfiguracja relacji 1:1 poprzez Entity
            .HasOne(u => u.Adress)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);


         
        }


    }
}
