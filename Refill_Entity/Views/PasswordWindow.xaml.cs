using System.Windows;
using System.Windows.Input;
using Refill_Entity.ViewModels;

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
                        this.DialogResult = true;
                    }
                }
                if (valueStatus == -1)
                {
                    MessageBox.Show("Введены неправильный логин или пароль", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            MainWindow.ViewModel.Title = $"Заправочный комплекс Лукойл (работает кассир: {userName})";
            
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
