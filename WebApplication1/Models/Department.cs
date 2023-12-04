using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Department
{
    public string? Dname { get; set; }

    public string Dnum { get; set; } = null!;

    public string? Mgrssn { get; set; }

    public DateTime? MgrstartDate { get; set; }

    public virtual ICollection<Empolyee> Empolyees { get; set; } = new List<Empolyee>();

    public virtual Empolyee? MgrssnNavigation { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
