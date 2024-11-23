using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using C971.Models;
using SQLite;

namespace C971.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection? _db;
        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var databasePath =
                Path.Combine(FileSystem.AppDataDirectory, "Terms.db");
            _db = new SQLiteAsyncConnection(databasePath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
        }

        #region Term methods

        public static async Task AddTerm(string name, DateTime startDate, DateTime endDate) // List<Course> courses
        {
            await Init();
            var term = new Term()
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
            };

            await _db.InsertAsync(term);

            var id = term.Id;
        }

        public static async Task RemoveTerm(int id)
        {
            await Init();

            await _db.DeleteAsync<Term>(id);
        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var terms = await _db.Table<Term>().ToListAsync();
            return terms;
        }

        public static async Task UpdateTerm(int id, string name, DateTime startDate, DateTime endDate) //List<Course> courses
        {
            await Init();

            var termQuery = await _db.Table<Term>()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Name = name;
                termQuery.StartDate = startDate;
                termQuery.EndDate = endDate;

                await _db.UpdateAsync(termQuery);
            }
        }


        #endregion

        #region Course methods

        public static async Task AddCourse(int termId, string name, DateTime startDate, DateTime endDate,
            string instructorName, string instructorPhone, string instructorEmail, Course.StatusType statusType, bool startNotification, bool endNotification) 
        {
            await Init();

            var course = new Course
            {
                TermId = termId,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,
                Status = statusType,
                StartNotification = startNotification,
                EndNotification = endNotification
            };

            await _db.InsertAsync(course);

            var id = course.Id;
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await _db.DeleteAsync<Course>(id);
        }

        public static async Task<IEnumerable<Course>> GetCourses(int termId)
        {
            await Init();

            var courses = await _db.Table<Course>().Where(c => c.TermId == termId).ToListAsync();

            return courses;
        }

        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();

            var courses = await _db.Table<Course>().ToListAsync();

            return courses;
        }

        public static async Task UpdateCourse(int id, string name, DateTime startDate, DateTime endDate,
            string instructorName, string instructorPhone, string instructorEmail, Course.StatusType statusType, string notes, bool startNotification, bool endNotification) 
        {
            await Init();

            var courseQuery = await _db.Table<Course>()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Name = name;
                courseQuery.StartDate = startDate;
                courseQuery.EndDate = endDate;
                courseQuery.InstructorName = instructorName;
                courseQuery.InstructorPhone = instructorPhone;
                courseQuery.InstructorEmail = instructorEmail;
                courseQuery.Status = statusType;
                courseQuery.Notes = notes;
                courseQuery.StartNotification = startNotification;
                courseQuery.EndNotification = endNotification;

                await _db.UpdateAsync(courseQuery);
            }
        }


        #endregion

        #region Assessment methods

        public static async Task AddAssessment(int courseId, string name, DateTime startDate, DateTime endDate,
            Assessment.AssessmentType assessmentType, bool startNotification, bool endNotification)
        {
            await Init();

            var assessment = new Assessment
            {
                CourseId = courseId,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                Type = assessmentType,
                StartNotification = startNotification,
                EndNotification = endNotification
            };

            await _db.InsertAsync(assessment);

            var id = assessment.Id;
        }

        public static async Task RemoveAssessment(int id)
        {
            await Init();

            await _db.DeleteAsync<Assessment>(id);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments(int courseId)
        {
            await Init();

            var assessments = await _db.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();

            return assessments;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();

            var assessments = await _db.Table<Assessment>().ToListAsync();
            return assessments;
        }

        public static async Task UpdateAssessment(int id, string name, DateTime startDate, DateTime endDate,
            Assessment.AssessmentType assessmentType, bool startNotification, bool endNotification)
        {
            await Init();

            var assessmentQuery = await _db.Table<Assessment>()
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Name = name;
                assessmentQuery.StartDate = startDate;
                assessmentQuery.EndDate = endDate;
                assessmentQuery.Type = assessmentType;
                assessmentQuery.StartNotification = startNotification;
                assessmentQuery.EndNotification = endNotification;

                await _db.UpdateAsync(assessmentQuery);
            }
        }


        #endregion

        #region DemoData

        public static async Task LoadSampleData()
        {
            await Init();
            Term term = new Term
            {
                Name = "Term 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(6),
            };

            await _db.InsertAsync(term);

            Course course = new Course
            {
                TermId = term.Id,
                Name = "Intro to Math",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                Status = Course.StatusType.InProgress,
            };

            await _db.InsertAsync(course);

            Assessment objectiveAssessment = new Assessment
            {
                CourseId = course.Id,
                Name = "Intro to Math OA",
                StartDate = DateTime.Now.AddDays(15),
                EndDate = DateTime.Now.AddMonths(30),
                Type = Assessment.AssessmentType.Objective,
                StartNotification = true,
                EndNotification = true,
            };

            await _db.InsertAsync(objectiveAssessment);

            Assessment performanceAssessment = new Assessment
            {
                CourseId = course.Id,
                Name = "Intro to Math PA",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(14),
                Type = Assessment.AssessmentType.Performance,
                StartNotification = true,
                EndNotification = true,
            };

            await _db.InsertAsync(performanceAssessment);
        }

        public static async Task ClearSampleData()
        {
            await Init();

            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Assessment>();

            _db = null;

            Settings.ClearSettings();
        }

        #endregion

        #region Count Determinations

        public static async Task<int> GetCourseCountAsync(int selectedTermId)
        {
            int courseCount =
                await _db.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM Course WHERE TermId = ?", selectedTermId);

            return courseCount;
        }

        public static async Task<int> GetAssessmentCountAsync(int selectedCourseId)
        {
            int assessmentCount =
                await _db.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM Assessment WHERE CourseId = ?",
                    selectedCourseId);

            return assessmentCount;
        }

        public static async Task<int> GetObjectiveAssessmentCountAsync(int selectedCourseId)
        {
            int objectiveCount = await _db.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM Assessment WHERE CourseId = ? AND Type = ?", selectedCourseId, (int)Assessment.AssessmentType.Objective);
            return objectiveCount;
        }

        public static async Task<int> GetObjectiveAssessmentCountExcludingAsync(int courseId, int assessmentIdToExclude)
        {
            await Init();
            int objectiveCount = await _db.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM Assessment WHERE CourseId = ? AND Type = ? AND Id != ?",
                courseId, (int)Assessment.AssessmentType.Objective, assessmentIdToExclude);
            return objectiveCount;
        }

        public static async Task<int> GetPerformanceAssessmentCountAsync(int selectedCourseId)
        {
            int performanceCount = await _db.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM Assessment WHERE CourseId = ? AND Type = ?", selectedCourseId, (int)Assessment.AssessmentType.Performance);
            return performanceCount;
        }

        public static async Task<int> GetPerformanceAssessmentCountExcludingAsync(int courseId,
            int assessmentIdToExclude)
        {
            await Init();
            int performanceCount = await _db.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM Assessment WHERE CourseId = ? AND Type = ? AND Id != ?",
                courseId, (int)Assessment.AssessmentType.Performance, assessmentIdToExclude);
            return performanceCount;
        }
        #endregion
    }
}
