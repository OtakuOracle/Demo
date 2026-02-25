using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Listtovar> Listtovars { get; set; } = new List<Listtovar>();
}
