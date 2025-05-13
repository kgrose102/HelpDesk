/*
 * Module Title: ViewModelTests.cs
 * Coder: Kenneth Rose
 * Purpose: Test the functionality of the ViewModel
 * Date: Oct. 27, 2024
 */
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        private readonly ITestOutputHelper output;

        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        // Test the getByEmail
        [Fact]
        public async Task Vm_GetByEmailTest()
        {
            EmployeeViewModel vm = new() { };
            await vm.GetByEmail("pp@abc.com");
            Assert.True(vm.Id > 0);
        }

        // Test the getAll
        [Fact]
        public async Task Vm_GetAllTest()
        {
            List<EmployeeViewModel> allEmployeesVms;
            EmployeeViewModel vm = new();
            allEmployeesVms = await vm.GetAll();
            Assert.True(allEmployeesVms.Count > 0);

        }

        // Test the addTest
        [Fact]
        public async Task Vm_AddTest()
        {
            EmployeeViewModel vm;
            vm = new()
            {
                Firstname = "Kenneth", //Change before each test
                Lastname = "Remove",
                Phoneno = "(555)867-5309",
                Email = "kr@schoolschool.com",
                Title = "Mr.",
                DepartmentId = 200,
                IsTech = true,
                StaffPicture64 = null 
            };
            await vm.Add();
            Assert.True(vm.Id > 0);

        }

        // Test the update
        [Fact]
        public async Task Vm_UpdateTest()
        {
            EmployeeViewModel vm = new() { };
            await vm.GetByID(13); // Student just added in Add test
            vm.Email = vm.Email == "some@abc.com" ? "some2@abc.com" : "some@abc.com";
            // will be -1 if failed 0 if no data changed, 1 if succcessful
            Assert.True(await vm.Update() == 1);
        }

        // Test the getById
        [Fact]
        public async Task Vm_GetByIdTest()
        {
            int Id = 2;
            EmployeeViewModel vm = new() { Id=2};
            await vm.GetByID(Id);
            Assert.True(vm.Id > 0);
        }

        // Test the delete
        [Fact]
        public async Task Vm_DeleteTest()
        {
            EmployeeViewModel vm = new() { };
            await vm.GetByID(13); // employee just added
            Assert.True(await vm.Delete() == 1); // 1 employee deleted
        }

        // Test the getByPhoneNumber
        [Fact]
        public async Task Vm_GetByPhoneNumber()
        {
            string phoneNum = "(555) 555-5552";
            EmployeeViewModel vm = new() { };
            await vm.GetByPhoneNumber(phoneNum);
            Assert.True(vm.Id > 0);
        }
        [Fact]
        public async Task Employee_ComprehensiveVMTest()
        {
            EmployeeViewModel evm = new()
            {
                Title = "Mr.",
                Firstname = "Some",
                Lastname = "Employee",
                Email = "some@abc.com",
                Phoneno = "(777)777-7777",
                DepartmentId = 100 // ensure department id is in Departments table
            };
            await evm.Add();
            output.WriteLine("New Employee Added - Id = " + evm.Id);
            int? id = evm.Id; // need id for delete later
            await evm.GetByID((int)id);
            output.WriteLine("New Employee " + id + " Retrieved");
            evm.Phoneno = "(555)555-1233";
            if (await evm.Update() == 1)
            {
                output.WriteLine("Employee " + id + " phone# was updated to - " +
               evm.Phoneno);
            }
            else
            {
                output.WriteLine("Employee " + id + " phone# was not updated!");
            }
            evm.Phoneno = "Another change that should not work";
            if (await evm.Update() == -2)
            {
                output.WriteLine("Employee " + id + " was not updated due to stale data");
            }
            evm = new EmployeeViewModel
            {
                Id = id
            };
            // need to reset because of concurrency error
            await evm.GetByID((int)id);
            if (await evm.Delete() == 1)
            {
                output.WriteLine("Employee " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Employee " + id + " was not deleted");
            }
            // should throw expected exception
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async ()
           => await evm.GetByID((int)id));
        }
        [Fact]
        public async Task Call_comprehensiveVMTest()
        {
            CallViewModel cvm = new()
            {
                EmployeeId = 11,
                ProblemId = 9,
                EmployeeName = "Kenneth Rose",
                ProblemDescription = "Memory Upgrade",
                TechName = "Burner",
                TechId = 7,
                DateOpened = DateTime.Now,
                DateClosed = null,
                Notes = "Kenneth Rose's has bad RAM, Burner to fix it"
            };
            await cvm.Add();
            output.WriteLine("New Call Generated - Id = " + cvm.Id);
            int? id = cvm.Id; // need id for delete later
            await cvm.GetByID((int)id);
            //output.WriteLine("New Employee " + id + " Retrieved");
            cvm.Notes = cvm.Notes + "\n Ordered new RAM!";
            if (await cvm.Update() == 1)
            {
                output.WriteLine("Call was updated" + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call Notes not updated");
            }
            cvm.Notes = "Another change that should not work";
            if (await cvm.Update() == -2)
            {
                output.WriteLine("Call was not updated due to stale data");
            }
            cvm = new CallViewModel
            {
                Id = (int)id
            };
            // need to reset because of concurrency error
            await cvm.GetByID((int)id);
            if (await cvm.Delete() == 1)
            {
                output.WriteLine("Employee " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Employee " + id + " was not deleted");
            }
            // should throw expected exception
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async ()
           => await cvm.GetByID((int)id));


        }

        //CallViewModel cvm = new();
        //EmployeeViewModel evm = new();
        //ProblemViewModel pvm = new();
        //cvm.EmployeeId = 11;
        //    pvm.Description = "Memory Upgrade";
            
        //    await pvm.GetByDescription(pvm.Description);
        //cvm.ProblemId = (int) pvm.Id;

        //await evm.GetByID(cvm.EmployeeId);
        //cvm.EmployeeName = evm.Firstname + " " + evm.Lastname;

        //    cvm.ProblemDescription = pvm.Description;
        //    cvm.TechName = "Burner";
        //    cvm.TechId = 7;
        //    cvm.DateOpened = DateTime.Now;
        //    cvm.DateClosed = null;
            
        //    cvm.OpenStatus = true;

        //    cvm.Notes = "Kenneth Rose has bad RAM, Burner to fix it";
    }
}
