$(() => { // main jQuery routine - executes every on page load, $ is short for jquery
    const getAll = async (msg) => {
        try {
            $("#CallList").text("Finding Call Information...");
            let response = await fetch(`api/Call`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                buildCallList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Calls Loaded") : $("#status").text(`${msg} - Callss Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else

            response = await fetch(`api/employee`);
            if (response.ok) {
                let emps = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("allemployees", JSON.stringify(emps));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else

            response = await fetch(`api/problem`);
            if (response.ok) {
                let probs = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("allproblems", JSON.stringify(probs));
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

    document.addEventListener("click", e => {
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
            ddlProblems: { required: true },
            ddlEmployee: { required: true },
            ddlTech: { required: true},
            texAreaNotes: { required: true }
        },
        errorElement: "div",
        messages: {
            ddlProblems: {
                required: "Select Problem"
            },
            ddlEmployee: {
                required: "Select Employee"
            },
            ddlTech: {
                required: "Select Tech"
            },
            texAreaNotes: {
                required: "required 1-250 chars."
            }
        }
    }); //EmployeeModalForm.validate



    $("#srch").on("keyup", () => {
        let alldata = JSON.parse(sessionStorage.getItem("allCalls"));
        let filtereddata = alldata.filter((cal) => cal.employeeName.match(new RegExp($("#srch").val(), 'i')));
        buildEmployeeList(filtereddata, false);
    }); // srch keyup


    $("#CallList").on('click', (e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allCalls"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);

        } else {
            return false; // ignore if they clicked on heading or status
        }
    }); // employeeListClick

    const clearModalFields = () => {
        loadProblemsDDL(-1);
        loadTechDDL(-1);
        loadEmployeeDDL(-1);
        $("#TextBoxProblem").val("");
        $("#TextBoxEmployee").val("");
        $("#TextBoxTech").val("");
        $("#dateOpened").val("");
        $("#dateClosed").val("");
        $("#texAreaNotes").val("");
        // clean out the other four text boxes go here as well
        sessionStorage.removeItem("call");
        $("#theModal").modal("toggle");
        let validator = $("#EmployeeModalForm").validate();
        validator.resetForm();
        

    }; // clearModalFields

    const setupForAdd = () => {
        $("#dialog").hide();
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add Call</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new Call Report");
        $("#theModalLabel").text("Add");
        
        clearModalFields();
        
    }; // setupForAdd

    const setupForUpdate = (id, data) => {

        $("#dialog").hide();
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>Call Information</h4>");
        clearModalFields();
        data.forEach(call => {
            if (call.id === parseInt(id)) {

                
                sessionStorage.setItem("dateOpened", formatDate(call.dateOpened));
                $("#dateOpened").text(formatDate(call.dateOpened).replace("T", " "));
              
                $("#dateClosed").val(call.dateClosed);
                //$("#isClosedCheck").val(cal.dateOpened);
                $("#texAreaNotes").val(call.Notes);
                
                
                // populate the other four text boxes here
                sessionStorage.setItem("call", JSON.stringify(call));
                $("#modalstatus").text("update data");
                $("#theModal").modal("toggle");
                $("#theModalLabel").text("Update");
                loadProblemsDDL(call.problemId);
                loadTechDDL(call.techId);
                loadEmployeeDDL(call.employeeId);

                if (call.dateClosed != null) {
                    sessionStorage.setItem("dateClosed", formatDate(call.dateClosed));
                    $("#dateClosed").val(call.dateClosed);
                }

                if (call.openStatus == false) {
                    $("ddlProblems").prop('disabled', true);
                    $("ddlEmployee").prop('disabled', true);
                    $("ddlTech").prop('disabled', true);
                    $("isClosedCheck").prop('checked', true);
                    $("isClosedCheck").prop('disabled', true);
                    $("texAreaNotes").prop('disabled', true);
                }
                
            } // if
        }); // data.forEach
    }; // setupForUpdate

    const update = async (e) => {

        // action button click event handler
        try {
            // set up a new client side instance of Student
            let cal = JSON.parse(sessionStorage.getItem("call"));
            // pouplate the properties
            var openstat = 0;
            cal.dateClosed = null;
            if ($("#closedCheckbox").prop('checked') == true) {
                open = 1;
                cal.dateClosed = Date.now();
            }
            cal.id = id;
            cal.employeeID = employeeID;
            cal.problemId = parseInt($("#ddlProblems").val());
            cal.techID = parseInt($("#ddlTech").val());
            cal.dateOpened = dateOpened;
            
            cal.openStatus = openstat;
            cal.Notes = $("#texAreaNotes").val();
            cal.timer = timer;
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

    const formatDate = (date) => {
        let d;
        (date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
        let _day = d.getDate();
        if (_day < 10) { _day = "0" + _day; }
        let _month = d.getMonth() + 1;
        if (_month < 10) { _month = "0" + _month; }
        let _year = d.getFullYear();
        let _hour = d.getHours();
        if (_hour < 10) { _hour = "0" + _hour; }
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }
        return _year + "-" + _month + "-" + _day + "T" + _hour + ":" + _min;
    } // formatDate


    const add = async () => {
        try {
            call = new Object();
            //$("#TextBoxProblem").val(cal.problemDescription);
            call.employeeID = parseInt($("#ddlEmployee").val());
            call.problemId = parseInt($("#ddlProblems").val());
            call.techID = parseInt($("#ddlTech").val());
            call.dateOpened = Date.now();
            call.dateClosed = null;
            call.openStatus = 1;
            call.Notes = $("#texAreaNotes").val();

            // populate the other four properties here from the text box contents
            // send the student info to the server asynchronously using POST
            let response = await fetch("api/Call", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(call)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                //data = data + " " + ${ emp.lastname };
                //data.msg = "Employee " + ${ emp.lastname } + " Added!"

                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#theModal").modal("toggle");
    }; // add

    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "update" ? update() : add();
    }); // actionbutton click


    const buildCallList = (data, usealldata=true) => {
        $("#CallList").empty();
        div = $(`<div class="list-group-item text-white bg-info row d-flex" id="status">Call Info</div>
            <div class= "list-group-item row d-flex text-center" id="heading">
            <div class="col-4 h4">Date</div>
            <div class="col-4 h4">For</div>
            <div class="col-4 h4">Problem</div>
            </div>`);
        div.appendTo($("#CallList"));
        
        usealldata ? sessionStorage.setItem("allCalls", JSON.stringify(data)) : null;

        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add Call</button>`);
        btn.appendTo($("#CallList"));

        data.forEach(cal => {
            btn = $(`<button class="list-group-item row d-flex" id="${cal.id}">`);
            btn.html(`<div class="col-4" id="calldate${cal.id}">${cal.dateOpened}</div>
             <div class="col-4" id="employeename${cal.id}">${cal.employeeName}</div>
             <div class="col-4" id="problemdescription${cal.id}">${cal.problemDescription}</div>`
            );
            btn.appendTo($("#CallList"));
        }); // forEach
    }; // buildStudentList

    const loadEmployeeDDL = (emps) => {
        html = '';
        $('#ddlEmployee').empty();
        let allEmployeess = JSON.parse(sessionStorage.getItem('allemployees'));
        allEmployeess.forEach((emp) => { html += `<option value="${emp.id}">${emp.lastname}</option>` });
        $('#ddlEmployee').append(html);
        $('#ddlEmployee').val(emps);
    }; // loadDivisionDDL

    const loadTechDDL = (emptec) => {
        html = '';
        $('#ddlTech').empty();
        let alltechs = JSON.parse(sessionStorage.getItem('allemployees'));
        alltechs.forEach((tec) => {
            if (tec.isTech == 1) { html += `<option value="${tec.id}">${tec.lastname}</option>` }
        });
        $('#ddlTech').append(html);
        $('#ddlTech').val(emptec);
    }; // loadDivisionDDL

    const loadProblemsDDL = (empprob) => {
        html = '';
        $('#ddlProblems').empty();
        let allProblems = JSON.parse(sessionStorage.getItem('allproblems'));
        allProblems.forEach((prob) => { html += `<option value="${prob.id}">${prob.description}</option>` });
        $('#ddlProblems').append(html);
        $('#ddlProblems').val(empprob);
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