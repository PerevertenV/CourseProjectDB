using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Models.VModels
{
    public class PaymentsVM
    {
        public Payments Payments { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CurrencyList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        public IEnumerable<SelectListItem> PaymentsTypeList { get; set; }

    }
}
