namespace Proyecto_Progra_Web.Models
{
    public class UserContacts
    {
        public User User { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
