using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Common;
public  abstract class EntityBase////<T> where T : IEquatable<T>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModufiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
