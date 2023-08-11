using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.Sqlite;
using OfficeOpenXml;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Test_B1_Task2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Batteries.Init();
            InitializeComponent();
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            ExelAddToDataBase data_class = new ExelAddToDataBase();
            data_class.AddExelFileToDb();           
        }

        private void OpenShowData(object sender, RoutedEventArgs e)
        {
            ShowData showData = new ShowData();
            showData.Show();
        }
    }
}
