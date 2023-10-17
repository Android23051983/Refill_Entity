using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Refill_Entity.Models;
using Refill_Entity.ViewModels;
using Refill_Entity.Views;

namespace Refill_Entity
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Sale Sale;
        public MainWindowViewModel ViewModel {  get; set; }
        public static Product? product;
        public static double count;
        public static decimal amount;
        private int selectedIndex;
        private decimal sumAmountCafe = 0;
        RefillAndMiniCafeContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new RefillAndMiniCafeContext();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // создание, подключение и вывод нового окна авторизации после загрузки MainWindow
            PasswordWindow passwordWindow = new()
            {
                Owner = this
            };
            passwordWindow.ShowDialog();
            titleLabel.Content = this.Title;
            MainWindowViewModel.PetrolLoaded();
            foreach (var petrol in MainWindowViewModel.petrolTitle)
            {
                petrolBox.Items.Add(petrol);
            }
            petrolBox.SelectedIndex = 0;
        }

        private void ServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordWindow.valueStatus == 2)
            {
                ServiceWindow serviceWindow = new()
                {
                    Owner = this
                };
                serviceWindow.ShowDialog();
            }
            else if (PasswordWindow.valueStatus != 2)
            {
                MessageBox.Show("Смените кассира на Администратора");

                PasswordWindow passwordWindow = new()
                {
                    Owner = this
                };
                passwordWindow.ShowDialog();
                titleLabel.Content = this.Title;
            }
        }

            private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
            {
                saleDataGrid.Items.Clear();
            }
            else if (PasswordWindow.valueStatus > 1)
            {
                PasswordWindow passwordWindow = new()
                {
                    Owner = this
                };
                passwordWindow.ShowDialog();
                titleLabel.Content = this.Title;
            }
        }

        public void CashierBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new()
            {
                Owner = this
            };
            passwordWindow.ShowDialog();
            titleLabel.Content = this.Title;

        }
       
        private void PetrolPrice()
        {
            using (RefillAndMiniCafeContext db = new())
            {
                var result = db.Refills.ToList();
                foreach (var petrol in result)
                {
                    if (petrol.Title == petrolBox.SelectedItem.ToString())
                    {
                        pricePetrolBlock.Text = petrol.Price.ToString();
                    }

                }
            }
        }

        private void ClosePSBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
            {
                System.Diagnostics.Process.Start("cmd", "/c shutdown -s -f -t 00");
            }
            else if (PasswordWindow.valueStatus == 0)
            {
                MessageBox.Show("Завершать программу разрешено Старшему Кассиру или Администратору", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
            {
                this.Close();
            }
            else if (PasswordWindow.valueStatus != 1 || PasswordWindow.valueStatus != 2)
            {
                MessageBox.Show("Для Выхода смените Кассира на Старшего Кассира или Администратора", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Column1Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Column2Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Column3Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Column4Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Column5Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Column6Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void petrolBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (RefillAndMiniCafeContext db = new())
            {
                var result = db.Refills.ToList();
                foreach (var petrol in result)
                {
                    if (petrol.Title == petrolBox.SelectedItem.ToString())
                    {
                        pricePetrolBlock.Text = petrol.Price.ToString("#.##");
                    }

                }
            }
        }

        private void L_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void R_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void menuDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = menuDataGrid.SelectedIndex;
            try
            {
                product = ViewModel.productsObserv[selectedIndex];
            }
            catch (Exception)
            {

            }
            //product = menuDataGrid.SelectedItem as Product;
            QuantityWindow quantityWindow = new()
            {
                Owner = this
            };
            quantityWindow.ShowDialog();
            if (quantityWindow.DialogResult == true)
            {
                Sale = new() { ProductName = product!.Title, Amount = amount, Quantity = count, NameUsers = PasswordWindow.userName!, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay };
                ViewModel.saleproductsObserv.Add(Sale);
                //saleDataGrid.Items.Add(ViewModel.saleproductsObserv.ToBindingList());
            }
            else
            {
                menuDataGrid.UnselectAll();
            }
            sumAmountCafe += amount;
            //using (RefillAndMiniCafeContext db = new RefillAndMiniCafeContext())
            //{
            //    db.Sales.Add(Sale);
            //    db.SaveChanges();
            //}
        }
       

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saleDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
            {
                MessageBoxResult result = MessageBox.Show("Удалить всё кол-во товара - Yes", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var index = saleDataGrid.SelectedIndex;
                        saleDataGrid.Items.RemoveAt(index);
                    }
                    catch
                    {
                        MessageBox.Show("ВЫДЕЛИТЕ ТОВАР ДЛЯ ОТМЕНЫ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else if (PasswordWindow.valueStatus == 0)
            {
                MessageBox.Show("Товар может отменять только Старший Кассир или Администратор", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CafePaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            //using (RefillAndMiniCafeContext db = new RefillAndMiniCafeContext())
            //{
            //    db.Sales.Add(Sale);
            //    db.SaveChanges();
            
            //}

        }

    }
}