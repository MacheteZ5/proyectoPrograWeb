using System;
using System.Collections.Generic;

namespace PPW.Models;

public partial class Contact
{
    public int Id { get; set; }

    public int PrimerUserId { get; set; }

    public int SegundoUserId { get; set; }

    public DateTime? FecTransac { get; set; }
}
