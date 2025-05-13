using HelpdeskDal;
using HelpdeskDAL;
using HelpdeskViewModels;

using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
namespace HelpdeskWebsite.Reports
{
    public class EmployeeReport
    {
        public async void GenerateReport(string rootpath)
        {
            PageSize pg = PageSize.A4;
            var helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            PdfWriter writer = new(rootpath + "/pdfs/employeereport.pdf",
            new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdf = new(writer);
            Document document = new(pdf); // PageSize(595, 842)
            document.Add(new Image(ImageDataFactory.Create(rootpath + "/images/icon.png"))
            .ScaleAbsolute(200, 100)
            .SetFixedPosition(((pg.GetWidth() - 200) / 2), 710));

            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Current Employees")
            .SetFont(helvetica)
           .SetFontSize(24)
            .SetBold()
           .SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            Table table = new(3);
            table
            .SetWidth(298) // roughly 50%
           .SetTextAlignment(TextAlignment.CENTER)
            .SetRelativePosition(0, 0, 0, 0)
            .SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.AddCell(new Cell().Add(new Paragraph("Title")
             .SetFontSize(16)
            .SetBold()
             .SetPaddingLeft(18)
            .SetTextAlignment(TextAlignment.LEFT))
             .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("First Name")
            .SetFontSize(16)
           .SetBold()
            .SetPaddingLeft(16)
           .SetTextAlignment(TextAlignment.LEFT))
            .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Last Name")
            .SetBold()
           .SetFontSize(16)
            .SetPaddingLeft(16)
            .SetTextAlignment(TextAlignment.LEFT))
           .SetBorder(Border.NO_BORDER));


            EmployeeViewModel viewmodel = new();
            List<EmployeeViewModel> allEmployees = await viewmodel.GetAll();

            foreach (var employee in allEmployees)
            {
                table.AddCell(new Cell().Add(new Paragraph(employee.Title)
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(employee.Firstname)
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(employee.Lastname)
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
            }


            document.Add(table);
            document.Add(new Paragraph("Employee report written on - " + DateTime.Now)
            .SetFontSize(6)
            .SetTextAlignment(TextAlignment.CENTER));

            document.Close();

        }

    }

    public class CallReport
    {
        public async void GenerateReport(string rootpath)
        {
            PageSize pg = PageSize.A4.Rotate();
            var helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            PdfWriter writer = new(rootpath + "/pdfs/callreport.pdf",
            new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdf = new(writer);
            Document document = new(pdf,pg); // PageSize(595, 842)
            document.Add(new Image(ImageDataFactory.Create(rootpath + "/images/icon.png"))
            .ScaleAbsolute(200, 100)
            .SetFixedPosition(((pg.GetWidth() - 200) / 2), 450));

            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Current Calls")
            .SetFont(helvetica)
           .SetFontSize(24)
            .SetBold()
           .SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            Table table = new(6);
            table
            .SetWidth(700) // roughly 50%
           .SetTextAlignment(TextAlignment.CENTER)
            .SetRelativePosition(0, 0, 0, 0)
            .SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.AddCell(new Cell().Add(new Paragraph("Opened")
             .SetFontSize(16)
            .SetBold()
             .SetPaddingLeft(5)
            .SetTextAlignment(TextAlignment.LEFT))
             .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Last Name")
            .SetFontSize(16)
           .SetBold()
            .SetPaddingLeft(5)
           .SetTextAlignment(TextAlignment.LEFT))
            .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Tech")
            .SetBold()
           .SetFontSize(16)
            .SetPaddingLeft(5)
            .SetTextAlignment(TextAlignment.LEFT))
           .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Problem")
            .SetBold()
           .SetFontSize(16)
            .SetPaddingLeft(5)
            .SetTextAlignment(TextAlignment.LEFT))
           .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Status")
            .SetBold()
           .SetFontSize(16)
            .SetPaddingLeft(5)
            .SetTextAlignment(TextAlignment.LEFT))
           .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Closed")
            .SetBold()
           .SetFontSize(16)
            .SetPaddingLeft(5)
            .SetTextAlignment(TextAlignment.LEFT))
           .SetBorder(Border.NO_BORDER));


            CallViewModel viewmodel = new();
            List<CallViewModel> allCalls = await viewmodel.GetAll();

            foreach (var calls in allCalls)
            {
                table.AddCell(new Cell().Add(new Paragraph(calls.DateOpened.ToShortDateString())
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(calls.EmployeeName)
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(calls.TechName)
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(calls.ProblemDescription)
                .SetFontSize(14)
               .SetPaddingLeft(24)
               .SetTextAlignment(TextAlignment.LEFT))
               .SetBorder(Border.NO_BORDER));
                if (calls.OpenStatus == true) {
                       table.AddCell(new Cell().Add(new Paragraph("Closed")
                       .SetFontSize(14)
                       .SetPaddingLeft(24)
                       .SetTextAlignment(TextAlignment.LEFT))
                       .SetBorder(Border.NO_BORDER)); }
                else
                {
                    table.AddCell(new Cell().Add(new Paragraph("Open")
                    .SetFontSize(14)
                    .SetPaddingLeft(24)
                    .SetTextAlignment(TextAlignment.LEFT))
                    .SetBorder(Border.NO_BORDER));
                }
                if (calls.DateClosed !=null)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(calls.DateClosed.ToString())
                        .SetFontSize(14)
                       .SetPaddingLeft(24)
                       .SetTextAlignment(TextAlignment.LEFT))
                       .SetBorder(Border.NO_BORDER));
                    }
                else
                {
                    table.AddCell(new Cell().Add(new Paragraph("-")
                       .SetFontSize(14)
                      .SetPaddingLeft(24)
                      .SetTextAlignment(TextAlignment.LEFT))
                      .SetBorder(Border.NO_BORDER));
                }
            }


            document.Add(table);
            document.Add(new Paragraph("Employee report written on - " + DateTime.Now)
            .SetFontSize(6)
            .SetTextAlignment(TextAlignment.CENTER));

            document.Close();

        }

    }

}
