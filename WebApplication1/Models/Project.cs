using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Project
{
    public string? Pname { get; set; }

    public string Pnumber { get; set; } = null!;

    public string? Plocation { get; set; }

    public string? Dnum { get; set; }

    public virtual Department? DnumNavigation { get; set; }

    public virtual ICollection<WorkOn> WorkOns { get; set; } = new List<WorkOn>();
}
