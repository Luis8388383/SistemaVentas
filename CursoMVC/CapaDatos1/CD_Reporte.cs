using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad1;
using System.Globalization;

namespace CapaDatos1
{
    public class CD_Reporte
    {
        public List<Reporte>Ventas(string fechainicio,string fechafin,string idtransaccion)
        {
            List<Reporte> Lista = new List<Reporte>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
               {
                   SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Reporte()
                            {
                                FechaVenta = dr["FechaVenta"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Precio = Convert.ToDecimal (dr["Precio"], new CultureInfo("es-PE")),
                                Cantidad = Convert.ToInt32 (dr["Cantidad"].ToString()),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                IdTransaccion = (dr["IdTransaccion"]).ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Reporte>();
            }
            return Lista;
        }

        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) 
                        {
                            objeto = new Dashboard();
                            {
                                objeto.TotalCliente = Convert.ToInt32(dr["TotalCliente"]);
                                objeto.TotalVenta = Convert.ToInt32(dr["TotalVenta"]);
                                objeto.TotalProducto = Convert.ToInt32(dr["TotalProducto"]);




                            };
                            
                        }
                    }
                }
            }
            catch
            {
                objeto = new Dashboard();
            }

            return objeto;
        }
    }
}

