using System.ComponentModel.DataAnnotations;

namespace Онлайн_билеты.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Client_Name { get; set; }
        public int Age { get; set; }
        public string Passport { get; set; }
        public string Phone { get; set; }
    }
}
