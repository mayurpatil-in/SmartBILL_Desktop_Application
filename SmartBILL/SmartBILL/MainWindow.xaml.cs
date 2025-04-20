using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CrystalDecisions.CrystalReports.Engine;
using SmartBILL.Reports;

namespace SmartBILL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void LoadCrystalReport()
        {
            //// 1. Instantiate and load your .rpt
            //var reportDoc = new ReportDocument();
            //string rptFile = System.IO.Path.Combine(
            //    AppDomain.CurrentDomain.BaseDirectory,
            //    "Reports",
            //    "test.rpt");
            //reportDoc.Load(rptFile);

            //// 2. (Optional) push in any data source or parameters
            ////    e.g., reportDoc.SetDataSource(myDataTable);
            ////          reportDoc.SetParameterValue("ParamName", value);

            //// 3. Assign to the WPF viewer and refresh
            //CrystalViewer.ViewerCore.ReportSource = reportDoc;
            //CrystalViewer.ViewerCore.RefreshReport();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            test obj = new test();
            obj.Load(@"test.rpt");
            CrystView.ViewerCore.ReportSource = obj;
        }
    }
}
