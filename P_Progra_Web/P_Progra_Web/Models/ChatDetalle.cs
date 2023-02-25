using System;
using System.Collections.Generic;

namespace P_Progra_Web.Models;

public partial class ChatDetalle
{
    public int Id { get; set; }

    public int ChatId { get; set; }

    public string Mensaje { get; set; } = null!;

    public int Archivos { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual Chat Chat { get; set; } = null!;
}
