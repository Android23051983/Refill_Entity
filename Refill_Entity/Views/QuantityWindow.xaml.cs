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
    /// Логика взаимодействия для QuantityWindow.xaml
    /// </summary>
    public partial class QuantityWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public QuantityWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
        }

        private void QWButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.count = Convert.ToInt32(quantityTextBox.Text);
                if (MainWindow.product.ProductCount >= MainWindow.count)
                {
                    MainWindow.amount = (decimal)MainWindow.count * MainWindow.product.price;
                    MainWindow.product.ProductCount -= Convert.ToInt32(quantityTextBox.Text);

                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                    MessageBox.Show("Не хватает количества товара на складе", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Введите количество товара", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
