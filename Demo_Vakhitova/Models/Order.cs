using System;
using System.Collections.Generic;

namespace Demo_Vakhitova.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? Listtovar { get; set; }

    public DateOnly? DateOrder { get; set; }

    public DateOnly? DateDelivery { get; set; }

    public int? PickuppointId { get; set; }

    public int? RoleId { get; set; }

    public int? Code { get; set; }

    public string? Status { get; set; }

    public int? Quantity { get; set; }

    public virtual Listtovar? ListtovarNavigation { get; set; }

    public virtual Pickuppoint? Pickuppoint { get; set; }

    public virtual Role? Role { get; set; }
}
