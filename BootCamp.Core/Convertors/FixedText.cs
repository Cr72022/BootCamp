using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCamp.Core.Convertors
{
    public static class FixedText
    {
        public static string FixedEmail(this string emial)
        {
            return emial.ToLower();
        }
    }
}
