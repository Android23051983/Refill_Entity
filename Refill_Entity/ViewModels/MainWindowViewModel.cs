using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refill_Entity.Models;


namespace Refill_Entity.ViewModels
{
    public class MainWindowViewModel : Notify
    {
        public ObservableCollection<User> UsersObserv { get; set; }
        public static List<string> petrolTitle = new();
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

        public MainWindowViewModel()
        {
            using (RefillAndMiniCafeContext db = new RefillAndMiniCafeContext())
            {
                var tempProduct = db.Products.ToList();
                productsObserv = new ObservableCollection<Product>();
                foreach (var item in tempProduct)
                {
                    Product product = new Product { Title = item.Title, Price = item.Price, ProductCount = item.ProductCount, Category = item.Category };
                    productsObserv.Add(product);
                }

                var tempUser = db.Users.ToList();
                UsersObserv = new ObservableCollection<User>();
                foreach (var item in tempUser)
                {
                    User user = new User { Name = item.Name, Passwd = item.Passwd, Status = item.Status };
                    UsersObserv.Add(user);
                }

                //var tempSale = db.Sales.ToList();
                saleproductsObserv = new ObservableCollection<Sale>();
                //foreach (var item in tempSale)
                //{
                //    Sale sale = new Sale { ProductName = item.ProductName, Amount = item.Amount, Quantity = item.Quantity, NameUsers = item.NameUsers, Date = item.Date, Time = item.Time };
                //    saleproductsObserv.Add(sale);
                //}
            }
            
            
        }
        public static void PetrolLoaded()
        {
            using (RefillAndMiniCafeContext db = new())
            {
                var result = db.Refills.ToList();
                foreach (var petrol in result)
                {
                    petrolTitle.Add(petrol.Title);
                }
            }
        }
        
        
    }
}
