using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CP.Models
{
    public class Payments
    {
        [Key]
		[DisplayName("ID платежу")]
		public int ID { get; set; }

        [Required]
        [DisplayName("Сума платежу")]
        public int Sum { get; set; }

        [Required]
        [DisplayName("Тип платежу")]
        public string Type { get; set; }

        [Required]
        [DisplayName("Дата створення платежу")]
        public DateTime DateOfMakingPayments { get; set; }
        [DisplayName("Опис платежу")]

        public string Description { get; set; }
    }
}
