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
        public bool CrearPartido(int ligaId, List<(int jugadorId, int puntos)> jugadoresConPuntos, int tipoPartido)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO PARTIDO (liga_id, tipo_partido_id, ganador_id) OUTPUT INSERTED.Id VALUES (@LigaId, @TipoPartido, @GanadorId)");
                    datos.AgregarParametro("@LigaId", ligaId);
                    datos.AgregarParametro("@TipoPartido", tipoPartido);
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
                finally
                {
                    datos.CerrarConexion();
                }
            }
        }

        public List<ListarPartidosDTO> listarPartidos(int? ligaId = null)
        {
            List<ListarPartidosDTO> partidos = new List<ListarPartidosDTO>();
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"LigaData.Id: {ligaId}");
                    string query = @"
                        SELECT P.id, P.fecha as Fecha,
                               J1.username AS Jugador1Nombre, J1.id as Jugador1Id, PJ1.puntos AS PuntosJugador1,
                               J2.username AS Jugador2Nombre, J2.id as Jugador2Id, PJ2.puntos AS PuntosJugador2,
                               P.ganador_id AS GanadorId,
                               P.tipo_partido_id AS TipoPartidoId,
                               (SELECT J.username FROM JUGADOR J WHERE J.id = P.ganador_id) AS GanadorNombre,
                               L.nombre as LigaNombre
                        FROM PARTIDO P
                        JOIN PARTIDO_JUGADOR PJ1 ON P.id = PJ1.partido_id
                        JOIN JUGADOR J1 ON PJ1.jugador_id = J1.id
                        LEFT JOIN PARTIDO_JUGADOR PJ2 ON P.id = PJ2.partido_id AND PJ2.jugador_id <> PJ1.jugador_id
                        LEFT JOIN JUGADOR J2 ON PJ2.jugador_id = J2.id
                        JOIN LIGA L ON P.liga_id = L.id
                        WHERE (PJ1.jugador_id < PJ2.jugador_id OR PJ2.jugador_id IS NULL)";

                    if (ligaId != null)
                    {
                        query += " AND P.liga_id = @LigaId ORDER BY P.fecha DESC";
                        datos.AgregarParametro("@LigaId", ligaId.Value);
                    }

                    datos.SetearConsulta(query);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        ListarPartidosDTO partido = new ListarPartidosDTO
                        {
                            Id = (int)datos.Lector["Id"],
                            TipoPartidoId = (int)datos.Lector["TipoPartidoId"],
                            Jugador1Id = (int)datos.Lector["Jugador1Id"],
                            Jugador1Nombre = (string)datos.Lector["Jugador1Nombre"],
                            PuntosJugador1 = (int)datos.Lector["PuntosJugador1"],
                            Jugador2Id = (int)datos.Lector["Jugador2Id"],
                            Jugador2Nombre = (string)datos.Lector["Jugador2Nombre"],
                            PuntosJugador2 = (int)datos.Lector["PuntosJugador2"],
                            GanadorId = datos.Lector["GanadorId"] != DBNull.Value ? (int)datos.Lector["GanadorId"] : 0,
                            GanadorNombre = datos.Lector["GanadorNombre"] != DBNull.Value ? (string)datos.Lector["GanadorNombre"] : "",
                            NombreLiga = (string)datos.Lector["LigaNombre"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
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

        public List<TipoPartido> ListarTiposPartidos()
        {
            List<TipoPartido> tipoPartidos = new List<TipoPartido>();
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta(@"SELECT * FROM TIPO_PARTIDO");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TipoPartido tipo = new TipoPartido
                    {
                        Id = (int)datos.Lector["id"],
                        Sets = (int)datos.Lector["sets"],
                        Puntos = (int)datos.Lector["puntos"],
                        TextoDelSelect = "A " + (int)datos.Lector["puntos"] + " Puntos",
                    };
                    tipoPartidos.Add(tipo);
                }

                return tipoPartidos;
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

        public List<ListarPartidosDTO> listarPartidosEntreJugadores(int jugador1Id, int jugador2Id)
        {
            List<ListarPartidosDTO> partidos = new List<ListarPartidosDTO>();
            AccesoDatosDB datos = new AccesoDatosDB();
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT P.id,
                       J1.id as Jugador1Id, J1.username AS Jugador1Nombre, PJ1.puntos AS PuntosJugador1,
                       J2.id as Jugador2Id, J2.username AS Jugador2Nombre, PJ2.puntos AS PuntosJugador2,
                       P.ganador_id AS GanadorId,
                       P.fecha AS Fecha,
                       (SELECT J.username FROM JUGADOR J WHERE J.id = P.ganador_id) AS GanadorNombre,
                        L.nombre AS LigaNombre,
                        T.nombre AS TorneoNombre
                FROM PARTIDO P
                JOIN PARTIDO_JUGADOR PJ1 ON P.id = PJ1.partido_id
                JOIN JUGADOR J1 ON PJ1.jugador_id = J1.id
                LEFT JOIN PARTIDO_JUGADOR PJ2 ON P.id = PJ2.partido_id AND PJ2.jugador_id <> PJ1.jugador_id
                LEFT JOIN JUGADOR J2 ON PJ2.jugador_id = J2.id
                LEFT JOIN LIGA L ON P.liga_id = L.id
                LEFT JOIN RONDA R ON P.ronda_id = R.id
                LEFT JOIN TORNEO T ON R.torneo_id = T.id
                WHERE PJ1.jugador_id = @Jugador1Id AND PJ2.jugador_id = @Jugador2Id
                ORDER BY P.fecha DESC
            ");
                    datos.AgregarParametro("@Jugador1Id", jugador1Id);
                    datos.AgregarParametro("@Jugador2Id", jugador2Id);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        ListarPartidosDTO partido = new ListarPartidosDTO
                        {
                            Id = (int)datos.Lector["Id"],
                            Jugador1Id = (int)datos.Lector["Jugador1Id"],
                            Jugador1Nombre = (string)datos.Lector["Jugador1Nombre"],
                            PuntosJugador1 = (int)datos.Lector["PuntosJugador1"],
                            Jugador2Id = (int)datos.Lector["Jugador2Id"],
                            Jugador2Nombre = (string)datos.Lector["Jugador2Nombre"],
                            PuntosJugador2 = (int)datos.Lector["PuntosJugador2"],
                            GanadorId = datos.Lector["GanadorId"] != DBNull.Value ? (int)datos.Lector["GanadorId"] : 0,
                            GanadorNombre = datos.Lector["GanadorNombre"] != DBNull.Value ? (string)datos.Lector["GanadorNombre"] : "",
                            NombreLiga = datos.Lector["LigaNombre"] != DBNull.Value ? (string)datos.Lector["LigaNombre"] : "",
                            NombreTorneo = datos.Lector["TorneoNombre"] != DBNull.Value ? (string)datos.Lector["TorneoNombre"] : "",
                            Fecha = (DateTime)datos.Lector["Fecha"],
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

        public bool ActualizarPartido(int partidoId, int Jugador1Id, int puntosJugador1, int Jugador2Id, int puntosJugador2, int ganadorId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                System.Diagnostics.Debug.WriteLine($"{partidoId}");
                System.Diagnostics.Debug.WriteLine($"{Jugador1Id} , {Jugador2Id}");
                System.Diagnostics.Debug.WriteLine($"{ganadorId}");
                System.Diagnostics.Debug.WriteLine($"{puntosJugador1} , {puntosJugador2}");

                int ganadorIdNuevo = puntosJugador1 > puntosJugador2 ? Jugador1Id : Jugador2Id;

                datos.SetearConsulta(@"
                UPDATE PARTIDO_JUGADOR 
                SET puntos = 
                    CASE 
                        WHEN jugador_id = @Jugador1Id THEN @PuntosJugador1
                        WHEN jugador_id = @Jugador2Id THEN @PuntosJugador2
                    END
                WHERE partido_id = @PartidoId");
                datos.AgregarParametro("@Jugador1Id", Jugador1Id);
                datos.AgregarParametro("@PuntosJugador1", puntosJugador1);
                datos.AgregarParametro("@Jugador2Id", Jugador2Id);
                datos.AgregarParametro("@PuntosJugador2", puntosJugador2);
                datos.AgregarParametro("@PartidoId", partidoId);
                datos.EjecutarAccion();

                datos.CerrarConexion();

                if (ganadorIdNuevo != ganadorId)
                {
                    datos = new AccesoDatosDB();

                    datos.SetearConsulta("UPDATE PARTIDO SET ganador_id = @NuevoGanadorId WHERE id = @PartidoId");
                    datos.AgregarParametro("@NuevoGanadorId", ganadorIdNuevo);
                    datos.AgregarParametro("@PartidoId", partidoId);
                    datos.EjecutarAccion();

                    datos.CerrarConexion();
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex}");
                Console.WriteLine("Error al actualizar el partido: " + ex.Message);
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }
        public bool EliminarPartido(int partidoId)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            try
            {
                datos.SetearConsulta("DELETE FROM PARTIDO_JUGADOR WHERE partido_id = @partido_id");
                datos.AgregarParametro("@partido_id", partidoId);
                datos.EjecutarAccion();

                datos.CerrarConexion();

                datos = new AccesoDatosDB();

                datos.SetearConsulta("DELETE FROM PARTIDO WHERE id = @id");
                datos.AgregarParametro("@id", partidoId);
                datos.EjecutarAccion();

                datos.CerrarConexion();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el partido: " + ex.Message);
                System.Diagnostics.Debug.WriteLine($"Error al registrar log en la base de datos: {ex.Message}");
                return false;
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

        public List<PartidoDTO> Ultimos10PartidosByJugadorID(int idJugador)
        {
            AccesoDatosDB datos = new AccesoDatosDB();
            List<PartidoDTO> partidos = new List<PartidoDTO>();

            try
            {
                datos.SetearConsulta(@"
                SELECT TOP 10 
                    p.id AS PartidoId,
                    p.fecha,
                    L.nombre AS LigaNombre,
                    j1.nombre AS NombreJugador1,
                    j1.apellido AS ApellidoJugador1,
                    pj1.puntos AS PuntosJugador1,
                    j2.nombre AS NombreJugador2,
                    j2.apellido AS ApellidoJugador2,
                    pj2.puntos AS PuntosJugador2,
                    CASE 
                        WHEN p.ganador_id = @IdJugador THEN 1
                        ELSE 0 
                     END AS EsGanador
                FROM Partido p
                JOIN Partido_Jugador pj1 ON p.id = pj1.partido_id
                JOIN Partido_Jugador pj2 ON p.id = pj2.partido_id AND pj1.jugador_id > pj2.jugador_id
                JOIN Jugador j1 ON pj1.jugador_id = j1.id
                JOIN Jugador j2 ON pj2.jugador_id = j2.id
                JOIN LIGA L ON p.liga_id = L.id
                WHERE pj1.jugador_id = @IdJugador OR pj2.jugador_id = @IdJugador
                ORDER BY p.fecha DESC");

                datos.AgregarParametro("@IdJugador", idJugador);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PartidoDTO partido = new PartidoDTO
                    {
                        PartidoId = (int)datos.Lector["PartidoId"],
                        Fecha = (DateTime)datos.Lector["fecha"],
                        NombreJugador1 = datos.Lector["NombreJugador1"].ToString(),
                        ApellidoJugador1 = datos.Lector["ApellidoJugador1"].ToString(),
                        PuntosJugador1 = (int)datos.Lector["PuntosJugador1"],
                        NombreJugador2 = datos.Lector["NombreJugador2"].ToString(),
                        ApellidoJugador2 = datos.Lector["ApellidoJugador2"].ToString(),
                        PuntosJugador2 = (int)datos.Lector["PuntosJugador2"],
                        EsGanador = Convert.ToBoolean(datos.Lector["EsGanador"]),
                        NombreLiga = datos.Lector["LigaNombre"].ToString()
                    };
                    partidos.Add(partido);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los partidos: " + ex.Message);
            }
            finally
            {
                datos.CerrarConexion();
            }
            return partidos;

        }

    }
}
