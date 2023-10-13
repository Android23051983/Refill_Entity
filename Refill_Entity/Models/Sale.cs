using System;
using System.Collections.Generic;

namespace Refill_Entity.Models;

public partial class Sale : Notify
{
    public int Id { get; set; }
    private string? productName;
    private decimal amount;
    private double quantity;
    private string? nameUsers;
    private DateTime date;
    private TimeSpan time;
    public string ProductName
    {
        get => productName!;
        set
        { productName = value; OnPropertyChanged("ProductName"); }
    }

    public decimal Amount
    {
        get => amount;
        set
        { amount = value; OnPropertyChanged("Amount"); }
    }

    public double Quantity
    {
        get => quantity;
        set
        { quantity = value; OnPropertyChanged("Quantity"); }
    }

    public string NameUsers
    {
        get => nameUsers!;
        set
        { nameUsers = value; OnPropertyChanged("NameUsers"); }
    }

    public DateTime Date
    {
        get => date;
        set
        { date = value; OnPropertyChanged("date"); }
    }

    public TimeSpan Time
    {
        get { return time; }
        set { time = value; OnPropertyChanged("Time"); }
    }
}
