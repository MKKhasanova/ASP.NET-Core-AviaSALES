using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Онлайн_билеты.Models
{
    public class Tickets
    {
        [Key]
        public int Id { get; set; }
        public string Flight_number { get; set; }
        public int OtkudaId { get; set; }
        public int KudaId { get; set; }
        public DateTime DateP { get; set; } // изменен тип данных на DateTime
        public Nullable<int> Kol { get; set; }
        public int Price { get; set; }
        public Otkuda otkuda { get; set; }
        public Kuda kuda { get; set; }
    }
}
