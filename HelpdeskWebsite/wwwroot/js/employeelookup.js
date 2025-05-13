$(() => { // main jQuery routine - executes every on page load, $ is short for jquery
    $("#getbutton").on('click', async (e) => { // click event handler makes aysynchronous fetch
        try {
            let email = $("#TextBoxEmail").val();
            $("#status").text("please wait...");
            let response = await fetch(`/api/employee/${email}`);
            if (response.ok) {
                let data = await response.json(); // this returns a promise, so we await it
                if (data.email !== "not found") {
                    $("#title").text(data.title);
                    $("#firstname").text(data.firstname);
                    $("#lastname").text(data.lastname);
                    $("#phone").text(data.phoneno);
                    $("#status").text("Employee found");
                } else {
                    $("#firstname").text("not found");
                    $("#status").text("no such Employee");
                }
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) { // catastrophic
            $("#status").text(error.message);
        } // try/catch
    }); // click event
}); // main jQuery method
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