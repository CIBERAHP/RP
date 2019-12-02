using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class orders
    {
        [Display(Name = "Nro Orden", Order = 0)]
        public int idpedido { get; set; }

        [Display(Name = "Fecha", Order = 1)]
        public DateTime fechapedido { get; set; }

        [Display(Name = "Nombre/Razon Social", Order = 2)]
        public string nombrecia { get; set; }

        [Display(Name = "Telefono", Order = 3)]
        public string fono { get; set; }

    }
}