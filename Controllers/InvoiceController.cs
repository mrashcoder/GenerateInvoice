using Microsoft.AspNetCore.Mvc;
using GenerateInvoice.Models;
using iText.Kernel.Pdf; // dotnet add package itext7 itext7.bouncy-castle-adapter
using iText.Layout;
using iText.Layout.Element;
using Newtonsoft.Json; //dotnet add package Newtonsoft.Json


namespace GenerateInvoice.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        [Route("GenerateInvoice")]
        public IActionResult GenerateInvoice()
        {
            var invoiceJson = System.IO.File.ReadAllText("invoiceData.Json");
            //var root = JsonSerializer.Deserialize<Root>(invoiceJson);
            var root = JsonConvert.DeserializeObject<Root>(invoiceJson);

            if (root == null)
            {
                return BadRequest("Deserialized request is null.");
            }
            else
            {
                var invoiceData = root.request[0];
                using (var memoryStream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(memoryStream);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    document.Add(new Paragraph($"Order ID: {invoiceData.order_id}"));
                    document.Add(new Paragraph($"Sales Person: {invoiceData.sales_person}"));
                    document.Add(new Paragraph($"Order Confirmed Date: {invoiceData.order_confirmed_date}"));
                    document.Add(new Paragraph($"Currency: {invoiceData.currency_name}"));
                    document.Add(new Paragraph($"Advertiser: {invoiceData.invoice_advertiser}"));
                    document.Add(new Paragraph($"Company: {invoiceData.invoice_company_name}"));
                    document.Add(new Paragraph($"Address: {invoiceData.invoice_address1}, {(!string.IsNullOrEmpty(invoiceData.invoice_address2) ? invoiceData.invoice_address2 + "," : string.Empty)} {(!string.IsNullOrEmpty(invoiceData.invoice_address2) ? invoiceData.invoice_address3 + "," : string.Empty)} {invoiceData.invoice_city}, {invoiceData.invoice_state_county}, {invoiceData.invoice_post_code}, {invoiceData.invoice_country_name}"));
                    document.Add(new Paragraph($"Contact: {invoiceData.invoice_contact_name}, {invoiceData.invoice_contact_email_address}"));

                    document.Add(new Paragraph("Items:"));

                    foreach (var item in invoiceData.Items)
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.SetMarginLeft(20);
                        paragraph.Add(new Text($" Item ID: {item.order_item_id},  Product: {item.product_name}, Purchase Order: {item.purchase_order}, Item: {item.item}, Month: {item.month_name}, Year: {item.year}, Gross Price: {item.gross_price}, Net Price: {item.net_price}"));
                        document.Add(paragraph);
                    }

                    document.Close();

                    byte[] byteArray = memoryStream.ToArray();
                    return File(byteArray, "application/pdf", "Invoice.pdf");
                }
            }
            //return Content(invoiceJson, "application/json");
            //https://localhost:7267/Invoice/GenerateInvoice
        }
    }
}

