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


namespace Refill_Entity
{
    /// <summary>
    /// Логика взаимодействия для ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        RefillAndMiniCafeContext db;
        public ServiceWindow()
        {
            InitializeComponent();
            db = new RefillAndMiniCafeContext();
            ProductsRB.IsChecked = true;
            Loaded += ServiceWindow_Loaded;
            this.Closing += ServiceWindow_Closing!;

        }

        private void ServiceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Users.Load();
            var user = db.Users.Select(s => s.Name);
            foreach (var item in user)
            {
                ReportUserCB.Items.Add(item);
            }
        }

        private void ServiceWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }


        private void Products_Checked(object sender, RoutedEventArgs e)
        {
            db.Products.Load();
            dataGrid.ItemsSource = db.Products.Local.ToBindingList(); // устанавливаем привязку к кэшу
            userGB.Visibility = Visibility.Hidden;
            userDG.Visibility = Visibility.Hidden;
            fuelGB.Visibility = Visibility.Hidden;
            fuelDG.Visibility = Visibility.Hidden;
            productGB.Visibility = Visibility.Visible;
            dataGrid.Visibility = Visibility.Visible;
        }

        private void Refills_Checked(object sender, RoutedEventArgs e)
        {
            db.Refills.Load();
            fuelDG.ItemsSource = db.Refills.Local.ToBindingList(); // устанавливаем привязку к кэшу
            userGB.Visibility = Visibility.Hidden;
            userDG.Visibility = Visibility.Visible;
            productGB.Visibility = Visibility.Hidden;
            dataGrid.Visibility = Visibility.Hidden;
            fuelGB.Visibility = Visibility.Visible;
            fuelDG.Visibility = Visibility.Visible;

        }

        private void UsersRB_Checked(object sender, RoutedEventArgs e)
        {
            db.Users.Load();
            userDG.ItemsSource = db.Users.Local.ToBindingList(); // устанавливаем привязку к кэшу
            productGB.Visibility = Visibility.Hidden;
            dataGrid.Visibility = Visibility.Hidden;
            fuelGB.Visibility = Visibility.Hidden;
            fuelDG.Visibility = Visibility.Hidden;
            userGB.Visibility = Visibility.Visible;
            userDG.Visibility = Visibility.Visible;
        }

        private void updateDBButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void deleteDBButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsRB.IsChecked == true)
            {
                if (dataGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
                    {
                        Product? product = dataGrid.SelectedItems[i] as Product;
                        if (product != null)
                        {
                            db.Products.Remove(product);
                        }
                    }
                }
                db.SaveChanges();
            }

            if (RefillsRB.IsChecked == true)
            {

                if (fuelDG.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < fuelDG.SelectedItems.Count; i++)
                    {
                        Refill? refill = fuelDG.SelectedItems[i] as Refill;
                        if (refill != null)
                        {
                            db.Refills.Remove(refill);
                        }
                    }
                }
                db.SaveChanges();

            }

            if (UsersRB.IsChecked == true)
            {

                if (userDG.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < userDG.SelectedItems.Count; i++)
                    {
                        User? user = userDG.SelectedItems[i] as User;
                        if (user != null)
                        {
                            db.Users.Remove(user);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        private void ReportByCategory_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_Reports.Items.Clear();
            CountPeriodSaleProduct.Text = "";
            CountSaleProduct.Text = "";
            db.Sales.Load();
            var value = db.Sales.Where(s => s.NameUsers == ReportUserCB.SelectedItem.ToString() && s.Date >= startDateCashier.SelectedDate && s.Date <= endDateCashier.SelectedDate);
            var countSum = value.Sum(s => s.Quantity);
            var sum = value.Sum(s => s.Amount);
            foreach (var item in value)
            {
                dataGrid_Reports.Items.Add(item);
            }
            CountSaleProduct.Text = $"{countSum} товара на сумму {sum:#####.##} рублей";
        }

        private void ReportPeriodByCategory_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_Reports.Items.Clear();
            CountPeriodSaleProduct.Text = "";
            CountSaleProduct.Text = "";
            db.Sales.Load();
            var value = db.Sales.Where(s => s.Date >= startDate.SelectedDate && s.Date <= endDate.SelectedDate);
            var countSum = value.Sum(s => s.Quantity);
            var sum = value.Sum(s => s.Amount);
            foreach (var item in value)
            {
                dataGrid_Reports.Items.Add(item);
            }
            dataGrid_Reports.Visibility = Visibility.Visible;
            CountPeriodSaleProduct.Text = $"{countSum} товара продано на сумму {sum:#####.##} рублей";
        }
    }
}

