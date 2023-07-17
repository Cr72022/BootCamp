using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BootCamp.Core.DTOs
{
   
    public class ChargeWalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }
    }
    public class WalletViewModel
    {

        [Display(Name = "مبلغ")]
        public int Amount { get; set; }
        [Display(Name = "نوع")]
        public int Type { get; set; }
        [Display(Name = "توضیح")]
        public string Description { get; set; }
        [Display(Name = "تاریخ")]
        public DateTime DateTime { get; set; }

    }
}
