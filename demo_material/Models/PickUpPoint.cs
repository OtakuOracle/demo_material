using System;
using System.Collections.Generic;

namespace demo_material.Models;

public partial class PickUpPoint
{
    public int PickUpPointId { get; set; }

    public string PickUpPointName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
