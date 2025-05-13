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
    public class ProblemViewModel
    {
        readonly private ProblemDAO _dao;

        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        public int? Id { get; set; }

        public string? Description { get; set; }

        public async Task<List<ProblemViewModel>> GetAll()
        {
            List<ProblemViewModel> allVms = new();

            try
            {
                List<Problem> allProblems = await _dao.GetAll();

                foreach (Problem prob in allProblems)
                {
                    ProblemViewModel depVM = new()
                    {
                        Id = prob.Id,
                        Description = prob.Description

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

        //public async Task<List<Problem>> GetByDescription(string description)
        //{
        //    try
        //    {
        //        Problem prob = await _dao.GetByDescription(description);
                
        //        Id = prob.Id;
        //        Description = prob.Description;
                
        //    }
        //    catch (NullReferenceException nex)
        //    {
        //        Debug.WriteLine(nex.Message);
        //        Description = "not found";
        //    }
        //    catch (Exception ex)
        //    {
        //        Description = "not found";
        //        Debug.WriteLine("Problem in " + GetType().Name + " " +
        //        MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
        //        throw;
        //    }
        //}
    }
}
