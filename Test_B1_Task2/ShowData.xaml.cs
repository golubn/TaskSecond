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
using System.Windows.Shapes;

namespace Test_B1_Task2
{
    /// <summary>
    /// Логика взаимодействия для ShowData.xaml
    /// </summary>
    public partial class ShowData : Window
    {
        public ShowData()
        {
            InitializeComponent();
            ShowDataBase showDataBase = new ShowDataBase();
            showDataBase.ShowData(dataGrid);
        }
    }
}
