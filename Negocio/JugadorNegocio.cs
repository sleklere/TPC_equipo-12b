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

    }
}
