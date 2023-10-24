using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Refill_Entity.Models;
using Refill_Entity.Views;


namespace Refill_Entity.ViewModels
{
    enum SelectRefill
    {
        Litres,
        Rubles
    }
    
    public class MainWindowViewModel : Notify
    {
        RefillAndMiniCafeContext db = new RefillAndMiniCafeContext();
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

        #region REFILL SALE DATA

        private string? pricePetrolBlock;
        private string? methodSale_RefillTextBox;
        private string? petrolLiters = "0.00";
        private string? totalPetrolPriceTB;
        private string? columnNumber;
        private string? selectSaleRefill_Content;
        private string? litersRB_Content = "Литры";
        private string? rublesRB_Content = "Рубли";

        public string PricePetrolBlock
        {
            get => pricePetrolBlock ?? string.Empty;
            set { pricePetrolBlock = value; OnPropertyChanged("PricePetrolBlock"); }
        }
        public string MethodSaleRefillTextBox
        {
            get => methodSale_RefillTextBox ?? string.Empty;
            set { methodSale_RefillTextBox = value; OnPropertyChanged("MethodSaleRefillTextBox"); }
        }

        public string PetrolLiters
        {
            get => petrolLiters ?? string.Empty;
            set { petrolLiters = value; OnPropertyChanged("PetrolLiters"); }
        }

        public string TotalPetrolPriceTB
        {
            get => totalPetrolPriceTB ?? string.Empty;
            set { totalPetrolPriceTB = value; OnPropertyChanged("TotalPetrolPriceRB"); }
        }

        public string ColumnNumber
        {
            get => columnNumber ?? string.Empty;
            set { columnNumber = value; OnPropertyChanged("ColumnNumber"); }
        }

        public string SelectSaleRefill_Content
        {
            get => selectSaleRefill_Content ?? string.Empty;
            set { selectSaleRefill_Content = value; OnPropertyChanged("SelectSaleRefill"); }
        }

        public string LitersRB_Comtent
        {
            get => litersRB_Content ?? string.Empty;
            set { litersRB_Content = value; OnPropertyChanged("LitersRB_Content"); }
        }

        public string RublesRB_Content
        {
            get => rublesRB_Content ?? string.Empty;
            set {  rublesRB_Content = value;OnPropertyChanged("RublesRB_Content"); }
        }

        #endregion


        public MainWindowViewModel()
        {
            
            db.Products.Load();
            productsObserv = db.Products.Local.ToObservableCollection();

            db.Users.Load();
            UsersObserv = db.Users.Local.ToObservableCollection();


            saleproductsObserv = new ObservableCollection<Sale>();

            
        }
        #region RADIOBUTTON REFILL


        SelectRefill selectRefill = SelectRefill.Litres;

        private SelectRefill SelectRefill
        { 
            get => selectRefill; 
            set 
            { 
                if(selectRefill == value) return;
                selectRefill = value; 
                OnPropertyChanged("SelectRefill"); 
                OnPropertyChanged("LitersRB_Check"); 
                OnPropertyChanged("RublesRB_Check"); 
                OnPropertyChanged("GetResult"); 
            } 
        }

        public bool LitersRB_Check
        {
            get { return SelectRefill == SelectRefill.Litres; }
            set { SelectRefill = value? SelectRefill.Litres: SelectRefill;}
        }

        public bool RublesRB_Check
        {
            get { return SelectRefill == SelectRefill.Rubles;}
            set { SelectRefill = value ? SelectRefill.Rubles : SelectRefill;}
        }

        public string GetResult
        {
            get
            {
                switch (SelectRefill)
                {
                    case SelectRefill.Litres:
                        return litersRB_Content;
                    case SelectRefill.Rubles:
                        return rublesRB_Content;
                }
                return "";
            }
        }
        #endregion

        #region THE FUNCTIONALITY OF BRINGING A CAFE WINDOW TO SERVE A NEW CUSTOMER
        public void NewCafeBuyer(ref TextBlock textblock)
        {
            textblock.Text = "0";
            if (saleproductsObserv is not null)
            {
                saleproductsObserv.Clear();
            }
        }
        #endregion


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

        #region METHODS TO PETROL AND COLUMN

        public void PetrolButton_Click(ref Label label, ref Button button,ref ComboBox combobox)
        {
            label.Content = $"{button.Name}";
            int number = combobox.Items.Count;
            Random rnd = new Random();
            int randValue = rnd.Next(0, number-1);
            combobox.SelectedIndex = randValue;

        }

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

        public void NewRefillBuyer(ref Label PetrolLiters, ref RadioButton LitersRB, ref RadioButton RublesRB, ref TextBox methodSale_RefillTextBox, ref TextBlock pricePetrolBlock)
        {
            if (LitersRB.IsChecked == true)
            {
                float n = float.Parse(methodSale_RefillTextBox.Text);
                for (float i = n; i > 0; i--)
                {
                    PetrolLiters.Content = i.ToString();
                }
            }
            else if(RublesRB.IsChecked == true)
            {
                float rub = float.Parse(methodSale_RefillTextBox.Text);
                float pricePetrol = float.Parse(pricePetrolBlock.Text);

            }
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

        #region COMMAND SHUTDOWN PС
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
        private void OpenDayReportSaleWindow()
        {
            DaySaleReportWindow newDaySaleReportWindow = new DaySaleReportWindow();
            SetParametersWindow(newDaySaleReportWindow);
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
        private RelayCommand openDayReportSaleWnd;
        public RelayCommand OpenDayReportSaleWnd
        {
            get
            {
                return openDayReportSaleWnd ?? new RelayCommand(obj=>
                {
                    OpenDayReportSaleWindow();
                });
            }
        }

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
