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
    public class PartidoNegocio
    {
        public bool CrearPartido(int ligaId, List<(int jugadorId, int puntos)> jugadoresConPuntos)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO PARTIDO (liga_id, tipo_partido_id, ganador_id) OUTPUT INSERTED.Id VALUES (@LigaId, 2, @GanadorId)");
                    datos.AgregarParametro("@LigaId", ligaId);
                    int maxPuntos = 0;
                    int ganadorId = jugadoresConPuntos.OrderByDescending(j => j.puntos).First().jugadorId;
                    datos.AgregarParametro("@GanadorId", ganadorId);
                    int partidoId = datos.EjecutarEscalar();
                    datos.CerrarConexion();

                    foreach (var jugador in jugadoresConPuntos)
                    {
                        datos = new AccesoDatosDB();
                        datos.SetearConsulta("INSERT INTO PARTIDO_JUGADOR (partido_id, jugador_id, puntos) VALUES (@PartidoId, @JugadorId, @Puntos)");
                        datos.AgregarParametro("@PartidoId", partidoId);
                        datos.AgregarParametro("@JugadorId", jugador.jugadorId);
                        datos.AgregarParametro("@Puntos", jugador.puntos);
                        datos.EjecutarAccion();
                        datos.CerrarConexion();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear el partido: " + ex.Message);
                    return false;
                }
            }
        }

        public List<Partido> listarPartidos()
        {
            List<Partido> partidos = new List<Partido>();
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT P.id,
                       J1.username AS Jugador1Nombre, PJ1.puntos AS PuntosJugador1,
                       J2.username AS Jugador2Nombre, PJ2.puntos AS PuntosJugador2,
                       P.ganador_id AS GanadorId,
                       (SELECT J.username FROM JUGADOR J WHERE J.id = P.ganador_id) AS GanadorNombre
                FROM PARTIDO P
                JOIN PARTIDO_JUGADOR PJ1 ON P.id = PJ1.partido_id
                JOIN JUGADOR J1 ON PJ1.jugador_id = J1.id
                LEFT JOIN PARTIDO_JUGADOR PJ2 ON P.id = PJ2.partido_id AND PJ2.jugador_id <> PJ1.jugador_id
                LEFT JOIN JUGADOR J2 ON PJ2.jugador_id = J2.id
                WHERE PJ1.jugador_id < PJ2.jugador_id OR PJ2.jugador_id IS NULL
            ");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Partido partido = new Partido
                        {
                            Id = (int)datos.Lector["Id"],
                            Jugador1Nombre = (string)datos.Lector["Jugador1Nombre"],
                            PuntosJugador1 = (int)datos.Lector["PuntosJugador1"],
                            Jugador2Nombre = (string)datos.Lector["Jugador2Nombre"],
                            PuntosJugador2 = (int)datos.Lector["PuntosJugador2"],
                            GanadorId = (int)datos.Lector["GanadorId"],
                            GanadorNombre = (string)datos.Lector["GanadorNombre"]
                        };
                        partidos.Add(partido);
                    }

                    return partidos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al listar los partidos: " + ex.Message);
                    return null;
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }
        }
        public List<Partido> listarPartidosEntreJugadores(int jugador1Id, int jugador2Id)
        {
            List<Partido> partidos = new List<Partido>();
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT P.id,
                       J1.username AS Jugador1Nombre, PJ1.puntos AS PuntosJugador1,
                       J2.username AS Jugador2Nombre, PJ2.puntos AS PuntosJugador2,
                       P.ganador_id AS GanadorId,
                       (SELECT J.username FROM JUGADOR J WHERE J.id = P.ganador_id) AS GanadorNombre
                FROM PARTIDO P
                JOIN PARTIDO_JUGADOR PJ1 ON P.id = PJ1.partido_id
                JOIN JUGADOR J1 ON PJ1.jugador_id = J1.id
                LEFT JOIN PARTIDO_JUGADOR PJ2 ON P.id = PJ2.partido_id AND PJ2.jugador_id <> PJ1.jugador_id
                LEFT JOIN JUGADOR J2 ON PJ2.jugador_id = J2.id
                WHERE PJ1.jugador_id = @Jugador1Id AND PJ2.jugador_id = @Jugador2Id
            ");
                    datos.AgregarParametro("@Jugador1Id", jugador1Id);
                    datos.AgregarParametro("@Jugador2Id", jugador2Id);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Partido partido = new Partido
                        {
                            Id = (int)datos.Lector["Id"],
                            Jugador1Nombre = (string)datos.Lector["Jugador1Nombre"],
                            PuntosJugador1 = (int)datos.Lector["PuntosJugador1"],
                            Jugador2Nombre = (string)datos.Lector["Jugador2Nombre"],
                            PuntosJugador2 = (int)datos.Lector["PuntosJugador2"],
                            GanadorId = (int)datos.Lector["GanadorId"],
                            GanadorNombre = (string)datos.Lector["GanadorNombre"]
                        };
                        partidos.Add(partido);
                    }

                    return partidos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al listar los partidos: " + ex.Message);
                    return null;
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }
        }

        public void ActualizarPartido(int partidoId, int ligaId, int jugador1Id, int jugador2Id, string resultado)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("UPDATE PARTIDO SET LigaId = @LigaId, Resultado = @Resultado WHERE Id = @Id");
                datos.AgregarParametro("@Id", partidoId);
                datos.AgregarParametro("@LigaId", ligaId);
                datos.AgregarParametro("@Resultado", resultado);
                datos.EjecutarAccion();

                datos.SetearConsulta("DELETE FROM PARTIDO_JUGADOR WHERE PartidoId = @PartidoId");
                datos.AgregarParametro("@PartidoId", partidoId);
                datos.EjecutarAccion();

                datos.SetearConsulta("INSERT INTO PARTIDO_JUGADOR (PartidoId, JugadorId) VALUES (@PartidoId, @JugadorId)");
                datos.AgregarParametro("@PartidoId", partidoId);

                datos.AgregarParametro("@JugadorId", jugador1Id);
                datos.EjecutarAccion();

                datos.AgregarParametro("@JugadorId", jugador2Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el partido: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }

        }
        public void EliminarPartido(int partidoId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("DELETE FROM PARTIDO_JUGADOR WHERE PartidoId = @PartidoId");
                datos.AgregarParametro("@PartidoId", partidoId);
                datos.EjecutarAccion();

                datos.SetearConsulta("DELETE FROM PARTIDO WHERE Id = @Id");
                datos.AgregarParametro("@Id", partidoId);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el partido: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public VersusDTO ObtenerVersus(int jugador1Id, int jugador2Id)
        {
            VersusDTO versus = null;
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT 
                            J1.id AS Jugador1Id, 
                            J1.username AS Jugador1Nombre, 
                            SUM(CASE WHEN PJ1.puntos > PJ2.puntos THEN 1 ELSE 0 END) AS Jugador1Victorias,
                            SUM(CASE WHEN PJ1.puntos < PJ2.puntos THEN 1 ELSE 0 END) AS Jugador1Derrotas,
                            
                            J2.id AS Jugador2Id, 
                            J2.username AS Jugador2Nombre, 
                            SUM(CASE WHEN PJ2.puntos > PJ1.puntos THEN 1 ELSE 0 END) AS Jugador2Victorias,
                            SUM(CASE WHEN PJ2.puntos < PJ1.puntos THEN 1 ELSE 0 END) AS Jugador2Derrotas,
                            
                            (SELECT COUNT(*) 
                             FROM PARTIDO_JUGADOR PJ 
                             WHERE PJ.jugador_id = J1.id) AS Jugador1TotalPartidos,
                            (SELECT COUNT(*) 
                             FROM PARTIDO P 
                             WHERE P.ganador_id = J1.id) AS Jugador1TotalVictorias,

                            (SELECT COUNT(*) 
                             FROM PARTIDO_JUGADOR PJ 
                             WHERE PJ.jugador_id = J2.id) AS Jugador2TotalPartidos,                        
                            (SELECT COUNT(*) 
                             FROM PARTIDO P 
                             WHERE P.ganador_id = J2.id) AS Jugador2TotalVictorias
                        FROM PARTIDO P
                        JOIN PARTIDO_JUGADOR PJ1 ON P.id = PJ1.partido_id AND PJ1.jugador_id = @Jugador1Id
                        JOIN PARTIDO_JUGADOR PJ2 ON P.id = PJ2.partido_id AND PJ2.jugador_id = @Jugador2Id
                        JOIN JUGADOR J1 ON PJ1.jugador_id = J1.id
                        JOIN JUGADOR J2 ON PJ2.jugador_id = J2.id
                        WHERE (J1.id = @Jugador1Id AND J2.id = @Jugador2Id) 
                           OR (J1.id = @Jugador2Id AND J2.id = @Jugador1Id)
                        GROUP BY J1.id, J1.username, J2.id, J2.username;
            ");

                    datos.AgregarParametro("@Jugador1Id", jugador1Id);
                    datos.AgregarParametro("@Jugador2Id", jugador2Id);
                    datos.EjecutarLectura();

                    if (datos.Lector.Read())
                    {
                        versus = new VersusDTO
                        {
                            Jugador1Id = (int)datos.Lector["Jugador1Id"],
                            Jugador1Nombre = (string)datos.Lector["Jugador1Nombre"],
                            Jugador1Victorias = (int)datos.Lector["Jugador1Victorias"],
                            Jugador1Derrotas = (int)datos.Lector["Jugador1Derrotas"],
                            Jugador1TotalPartidos = (int)datos.Lector["Jugador1TotalPartidos"],
                            Jugador1TotalVictorias = (int)datos.Lector["Jugador1TotalVictorias"],

                            Jugador2Id = (int)datos.Lector["Jugador2Id"],
                            Jugador2Nombre = (string)datos.Lector["Jugador2Nombre"],
                            Jugador2Victorias = (int)datos.Lector["Jugador2Victorias"],
                            Jugador2Derrotas = (int)datos.Lector["Jugador2Derrotas"],
                            Jugador2TotalPartidos = (int)datos.Lector["Jugador2TotalPartidos"],
                            Jugador2TotalVictorias = (int)datos.Lector["Jugador2TotalVictorias"],
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener datos de head-to-head: " + ex.Message);
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }
            return versus;
        }


    }
}
