using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Refill_Entity.Models;

public partial class Product : Notify
{
    public int Id { get; set; }

    public string? title;
    public decimal price;
    public double productCount;
    public string? category;
    public string Title
    {
        get { return title!; }
        set { title = value; OnPropertyChanged("Title"); }
    }

    public decimal Price
    {
        get { return price; }
        set { price = value; OnPropertyChanged("Price"); }
    }

    public double ProductCount
    {
        get { return productCount; }
        set { productCount = value; OnPropertyChanged("ProductCount"); }
    }

    public string? Category
    {
        get { return category; }
        set { category = value; OnPropertyChanged("Category"); }
    }


}
