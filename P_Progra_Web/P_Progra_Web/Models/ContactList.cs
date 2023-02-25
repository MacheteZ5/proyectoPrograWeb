using System;
using System.Collections.Generic;

namespace P_Progra_Web.Models;

public partial class ContactList
{
    public int Id { get; set; }

    public int PrimerUserId { get; set; }

    public int SegundoUserId { get; set; }

    public int StatusId { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual ICollection<Chat> Chats { get; } = new List<Chat>();

    public virtual Person PrimerUser { get; set; } = null!;

    public virtual Person SegundoUser { get; set; } = null!;
}
