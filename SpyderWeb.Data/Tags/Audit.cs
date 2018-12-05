using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyderWeb.Data.Tags
{
    public class Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TagId { get; set; }
        public long __UserId { get; set; }
        [NotMapped]
        public ulong UserId
        {
            get => (ulong)__UserId;
            set => __UserId = (long)value;
        }
        public DateTimeOffset Timestamp { get; set; }
        public AuditType AuditType { get; set; }
    }
}
