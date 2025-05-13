/*
 * Module Title: EmployeeViewModel.cs
 * Coder: Kenneth Rose
 * Purpose: Creates the View Model for the employees
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
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;
        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? Id { get; set; }
        public bool? IsTech { get; set; }
        public string? StaffPicture64 { get; set; }
        // constructor
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        public async Task GetByEmail(string email)
        {
            try
            {
                Employee emp = await _dao.GetByEmail(email);

                //data manupulation for our logical layer
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "not found";
            }
            catch (Exception ex)
            {
                Email = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        
        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new();
            List<Employee> allEmployees = new();
            try
            {
                allEmployees = await _dao.GetAll();

                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVM = new()
                    {
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        IsTech = emp.IsTech,
                        Timer = Convert.ToBase64String(emp.Timer)
                    };

                    if (emp.StaffPicture != null)
                    {
                        empVM.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    }

                    allVms.Add(empVM);
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

        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new()
                {
                    Title = this.Title,
                    FirstName = this.Firstname,
                    LastName = this.Lastname,
                    PhoneNo = this.Phoneno,
                    Email = this.Email,
                    DepartmentId = this.DepartmentId,
                    IsTech = this.IsTech,
                    StaffPicture = this.StaffPicture64 != null ? Convert.FromBase64String(this.StaffPicture64!) : null
                };
                Id = await _dao.Add(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                Employee emp = new()
                {
                    Title = this.Title,
                    FirstName = this.Firstname,
                    LastName = this.Lastname,
                    PhoneNo = this.Phoneno,
                    Email = this.Email,
                    Id = (int)this.Id!,
                    DepartmentId = this.DepartmentId,
                    IsTech = this.IsTech,
                    StaffPicture = this.StaffPicture64 != null ? Convert.FromBase64String(this.StaffPicture64!) : null,
                    Timer = Convert.FromBase64String(Timer!)
                };
                
                updateStatus = Convert.ToInt16(await _dao.Update(emp)); // overwrite status
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }

        public async Task GetByID(int Id)
        {
            try
            {
                Employee emp = await _dao.GetByID((Id!));
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                this.Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<int> Delete()
        {
            try
            {
                // dao will return # of rows deleted
                return await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task GetByPhoneNumber(string phoneNum)
        {
            try
            {
                Employee emp = await _dao.GetByPhoneNumber(phoneNum);

                //data manupulation for our logical layer
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech ?? false;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        
    }

}
