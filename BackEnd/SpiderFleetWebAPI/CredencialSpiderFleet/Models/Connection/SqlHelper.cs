using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Connection
{
    public class SqlHelper
    {
        private SqlConnection connection = new SqlConnection();

        /// <summary>
        /// Metodo de conexion e inicializacion de variales de coneccion
        /// </summary>
        public SqlHelper()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DB_SpiderSQL"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Metodo que verifica la conexion 
        /// </summary>
        public bool IsConnection
        {
            get
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                return true;
            }
        }

        /// <summary>
        /// Metodo que devuelve la 
        /// </summary>
        public SqlConnection Connection()
        {
            return connection;
        }
    }
}