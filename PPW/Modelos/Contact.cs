using System;
using System.Collections.Generic;

namespace Modelos;

public partial class Contact
{
    public int Id { get; set; }

    public int PuserId { get; set; }

    public int SuserId { get; set; }

    public DateTime? FecTransac { get; set; }

    /*public virtual ICollection<Chat> Chats { get; } = new List<Chat>();

    public virtual User Puser { get; set; } = null!;

    public virtual User Suser { get; set; } = null!;*/
}
