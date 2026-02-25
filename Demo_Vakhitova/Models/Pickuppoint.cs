using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Pickuppoint
{
    public int PickuppointId { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
