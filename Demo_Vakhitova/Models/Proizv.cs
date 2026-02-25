using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Proizv
{
    public int ProizvId { get; set; }

    public string? ProizvName { get; set; }

    public virtual ICollection<Listtovar> Listtovars { get; set; } = new List<Listtovar>();
}
