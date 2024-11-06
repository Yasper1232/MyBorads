using Microsoft.EntityFrameworkCore;
using MyBorads.Entities.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace MyBorads.Entities
{
    public class MyBoardsContext : DbContext   //ta klasa reprezentuje cala baze danych
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }
        //teraz zdefiniujemy tabele ktore beda sie w bazie danych znajdowac

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public DbSet<WorkItemTag> WorkItemTag { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkItemState> WorkItemsStates { get; set; }
        public DbSet<TopAuthor> ViewTopAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItemState>(eb =>
            {
                eb.Property(s => s.Value).IsRequired();
                eb.Property(s => s.Value).HasMaxLength(60);
            });

            modelBuilder.Entity<Epic>()
                .Property(wi => wi.EndDate)
                .HasPrecision(3);

            modelBuilder.Entity<Task>(ta => {
                ta.Property(wi => wi.Activity).HasMaxLength(200);
                ta.Property(wi => wi.RemaningWork).HasPrecision(14, 2);
                });

            modelBuilder.Entity<Issue>().Property(x => x.Efford).HasColumnType("decimal(5,2)");




            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.HasOne(w => w.State)
                .WithMany()
                .HasForeignKey(w => w.StateId);


                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                
               
                
                
                eb.Property(wi => wi.Priority).HasDefaultValue(1);
                eb.HasMany(w => w.Comments) //relacja 1:wielu gdzie workitem ma wiele komentarzy , a te komentarze maja jeden workitem
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c =>c.WorkItemId);

                    //relacja 1:wielu , jeden uzytkownik moze miec wiele workitemow
                eb.HasOne(w=>w.Author)
                .WithMany(u=>u.WorkItems)
                .HasForeignKey(wi=>wi.AuthorId);

                eb.HasMany(w => w.Tags)
                .WithMany(t => t.WorkItems)
                .UsingEntity<WorkItemTag>(
                    w => w.HasOne(wit => wit.Tag)
                    .WithMany()
                    .HasForeignKey(wit => wit.TagId),

                     w => w.HasOne(wit => wit.WorkItem)
                    .WithMany()
                    .HasForeignKey(wit => wit.WorkItemId),

                     wit =>
                     {
                         wit.HasKey(x => new { x.TagId, x.WorkItemId });
                         wit.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                     }

                    );
      

                

                

            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();

                eb.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.ClientCascade);
                
            });

            modelBuilder.Entity<User>() //konfiguracja relacji 1:1 poprzez Entity
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);


            modelBuilder.Entity<WorkItemState>()
                .HasData(new WorkItemState() {Id = 1, Value = "To Do" },
                new WorkItemState() {Id = 2,Value = "Doing" },
                new WorkItemState() {Id = 3, Value = "Done" });

            modelBuilder.Entity<Tag>()
                .HasData(new Tag() {Id=1,Value="Web" },
                new Tag() { Id = 2, Value ="UI" },
                new Tag() { Id = 3, Value ="Desktop" },
                new Tag() { Id = 4, Value ="API" },
                new Tag() { Id = 5, Value ="Service" });

            modelBuilder.Entity<TopAuthor>(eb =>
            {
                eb.ToView("View_TopAuthors");
                eb.HasNoKey();
            });


            modelBuilder.Entity<Address>()
                .OwnsOne(a => a.Coordinate, cmb =>
                {
                    cmb.Property(c => c.Latitude).HasPrecision(18, 7);
                    cmb.Property(c => c.Longitude).HasPrecision(18, 7);

                    
                });

            
            
        }


    }
}
