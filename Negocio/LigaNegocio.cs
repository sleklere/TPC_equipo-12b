using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Negocio
{
    public class LigaNegocio
    {
        public List<Liga> listarLigas(int jugadorId)
        {
            List<Liga> ligas = new List<Liga>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                        SELECT L.Id AS LigaId, L.Nombre AS LigaNombre, J.id AS JugadorId, J.nombre AS JugadorNombre, J.apellido AS JugadorApellido, J.username AS JugadorUsername, J.email AS JugadorEmail
                        FROM LIGA L
                        LEFT JOIN LIGA_JUGADOR LJ ON L.Id = LJ.liga_id
                        LEFT JOIN JUGADOR J ON LJ.jugador_id = J.id
                          WHERE LJ.liga_id IN (
                    SELECT liga_id FROM LIGA_JUGADOR WHERE jugador_id = @JugadorId
                )
                ");
                accesoDatos.AgregarParametro("@JugadorId", jugadorId);
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    int ligaId = (int)accesoDatos.Lector["LigaId"];

                    Liga liga = ligas.FirstOrDefault(l => l.Id == ligaId);

                    if (liga == null)
                    {
                        liga = new Liga
                        {
                            Id = ligaId,
                            Nombre = (string)accesoDatos.Lector["LigaNombre"],
                            Jugadores = new List<Jugador>()
                        };
                        ligas.Add(liga);
                    }

                    if (accesoDatos.Lector["JugadorId"] != DBNull.Value)
                    {
                            Jugador jugador = new Jugador
                            {
                            Id = (int)accesoDatos.Lector["JugadorId"],
                                Nombre = (string)accesoDatos.Lector["JugadorNombre"],
                                Apellido = (string)accesoDatos.Lector["JugadorApellido"],
                                Username = (string)accesoDatos.Lector["JugadorUsername"],
                                Email = (string)accesoDatos.Lector["JugadorEmail"]
                            };


                        liga.Jugadores.Add(jugador);
                    }
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

        public List<Liga> ListarLigasJugador(int jugadorId)
        {
            List<Liga> ligas = new List<Liga>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                        SELECT L.Id AS LigaId, L.Nombre AS LigaNombre, J.id AS JugadorId, J.nombre AS JugadorNombre, J.apellido AS JugadorApellido, J.username AS JugadorUsername, J.email AS JugadorEmail
                        FROM LIGA L
                        LEFT JOIN LIGA_JUGADOR LJ ON L.Id = LJ.liga_id
                        LEFT JOIN JUGADOR J ON LJ.jugador_id = J.id 
                        WHERE LJ.jugador_id = @JugadorId");
                accesoDatos.AgregarParametro("@JugadorId", jugadorId);
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    int ligaId = (int)accesoDatos.Lector["LigaId"];

                    Liga liga = ligas.FirstOrDefault(l => l.Id == ligaId);

                    if (liga == null)
                    {
                        liga = new Liga
                        {
                            Id = ligaId,
                            Nombre = (string)accesoDatos.Lector["LigaNombre"],
                            //Jugadores = new List<Jugador>()
                        };
                        ligas.Add(liga);
                    }
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

        public List<Liga> listarLigasSinParticipacion(int jugadorId)
        {
            List<Liga> ligas = new List<Liga>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                        SELECT L.Id AS LigaId, L.Nombre AS LigaNombre, J.id AS JugadorId, J.nombre AS JugadorNombre, J.apellido AS JugadorApellido, J.username AS JugadorUsername, J.email AS JugadorEmail
                        FROM LIGA L
                        LEFT JOIN LIGA_JUGADOR LJ ON L.Id = LJ.liga_id
                        LEFT JOIN JUGADOR J ON LJ.jugador_id = J.id 
                        WHERE LJ.jugador_id <> @JugadorId");
                accesoDatos.AgregarParametro("@JugadorId", jugadorId);
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    int ligaId = (int)accesoDatos.Lector["LigaId"];

                    Liga liga = ligas.FirstOrDefault(l => l.Id == ligaId);

                    if (liga == null)
                    {
                        liga = new Liga
                        {
                            Id = ligaId,
                            Nombre = (string)accesoDatos.Lector["LigaNombre"],
                            Jugadores = new List<Jugador>()
                        };
                        ligas.Add(liga);
                    }

                    if (accesoDatos.Lector["JugadorId"] != DBNull.Value)
                    {
                        Jugador jugador = new Jugador
                        {
                            Id = (int)accesoDatos.Lector["JugadorId"],
                            Nombre = (string)accesoDatos.Lector["JugadorNombre"],
                            Apellido = (string)accesoDatos.Lector["JugadorApellido"],
                            Username = (string)accesoDatos.Lector["JugadorUsername"],
                            Email = (string)accesoDatos.Lector["JugadorEmail"]
                        };

                        liga.Jugadores.Add(jugador);
                    }
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

        public int CrearLiga(string nombreLiga)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                string codigoGenerado = GenerarCodigoLiga();

                datos.SetearConsulta("INSERT INTO LIGA (nombre, codigo, fecha_creacion) " +
                    "OUTPUT INSERTED.Id VALUES (@nombre, @codigo, @fecha_creacion)");
                datos.AgregarParametro("@nombre", nombreLiga);
                datos.AgregarParametro("@codigo", codigoGenerado);
                datos.AgregarParametro("@fecha_creacion", DateTime.Now);

                int id = (int)datos.EjecutarEscalar();
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public string GenerarCodigoLiga()
        {
            // caracteres para usar
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new StringBuilder(10);

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[1];

                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(buffer);
                    int index = buffer[0] % chars.Length;
                    result.Append(chars[index]);
                }
            }

            return result.ToString();
        }


        public bool AsociarJugadoresALiga(int ligaId, List<Jugador> jugadores)
        {
            try
            {
                foreach (var jugador in jugadores)
                {
                    AccesoDatosDB datos = new AccesoDatosDB();
                    datos.SetearConsulta("INSERT INTO LIGA_JUGADOR (liga_id, jugador_id) VALUES (@liga_id, @jugador_id)");
                    datos.AgregarParametro("@liga_id", ligaId);
                    datos.AgregarParametro("@jugador_id", jugador.Id);
                    datos.EjecutarAccion();
                    datos.CerrarConexion();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool AsociarJugadorALiga(int ligaId, int jugadorId)
        {
            try
            {
                AccesoDatosDB datos = new AccesoDatosDB();
                datos.SetearConsulta("INSERT INTO LIGA_JUGADOR (liga_id, jugador_id) VALUES (@liga_id, @jugador_id)");
                datos.AgregarParametro("@liga_id", ligaId);
                datos.AgregarParametro("@jugador_id", jugadorId);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
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

        public int DeleteLiga(int idLiga)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            Console.WriteLine("Intentando eliminar liga con ID: " + idLiga);
            try
            {
                // veo si hay tornes asociados a esa liga
                datos.SetearConsulta(@"SELECT COUNT(*) FROM TORNEO WHERE liga_id = @LigaId");
                datos.AgregarParametro("@LigaId", idLiga);

                int cantidadLigasAsociadas = datos.EjecutarEscalar();
                datos.CerrarConexion();

                if(cantidadLigasAsociadas > 0)
                {
                    return 0;
                }

                // elimino a los jugadores de los partidos de esa liga
                datos = new AccesoDatosDB();
                datos.SetearConsulta(@"DELETE PJ FROM PARTIDO_JUGADOR PJ
                                       INNER JOIN PARTIDO P ON PJ.partido_id = P.id
                                       WHERE P.liga_id = @LigaId");
                datos.AgregarParametro("@LigaId", idLiga);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // elimino a los partidos
                datos = new AccesoDatosDB();
                datos.SetearConsulta(@"DELETE FROM PARTIDO WHERE liga_id = @LigaId");
                datos.AgregarParametro("@LigaId", idLiga);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // elimino a los jugadores de esa liga
                datos = new AccesoDatosDB();
                datos.SetearConsulta(@"DELETE FROM LIGA_JUGADOR WHERE liga_id = @LigaId");
                datos.AgregarParametro("@LigaId", idLiga);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // elimino la liga
                datos = new AccesoDatosDB();
                datos.SetearConsulta("DELETE FROM LIGA WHERE id = @id");
                datos.AgregarParametro("@id", idLiga);
                datos.EjecutarAccion();

                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine($"{ex}");
                return -1;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Liga getLigaById(int id)
        {
            Liga liga = null;
            try
            {
                AccesoDatosDB datos = new AccesoDatosDB();
                datos.SetearConsulta(@"SELECT Id, Nombre, Codigo FROM LIGA L WHERE Id = @id");
                datos.AgregarParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    liga = new Liga
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Codigo = (string)datos.Lector["Codigo"]
                    };
                }
                datos.CerrarConexion();

                if (liga != null)
                {
                    datos = new AccesoDatosDB();
                    datos.SetearConsulta(@"
                        SELECT J.id, J.nombre, J.apellido, J.username, J.email
                        FROM LIGA_JUGADOR LJ
                        JOIN JUGADOR J ON LJ.jugador_id = J.id
                        WHERE LJ.liga_id = @id");
                    datos.AgregarParametro("@id", id);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Jugador jugador = new Jugador
                        {
                            Id = (int)datos.Lector["id"],
                            Nombre = (string)datos.Lector["nombre"],
                            Apellido = (string)datos.Lector["apellido"],
                            Username = (string)datos.Lector["username"],
                            Email = (string)datos.Lector["email"]
                        };
                        liga.Jugadores.Add(jugador);
                    }
                    datos.CerrarConexion();
                }
                return liga;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        public Liga getLigaByCodigo(string codigo)
        {
            Liga liga = null;
            try
            {
                AccesoDatosDB datos = new AccesoDatosDB();
                datos.SetearConsulta(@"SELECT Id, Nombre FROM LIGA WHERE codigo = @codigo");
                datos.AgregarParametro("@codigo", codigo);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    liga = new Liga
                    {
                        Id = (int)datos.Lector["id"],
                        Nombre = (string)datos.Lector["nombre"]
                    };
                }
                datos.CerrarConexion();

                if (liga != null)
                {
                    datos = new AccesoDatosDB();
                    datos.SetearConsulta(@"
                        SELECT J.id, J.nombre, J.apellido, J.username, J.email
                        FROM LIGA_JUGADOR LJ
                        JOIN JUGADOR J ON LJ.jugador_id = J.id
                        WHERE LJ.liga_id = @id");
                    datos.AgregarParametro("@codigo", codigo);
                    datos.AgregarParametro("@id", liga.Id);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Jugador jugador = new Jugador
                        {
                            Id = (int)datos.Lector["id"],
                            Nombre = (string)datos.Lector["nombre"],
                            Apellido = (string)datos.Lector["apellido"],
                            Username = (string)datos.Lector["username"],
                            Email = (string)datos.Lector["email"]
                        };
                        liga.Jugadores.Add(jugador);
                    }
                    datos.CerrarConexion();
                }
                return liga;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        public List<Jugador> getJugadoresByLigaId(string ligaId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<Jugador> jugadores = new List<Jugador>();

            try
            {
                datos.SetearConsulta(@"SELECT J.id, J.nombre, J.apellido, J.username, J.email,
                                        SUM(CASE WHEN P.ganador_id = J.id THEN 1 ELSE 0 END) AS PartidosGanados,
                                        SUM(CASE WHEN P.ganador_id != J.id THEN 1 ELSE 0 END) AS PartidosPerdidos
                                        FROM JUGADOR J
                                        INNER JOIN LIGA_JUGADOR LJ ON J.id = LJ.jugador_id
                                        LEFT JOIN PARTIDO P ON P.liga_id = LJ.liga_id
                                        WHERE LJ.liga_id = @LigaId
                                        GROUP BY 
                                            J.id, J.nombre, J.apellido, J.username, J.email
                                        ORDER BY 
                                            SUM(CASE WHEN P.ganador_id = J.id THEN 1 ELSE 0 END) DESC");
                datos.AgregarParametro("@LigaId", ligaId);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Jugador aux = new Jugador
                    {
                        Id = (int)datos.Lector["id"],
                        Nombre = (string)datos.Lector["nombre"],
                        Apellido = (string)datos.Lector["apellido"],
                        Username = (string)datos.Lector["username"],
                        Email = (string)datos.Lector["email"],
                        PartidosGanados = (int)datos.Lector["PartidosGanados"],
                        PartidosPerdidos = (int)datos.Lector["PartidosPerdidos"],
                        PartidosJugados = (int)datos.Lector["PartidosGanados"] + (int)datos.Lector["PartidosPerdidos"],
                    };

                    jugadores.Add(aux);
                }

                return jugadores;
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

    }
}
