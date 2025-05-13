$(() => { // main jQuery routine - executes every on page load, $ is short for jquery
    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                buildEmployeeList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Emplyoees Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
            response = await fetch(`api/department`);
            if (response.ok) {
                let deps = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("alldepartments", JSON.stringify(deps));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        }
    }; // getAll

    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass(); //remove any existing css on div
        if ($("#EmployeeModalForm").valid()) {
            $("#actionbutton").prop("disabled", false); 
            $("#modalstatus").attr("class", "badge bg-success"); //green
            $("#modalstatus").text("data entered is valid");
        }
        else {
            $("#actionbutton").prop("disabled", true); 
            $("#modalstatus").attr("class", "badge bg-danger"); //red
            $("#modalstatus").text("fix all errors");
            
        }
    });

    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirst: { maxlength: 25, required: true },
            TextBoxSurname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirst: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxSurname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "Invalid email format"
            }
        }
    }); //EmployeeModalForm.validate

    $.validator.addMethod("validTitle", (value) => { //custome rule
        return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, ""); //.validator.addMethod

    $("#srch").on("keyup", () => {
        let alldata = JSON.parse(sessionStorage.getItem("allemployees"));
        let filtereddata = alldata.filter((stu) => stu.lastname.match(new RegExp($("#srch").val(), 'i')));
        buildEmployeeList(filtereddata, false);
    }); // srch keyup

    $("input:file").on("change", () => {
        try {
            const reader = new FileReader();
            const file = $("#uploader")[0].files[0];
            $("#uploadstatus").text("");
            file ? reader.readAsBinaryString(file) : null;
            reader.onload = (readerEvt) => {
                // get binary data then convert to encoded string
                const binaryString = reader.result;
                const encodedString = btoa(binaryString);
                // replace the picture in session storage
                if (sessionStorage.getItem("employee") != null) {
                    let emp = JSON.parse(sessionStorage.getItem("employee"));
                    emp.staffPicture64 = encodedString;
                    sessionStorage.setItem("employee", JSON.stringify(emp));
                    $("#uploadstatus").text("retrieved local pic")
                }
                else {
                    sessionStorage.setItem("employeepic", encodedString);
                    $("#uploadstatus").text("retrieved local pic for add")
                }
            };
        } catch (error) {
            $("#uploadstatus").text("pic upload failed")
        }
    });

    $("#employeeList").on('click', (e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);

        } else {
            return false; // ignore if they clicked on heading or status
        }
    }); // employeeListClick

    const clearModalFields = () => {
        loadDepartmentsDDL(-1);
        $("#TextBoxTitle").val("");
        $("#TextBoxFirst").val("");
        $("#TextBoxSurname").val("");
        $("#TextBoxEmail").val("");
        $("#TextBoxPhone").val("");
        // clean out the other four text boxes go here as well
        sessionStorage.removeItem("employee");
        $("#theModal").modal("toggle");
        let validator = $("#EmployeeModalForm").validate();
        validator.resetForm();
        sessionStorage.removeItem("picture");
        $("#uploadstatus").text("");
        $("#imageHolder").html("");
        $("#uploader").val("");

    }; // clearModalFields

    const setupForAdd = () => {
        $("#dialog").hide();
        $("#deletebutton").hide();
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employee");
        $("#theModalLabel").text("Add");
        clearModalFields();
    }; // setupForAdd

    const setupForUpdate = (id, data) => {
        $("#dialog").hide();
        $("#actionbutton").val("update");
        $("#deletebutton").show();
        $("#modaltitle").html("<h4>Update Employee</h4>");
        clearModalFields();
        data.forEach(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirst").val(employee.firstname);
                $("#TextBoxSurname").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);
                
                
                // populate the other four text boxes here
                sessionStorage.setItem("employee", JSON.stringify(employee));
                $("#modalstatus").text("update data");
                $("#theModal").modal("toggle");
                $("#theModalLabel").text("Update");
                loadDepartmentsDDL(employee.departmentId);
                $("#imageHolder").html(`<img height="120" width="110" src="data:img/png;base64,${employee.staffPicture64}" />`);
            } // if
        }); // data.forEach
    }; // setupForUpdate

    const _delete = async () => {
        let employee = JSON.parse(sessionStorage.getItem("employee"));
        try {
            let response = await fetch(`api/employee/${employee.id}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // _delete

    const update = async (e) => {

        // action button click event handler
        try {
            // set up a new client side instance of Student
            let emp = JSON.parse(sessionStorage.getItem("employee"));
            // pouplate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirst").val();
            emp.lastname = $("#TextBoxSurname").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());
            // send the updated back to the server asynchronously using Http PUT
            let response = await fetch("api/employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp),
            });
            if (response.ok) {
                // or check for response.status
                let payload = await response.json();
                getAll(payload.msg);
                $("#theModal").modal("toggle");

            } else if (response.status !== 404) {
                // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
                $("#theModal").modal("toggle");

            } else {
                // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch
    }; // update

    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirst").val();
            emp.lastname = $("#TextBoxSurname").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            // populate the other four properties here from the text box contents
            emp.departmentId = parseInt($("#ddlDepartments").val());
            //emp.departnmentName = NULL;
            emp.id = -1;
            emp.timer = null;
            emp.picture64 = sessionStorage.getItem("employeepic");
            // send the student info to the server asynchronously using POST
            let response = await fetch("api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                sessionStorage.removeItem("employeepic");
                //data = data + " " + ${ emp.lastname };
                //data.msg = "Employee " + ${ emp.lastname } + " Added!"

                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
                sessionStorage.removeItem("employeepic");
            } else { // else 404 not found
                $("#status").text("no such path on server");
                sessionStorage.removeItem("employeepic");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#theModal").modal("toggle");
    }; // add

    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "update" ? update() : add();
    }); // actionbutton click

    $("#deletebutton").on("click", () => {
        $("#dialog").show();
        $("#status").text("");
        $("#dialogbutton").hide();
        //_delete();
    }); // deletebutton click

    $("#nobutton").on("click", (e) => {
        $("#dialog").hide();
        $("#modalstatus").text("Employee Not deleted");
        $("#dialogbutton").show();
    });
    $("#yesbutton").on("click", () => {
        _delete();
    }); 

    const buildEmployeeList = (data, usealldata=true) => {
        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-info row d-flex" id="status">Student Info</div>
            <div class= "list-group-item row d-flex text-center" id="heading">
            <div class="col-4 h4">Title</div>
            <div class="col-4 h4">First</div>
            <div class="col-4 h4">Last</div>
            </div>`);
        div.appendTo($("#employeeList"));
        
        usealldata ? sessionStorage.setItem("allemployees", JSON.stringify(data)) : null;

        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add employee</button>`);
        btn.appendTo($("#employeeList"));

        data.forEach(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div>
             <div class="col-4" id="employeefname${emp.id}">${emp.firstname}</div>
             <div class="col-4" id="employeelastnam${emp.id}">${emp.lastname}</div>`
            );
            btn.appendTo($("#employeeList"));
        }); // forEach
    }; // buildStudentList

    const loadDepartmentsDDL = (empdep) => {
        html = '';
        $('#ddlDepartments').empty();
        let allDepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        allDepartments.forEach((dep) => { html += `<option value="${dep.id}">${dep.departmentName}</option>` });
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(empdep);
    }; // loadDivisionDDL


    getAll(""); // first grab the data from the server


}); // jQuery ready method
// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else
}