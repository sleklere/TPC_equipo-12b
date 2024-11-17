using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class JugadorNegocio
    {

        public bool CrearJugador(Jugador jugador)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta("INSERT INTO Jugador (nombre, apellido, username, password, email, fecha_creacion) VALUES (@nombre, @apellido, @username, @password, @email, @fecha_creacion)");

                datos.AgregarParametro("@nombre", jugador.Nombre);
                datos.AgregarParametro("@apellido", jugador.Apellido);
                datos.AgregarParametro("@username", jugador.Username);
                datos.AgregarParametro("@password", jugador.Password);
                datos.AgregarParametro("@email", jugador.Email);
                datos.AgregarParametro("@fecha_creacion", DateTime.Now);

                datos.EjecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear el jugador: " + ex.Message);
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool emailExistente(string email)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                // Configurar la consulta para verificar si el email ya existe
                datos.SetearConsulta("SELECT * FROM JUGADOR WHERE email = @email");
                datos.AgregarParametro("@email", email);

                // Ejecutar la lectura en lugar de una acción de escritura
                datos.EjecutarLectura();

                // Verificar si se ha encontrado algún resultado
                if (datos.Lector != null && datos.Lector.Read())
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar el email: " + ex.Message);
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Jugador Login(string email, string password)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta("SELECT * FROM Jugador WHERE email = @email AND password = @password");
                datos.AgregarParametro("@email", email);
                datos.AgregarParametro("@password", password);

                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    Jugador jugador = new Jugador
                    {
                        Id = (int)datos.Lector["id"],
                        Nombre = datos.Lector["nombre"].ToString(),
                        Apellido = datos.Lector["apellido"].ToString(),
                        Username = datos.Lector["username"].ToString(),
                        Password = datos.Lector["password"].ToString(),
                        Email = datos.Lector["email"].ToString(),
                    };
                    return jugador;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar sesion: " + ex.Message);
                return null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

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

        public Jugador UpdateJugador(int id, string nombre, string apellido, string username, string email)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta("UPDATE Jugador SET nombre = @nombre, apellido = @apellido, username = @username, email = @email WHERE id = @id");
                datos.AgregarParametro("@nombre", nombre);
                datos.AgregarParametro("@apellido", apellido);
                datos.AgregarParametro("@username", username);
                datos.AgregarParametro("@email", email);
                datos.AgregarParametro("@id", id);

                datos.EjecutarLectura();

                datos.CerrarConexion();

                datos = new AccesoDatosDB();

                datos.SetearConsulta("SELECT id, nombre, apellido, username, password, email FROM Jugador WHERE id = @id");
                datos.AgregarParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Jugador
                    {
                        Id = (int)datos.Lector["id"],
                        Nombre = datos.Lector["nombre"].ToString(),
                        Apellido = datos.Lector["apellido"].ToString(),
                        Username = datos.Lector["username"].ToString(),
                        Password = datos.Lector["password"].ToString(),
                        Email = datos.Lector["email"].ToString(),
                    };
                }

                datos.CerrarConexion();

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar sesion: " + ex.Message);
                System.Diagnostics.Debug.WriteLine($"{ex}");
                return null;
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
