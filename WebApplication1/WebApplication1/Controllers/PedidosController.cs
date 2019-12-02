using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.Controllers
{
    public class PedidosController : Controller
    {
        SqlConnection cn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["cadena"].ConnectionString);

        List<orders> OrdersPending()
        {
            List<orders> temporal = new List<orders>();

            SqlCommand cmd = new SqlCommand("proc_order_cust", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@confir", 0);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                orders reg = new orders
                {
                    idpedido = dr.GetInt32(0),
                    fechapedido = dr.GetDateTime(1),
                    nombrecia = dr.GetString(2),
                    fono = dr.GetString(3)
                };

                temporal.Add(reg);
            }
            dr.Close();
            cn.Close();

            return temporal;
        }
        public ActionResult PedidosPendientes()
        {
            return View(OrdersPending());
        }
        public ActionResult Home()
        {
            return View(OrdersPending());
        }

        List<orderdetail> OrdersDetails(int id)
        {
            String cliente = "";
            int idOrden = 0;
            Decimal total = 0;

            List<orderdetail> temporal = new List<orderdetail>();

            SqlCommand cmd = new SqlCommand("proc_order_detail", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idorder", id);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                orderdetail reg = new orderdetail
                {
                    idproducto = dr.GetInt32(0),
                    nombreproducto = dr.GetString(1),
                    cantidad = dr.GetInt16(2),
                    precio = dr.GetDecimal(3),
                    subtotal = dr.GetDecimal(4)
                };

                temporal.Add(reg);
                cliente = dr.GetString(5);
                idOrden = dr.GetInt32(6);
                total = total + dr.GetDecimal(4);
            }
            dr.Close();
            cn.Close();

            ViewBag.cliente = cliente;
            ViewBag.idOrden = idOrden;
            ViewBag.total = total;


            return temporal;
        }
        public ActionResult Detail(int id)
        {
            return View(OrdersDetails(id));
        }

        [HttpPost]
        public ActionResult Update(int idord, string fecha, string comentarios, bool confirmado= false)
        {
            int confirmed = 0;

            /*Validamos el formato de fecha
             Tamaño, Formato "/" y numeros
             */            
            if (fecha.Length < 10 ||
                fecha.Substring(2, 1) != "/" || fecha.Substring(5, 1) != "/" ||
                !IsNumeric(fecha.Substring(0, 2)) || !IsNumeric(fecha.Substring(3, 2)) || 
                !IsNumeric(fecha.Substring(6, 4))
                )
                return RedirectToAction("Detail", new { id = idord });

            if (confirmado == true)
                confirmed = 1;//Confirmado:1, No confirmado:0

            try
            {
                SqlCommand cmd = new SqlCommand("proc_update_order", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idorder", idord);
                cmd.Parameters.AddWithValue("@confirm", confirmed);
                cmd.Parameters.AddWithValue("@fec", fecha);
                cmd.Parameters.AddWithValue("@com", comentarios);

                cn.Open();

                int q = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                ViewBag.mensaje = ex.Message;                
            }
            finally
            {
                cn.Close();
            }

            return RedirectToAction("PedidosPendientes");            
        }

        //metodo para validar si los valores son numericos
        private bool IsNumeric(string num)
        {
            try
            {
                double x = Convert.ToDouble(num);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}