using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TorneoNegocio
    {
        public List<Torneo> ListarTorneos(int jugadorLogueadoId)
        {
            List<Torneo> torneos = new List<Torneo>();
            AccesoDatosDB accesoDatos = new AccesoDatosDB();

            try
            {
                accesoDatos.SetearConsulta(@"
                        SELECT T.Id AS TorneoId, T.Nombre AS TorneoNombre, J.id AS JugadorId, J.nombre AS JugadorNombre, J.apellido AS JugadorApellido, J.username AS JugadorUsername, J.email AS JugadorEmail
                        FROM TORNEO T
                        JOIN TORNEO_JUGADOR TJ ON T.Id = TJ.torneo_id
                        JOIN JUGADOR J ON TJ.jugador_id = J.id
                        WHERE T.id IN (
                            SELECT DISTINCT T2.id
                            FROM TORNEO T2
                            JOIN TORNEO_JUGADOR TJ2 ON T2.id = TJ2.torneo_id
                            WHERE TJ2.jugador_id = @jugador_id
                        )");
                accesoDatos.AgregarParametro("@jugador_id", jugadorLogueadoId);
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
            System.Diagnostics.Debug.WriteLine($"{idTorneo}");

            try
            {
                // eliminar jugadores de los partidos de las rondas (de la tabla PARTIDO_JUGADOR)
                datos.SetearConsulta(@"
                    DELETE PJ
                    FROM PARTIDO_JUGADOR PJ
                    INNER JOIN PARTIDO P ON PJ.partido_id = P.id
                    INNER JOIN RONDA R ON P.ronda_id = R.id
                    WHERE R.torneo_id = @id");
                datos.AgregarParametro("@id", idTorneo);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // eliminar partidos asociados a las rondas del torneo
                datos = new AccesoDatosDB();
                datos.SetearConsulta(@"
                    DELETE P 
                    FROM PARTIDO P
                    INNER JOIN RONDA R ON P.ronda_id = R.id
                    WHERE R.torneo_id = @id");
                datos.AgregarParametro("@id", idTorneo);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // eliminar rondas asociadas a ese torneo
                datos = new AccesoDatosDB();
                datos.SetearConsulta("DELETE FROM RONDA WHERE torneo_id = @id");
                datos.AgregarParametro("@id", idTorneo);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // eliminar jugadores asociados al torneo (tabla TORNEO_JUGADOR)
                datos = new AccesoDatosDB();
                datos.SetearConsulta("DELETE FROM TORNEO_JUGADOR WHERE torneo_id = @id");
                datos.AgregarParametro("@id", idTorneo);
                datos.EjecutarAccion();
                datos.CerrarConexion();

                // eliminar torneo
                datos = new AccesoDatosDB();
                datos.SetearConsulta("DELETE FROM TORNEO WHERE id = @id");
                datos.AgregarParametro("@id", idTorneo);
                datos.EjecutarAccion();
                datos.CerrarConexion();


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine($"{ex}");
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
                        torneo.Jugadores = new List<Jugador>();
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


        public void GenerarPartidos(int torneoId, List<int> jugadoresIds, int? rondaNumero = null)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<int> jugadoresIdsMezclados = new List<int>();

            var random = new Random();
            jugadoresIdsMezclados = jugadoresIds.OrderBy(j => random.Next()).ToList();

            if(rondaNumero == null)
            {
                rondaNumero = 1;
            }

            datos.SetearConsulta("INSERT INTO RONDA (torneo_id, numero, nombre) OUTPUT INSERTED.id VALUES (@torneoId, @numero, @nombre)");
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

        public List<RondaPartidosDTO> ListarRondasConPartidos(int torneoId)
        {
            List<RondaPartidosDTO> rondas = new List<RondaPartidosDTO>();
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta(@"
                    SELECT id, nombre, numero 
                    FROM RONDA 
                    WHERE torneo_id = @TorneoId 
                    ORDER BY numero");
                datos.AgregarParametro("@TorneoId", torneoId);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Ronda ronda = new Ronda
                    (
                        (int)datos.Lector["id"],
                        (int)datos.Lector["numero"],
                        (string)datos.Lector["nombre"]
                    );
                    List<PartidoDTO> partidosRonda = ListarPartidosPorRonda((int)datos.Lector["id"]);
                    RondaPartidosDTO dto = new RondaPartidosDTO
                    (
                        ronda,
                        partidosRonda
                    );
                    rondas.Add(dto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar rondas: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return rondas;
        }

        public List<PartidoDTO> ListarPartidosPorRonda(int rondaId)
        {
            List<PartidoDTO> partidos = new List<PartidoDTO>();
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                string query = @"
            SELECT P.id AS PartidoId,
                   P.fecha AS Fecha,
                   J1.id AS Jugador1Id,
                   J2.id AS Jugador2Id,
                   P.ganador_id AS GanadorId,
                   J1.username AS NombreJugador1, J1.id as Jugador1Id, PJ1.puntos AS PuntosJugador1,
                   J2.username AS NombreJugador2, J2.id as Jugador2Id, PJ2.puntos AS PuntosJugador2,
                   P.ganador_id AS GanadorId,
                   (SELECT J.username FROM JUGADOR J WHERE J.id = P.ganador_id) AS GanadorNombre,
                   P.tipo_partido_id AS TipoPartidoId 
            FROM PARTIDO P
            JOIN PARTIDO_JUGADOR PJ1 ON P.id = PJ1.partido_id
            JOIN JUGADOR J1 ON PJ1.jugador_id = J1.id
            LEFT JOIN PARTIDO_JUGADOR PJ2 ON P.id = PJ2.partido_id AND PJ2.jugador_id <> PJ1.jugador_id
            LEFT JOIN JUGADOR J2 ON PJ2.jugador_id = J2.id
            JOIN RONDA R ON P.ronda_id = R.id
            WHERE P.ronda_id = @RondaId
            AND (PJ1.jugador_id < PJ2.jugador_id OR PJ2.jugador_id IS NULL)";

                datos.SetearConsulta(query);
                datos.AgregarParametro("@RondaId", rondaId);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    var partido = new PartidoDTO
                    {
                        PartidoId = (int)datos.Lector["PartidoId"],
                        //Fecha = (DateTime)datos.Lector["Fecha"],
                        Jugador1Id = (int)datos.Lector["Jugador1Id"],
                        NombreJugador1 = (string)datos.Lector["NombreJugador1"],
                        PuntosJugador1 = datos.Lector["PuntosJugador1"] != DBNull.Value ? (int)datos.Lector["PuntosJugador1"] : 0,
                        Jugador2Id = (int)datos.Lector["Jugador2Id"],
                        NombreJugador2 = datos.Lector["NombreJugador2"] != DBNull.Value ? (string)datos.Lector["NombreJugador2"] : "",
                        PuntosJugador2 = datos.Lector["PuntosJugador2"] != DBNull.Value ? (int)datos.Lector["PuntosJugador2"] : 0,
                        GanadorId = datos.Lector["GanadorId"] != DBNull.Value ? (int)datos.Lector["GanadorId"] : 0,
                        TipoPartidoId = (int)datos.Lector["TipoPartidoId"],
                    };
                    partidos.Add(partido);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar partidos: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return partidos;
        }

        public int RondaActualId(int torneoId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta(@"SELECT TOP 1 id FROM RONDA WHERE torneo_id = @TorneoId ORDER BY numero DESC");
                datos.AgregarParametro("@TorneoId", torneoId);

                return datos.EjecutarEscalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar partidos: " + ex.Message);
                return -1;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool RondaActualCompletada(int roundId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();

            try
            {
                datos.SetearConsulta(@"SELECT COUNT (*) FROM PARTIDO WHERE ronda_id = @RondaId AND ganador_id = 0");
                datos.AgregarParametro("RondaId", roundId);

                int partidosSinGanador = (int)datos.EjecutarEscalar();

                return partidosSinGanador == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar partidos: " + ex.Message);
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool AvanzarRonda(int torneoId, int previousRoundId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<int> ganadoresIds = new List<int>();
            int numeroRondaPrevia = -1;

            try
            {
                // encuentro el numero de la ronda previa
                datos.SetearConsulta("SELECT numero FROM RONDA WHERE id = @PreviousRoundId");
                datos.AgregarParametro("@PreviousRoundId", previousRoundId);
                numeroRondaPrevia = datos.EjecutarEscalar();
                datos.CerrarConexion();

                if (numeroRondaPrevia == -1)
                {
                    throw new Exception("no se encontro la ronda previa.");
                }

                // encuentro los ids de los jugadores q ganaron en la ronda previa
                datos = new AccesoDatosDB();
                datos.SetearConsulta(@"SELECT P.ganador_id FROM PARTIDO P WHERE P.ronda_id = @PreviousRoundId AND P.ganador_id IS NOT NULL");
                datos.AgregarParametro("@PreviousRoundId", previousRoundId);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    int ganadorId = datos.Lector.GetInt32(0);
                    ganadoresIds.Add(ganadorId);
                }

                datos.CerrarConexion();

                GenerarPartidos(torneoId, ganadoresIds, numeroRondaPrevia + 1);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar partidos: " + ex.Message);
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

    }
}
