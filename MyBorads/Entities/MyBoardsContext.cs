using Microsoft.EntityFrameworkCore;
using MyBorads.Entities.Configutations;
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
            
            //new AddressConfiguration().Configure(modelBuilder.Entity<Address>());
           // new WorkItemConfiguration().Configure(modelBuilder.Entity<WorkItem>());

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);


                }


    }
}
