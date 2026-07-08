using Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositoires.GenericRepositories
{
    public class GradeRepo : GenericRepository<Grade,int>, IGradeRepo
    {
        private readonly AppDbContext _context;

        public GradeRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grade>> GetAllWithStudentAsync()
        {
            return await _context.Grades
                .Include(g => g.Student)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Where(g => g.StudentID == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetBySubjectAsync(string subject)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Where(g => g.Subject != null &&
                            g.Subject.ToLower() == subject.ToLower())
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetAllForAverageAsync()
        {
            return await _context.Grades
                .Include(g => g.Student)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
