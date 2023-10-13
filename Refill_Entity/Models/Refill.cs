using System;
using System.Collections.Generic;

namespace Refill_Entity.Models;

public partial class Refill : Notify
{
    public int Id { get; set; }
    private string? title;
    private decimal price;
    private double productCount;
    public string Title
    {
        get => title!;
        set
        { title = value; OnPropertyChanged("Title"); }
    }

    public decimal Price
    {
        get => price;
        set { price = value; OnPropertyChanged("Price"); }
    }

    public double ProductCount
    {
        get => productCount;
        set { productCount = value; OnPropertyChanged("ProductCount"); }
    }
}
