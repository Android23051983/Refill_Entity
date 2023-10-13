using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
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
using Refill_Entity.ViewModels;
using Refill_Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Refill_Entity
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public static int valueStatus = -1; // переменная хранящая статус пользователя. Если пользователь не авторизовался значение равно -1
        public static string? userName;
        public PasswordWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            this.Loaded += Window_load;
        }

        private void Window_load(object sender, RoutedEventArgs e)
        {
            using (RefillAndMiniCafeContext db = new RefillAndMiniCafeContext())
            {
                var result = db.Users.ToList();
                foreach (var user in result)
                {
                    nameBox.Items.Add(user.Name);
                }
                nameBox.SelectedIndex = 0;
            }
             
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            using (RefillAndMiniCafeContext db = new RefillAndMiniCafeContext())
            {
                var users = db.Users.ToList();
                foreach (var user in users)
                {
                    if (user.Name == nameBox.Text && user.Passwd == passwordBox.Password)
                    {
                        valueStatus = user.Status;
                        userName = user.Name;
                        this.Owner.Title = $"Заправочный комплекс Лукойл (работает кассир: {user.Name})";
                        this.DialogResult = true;
                        nameBox.SelectedIndex = 0;
                    }
                }
                if (valueStatus == -1)
                {
                    MessageBox.Show("Введены неправильный логин или пароль", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.F4)
            {
                e.Handled = true;
            }
        }
    }
}
