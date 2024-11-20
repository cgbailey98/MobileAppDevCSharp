using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace C971.Models
{
    public class Course
    {
        public enum StatusType
        {
            None,
            InProgress,
            Completed,
            Dropped,
            Planned
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public string InstructorName { get; set; } = string.Empty;
        public string InstructorPhone { get; set; } = string.Empty;
        public string InstructorEmail { get; set;} = string.Empty;
        public bool StartNotification { get; set; }
        public StatusType Status { get; set; } = StatusType.None;
        public string Notes { get; set; }
        //public List<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}
