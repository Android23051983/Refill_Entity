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
            this.Closing += ServiceWindow_Closing!;

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
            
    }
}

