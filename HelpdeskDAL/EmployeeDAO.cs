/*
 * Module Title: EmployeeDAO.cs
 * Coder: Kenneth Rose
 * Purpose: Create the Employee Data Access Object
 * Date: Oct. 27, 2024
 */

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDal
{

    public class EmployeeDAO
    {
        readonly IRepository<Employee> _repo;
        public EmployeeDAO()
        {
            _repo = new EmployeeRepository<Employee>();
        }

        public async Task<Employee> GetByEmail(string email)
        {
            return await _repo.GetOne(emp => emp.Email == email);

        }

        public async Task<int> Add(Employee newEmployee)
        {
            try
            {
                await _repo.Add(newEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return newEmployee.Id;
        }


        public async Task<int> Delete(int? id)
        {
            return await _repo.Delete((int)id!);
        }


        public async Task<List<Employee>> GetAll()
        {
            return await _repo.GetAll();
        }


        public async Task<Employee> GetByID(int Id)
        {
            return await _repo.GetOne(emp => emp.Id == Id);
        }


        public async Task<Employee> GetByPhoneNumber(String phoneNumber)
        {
            return await _repo.GetOne(emp => emp.PhoneNo == phoneNumber);
        }

        public async Task<UpdateStatus> Update(Employee updatedEmployee)
        {
            return await _repo.Update(updatedEmployee);
        }

    }
}
