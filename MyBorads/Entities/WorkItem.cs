using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace MyBorads.Entities
{

    public class Epic : WorkItem
    {
        //epic
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
    public class Issue : WorkItem
    {
        //Issue
        public decimal Efford { get; set; }
    }
    public class Task : WorkItem
    {
        //Task
        public string Activity { get; set; }
        public decimal RemaningWork { get; set; }
    }
    public abstract class WorkItem
    {
        public int Id { get; set; } //klucz glowny
        //public string State { get; set; } wydzielam to do osobnej encji
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; } 


        public List<Comment> Comments { get; set; } = new List<Comment>();
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
        public List<Tag> Tags { get; set; }
        public WorkItemState State { get; set; } //odwolanie
        public int StateId { get; set; } //foreignkey

        // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
    }
}
