using System.ComponentModel.DataAnnotations;

namespace Онлайн_билеты.Models
{
    public class Check
    {
        [Key]
        public int Id { get; set; }
        public string Flight_number { get; set; }
        public string Client_Name { get; set; }
        public DateTime Data { get; set; }
        public int Price { get; set; }

    }
}
