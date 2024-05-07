using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        [DisplayName("ІD користувача що здійсеює операцію")]
        public int IDOfUser { get; set; }
        [ForeignKey(nameof(IDOfUser))]
        [ValidateNever]
        public User User { get; set; }

        [Required]
        [DisplayName("Внесені кошти")]
        public double DepositedMoney { get; set; }

        [Required]
        [DisplayName("Валюта для обміну")]
        public double SumOfCurrency { get; set; }

        [Required]
        [DisplayName("Решта")]
        public double MoneyToReturn { get; set; }

        public static double PDVPercent = 0.2;

        [Required]
        [DisplayName("Сума для обміну")]
        public double SumInUAH { get; set; }

        [Required]
        [DisplayName("Дата час здійснення операції")]
        public DateTime DateOfMakingPurchase { get; set; }

    }
}
