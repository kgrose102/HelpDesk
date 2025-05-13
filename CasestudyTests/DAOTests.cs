/*
 * Module Title: DAOTests.cs
 * Coder: Kenneth Rose
 * Purpose: Test the methods defined in the StudentDAO.cs file
 * Date: Oct. 27, 2024
 */

using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDal;
using HelpdeskDAL;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CasestudyTests;



public class DAOTests
{

    private readonly ITestOutputHelper output;

    public DAOTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    // Test the getByEmail
    [Fact]
    public async Task Employee_GetByEmailTest()
    {
        EmployeeDAO dao = new();
        Employee selectedEmployee = await dao.GetByEmail("pp@abc.com");
        Assert.NotNull(selectedEmployee);
        //Assert.NotNull(true) ;
    }
    
    // Test the addTest
    [Fact]
    public async Task Employee_AddTest()
    {
        EmployeeDAO dao = new();
        Employee newEmplyoee = new()
        {
            
            FirstName = "Keityyth", //Change before each test
            LastName = "delete",
            PhoneNo = "(555)867-5309",
            Email = "kddr@someschool.com",
            Title = "Mr.",
            DepartmentId = 200,
            IsTech = true,
            StaffPicture = null
        };
        Assert.True(await dao.Add(newEmplyoee) > 0);
    }
    
    // Test the deleteTest
    [Fact]
    public async Task Employee_DeleteTest()
    {
        EmployeeDAO dao = new();
        Assert.True(await dao.Delete(12) == 1);
        
    }
    
    // Test the getAll
    [Fact]
    public async Task Employee_GetAllTest()
    {
        EmployeeDAO dao = new();
        List<Employee> allEmployees = await dao.GetAll();
        Assert.True(allEmployees.Count > 0);
    }
    
    // Test the getById
    [Fact]
    public async void Employee_GetByidTest()
    {
        EmployeeDAO dao = new();
        Employee? selectedEmployee = await dao.GetByID(2);
        Assert.NotNull(selectedEmployee);
    }
    
    // Test the getByPhoneNumber
    [Fact]
    public async void Employee_GetByPhoneNumberTest()
    {
        EmployeeDAO dao = new();
        Employee? selectedEmployee = await dao.GetByPhoneNumber("(555) 555-5551");
        Assert.NotNull(selectedEmployee);
    }
    
    // Test the UpdateTest
    [Fact]
    public async void Employee_UpdateTest()
    {
        EmployeeDAO dao = new();
        Employee? employeeForUpdate = await dao.GetByID(5);
        if (employeeForUpdate != null)
        {
            string oldPhoneNo = employeeForUpdate.PhoneNo!;
            string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
            employeeForUpdate!.PhoneNo = newPhoneNo;
        }

        Assert.True(await dao.Update(employeeForUpdate!) == UpdateStatus.Ok);

    }

    // Test the concurrency checker
    [Fact]
    public async Task Employee_ConcurrencyTest()
    {
        EmployeeDAO dao1 = new();
        EmployeeDAO dao2 = new();
        Employee studentForUpdate1 = await dao1.GetByID(2);
        Employee studentForUpdate2 = await dao2.GetByID(2);
        if (studentForUpdate1 != null)
        {
            string? oldPhoneNo = studentForUpdate1.PhoneNo;
            string? newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
            studentForUpdate1.PhoneNo = newPhoneNo;
            if (await dao1.Update(studentForUpdate1) == UpdateStatus.Ok)
            {
                // need to change the phone # to something else
                studentForUpdate2.PhoneNo = "666-666-6668";
                Assert.True(await dao2.Update(studentForUpdate2) == UpdateStatus.Stale);
            }
            else
                Assert.True(false); // first update failed
        }
        else
            Assert.True(false); // didn't find student 1
    }

    [Fact]
    public async Task Employee_LoadPicsTest()
    {
        {
            PicsUtility util = new();
            Assert.True(await util.AddEmployeePicsToDb());
        }
    }

