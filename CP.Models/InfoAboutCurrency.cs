using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace CP.Models
{
    public class InfoAboutCurrency
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(20)]
        [DisplayName("Валюта")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Цiна")]
        [Range(1, 300, ErrorMessage = "Ціна не може бути більше 300 грн за одиницю та дорівнювати 0")]
        public double AskedCoursePriceTo { get; set; }
        [Required]
        [DisplayName("Доступна к-сть")]
        [Range(1, 10000, ErrorMessage = "Доступна кiлькiсть не може перевищувати 10000 та дорівнювати 0")]
        public int AvailOfAskedCourse { get; set; }
    }
}
