using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Core.Domain
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } 
        public bool IsCompleted { get; set; }
        [Required]
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime2(7)")]

        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        [Column(TypeName = "datetime2(7)")]
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
