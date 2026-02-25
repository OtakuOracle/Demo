using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Tovar
{
    public int TovarId { get; set; }

    public string? TovarName { get; set; }

    public virtual ICollection<Listtovar> Listtovars { get; set; } = new List<Listtovar>();
}
