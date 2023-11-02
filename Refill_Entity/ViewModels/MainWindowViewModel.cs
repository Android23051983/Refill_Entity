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
using System.Printing;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Automation;


namespace Refill_Entity.ViewModels
{
    enum SelectRefill
    {
        Litres,
        Rubles
    }
    
    public class MainWindowViewModel : Notify
    {
        //Коллекции для таблиц из базы данных и их свойства и некоторые переменные для таймера и строки для печати
        RefillAndMiniCafeContext db = new RefillAndMiniCafeContext();
        string? PrintStr = "";
        private DispatcherTimer timer;
        private string currentTime;
        private int time;
        #region OBSERVABLE COLLECTION AND FEATURES
        public static Product? product;
        public static Refill? refill;       
        public ObservableCollection<User> UsersObserv { get; set; }
        public  List<string> petrolTitle = new();
        private User? selectedUser;
        private Sale Sale;

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
                try
                {
                    if (productsObserv is not null)
                    {
                        product = SelectedProduct;
                    }

                }
                catch (Exception)
                {

                }
                var DialogResult = OpenQuantityWindow();
                db.Products.UpdateRange(productsObserv);
                db.SaveChanges();
                if (DialogResult == true)
                {
                    Sale = new() { ProductName = product!.Title, Amount = amount, Quantity = count, NameUsers = PasswordWindow.userName!, Date = DateTime.Today, Time = DateTime.Now.TimeOfDay };
                    saleproductsObserv!.Add(Sale);
                }

