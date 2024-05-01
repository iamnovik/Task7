using Microsoft.AspNetCore.Identity;

namespace lab7.Models;

public class Contact : IdentityUser
{
    public string FullName { get; set; }
    
    public string Email { get; set; }
    
    public virtual ICollection<ContactRelationship> ContactRelationships { get; set; }
    
    public bool IsInAddressBook(string userId)
    {
        return false;
        //return ContactsRelatedList.Any(c => c.Id == userId);
    }


}


