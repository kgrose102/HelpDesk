using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class CallDAO
    {
        readonly IRepository<Call> _repo;
        public CallDAO()
        {
            _repo = new EmployeeRepository<Call>();
        }

        public async Task<int> Add(Call newCall)
        {
            try
            {
                await _repo.Add(newCall);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return newCall.Id;
        }


        public async Task<int> Delete(int? id)
        {
            return await _repo.Delete((int)id!);
        }


        public async Task<List<Call>> GetAll()
        {
            return await _repo.GetAll();
        }


        public async Task<Call> GetByID(int Id)
        {
            return await _repo.GetOne(emp => emp.Id == Id);
        }

        public async Task<UpdateStatus> Update(Call updatedCall)
        {
            return await _repo.Update(updatedCall);
        }


    }
}
