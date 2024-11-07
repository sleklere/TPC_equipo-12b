using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Negocio
{
    public class LigaNegocio
    {
        public List<Liga> listarLigas()
        {
            List<Liga> ligas = new List<Liga>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta("SELECT L.Id, L.Nombre FROM LIGA L");
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    Liga aux = new Liga();
                    aux.Id = (int)accesoDatos.Lector["Id"];
                    aux.Nombre = (string)accesoDatos.Lector["Nombre"];

                    ligas.Add(aux);

                }

                return ligas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.CerrarConexion();
            }
        }

        public bool CrearLiga(string nombreLiga)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("INSERT INTO LIGA (nombre, fecha_creacion) VALUES (@nombre, @fecha_creacion)");

                datos.AgregarParametro("@nombre", nombreLiga);
                datos.AgregarParametro("@fecha_creacion", DateTime.Now); 

                datos.EjecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool UpdateLiga(int idLiga, string nuevoNombre)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("UPDATE LIGA SET nombre = @nombre WHERE id = @id");

                datos.AgregarParametro("@nombre", nuevoNombre);
                datos.AgregarParametro("@id", idLiga);

                datos.EjecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Liga getLigaById(string id)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            Liga liga = new Liga();

            try
            {
                datos.SetearConsulta("SELECT * FROM LIGA WHERE Id = @Id");
                datos.AgregarParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    liga.Id = (int)datos.Lector["Id"];
                    liga.Nombre = (string)datos.Lector["Nombre"];

                    return liga;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }

        public List<Jugador> getJugadoresByLigaId(string id)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<Jugador> jugadores = new List<Jugador>();

            try
            {
                datos.SetearConsulta("SELECT * FROM JUGADOR WHERE liga_id = @Id");
                datos.AgregarParametro("@Id", id);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Jugador aux = new Jugador();
                    aux.Id = (int)datos.Lector["id"];
                    aux.Nombre = (string)datos.Lector["nombre"];
                    aux.Apellido = (string)datos.Lector["apellido"];
                    aux.Username = (string)datos.Lector["username"];
                    aux.Email = (string)datos.Lector["email"];
                    aux.LigaId = (int)datos.Lector["liga_id"];

                    jugadores.Add(aux);
                }

                return jugadores;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }

    }
}
