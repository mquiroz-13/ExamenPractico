using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;
using CajeroAutomaticoAPI.Models;

[EnableCors(origins: "*", headers: "*", methods: "*")] 
public class CajeroController : ApiController
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    // GET: api/Cajero/ObtenerTiposOperacion
    [HttpGet]
    public IHttpActionResult ObtenerTiposOperacion()
    {
        var response = new ApiResponse();
        var tipos = new List<TipoOperacion>();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TC_TIPOOPERA", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipos.Add(new TipoOperacion
                    {
                        Fiidoperacion = Convert.ToInt32(reader["Fiidoperacion"]),
                        Fcdescoper = reader["Fcdescoper"].ToString(),
                        Fcusuarioalta = reader["Fcusuarioalta"].ToString(),
                        Fdfechaalta = Convert.ToDateTime(reader["Fdfechaalta"])
                    });
                }
            }

            response.Codigo = 200;
            response.Mensaje = "Operación exitosa";
            response.Data = tipos;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Codigo = 500;
            response.Mensaje = "Error: " + ex.Message;
            return InternalServerError(ex);
        }
    }

    // POST: api/Cajero/InsertarTipoOperacion
    [HttpPost]
    public IHttpActionResult InsertarTipoOperacion([FromBody] TipoOperacion tipoOperacion)
    {
        var response = new ApiResponse();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO TC_TIPOOPERA (Fiidoperacion, Fcdescoper, Fcusuarioalta, Fdfechaalta) " +
                    "VALUES (@Fiidoperacion, @Fcdescoper, @Fcusuarioalta, @Fdfechaalta)", conn);

                cmd.Parameters.AddWithValue("@Fiidoperacion", tipoOperacion.Fiidoperacion);
                cmd.Parameters.AddWithValue("@Fcdescoper", tipoOperacion.Fcdescoper);
                cmd.Parameters.AddWithValue("@Fcusuarioalta", tipoOperacion.Fcusuarioalta);
                cmd.Parameters.AddWithValue("@Fdfechaalta", DateTime.Now);

                cmd.ExecuteNonQuery();
            }

            response.Codigo = 200;
            response.Mensaje = "Tipo de operación registrado correctamente";
            response.Data = tipoOperacion;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Codigo = 500;
            response.Mensaje = "Error: " + ex.Message;
            return InternalServerError(ex);
        }
    }

    [HttpGet]
    public IHttpActionResult ObtenerEntradasJournal()
    {
        var response = new ApiResponse();
        var entradas = new List<Transaccion>(); 

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT
                        TJ.Fiidtrans,
                        TJ.Fczona,
                        TJ.Fdfechaopera,
                        TJ.Fiidoperacion,
                        TJ.Fcusuarioalta,
                        TJ.Fdfechaalta,
                        TT.Fcdescoper AS TipoOperacion_Fcdescoper,
                        TT.Fcusuarioalta AS TipoOperacion_Fcusuarioalta,
                        TT.Fdfechaalta AS TipoOperacion_Fdfechaalta
                    FROM TA_JOURNAL AS TJ
                    INNER JOIN TC_TIPOOPERA AS TT ON TJ.Fiidoperacion = TT.Fiidoperacion";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    entradas.Add(new Transaccion
                    {
                        Fiidtrans = Convert.ToInt32(reader["Fiidtrans"]),
                        Fczona = reader["Fczona"].ToString(),
                        Fdfechaopera = Convert.ToDateTime(reader["Fdfechaopera"]),
                        Fiidoperacion = Convert.ToInt32(reader["Fiidoperacion"]),
                        Fcusuarioalta = reader["Fcusuarioalta"].ToString(),
                        Fdfechaalta = Convert.ToDateTime(reader["Fdfechaalta"]),
                        TipoOperacion = new TipoOperacion 
                        {
                            Fiidoperacion = Convert.ToInt32(reader["Fiidoperacion"]), 
                            Fcdescoper = reader["TipoOperacion_Fcdescoper"].ToString(),
                            Fcusuarioalta = reader["TipoOperacion_Fcusuarioalta"].ToString(),
                            Fdfechaalta = Convert.ToDateTime(reader["TipoOperacion_Fdfechaalta"])
                        }
                    });
                }
            }

            response.Codigo = 200;
            response.Mensaje = "Operación exitosa";
            response.Data = entradas;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Codigo = 500;
            response.Mensaje = "Error: " + ex.Message;
            return InternalServerError(ex);
        }
    }

    // POST: api/Cajero/InsertarEntradaJournal
    [HttpPost]
    public IHttpActionResult InsertarEntradaJournal([FromBody] Transaccion journalEntry) 
    {
        var response = new ApiResponse();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO TA_JOURNAL (Fiidtrans, Fczona, Fdfechaopera, Fiidoperacion, Fcusuarioalta, Fdfechaalta) " +
                    "VALUES (@Fiidtrans, @Fczona, @Fdfechaopera, @Fiidoperacion, @Fcusuarioalta, @Fdfechaalta)", conn);

                cmd.Parameters.AddWithValue("@Fiidtrans", journalEntry.Fiidtrans);
                cmd.Parameters.AddWithValue("@Fczona", journalEntry.Fczona);
                cmd.Parameters.AddWithValue("@Fdfechaopera", journalEntry.Fdfechaopera);
                cmd.Parameters.AddWithValue("@Fiidoperacion", journalEntry.Fiidoperacion); 
                cmd.Parameters.AddWithValue("@Fcusuarioalta", journalEntry.Fcusuarioalta);
                cmd.Parameters.AddWithValue("@Fdfechaalta", DateTime.Now);

                cmd.ExecuteNonQuery();
            }

            response.Codigo = 200;
            response.Mensaje = "Entrada de Journal registrada correctamente";
            response.Data = journalEntry; 

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Codigo = 500;
            response.Mensaje = "Error: " + ex.Message;
            return InternalServerError(ex);
        }
    }
}