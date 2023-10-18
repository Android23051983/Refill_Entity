using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Refill_Entity.Models;


namespace Refill_Entity.ViewModels
{
    public class MainWindowViewModel : Notify
    {
        #region OBSERVABLE COLLECTION AND FEATURES
        public ObservableCollection<User> UsersObserv { get; set; }
        public  List<string> petrolTitle = new();
        private User? selectedUser;

        public User SelectedUser
        {
            get => selectedUser!;
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public ObservableCollection<Product>? productsObserv { get; set; }

        public Product? selectedProduct;

        public Product SelectedProduct
        {
            get => selectedProduct!;
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public ObservableCollection<Refill> refillproductsObserv;

        private Refill? refillselectedProduct;

        public Refill RefillselectedProduct
        {
            get => refillselectedProduct!;
            set
            {
                refillselectedProduct = value;
                OnPropertyChanged("RefillselectedProduct");
            }
        }

        public ObservableCollection<Sale>? saleproductsObserv { get; set; }

        /// <summary>
        /// выбранный из купленного товар
        /// </summary>
        private Sale? saleselectedProduct;

        /// <summary>
        /// Свойство выбранный из купленного товар
        /// </summary>
        public Sale SaleSelectedProduct
        {
            get => saleselectedProduct!;
            set
            {
                saleselectedProduct = value;
                OnPropertyChanged("SaleSelectedProduct");
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            using (RefillAndMiniCafeContext db = new RefillAndMiniCafeContext())
            {
                db.Products.Load();
                productsObserv = db.Products.Local.ToObservableCollection();

                db.Users.Load();
                UsersObserv = db.Users.Local.ToObservableCollection();

                saleproductsObserv = new ObservableCollection<Sale>();
            }
            
            
        }


        #region CANCELLATION OF THE PRODUCT IN saleDataGrid
        public void saleGridDeleteProduct(ref DataGrid datagrid)
        {
            if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
            {
                MessageBoxResult result = MessageBox.Show("Удалить всё кол-во товара - Yes", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var index = datagrid.SelectedIndex;
                        datagrid.Items.RemoveAt(index);
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

        #endregion

        #region METHOD TO PETROL

        public void PetrolSelection(ref ComboBox combobox, ref TextBlock textblock)
        {
            using (RefillAndMiniCafeContext db = new())
            {
                var result = db.Refills.ToList();
                foreach (var petrol in result)
                {
                    if (petrol.Title == combobox.SelectedItem.ToString())
                    {
                        textblock.Text = petrol.Price.ToString("#.##");
                    }

                }
            }
        }
        public void PetrolLoaded(ref ComboBox combobox)
        {
            using (RefillAndMiniCafeContext db = new())
            {
                var result = db.Refills.ToList();
                foreach (var petrol in result)
                {
                    petrolTitle.Add(petrol.Title);
                }
            }
            foreach (var petrol in petrolTitle)
            {
                combobox.Items.Add(petrol);
            }
            combobox.SelectedIndex = 0;
        }
        #endregion

        #region METHOD SHUTDOWN PC 
        private void ShutdownPsMethod()
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
        #endregion

        #region COMMAND SHUTDOWN PS
        private RelayCommand shutdownPs;
        public RelayCommand ShutdownPs
        {
            get
            {
                return shutdownPs ?? new RelayCommand(obj =>
                {
                    ShutdownPsMethod();
                });
            }
        }
        #endregion

        #region METHODS TO OPEN WINDOWS
        //Методы открытия окон
        private void OpenServiceWindowMethod()
        {
            if (PasswordWindow.valueStatus == 2)
            {
                ServiceWindow newServiceWindow = new ServiceWindow();
                SetParametersWindow(newServiceWindow);
            }
            else if (PasswordWindow.valueStatus != 2)
            {
                MessageBox.Show("Сервисное обслуживание может открывать только Администратор. Смените Кассира на администратора", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
        }

        private void OpenPasswordWindowMethod()
        {
            PasswordWindow newPasswordWindow = new PasswordWindow();
            SetParametersWindow(newPasswordWindow);
            
        }
        //параметры устанавливаемые для открывающихся окон
        private void SetParametersWindow(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion

        #region COMMANDS TO OPEN WINDOWS

        private RelayCommand openServiceWind;
        public RelayCommand OpenServiceWind
        {
            get
            {
                return openServiceWind ?? new RelayCommand(obj =>
                {
                    OpenServiceWindowMethod();
                });
            }
        }

        private RelayCommand openPasswordWind;
        public RelayCommand OpenPasswordWind
        {
            get
            {
                return openPasswordWind ?? new RelayCommand(obj =>
                {
                    OpenPasswordWindowMethod();
                });
            }
        }

        #endregion
    }
}
