using System;
using System.Collections.Generic;

namespace demo_material.Models;

public partial class OrderTovar
{
    public int OrderTovarsId { get; set; }

    public int OrderId { get; set; }

    public int TovarId { get; set; }

    public int? Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Tovar Tovar { get; set; } = null!;
}
