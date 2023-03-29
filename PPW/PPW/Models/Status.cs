using System;
using System.Collections.Generic;

namespace PPW.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Vigente { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
