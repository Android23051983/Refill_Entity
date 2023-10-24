using Microsoft.EntityFrameworkCore;
using Refill_Entity.Models;
using Refill_Entity.ViewModels;
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

namespace Refill_Entity.Views
{
    /// <summary>
    /// Логика взаимодействия для DaySaleReport.xaml
    /// </summary>
    public partial class DaySaleReportWindow : Window
    {
        RefillAndMiniCafeContext db = new RefillAndMiniCafeContext();
        public DaySaleReportWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Sales.Where(s => s.Date == DateTime.Today).Load();
            sDataGrid.ItemsSource = db.Sales.Local.ToBindingList();
        }

    }
}
