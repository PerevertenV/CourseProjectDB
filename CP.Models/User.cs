using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace CP.Models
{
    public class User
    {
        [Key]
		[Required(ErrorMessage = "Не лишайте поле пустим!")]
		public int ID { get; set; }
        [Required]
        [StringLength(20)]
        [DisplayName("user name")]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Прізвище Ім'я")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        public string role { get; set; }


    }
}
