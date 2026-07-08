using Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositoires.GenericRepositories
{
    public class StudentNoteRepo : GenericRepository<StudentNote, int>, IStudentNoteRepo
    {
        private readonly AppDbContext _context;
        public StudentNoteRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
