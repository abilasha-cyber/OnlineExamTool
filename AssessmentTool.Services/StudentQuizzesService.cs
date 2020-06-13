using AssessmentTool.Data;
using AssessmentTool.Entities;
using AssessmentTool.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentTool.Services
{
    public class StudentQuizzesService
    {
        #region Define as Singleton
        private static StudentQuizzesService _Instance;

        public static StudentQuizzesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new StudentQuizzesService();
                }

                return (_Instance);
            }
        }

        private StudentQuizzesService()
        {
        }
        #endregion
        public StudentQuizzesSearch GetQuizAttempts(string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new AssessmentToolContext())
            {
                var search = new StudentQuizzesSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.StudentQuizzes = context.StudentQuizzes
                                        .Include("AttemptedQuestions")
                                        .Include("Quiz")
                                        .Include("Quiz.Questions")
                                        .Include("Quiz.Questions.Options")
                                        .Include("Student")
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.StudentQuizzes.Count();
                }
                else
                {
                    search.StudentQuizzes = context.StudentQuizzes
                                        .Include("AttemptedQuestions")
                                        .Include("Quiz")
                                        .Include("Quiz.Questions")
                                        .Include("Quiz.Questions.Options")
                                        .Include("Student")
                                        .Where(x => x.Quiz.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.StudentQuizzes.Include(q => q.Quiz)
                                        .Where(x => x.Quiz.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public StudentQuizzesSearch GetUserQuizAttempts(string userID, string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new AssessmentToolContext())
            {
                var search = new StudentQuizzesSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.StudentQuizzes = context.StudentQuizzes
                                        .Where(x => x.StudentID == userID)
                                        .Include("AttemptedQuestions")
                                        .Include("Quiz")
                                        .Include("Quiz.Questions")
                                        .Include("Quiz.Questions.Options")
                                        .Include("Student")
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.StudentQuizzes
                                        .Where(x => x.StudentID == userID).Count();
                }
                else
                {
                    search.StudentQuizzes = context.StudentQuizzes
                                        .Where(x => x.StudentID == userID)
                                        .Include("AttemptedQuestions")
                                        .Include("Quiz")
                                        .Include("Quiz.Questions")
                                        .Include("Quiz.Questions.Options")
                                        .Include("Student")
                                        .Where(x => x.Quiz.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.StudentQuizzes
                                        .Where(x => x.StudentID == userID).Include(q => q.Quiz)
                                        .Where(x => x.Quiz.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public StudentQuiz GetStudentQuiz(int ID)
        {
            using (var context = new AssessmentToolContext())
            {
                return context.StudentQuizzes
                    .Where(x=>x.ID == ID)
                    .Include("AttemptedQuestions")
                    .Include("AttemptedQuestions.SelectedOptions")
                    .Include("AttemptedQuestions.SelectedOptions.Option")
                    .Include("AttemptedQuestions.SelectedOptions.Option.Image")
                    .Include("Quiz")
                    .Include("Quiz.Questions")
                    .Include("Quiz.Questions.Options")
                    .Include("Quiz.Questions.Options.Image")
                    .Include("Student")
                    .FirstOrDefault();
            }
        }

        public async Task<bool> NewStudentQuiz(StudentQuiz studentQuiz)
        {
            using (var context = new AssessmentToolContext())
            {
                context.StudentQuizzes.Add(studentQuiz);

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateStudentQuiz(StudentQuiz studentQuiz)
        {
            using (var context = new AssessmentToolContext())
            {
                context.Entry(studentQuiz).State = System.Data.Entity.EntityState.Modified;

                return await context.SaveChangesAsync() > 0;
            }
        }
        
        public async Task<bool> NewAttemptedQuestion(AttemptedQuestion attemptedQuestion)
        {
            using (var context = new AssessmentToolContext())
            {
                context.AttemptedQuestions.Add(attemptedQuestion);

                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
