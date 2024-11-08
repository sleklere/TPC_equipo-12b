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
                accesoDatos.SetearConsulta(@"
                        SELECT L.Id AS LigaId, L.Nombre AS LigaNombre, J.id AS JugadorId, J.nombre AS JugadorNombre, J.apellido AS JugadorApellido, J.username AS JugadorUsername, J.email AS JugadorEmail
                        FROM LIGA L
                        LEFT JOIN LIGA_JUGADOR LJ ON L.Id = LJ.liga_id
                        LEFT JOIN JUGADOR J ON LJ.jugador_id = J.id");
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
                datos.SetearConsulta("INSERT INTO LIGA (nombre, fecha_creacion) " +
                    "OUTPUT INSERTED.Id VALUES (@nombre, @fecha_creacion)");
                datos.AgregarParametro("@nombre", nombreLiga);
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

        public bool DeleteLiga(int idLiga)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            Console.WriteLine("Intentando eliminar liga con ID: " + idLiga);
            try
            {
                datos.SetearConsulta("DELETE FROM LIGA WHERE id = @id");
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

        public Liga getLigaById(int id)
        {
            Liga liga = null;
            try
            {
                AccesoDatosDB datos = new AccesoDatosDB();
                datos.SetearConsulta(@"SELECT Id, Nombre FROM LIGA WHERE Id = @id");
                datos.AgregarParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    liga = new Liga
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = (string)datos.Lector["Nombre"]
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

        public List<Jugador> getJugadoresByLigaId(string ligaId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<Jugador> jugadores = new List<Jugador>();

            try
            {
                datos.SetearConsulta(@"SELECT J.id, J.nombre, J.apellido, J.username, J.email
                                        FROM JUGADOR J
                                        INNER JOIN LIGA_JUGADOR LJ ON J.id = LJ.jugador_id
                                        WHERE LJ.liga_id = @LigaId");
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
                        Email = (string)datos.Lector["email"]
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
