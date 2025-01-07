using System;
using System.IO;
using System.Collections.Generic;
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
using SmartBILL.Reports;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using CrystalDecisions.ReportAppServer;
using System.Reflection;

namespace SmartBILL.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Window
    {
        public ReportView()
        {
            InitializeComponent();
        }

        private void BtnReportMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnReportClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnPrintReport_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    // Create a new ReportDocument instance
            //    ReportDocument report = new ReportDocument();

            //    // Get the embedded resource name
            //    string resourceName = "SmartBILL.Reports.test.rpt";  // Adjust this based on your namespace and file name

            //    // Read the embedded report file into a stream
            //    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            //    {
            //        if (stream != null)
            //        {
            //            // Load the report from the stream
            //            report.Load(stream);

            //            // Assuming Cryst is the CrystalReportViewer control, set the report source
            //            PrintReport.ViewerCore.ReportSource = report;

            //            // Optionally, display the report in the viewer

            //        }
            //        else
            //        {
            //            Console.WriteLine("Embedded report not found.");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Handle any exceptions
            //    Console.WriteLine("Error loading report: " + ex.Message);
            //}

            ReportDocument report = new ReportDocument();
            report.Load(@"D:\2025\SmartBILL Software\SmartBILL_Desktop_Application\SmartBILL\SmartBILL\Reports\test.rpt");

            PrintReport.ViewerCore.ReportSource = report;

            //string reportFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            //string reportPath = System.IO.Path.Combine(reportFolder, "test.rpt");

            //// Ensure the report exists in the folder before trying to load
            //if (File.Exists(reportPath))
            //{
            //    // Load the report from the file
            //    report.Load(reportPath);

            //    // Assuming Cryst is the CrystalReportViewer control, set the report source
            //    PrintReport.ViewerCore.ReportSource = report;

            //    // Optionally, display the report in the viewer

            //}
            //else
            //{
            //    Console.WriteLine("Report file not found at: " + reportPath);
            //}

            //try
            //{
            //    ReportDocument report = new ReportDocument();
            //    string appDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SmartBILL", "Reports", "test.rpt");
            //    report.Load(appDataPath);


            //    PrintReport.ViewerCore.ReportSource = report;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //    System.Windows.MessageBox.Show($"Error: {ex.Message}");
            //}


            //// Create a new ReportDocument instance
            //ReportDocument report = new ReportDocument();

            //// Get the base directory path of the application
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;

            //// Build the relative path to the report file
            //string reportPath = System.IO.Path.Combine(basePath, @"Reports\test.rpt");

            //// Check if the report file exists
            //if (System.IO.File.Exists(reportPath))
            //{
            //    try
            //    {
            //        // Load the report using the correct path
            //        report.Load(reportPath);

            //        // Set the report source for the viewer
            //        PrintReport.ViewerCore.ReportSource = report;
            //    }
            //    catch (Exception ex)
            //    {
            //        // Display an error message if the report fails to load
            //        MessageBox.Show($"Error loading report: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    // Display an error message if the file is not found
            //    MessageBox.Show($"Report file not found at: {reportPath}");
            //}
        }
    }
}
