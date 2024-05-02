using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Models;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    
    [ForeignKey("Chat")]
    public int ChatId { get; set; }
    
    public virtual Chat Chat { get; set; }
    
    public string SenderId { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}