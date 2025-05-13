/*
 * Module Title: DepartmentViewModel.cs
 * Coder: Kenneth Rose
 * Purpose: Creates the View model for the department aspects
 * Date: Oct. 27, 2024
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDal;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        readonly private DepartmentDAO _dao;

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public string? DepartmentName { get; set; }

        public int? Id { get; set; }

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allVms = new();
            try
            {
                List<Department> allDepartments = await _dao.GetAll();

                foreach (Department dep in allDepartments)
                {
                    DepartmentViewModel depVM = new()
                    {
                        Id = dep.Id,
                        DepartmentName = dep.DepartmentName,
                        
                    };
                    allVms.Add(depVM);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return allVms;
        }

        
    }

}
