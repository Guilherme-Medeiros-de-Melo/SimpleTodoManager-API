using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTodoManager
{
    public enum Status { Pendente=0, EmProgresso=1, Concluída=2 }
    public class Todo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string title { get; set; }
        public string? description { get; set; }
        public DateOnly? endDate { get; set; }
        public Status status {  get; set; }
    } 
}
