using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Models.VModels
{
	public class RequestsVM
	{
		public User User { get; set; }
		public InfoAboutCurrency IAC { get; set; }
		public Purchase Purchase { get; set; }
		public Payments Payments { get; set; }

		public IEnumerable<SelectListItem> SelectUser {  get; set; }
		public IEnumerable<SelectListItem> SelectIAC{  get; set; }
		public IEnumerable<SelectListItem> SelectPurchase{  get; set; }
		public IEnumerable<SelectListItem> SelectPayments {  get; set; }

		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }

    }
}
