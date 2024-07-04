using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfiumViewer;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using ParkingServis.Server.Services.ParkingSessionServices.Command;
using ParkingServis.Server.Controllers.ParkingSessionController;
using ParkingServis.Client.Views;


namespace ParkingServis.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for ParkingPaymentByAdmin.xaml
    /// </summary>
    public partial class ParkingPaymentByAdmin : Window
    {
        private readonly ParkingcSessionCommandController _parkingSessionCommandController;
        public static ParkingSession CurrentSession { get; set; }
        public static HomeWindow HomeWindow { get; set; }
        public static ParkingPayment ParkingPayment { get; set; }
        decimal priceToPay;
        DateTime endPark;
        public event EventHandler ReloadHome;
        string templatePath = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "parkingBill.pdf"
        );

        string outputPath = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "parkingBillOutput.pdf"
        );
        public ParkingPaymentByAdmin(ParkingcSessionCommandController parkingcSessionCommandController)
        {
            InitializeComponent();
            _parkingSessionCommandController = parkingcSessionCommandController;
            VehicleRegNumberTextBlock.Text = CurrentSession.VehicleReg;
            DateOfParkStartTextBlock.Text = CurrentSession.ParkingStart.ToString();
            endPark = DateTime.Now;
            DateOfParkEndTextBlock.Text = endPark.ToString();
            
            TimeSpan timeDiff = endPark - CurrentSession.ParkingStart;
            decimal hours = timeDiff.Hours + 1;
            priceToPay = hours * 50;
            PriceToPayTextBlock.Text = $"{priceToPay} RSD";
            
        }

        private void PaidPriceTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(decimal.TryParse(PaidPriceTextBlock.Text, out decimal paidValue))
            {
                ReturnMoneyTextBlock.Text = $"{paidValue - priceToPay}";
                if(paidValue - priceToPay < 0)
                {
                    ReturnMoneyTextBlock.Foreground = Brushes.IndianRed;
                }
                else
                {
                    ReturnMoneyTextBlock.Foreground = Brushes.LimeGreen;
                }
            }
        }

        private async void printParkBill()
        {
            PdfReader pdfReader = new PdfReader(templatePath);
            PdfWriter pdfWriter = new PdfWriter(outputPath);
            iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader, pdfWriter);
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            if (GeneratePdf(form, pdfDocument))
            {
                if (PrintPdf(outputPath))
                {
                    bool isPaid = await _parkingSessionCommandController.PayParking(CurrentSession.Id, priceToPay, endPark);
                    if (isPaid)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show(
                        "Uspesno ste istampali racun",
                        "Stampanje",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                        HomeWindow.ReloadHomeWindow();
                        this.Close();
                    }
                    
                }
            }
        }

        private bool GeneratePdf(PdfAcroForm form, iText.Kernel.Pdf.PdfDocument pdfDocument)
        {
            ;
            string fontPath = "C:\\Windows\\Fonts\\Arial.ttf";
            PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H");
            try
            {
                SetFieldValue(form, "reg", VehicleRegNumberTextBlock.Text);
                    SetFieldValue(form, "parkingStart", DateOfParkStartTextBlock.Text);
                    SetFieldValue(form, "parkingEnd", DateOfParkEndTextBlock.Text);
                    SetFieldValue(form, "priceToPay", PriceToPayTextBlock.Text);
                SetFieldValue(form, "paidPrice", PaidPriceTextBlock.Text);
                SetFieldValue(form, "returnMoney", ReturnMoneyTextBlock.Text);


                pdfDocument.Close();

               
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Doslo je do greske, pdf nije kreiran! {ex.Message}");
                return false;
            }
        }

        private void SetFieldValue(PdfAcroForm form, string fieldName, string value)
        {
            var field = form.GetField(fieldName);

            if (field != null)
            {
                try
                {
                    field.SetValue(value);
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show($"Null Reference Exception: {ex.Message}");
                    // Handle the exception or log the error
                }
            }
            else
            {
                // Handle the case where the field is null
                // For example, throw an exception, log an error, or handle it in another appropriate way
                MessageBox.Show($"The field '{fieldName}' is null. Cannot set value.");
            }
        }


        private bool PrintPdf(string pdfFilePath)
        {
            // Check if the file exists
            if (!System.IO.File.Exists(pdfFilePath))
            {
                MessageBox.Show("PDF file not found.");
                return false;
            }

            // Start the default PDF viewer process to print the file
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pdfFilePath,
                Verb = "Print",
                UseShellExecute = true
            };

            try
            {
                Process.Start(startInfo);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(
                    $"Doslo je do greske, reklamacija nece biti stampana! {ex.Message}"
                );
                return false;
            }
        }

        private async void PayByCardButton_Click(object sender, RoutedEventArgs e)
        {
            printParkBill();
            
        }
    }
}
