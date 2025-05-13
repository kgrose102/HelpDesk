using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        readonly IRepository<Problem> _repo;
        public ProblemDAO()
        {
            _repo = new EmployeeRepository<Problem>();
        }

        public async Task<List<Problem>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Problem> GetByDescription(string description)
        {
            return await _repo.GetOne(prob => prob.Description == description);
        }
    }
}
