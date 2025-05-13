/*
 * Module Title: DepartmentDAO.cs
 * Coder: Kenneth Rose
 * Purpose: Creation of the Data Access Object for the Department entity
 * Date: Oct. 27, 2024
 */

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDal;

public class DepartmentDAO
{ 
    readonly IRepository<Department> _repo;
    public DepartmentDAO()
    {
        _repo = new EmployeeRepository<Department>();
    }
       
    public async Task<List<Department>> GetAll()
    {
        return await _repo.GetAll();
    }
    
}
