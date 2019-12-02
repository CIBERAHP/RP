using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class orderdetail
    {
        [Display(Name = "Codigo", Order = 0)]
        public int idproducto { get; set; }

        [Display(Name = "Descripcion", Order = 1)]
        public string nombreproducto { get; set; }

        [Display(Name = "Cantidad", Order = 2)]
        public decimal cantidad { get; set; }

        [Display(Name = "Precio", Order = 3)]
        public decimal precio { get; set; }

        [Display(Name = "SubTotal", Order = 4)]
        public decimal subtotal { get; set; }    

    }
}