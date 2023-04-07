using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Web.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int StatusId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string Email { get; set; } = null!;

    public bool Genero { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual ICollection<Contact> ContactPrimerUsers { get; } = new List<Contact>();

    public virtual ICollection<Contact> ContactSegundoUsers { get; } = new List<Contact>();

    /*public virtual Status Status { get; set; } = null!;*/
}
