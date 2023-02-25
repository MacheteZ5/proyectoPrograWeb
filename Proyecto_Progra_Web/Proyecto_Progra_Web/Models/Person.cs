using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Web.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string Email { get; set; } = null!;

    public bool Genero { get; set; }

    public DateTime? FecTransac { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
