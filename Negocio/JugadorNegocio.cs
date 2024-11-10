using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class JugadorNegocio
    {

        public Jugador findJugadorByCodigo(string codigo)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta("SELECT id, nombre, apellido, username, codigo FROM JUGADOR WHERE codigo = @codigo");
                datos.AgregarParametro("@codigo", codigo);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Jugador
                    {
                        Id = datos.Lector.GetInt32(0),
                        Nombre = datos.Lector.GetString(1),
                        Apellido = datos.Lector.GetString(2),
                        Username = datos.Lector.GetString(3),
                        Codigo = datos.Lector.GetString(4)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Jugador> listarJugadores()
        {
            List<Jugador> jugadores = new List<Jugador>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"SELECT id, nombre, apellido, username FROM JUGADOR");
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    int jugadorId = (int)accesoDatos.Lector["id"];

                    Jugador jugador = jugadores.FirstOrDefault(j => j.Id == jugadorId);

                    if (jugador == null)
                    {
                        jugador = new Jugador
                        {
                            Id = jugadorId,
                            Nombre = (string)accesoDatos.Lector["nombre"],
                            Apellido = (string)accesoDatos.Lector["apellido"],
                            Username = (string)accesoDatos.Lector["username"],
                        };
                        jugadores.Add(jugador);
                    }
                }

                return jugadores;
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
    }
}
