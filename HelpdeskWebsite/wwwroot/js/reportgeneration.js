$(() => {
    // hellopdf.js
    $("#employeeReportButton").on("click", async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/employeereport`);
            if (!response.ok)
                // check for response.status
                throw new Error(
                    `Status - ${response.status}, Text - ${response.statusText}`
                );
            let data = await response.json(); // this returns a promise, so we await it
            $("#lblstatus").text("report generated");
            data.msg === "Report Generated"
                ? window.open("/pdfs/employeereport.pdf")
                : $("#lblstatus").text("problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        } // try/catch
    }); // button click

    $("#callReportButton").on("click", async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/callreport`);
            if (!response.ok)
                // check for response.status
                throw new Error(
                    `Status - ${response.status}, Text - ${response.statusText}`
                );
            let data = await response.json(); // this returns a promise, so we await it
            $("#lblstatus").text("report generated");
            data.msg === "Report Generated"
                ? window.open("/pdfs/callreport.pdf")
                : $("#lblstatus").text("problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        } // try/catch
    }); // button click
}); // jQuery