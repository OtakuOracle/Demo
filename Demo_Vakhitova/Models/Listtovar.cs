using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Listtovar
{
    public int ListtovarId { get; set; }

    public string? Art { get; set; }

    public int? TovarId { get; set; }

    public string? Unity { get; set; }

    public int? Count { get; set; }

    public int? PostavschikId { get; set; }

    public int? ProizvId { get; set; }

    public int? CategoryId { get; set; }

    public int? Discountnow { get; set; }

    public int? Kolvo { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }
    public Bitmap GetPhoto
    {
        get
        {
            if (Photo != null && Photo != "")
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Photo);
            }
            else
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/images/11.jpeg");
            }
        }
    }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Postavschik? Postavschik { get; set; }

    public virtual Proizv? Proizv { get; set; }

    public virtual Tovar? Tovar { get; set; }
}
