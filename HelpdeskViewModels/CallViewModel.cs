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
    public class CallViewModel
    {
        private readonly CallDAO _dao;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ProblemDescription { get; set; }
        public string? TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string? Notes { get; set; }
        public string? Timer { get; set; }

        public CallViewModel()
        {
            _dao = new CallDAO();
        }

        public async Task Add()
        {
            Id = -1;
            try
            {
                Call cal = new()
                {
                    EmployeeId = this.EmployeeId,
                    ProblemId = this.ProblemId,
                    TechId = this.TechId,
                    DateOpened = this.DateOpened,
                    DateClosed = this.DateClosed,
                    OpenStatus = this.OpenStatus,
                    Notes = this.Notes
                };
                Id = await _dao.Add(cal);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<List<CallViewModel>> GetAll()
        {
            List<CallViewModel> allVms = new();
            List<Call> allCalls = new();
            try
            {
                allCalls = await _dao.GetAll();

                foreach (Call cal in allCalls)
                {
                    CallViewModel callVM = new()
                    {
                        Id = cal.Id,
                        EmployeeId = cal.EmployeeId,
                        ProblemId = cal.ProblemId,
                        EmployeeName = cal.Employee.LastName,
                        TechName = cal.Tech.LastName,
                        TechId = cal.TechId,
                        DateOpened = cal.DateOpened,
                        DateClosed = cal.DateClosed,
                        Notes = cal.Notes,
                        Timer = Convert.ToBase64String(cal.Timer!),
                        ProblemDescription = cal.Problem.Description
                        


                        //List < Employee > empCall = await EmployeeViewModel(GetByID(EmployeeId))
                    };

                   allVms.Add(callVM);
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

        public async Task GetByID(int Id)
        {
            try
            {
                Call cal = await _dao.GetByID((Id!));
                this.Id = cal.Id;
                EmployeeId = cal.EmployeeId;
                ProblemId = cal.ProblemId;
                TechId = cal.TechId;
                DateOpened = cal.DateOpened;
                DateClosed = cal.DateClosed;
                Notes = cal.Notes;
                Timer = Convert.ToBase64String(cal.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Notes = "not found";
            }
            catch (Exception ex)
            {
                Notes = "not found";
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
                Call cal = new()
                {
                    Id = (int)this.Id!,
                    EmployeeId = this.EmployeeId,
                    ProblemId = this.ProblemId,
                    TechId = this.TechId,
                    DateOpened = this.DateOpened,
                    DateClosed = this.DateClosed,
                    OpenStatus = this.OpenStatus,
                    Notes = this.Notes,
                    Timer = Convert.FromBase64String(Timer!)
                };

                updateStatus = Convert.ToInt16(await _dao.Update(cal)); // overwrite status
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
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

    }
}
