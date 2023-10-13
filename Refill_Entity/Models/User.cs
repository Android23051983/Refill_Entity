using System;
using System.Collections.Generic;

namespace Refill_Entity.Models;

public partial class User : Notify
{
    public int Id { get; set; }
    private string? name;
    private string? passwd;
    private int status;
    public string Name
    {
        get => name!;
        set { name = value; OnPropertyChanged("Name"); }
    }
    public string? Passwd
    {
        get => passwd;
        set { passwd = value!; OnPropertyChanged("Passwd"); }
    }

    public int Status
    {
        get => status;
        set { status = value; OnPropertyChanged("Status"); }
    }
}
