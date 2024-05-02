using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Models;

public class Chat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }

    public virtual ICollection<Message> Messages { get; set; }
}