using System;
using System.Collections.Generic;

namespace P_Progra_Web.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int ListaId { get; set; }

    public string Mensaje { get; set; } = null!;

    public int Archivos { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual ContactList Lista { get; set; } = null!;
}
