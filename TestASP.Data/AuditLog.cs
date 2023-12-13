using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using TestASP.Data.Enums;

namespace TestASP.Data
{

    public class AuditLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public Dictionary<string, object> KeyValues { get; set;} 
        public Dictionary<string, object>? OldValues { get; set;} 
        public Dictionary<string, object> NewValues { get; set; }
        public List<string> AffectedColumns { get; set; }
        public string Actor { get; set; }
    }

    public class AuditEntry
    {
        public EntityEntry Entry { get; }
        public string Actor { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();

        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public AuditLog ToAudit()
        {
            var audit = new AuditLog();
            audit.Action = AuditType.ToString();
            audit.TableName = TableName;
            audit.DateTime = DateTime.Now;
            audit.KeyValues = KeyValues;
            audit.OldValues = OldValues;
            audit.NewValues = NewValues;
            audit.AffectedColumns = ChangedColumns;
            audit.Actor = Actor;
            return audit;
        }
    }
}

