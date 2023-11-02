using System.Collections.Immutable;
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
using System.Windows.Threading;
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

        public MainWindowViewModel ViewModel { get; set; }
     
        public MainWindow()
        {
            InitializeComponent();
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
            litersRB.IsChecked = true;

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
            if (ViewModel.saleproductsObserv.Count() > 0)
            {
                MessageBox.Show("Выход невозможен. Завершите продажу или отмените товары", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ViewModel.saleproductsObserv.Count() == 0)
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

        }
        /// <summary>
        /// Нажатие клавиши колонки. Сделано для примера как можно сделать для нескольких колонок, функционал которых здесь не реализован.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Column1Btn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PetrolButton_Click(ref columnNumber, ref ColumnNamber_1, ref petrolBox);
        }

        /// <summary>
        /// Выполнение метода ViewModel.PetrolSelection для передачи цены бензина из базы данных на экран. Поле petrolBox - это наименование бензина, а price PetrolBlock - поле с ценой бензина.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void petrolBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.PetrolSelection(ref petrolBox, ref pricePetrolBlock);
        }


    }
}