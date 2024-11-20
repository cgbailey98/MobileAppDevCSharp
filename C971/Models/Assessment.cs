using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace C971.Models
{
    public class Assessment
    {
        public enum AssessmentType
        {
            None,
            Objective,
            Performance
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public AssessmentType Type { get; set; } = AssessmentType.None;
    }
}
