using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyderWeb.Database.Models
{
    public class CommandModule : BaseDatabaseObject
    {
        public string ModuleName { get; set; }
        public List<int> CommandIds { get; set; }
        public int ServerId { get; set; }
        public bool Enabled { get; set; }

        public List<int> AuthorizedUsers { get; set; }
        public List<int> UnauthorizedUsers { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}