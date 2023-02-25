using System;
using System.Collections.Generic;

namespace P_Progra_Web.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int StatusId { get; set; }

    public int IdPersona { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual Person IdPersonaNavigation { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
