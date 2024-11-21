using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TorneoNegocio
    {
        public List<Torneo> ListarTorneos()
        {
            List<Torneo> torneos = new List<Torneo>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                        SELECT T.Id AS TorneoId, T.Nombre AS TorneoNombre, J.id AS JugadorId, J.nombre AS JugadorNombre, J.apellido AS JugadorApellido, J.username AS JugadorUsername, J.email AS JugadorEmail
                        FROM TORNEO T
                        LEFT JOIN TORNEO_JUGADOR TJ ON T.Id = TJ.torneo_id
                        LEFT JOIN JUGADOR J ON TJ.jugador_id = J.id");
                accesoDatos.EjecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    int torneoId = (int)accesoDatos.Lector["TorneoId"];

                    Torneo torneo = torneos.FirstOrDefault(t => t.Id == torneoId);

                    if (torneo == null)
                    {
                        torneo = new Torneo
                        {
                            Id = torneoId,
                            Nombre = (string)accesoDatos.Lector["TorneoNombre"],
                            Jugadores = new List<Jugador>()
                        };
                        torneos.Add(torneo);
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

                        torneo.Jugadores.Add(jugador);
                    }
                }

                return torneos;
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

        public int CrearTorneo(string nombreTorneo, int ligaId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("INSERT INTO TORNEO (nombre, liga_id, fecha_creacion) " +
                    "OUTPUT INSERTED.Id VALUES (@nombre, @liga_id, @fecha_creacion)");
                datos.AgregarParametro("@nombre", nombreTorneo);
                datos.AgregarParametro("@liga_id", ligaId);
                datos.AgregarParametro("@fecha_creacion", DateTime.Now);

                int id = datos.EjecutarEscalar();
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

        public bool UpdateTorneo(int idTorneo, string nuevoNombre)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("UPDATE TORNEO SET nombre = @nombre WHERE id = @id");

                datos.AgregarParametro("@nombre", nuevoNombre);
                datos.AgregarParametro("@id", idTorneo);

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

        public bool DeleteTorneo(int idTorneo)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta("DELETE T FROM TORNEO T WHERE id = @id");
                datos.AgregarParametro("@id", idTorneo);
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

        public Torneo GetTorneoById(int id)
        {
            Torneo torneo = null;
            try
            {
                AccesoDatosDB datos = new AccesoDatosDB();
                datos.SetearConsulta(@"SELECT Id, Nombre FROM TORNEO T WHERE Id = @id");
                datos.AgregarParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    torneo = new Torneo
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = (string)datos.Lector["Nombre"]
                    };
                }
                datos.CerrarConexion();

                if (torneo != null)
                {
                    datos = new AccesoDatosDB();
                    datos.SetearConsulta(@"
                        SELECT J.id, J.nombre, J.apellido, J.username, J.email
                        FROM TORNEO_JUGADOR TJ
                        JOIN JUGADOR J ON TJ.jugador_id = J.id
                        WHERE TJ.torneo_id = @id");
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
                        torneo.Jugadores.Add(jugador);
                    }
                    datos.CerrarConexion();
                }
                return torneo;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        public bool AsociarJugadores(int torneoId, List<int> jugadoresIds)
        {
            try
            {
                foreach (int jugadorId in jugadoresIds)
                {
                    AccesoDatosDB datos = new AccesoDatosDB();
                    datos.SetearConsulta("INSERT INTO TORNEO_JUGADOR (torneo_id, jugador_id) VALUES (@torneo_Id, @jugador_Id)");
                    datos.AgregarParametro("@torneo_Id", torneoId);
                    datos.AgregarParametro("@jugador_Id", jugadorId);
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


        public void GenerarPartidos(int torneoId, List<int> jugadoresIds)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<int> jugadoresIdsMezclados = new List<int>();

            var random = new Random();
            jugadoresIdsMezclados = jugadoresIds.OrderBy(j => random.Next()).ToList();

            int rondaNumero = 1;

            datos.SetearConsulta(
                "INSERT INTO RONDA (torneo_id, numero, nombre) OUTPUT INSERTED.id VALUES (@torneoId, @numero, @nombre)");
            datos.AgregarParametro("@torneoId", torneoId);
            datos.AgregarParametro("@numero", rondaNumero);
            datos.AgregarParametro("@nombre", $"Ronda {rondaNumero}");

            int rondaId = datos.EjecutarEscalar();

            datos.CerrarConexion();

            // generar partidos para la ronda actual
            for (int i = 0; i < jugadoresIdsMezclados.Count; i += 2)
            {
                datos = new AccesoDatosDB();

                datos.SetearConsulta(
                    "INSERT INTO PARTIDO (ronda_id, tipo_partido_id) OUTPUT INSERTED.id VALUES (@rondaId, 2)");
                datos.AgregarParametro("@rondaId", rondaId);

                int partidoId = datos.EjecutarEscalar();

                datos = new AccesoDatosDB();

                // insertar los jugadores en la tabla intermedia 
                datos = new AccesoDatosDB();

                datos.SetearConsulta(
                    "INSERT INTO PARTIDO_JUGADOR (partido_id, jugador_id, puntos) VALUES (@partidoId, @jugador1Id, 0)");
                datos.AgregarParametro("@partidoId", partidoId);
                datos.AgregarParametro("@jugador1Id", jugadoresIdsMezclados[i]);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                datos = new AccesoDatosDB();
                datos.SetearConsulta(
                    "INSERT INTO PARTIDO_JUGADOR (partido_id, jugador_id, puntos) VALUES (@partidoId, @jugador2Id, 0)");
                datos.AgregarParametro("@partidoId", partidoId);
                datos.AgregarParametro("@jugador2Id", jugadoresIdsMezclados[i + 1]);
                datos.EjecutarAccion();
                datos.CerrarConexion();
            }

        }


    }
}
