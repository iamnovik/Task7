using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Models;

public class ContactRelationship
{
    public string SourceContactId { get; set; }
    public Contact SourceContact { get; set; }

    public string TargetContactId { get; set; }
    public Contact TargetContact { get; set; }
}
