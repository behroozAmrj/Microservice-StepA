using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Common;
public  abstract class EntityBase//<T> where T : struct //Equatable<T>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModufiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
