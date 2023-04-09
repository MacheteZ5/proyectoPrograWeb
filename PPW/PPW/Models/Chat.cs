using System;
using System.Collections.Generic;

namespace PPW.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int ContactId { get; set; }

    public int? UserId { get; set; }

    public string? Mensaje { get; set; }

    public string? Archivos { get; set; }

    public DateTime? FecTransac { get; set; }

    /*public virtual Contact Contact { get; set; } = null!;

    public virtual User? User { get; set; }*/
}
