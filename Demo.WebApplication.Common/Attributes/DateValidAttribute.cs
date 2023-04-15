using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Attributes
{
    public class DateValidAttribute : Attribute
    {
        public DateTime Date { get; } = DateTime.Now;
    }
}
