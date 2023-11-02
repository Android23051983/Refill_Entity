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
        RefillAndMiniCafeContext db = new RefillAndMiniCafeContext();
        public QuantityWindow()
        {
            InitializeComponent();
           
        }

        private void QWButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel.count = Convert.ToInt32(quantityTextBox.Text);
                if (MainWindowViewModel.product!.ProductCount >= MainWindowViewModel.count)
                {
                    MainWindowViewModel.amount = (decimal)MainWindowViewModel.count * MainWindowViewModel.product.price;
                    MainWindowViewModel.product.ProductCount -= Convert.ToInt32(quantityTextBox.Text);

                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                    MessageBox.Show("Не хватает количества товара на складе", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                MessageBox.Show("Введите количество товара", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
