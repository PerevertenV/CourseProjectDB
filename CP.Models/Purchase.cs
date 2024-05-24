using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Models
{
    public class Purchase
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("Валюта")]
        public int CurrencyID { get; set; }
        [ForeignKey(nameof(CurrencyID))]
        [ValidateNever]
        public InfoAboutCurrency InfoAboutCurrency { get; set; }

        
        [DisplayName("ІD користувача що здійснює операцію")]
        public int? IDOfUser { get; set; }
        [ForeignKey(nameof(IDOfUser))]
        [ValidateNever]
        public User? User { get; set; }
        
        [DisplayName("ІD працівника що обробляє операцію")]
        public int? IDOfEmployee { get; set; }
		[ForeignKey(nameof(IDOfEmployee))]
		[ValidateNever]
		public User? UserEmployee { get; set; }

		[DisplayName("Внесені кошти")]
        [Range(1, 100000, ErrorMessage = "Вкажіть внесену суму!")]
        public double? DepositedMoney { get; set; }

        
        [DisplayName("Сума для обміну")]
		[Range(1, 5000, ErrorMessage = "Cума має бути від 1 до 5000 одиниць!")]
		public double SumOfCurrency { get; set; }

        
        [DisplayName("Решта")]
        public double? MoneyToReturn { get; set; }

        public static double PDVPercent = 0.2;

        [Required]
        [DisplayName("Сума для обміну в грн")]
        public double SumInUAH { get; set; }

        
        [DisplayName("Дата час здійснення операції")]
        public DateTime? DateOfMakingPurchase { get; set; }

		[Required]
		[DisplayName("Статус обробки")]
		public string State { get; set; }
    }

}
