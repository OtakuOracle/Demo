using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Postavschik
{
    public int PostavschikId { get; set; }

    public string? PostavschikName { get; set; }

    public virtual ICollection<Listtovar> Listtovars { get; set; } = new List<Listtovar>();
}
