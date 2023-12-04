using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Debendent
{
    public string Essn { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Sex { get; set; }

    public DateTime? Date { get; set; }

    public virtual Empolyee EssnNavigation { get; set; } = null!;
}
