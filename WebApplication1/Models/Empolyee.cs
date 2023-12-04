using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Empolyee
{
    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string Ssn { get; set; } = null!;

    public DateTime? Date { get; set; }

    public string? Address { get; set; }

    public string? Sex { get; set; }

    public decimal? Salary { get; set; }

    public string? Superson { get; set; }

    public string? Dno { get; set; }

    public virtual ICollection<Debendent> Debendents { get; set; } = new List<Debendent>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual Department? DnoNavigation { get; set; }

    public virtual ICollection<Empolyee> InverseSupersonNavigation { get; set; } = new List<Empolyee>();

    public virtual Empolyee? SupersonNavigation { get; set; }

    public virtual ICollection<WorkOn> WorkOns { get; set; } = new List<WorkOn>();
    public override string ToString()
    {
        return $"{Fname} {Lname}";
    }
}