                OnPropertyChanged("SelectedProduct");
            }
        }

        public ObservableCollection<Refill> refillProductsObserv;

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
        #region REFILL SALE DATA FEATURES

        private string? pricePetrolBlock;
        private string? methodSale_RefillTextBox;
        private string? petrolLiters;
        private string? totalPetrolPriceTB;
        private string? columnNumber;
        private string? selectSaleRefill_Content;
        private string? litersRB_Content = "Литры";
        private string? rublesRB_Content = "Рубли";
        private string? totalCafePriceTB = "0";
        private string? totalSale;
        private string? sPetrolTitle;
        public static double count;
        public static decimal amount;
        public static float total;

        public string PricePetrolBlock
        {
            get => pricePetrolBlock;
            set { pricePetrolBlock = value; OnPropertyChanged("PricePetrolBlock"); }
        }
        public string MethodSale_RefillTextBox
        {
            get => methodSale_RefillTextBox;
            set { methodSale_RefillTextBox = value; OnPropertyChanged("MethodSale_RefillTextBox"); }
        }

        public string PetrolLiters
        {
            get => petrolLiters;
            set { petrolLiters = value; OnPropertyChanged("PetrolLiters"); }
        }

        public string TotalPetrolPriceTB
        {
            get => totalPetrolPriceTB!;
            set { totalPetrolPriceTB = value; OnPropertyChanged("TotalPetrolPriceTB"); }
        }

        public string ColumnNumber
        {
            get => columnNumber;
            set { columnNumber = value; OnPropertyChanged("ColumnNumber"); }
        }

        public string SelectSaleRefill_Content
        {
            get => selectSaleRefill_Content;
            set { selectSaleRefill_Content = value; OnPropertyChanged("SelectSaleRefill"); }
        }

        public string LitersRB_Comtent
        {
            get => litersRB_Content;
            set { litersRB_Content = value; OnPropertyChanged("LitersRB_Content"); }
        }

        public string RublesRB_Content
        {
            get => rublesRB_Content;
            set {  rublesRB_Content = value;OnPropertyChanged("RublesRB_Content"); }
        }

        public string TotalCafePriceTB
        {
            get => totalCafePriceTB;
            set { totalCafePriceTB = value; OnPropertyChanged("TotalCafePriceTB"); }
        }

        public string TotalSale
        {
            get => totalSale;
            set { totalSale = value; OnPropertyChanged("TotalSale"); }
        }

        public string SPetrolTitle
        {
            get => sPetrolTitle!;
            set { sPetrolTitle = value; OnPropertyChanged("SPetrolTitle"); }
        }

        //public double Count
        //{
        //    get => count;
        //    set { count = value; OnPropertyChanged("Count"); }
        //}

        //public decimal Amount
        //{
        //    get => amount;
        //    set { amount = value; OnPropertyChanged("Amount"); }
        //}

        //public float Total
        //{
        //    get => total;
        //    set { total = value; OnPropertyChanged("Total");}
        //}
        #endregion

        #region CURENT TIME FEATURES
        public string CurentTime
        {
            get
            {
                return this.currentTime;
            }
            set { this.currentTime = value; OnPropertyChanged("CurrentTime"); }
        }
        #endregion
        public MainWindowViewModel()
        {
            //Первоначальная загрузка таблицы Products из базы данных
            db.Products.Load();
            productsObserv = db.Products.Local.ToObservableCollection();

            //Первоначальная загрузка таблицы Users из базы данных
            db.Users.Load();
            UsersObserv = db.Users.Local.ToObservableCollection();

            db.Refills.Load();
            refillProductsObserv = db.Refills.Local.ToObservableCollection();

            //Первоначальная инициализация ObservableCollection saleproductsObserv для сохранения продаваемых товаров
            saleproductsObserv = new ObservableCollection<Sale>();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            //реализация timer.Tick для MVVM ViewModel
            timer.Tick += (sender, args) =>
            {
                time = Convert.ToInt32(PetrolLiters);
                if (time > 0)
                {
                    time--;
                    PetrolLiters = string.Format("{0}", time % 60);
                }
                else
                {
                    timer.Stop();
                    MethodSale_RefillTextBox = "";
                    TotalPetrolPriceTB = "0";
                }
            };
            timer.Interval = new TimeSpan(0, 0, 1);

        }

        #region RADIOBUTTON REFILL

        //выбор значения по умолчанию из enum SelectRefill
        SelectRefill selectRefill = SelectRefill.Litres;
        /// <summary>
        /// свойство для отслеживания выбора из enum SelectRefill
        /// </summary>
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
        /// <summary>
        /// свойство для передачи значения при выборе RadioButton LitersRB
        /// </summary>
        public bool LitersRB_Check
        {
            get { return SelectRefill == SelectRefill.Litres; }
            set { SelectRefill = value? SelectRefill.Litres: SelectRefill;}
        }
        /// <summary>
        /// свойство для передачи значения при выборе RadioButton RublesRB
        /// </summary>
        public bool RublesRB_Check
        {
            get { return SelectRefill == SelectRefill.Rubles;}
            set { SelectRefill = value ? SelectRefill.Rubles : SelectRefill;}
        }
        /// <summary>
        /// свойство проверяющее какой переключатель выбран и передачи нужного значения при нужном переключателе
        /// </summary>
        public string GetResult
        {
            get
            {
                switch (SelectRefill)
                {
                    case SelectRefill.Litres:
                        return litersRB_Content!;
                    case SelectRefill.Rubles:
                        return rublesRB_Content!;
                }
                return "";
            }
        }
        #endregion

        #region THE FUNCTIONALITY OF BRINGING A CAFE WINDOW TO SERVE A NEW CUSTOMER
        /// <summary>
        /// Метод приведения полей Cafe к первоначальным значениям по умолчанию
        /// </summary>
        public void NewCafeBuyer()
        {
            TotalCafePriceTB = "0";
        }
        public void NewPetrolBuyer()
        {
            TotalPetrolPriceTB = "0";
            MethodSale_RefillTextBox = "0";
        }
        #endregion

        #region CANCELLATION OF THE PRODUCT IN saleDataGrid
      
        //public void saleGridDeleteProduct(ref DataGrid datagrid)
        //{
        //    if (PasswordWindow.valueStatus == 1 || PasswordWindow.valueStatus == 2)
        //    {
        //        MessageBoxResult result = MessageBox.Show("Удалить всё кол-во товара - Yes", "Warning", MessageBoxButton.YesNo);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                var index = datagrid.SelectedIndex;
        //                datagrid.Items.RemoveAt(index);
        //            }
        //            catch
        //            {
        //                MessageBox.Show("ВЫДЕЛИТЕ ТОВАР ДЛЯ ОТМЕНЫ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            }
        //        }
        //    }
        //    else if (PasswordWindow.valueStatus == 0)
        //    {
        //        MessageBox.Show("Товар может отменять только Старший Кассир или Администратор", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}

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
            foreach (var petrol in refillProductsObserv)
            {
                if (petrol.Title == combobox.SelectedItem.ToString())
                {
                    textblock.Text = petrol.Price.ToString("#.##");
                }

            }
           
        }
        public void PetrolLoaded(ref ComboBox combobox)
        {

            foreach (var petrol in refillProductsObserv)
            {
                petrolTitle.Add(petrol.Title);
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
        /// <summary>
        /// метод выключения компьютера после завершения работы заправки, если заправка работает не круглосуточно либо нужно по какой-то причине выключить рабочее место кассира
        /// </summary>
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
        /// <summary>
        /// команда выключения компьютера после завершения работы заправки передаваемая на клавишу "Завершение работы системы" через XAML Binding
        /// </summary>
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
        //Методы открытия вспомогательных окон
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

        private bool? OpenQuantityWindow()
        {
            QuantityWindow quantityWindow = new QuantityWindow();
            SetParametersWindow(quantityWindow);
            return quantityWindow.DialogResult;
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
        //Команды открытия вспомогательных окон передаваемые далее в главное окно через XAML Biding
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

        #region METHOD CHECK AND PRINTING

        /// <summary>
        /// Метод для клавиши Печать чека. Метод формирует строку для печати и сохраняет продажи из saleproductsObserv в базу данных
        /// </summary>
        private void PrintAndSaveSale()
        {                
            PrintStr += "--------------------------------------------\n";
            PrintStr += "Заправочный комплекс Лукойл \n";
            PrintStr += "--------------------------------------------\n";
            foreach (var item in saleproductsObserv) 
            {
                PrintStr += $"{item.ProductName} X {item.Quantity} = {item.Amount.ToString("#####.##")} рублей\n";
            }
            var totalAmount = saleproductsObserv.Sum(x => x.Amount);
            PrintStr += $"------------------------------------------------------------------\n";
            PrintStr += $"ИТОГО: {totalAmount.ToString("#####.##")} рублей\n";
            PrintStr += $"------------------------------------------------------------------\n";
            PrintStr += $"Продавец: {PasswordWindow.userName}\nДата\\Время: {DateTime.Now}";
            MessageBoxResult result = MessageBox.Show(PrintStr, "Чек", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    FixedDocument fixedDocument = CreateFixedDocument();
                    printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Fixed Document");
                }
                PrintStr = "";
            }
            else if (result == MessageBoxResult.Cancel)
            {
                PrintStr = "";
            }
            db.Sales.UpdateRange(saleproductsObserv);
            db.SaveChanges();
            NewCafeBuyer();
            NewPetrolBuyer();
            TotalSale = "";
            total = 0;
            if (saleproductsObserv is not null)
            {
                saleproductsObserv.Clear();
            }
        }
        /// <summary>
        /// Метод создания фиксированного документа, такого как чек
        /// </summary>
        /// <returns>fixedDocument</returns>
        private FixedDocument CreateFixedDocument()
        {
            FixedDocument fixedDocument = new FixedDocument();
            fixedDocument.DocumentPaginator.PageSize = new Size(96 * 8.5, 96 * 11); // Размер страницы в пунктах (1 дюйм = 96 пунктов)

            PageContent pageContent = new PageContent();
            fixedDocument.Pages.Add(pageContent);

            FixedPage fixedPage = new FixedPage();
            pageContent.Child = fixedPage;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = PrintStr;
            textBlock.FontSize = 14;
            textBlock.Margin = new Thickness(16, 2, 0, 0);
            fixedPage.Children.Add(textBlock);

            return fixedDocument;
        }

        #endregion

        #region COMMANDA CHECK

        /// <summary>
        /// Команда передаваемая на клавишу Печать чека
        /// </summary>
        private RelayCommand printCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return printCommand ?? new RelayCommand(obj => 
                {
                    PrintAndSaveSale();
                });
            }
        }

        #endregion

        #region METHOD REFILL PAYMENT

        /// <summary>
        /// Метод для клавиши Оплата заправка
        /// </summary>
        private void RefillPayment_Method()
        {
            
            if (GetResult == "Литры")
            {
                try
                {
                    float result = float.Parse(PricePetrolBlock) * float.Parse(MethodSale_RefillTextBox);
                    
                    PetrolLiters = MethodSale_RefillTextBox;
                    TotalPetrolPriceTB = result.ToString();
                    Sale = new() { ProductName = SPetrolTitle, Amount = decimal.Parse(TotalPetrolPriceTB), Quantity = double.Parse(MethodSale_RefillTextBox), NameUsers = PasswordWindow.userName!, Date = DateTime.Today, Time = DateTime.Now.TimeOfDay };
                    var refill = refillProductsObserv.Where(x=> x.Title == Sale.ProductName).First();
                    refill.ProductCount = refill.ProductCount - double.Parse(MethodSale_RefillTextBox);
                    db.Refills.Update(refill);
                    db.SaveChanges();
                    timer.Start();

                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    //MessageBox.Show("ВВедите количество литров");
                }
            }
            else if(GetResult == "Рубли")
            {
                TotalPetrolPriceTB = MethodSale_RefillTextBox;
                double litersD = Convert.ToDouble(MethodSale_RefillTextBox) / Convert.ToDouble(PricePetrolBlock);
                litersD = Math.Round(litersD);
                PetrolLiters = litersD.ToString();
                Sale = new() { ProductName = SPetrolTitle, Amount = decimal.Parse(TotalPetrolPriceTB), Quantity = litersD, NameUsers = PasswordWindow.userName!, Date = DateTime.Today, Time = DateTime.Now.TimeOfDay };
                var refill = refillProductsObserv.Where(x => x.Title == Sale.ProductName).First();
                refill.ProductCount = refill.ProductCount - litersD;
                db.Refills.Update(refill);
                db.SaveChanges();
                timer.Start();
            }
            var productCount = db.Refills.Where(x => x.Title == SPetrolTitle).Select(x => x.ProductCount);
            saleproductsObserv!.Add(Sale);
            total += float.Parse(TotalPetrolPriceTB);
            TotalSale = total.ToString();
            foreach (var product in productCount)
            {
                MessageBox.Show($"{SPetrolTitle} осталось {product.ToString()} литров", "Остаток бензина на заправке");
            }
        }
        #endregion

        #region COMMAND REFILL PAYMENT
        private RelayCommand refillPayment_Command;

        /// <summary>
        /// Команда передающая метод RefillPayment_Method для клавиши Оплата заправка 
        /// </summary>
        public RelayCommand RefillPayment_Command
        {
            get
            {
                return refillPayment_Command ?? new RelayCommand(obj =>
                {
                    RefillPayment_Method();
                });
            }
        }
        #endregion

        #region METHOD CAFE PAYMENT

        /// <summary>
        /// Метод для клавиши Оплата кафе
        /// </summary>
        private void CafePayment_Method()
        {
            decimal Sum = 0;
            if (productsObserv is not null)
            {
                db.Products.UpdateRange(productsObserv);
                db.SaveChanges();
            }

            if (saleproductsObserv is not null)
            {
                foreach (Product product in db.Products)
                {
                    var result = saleproductsObserv.Where(w => w.ProductName == product.title).Select(s => s.Amount);
                    Sum += result.Sum();
                    TotalCafePriceTB = Sum.ToString("#####.##");
                }
            }
            total += float.Parse(TotalCafePriceTB);
            TotalSale= total.ToString("#####.##");
        }
        #endregion

        #region COMMAND CAFE PAYMENT
        private RelayCommand cafePayment_Command;

        /// <summary>
        /// Команда передающая метод CafePayment_Method для клавиши Оплата кафе
        /// </summary>
        public RelayCommand CafePayment_Command
        {
            get
            {
                return cafePayment_Command ?? new RelayCommand(obj =>
                {
                    CafePayment_Method();
                });
            }
        }
        #endregion

        #region METHODS CANCELLATION OF THE PRODUCT AND CANCELLATION OF THE SALE
        private void CancellationProduct_Method()
        {
            foreach (var item in refillProductsObserv)
            {


                if (SaleSelectedProduct is not null)
                {
                    Sale = SaleSelectedProduct;
                    if (Sale.ProductName != item.Title)
                    {
                        product = productsObserv!.First(x => x.Title == Sale.ProductName);
                        product.ProductCount = product.ProductCount + Sale.Quantity;
                        saleproductsObserv.Remove(Sale);
                        db.Products.UpdateRange(productsObserv!);
                        db.SaveChanges();
                    }
                    else if (Sale.ProductName == item.Title)
                    {
                        refill = refillProductsObserv.First(x => x.Title == Sale.ProductName);
                        refill.ProductCount = refill.ProductCount + Sale.Quantity;
                        saleproductsObserv.Remove(Sale);
                        db.Refills.UpdateRange(refillProductsObserv);
                        db.SaveChanges();
                    }
                }
            }
        }

        private void CancellationSale_Method()
        {

        }

        #endregion

        #region COMMAND CANCELLATION OF THE PRODUCT AND CANCELLATION OF THE SALE
        private RelayCommand cancellationProduct_Command;
        public RelayCommand CancellationProduct_Command
        {
            get
            {
                return cancellationProduct_Command ?? new RelayCommand(obj =>
                {
                    CancellationProduct_Method();
                });
            }
        }

        private RelayCommand cancellationSale_Command;
        public RelayCommand CancellationSale_Command
        {
            get
            {
                return cancellationSale_Command ?? new RelayCommand(obj =>
                { 
                    CancellationSale_Method();
                });
            }
        }


        #endregion
    }
}
