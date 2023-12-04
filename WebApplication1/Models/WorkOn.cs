using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class WorkOn
{
    public string Essn { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Hour { get; set; }

    public virtual Empolyee EssnNavigation { get; set; } = null!;

    public virtual Project PnoNavigation { get; set; } = null!;
}
