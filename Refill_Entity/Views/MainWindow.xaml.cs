﻿using System.Collections.Immutable;
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            //Загрузка данных из базы бензин в лист petrolTitle
            //Переброска из petrolTitle в petrolBox
            ViewModel.PetrolLoaded(ref petrolBox);
            //выведение сообщения в правый нижний угол главного окна о работающем кассире
            PasswordWindow.Cashier(ref titleLabel);
            
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
            {
                saleDataGrid.Items.Clear();
            }
            else if (PasswordWindow.valueStatus < 1)
            {
                PasswordWindow passwordWindow = new()
                {
                    Owner = this
                };
                passwordWindow.ShowDialog();
                titleLabel.Content = this.Title;
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
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_1);
        }

        private void Column2Btn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_2);
        }

        private void Column3Btn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_3);
        }

        private void Column4Btn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_4);

        }

        private void Column5Btn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_5);
        }

        private void Column6Btn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_6);
        }

        private void petrolBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.PetrolSelection(ref petrolBox, ref pricePetrolBlock);
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
                if (ViewModel.productsObserv is not null)
                {
                    product = ViewModel.productsObserv[selectedIndex];
                }
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
                
            }
            else
            {
                menuDataGrid.UnselectAll();
            }
            sumAmountCafe += amount;
           
        }

        private void saleDelBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.saleGridDeleteProduct(ref saleDataGrid);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void CafePaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            

            if (ViewModel.saleproductsObserv is not null)
            {
                db.Sales.UpdateRange(ViewModel.saleproductsObserv);
                db.SaveChanges();
            }
            if (ViewModel.productsObserv is not null)
            {
                db.Products.UpdateRange(ViewModel.productsObserv);
                db.SaveChanges();
            }

            ViewModel.NewCafeBuyer(ref cafeTxt);
        }

    }
}