using System.ComponentModel.DataAnnotations.Schema;

namespace SpyderWeb.Database.Models
{
    public class BaseDatabaseObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
    }
}