    [Fact]
    public async Task Employee_ComprehensiveTest()
    {
        EmployeeDAO dao = new();
        Employee newEmployee = new()
        {
            FirstName = "Joe",
            LastName = "Smith",
            PhoneNo = "(555)555-1234",
            Title = "Mr.",
            DepartmentId = 100,
            Email = "js@abc.com"
        };
        int newEmployeeId = await dao.Add(newEmployee);
        output.WriteLine("New Employee Generated - Id = " + newEmployeeId);
        newEmployee = await dao.GetByID(newEmployeeId);
        byte[] oldtimer = newEmployee.Timer!;
        output.WriteLine("New Employee " + newEmployee.Id + " Retrieved");
        newEmployee.PhoneNo = "(555)555-1233";
        if (await dao.Update(newEmployee) == UpdateStatus.Ok)
        {
            output.WriteLine("Employee " + newEmployeeId + " phone# was updated to -" + newEmployee.PhoneNo);
        }
        else
        {
            output.WriteLine("Employee " + newEmployeeId + " phone# was not updated!");
        }
        newEmployee.Timer = oldtimer; // to simulate another user
        newEmployee.PhoneNo = "doesn't matter data is stale now";
        if (await dao.Update(newEmployee) == UpdateStatus.Stale)
        {
            output.WriteLine("Employee " + newEmployeeId + " was not updated due to stale data");
        }

        dao = new();
        await dao.GetByID(newEmployeeId);
        if (await dao.Delete(newEmployeeId) == 1)
        {
            output.WriteLine("Employee " + newEmployeeId + " was deleted!");
        }
        else
        {
            output.WriteLine("Employee " + newEmployeeId + " was not deleted");
        }
        // should be null because it was just deleted
        Assert.Null(await dao.GetByID(newEmployeeId));
    }

    [Fact]
    public async Task Call_ComprehensiveTest()
    {
        CallDAO dao = new();
        //EmployeeDAO edao = new();
        //ProblemDAO pdao = new();
        Call newCall = new()
        {
            EmployeeId = 11,
            ProblemId = 3, //await pdao.GetByDescription("Hard Drive Failure"),
            TechId = 7,
            DateOpened = DateTime.Now,
            DateClosed = null,
            OpenStatus = true,
            Notes = "Rose’s drive is shot, Burner to fix it"
        };
        int newCallId = await dao.Add(newCall);
        output.WriteLine("New Call Generated - Id = " + newCallId);
        newCall = await dao.GetByID(newCallId);
        byte[] oldtimer = newCall.Timer!;
        output.WriteLine("New Call " + newCall.Id + " Retrieved");
        newCall.Notes = newCall.Notes + "\n ordered new drive!";
        
        if(await dao.Update(newCall) == UpdateStatus.Ok)
        {
            output.WriteLine("Call was updated " + newCall.Notes);
        }
        else
        {
            output.WriteLine("error updating notes");
        }
        newCall.Timer = oldtimer; // to simulate another user
        newCall.Notes = "failure test";
        if (await dao.Update(newCall) == UpdateStatus.Stale)
        {
            output.WriteLine("Call " + newCallId + " was not updated due to stale data");
        }

        dao = new();
        await dao.GetByID(newCallId);
        if (await dao.Delete(newCallId) == 1)
        {
            output.WriteLine("Call " + newCallId + " was deleted!");
        }
        else
        {
            output.WriteLine("Call " + newCallId + " was not deleted");
        }
        // should be null because it was just deleted
        Assert.Null(await dao.GetByID(newCallId));
    }
    [Fact]
    public async Task Call_GetAllTest()
    {
        CallDAO dao = new();
        List<Call> allCalls = await dao.GetAll();
        Assert.True(allCalls.Count > 0);
    }
    [Fact]
    public async Task Call_AddTest()
    {
        CallDAO dao = new();
        Call newCall = new()
        {

            EmployeeId = 7,
            ProblemId = 5, //await pdao.GetByDescription("Hard Drive Failure"),
            TechId = 8,
            DateOpened = DateTime.Now,
            DateClosed = null,
            OpenStatus = true,
            Notes = "Bunsen Burner’s Cpu Fan failed, Dish to fix it \n new one ordered"
        };
        Assert.True(await dao.Add(newCall) > 0);
    }
}