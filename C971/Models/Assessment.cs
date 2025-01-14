﻿using System;
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
            None = 0,
            Objective = 1,
            Performance = 2
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public AssessmentType Type { get; set; } = AssessmentType.None;
        public bool StartNotification { get; set; }
        public bool EndNotification { get; set; }
    }

}
