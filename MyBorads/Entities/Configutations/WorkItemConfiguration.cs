using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBorads.Entities.Configutations;

    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> eb)
        {

        eb.HasOne(w => w.State)
            .WithMany()
            .HasForeignKey(w => w.StateId);


        eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
        eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");




        eb.Property(wi => wi.Priority).HasDefaultValue(1);
        eb.HasMany(w => w.Comments) //relacja 1:wielu gdzie workitem ma wiele komentarzy , a te komentarze maja jeden workitem
        .WithOne(c => c.WorkItem)
        .HasForeignKey(c => c.WorkItemId);

        //relacja 1:wielu , jeden uzytkownik moze miec wiele workitemow
        eb.HasOne(w => w.Author)
        .WithMany(u => u.WorkItems)
        .HasForeignKey(wi => wi.AuthorId);

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


    }
    }


