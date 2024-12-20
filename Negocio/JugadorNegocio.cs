﻿using AccesoDatos;
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
                datos.SetearConsulta("SELECT * FROM JUGADOR WHERE email = @email");
                datos.AgregarParametro("@email", email);
                datos.EjecutarLectura();

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

        public List<Jugador> listarJugadoresVersus(int jugadorId)
        {
            List<Jugador> jugadores = new List<Jugador>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@" SELECT DISTINCT J.id, J.nombre, J.apellido, J.username
                                            FROM JUGADOR J
                                            INNER JOIN LIGA_JUGADOR LJ ON J.id = LJ.jugador_id
                                            WHERE LJ.liga_id IN (
                                                SELECT liga_id
                                                FROM LIGA_JUGADOR
                                                WHERE jugador_id = @JugadorId)");
                accesoDatos.AgregarParametro("@JugadorId", jugadorId);
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    int jugId = (int)accesoDatos.Lector["id"];

                    Jugador jugador = jugadores.FirstOrDefault(j => j.Id == jugId);

                    if (jugador == null)
                    {
                        jugador = new Jugador
                        {
                            Id = jugId,
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
        public int RachaVictorias(int jugadorId)
        {
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                    SELECT COUNT(*)
                    FROM PARTIDO
                    WHERE ganador_id = @JugadorId
                      AND fecha > (
                          SELECT COALESCE(MAX(fecha), '1900-01-01')
                          FROM PARTIDO P
                          JOIN PARTIDO_JUGADOR PJ ON P.id = PJ.partido_id
                          WHERE PJ.jugador_id = @JugadorId
                          AND P.ganador_id != @JugadorId
                    )
                ;");
                accesoDatos.AgregarParametro("@JugadorId", jugadorId);
                accesoDatos.EjecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    return (int)accesoDatos.Lector[0];
                }

                return 0;
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

        public List<Jugador> ListarJugadoresByLiga(int ligaId)
        {
            List<Jugador> jugadores = new List<Jugador>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                SELECT j.id, j.nombre, j.apellido, j.username 
                FROM JUGADOR j
                INNER JOIN LIGA_JUGADOR lj ON j.id = lj.jugador_id
                WHERE lj.liga_id = @ligaId"
                );
                accesoDatos.AgregarParametro("@ligaId", ligaId);
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    Jugador jugador = new Jugador
                    {
                        Id = (int)accesoDatos.Lector["id"],
                        Nombre = (string)accesoDatos.Lector["nombre"],
                        Apellido = (string)accesoDatos.Lector["apellido"],
                        Username = (string)accesoDatos.Lector["username"],
                    };

                    jugadores.Add(jugador);
                }

                return jugadores;
            }
            catch (Exception ex)
            {
                throw ex; // Puedes personalizar la excepción según tus necesidades
            }
            finally
            {
                accesoDatos.CerrarConexion();
            }
        }
    }
}
