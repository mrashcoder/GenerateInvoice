Thank you Mike for giving me the opportunity to take the test.
I have created a simple solution without and bells and whistles- I just followed the specified requirements, I did not implement error handling, PDF formatting or application success confirmation.

I saved the JSON file in the root directory with the filename "invoiceData.json." To efficiently complete the challenge, I used NuGet packages including NewtonSoft.Json and iText7 packages (along with the iText7 Bouncy Castle adapter).

To easily get the required packages, please run the following commands from the Package Manager Console:
- dotnet add package itext7
- dotnet add package itext7.bouncy-castle-adapter
- dotnet add package Newtonsoft.Json

To generate and download the invoice PDF, run the application from Visual Studio and then navigate to 'https://localhost:7164/Invoice/GenerateInvoice' Please adjust the port number if necessary.
