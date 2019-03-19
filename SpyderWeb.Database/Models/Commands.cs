using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyderWeb.Database.Models
{
    public class Commands : BaseDatabaseObject
    {
        public string CommandName { get; set; }
        public bool Enabled { get; set; }
        public object CommandData { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}