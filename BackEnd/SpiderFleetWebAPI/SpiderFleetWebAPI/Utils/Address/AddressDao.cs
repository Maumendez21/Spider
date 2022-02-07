using CredencialSpiderFleet.Models.Address;
using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Address
{
    public class AddressDao
    {
        public AddressDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public int Create(CredencialSpiderFleet.Models.Address.Address address)
        {
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_device_by_address", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@device", address.Device));
                    cmd.Parameters.Add(new SqlParameter("@date", address.Date));
                    cmd.Parameters.Add(new SqlParameter("@point", address.Point));
                    cmd.Parameters.Add(new SqlParameter("@latitude", address.Latitude));
                    cmd.Parameters.Add(new SqlParameter("@longitude", address.Longitude));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                                      
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return respuesta;
        }

        public AddressConsult GetAddress(string device, DateTime date, string latitude, string longitude)
        {
            var address = new AddressConsult();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_device_by_address", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", device));
                        cmd.Parameters.Add(new SqlParameter("@date", date));
                        cmd.Parameters.Add(new SqlParameter("@latitude", latitude));
                        cmd.Parameters.Add(new SqlParameter("@longitude", longitude));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                address = MapToValue(reader);
                            }
                        }

                        return address;
                    }
                }
                else
                {
                    address = null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return address;
        }

        private AddressConsult MapToValue(SqlDataReader reader)
        {
            return new AddressConsult()
            {
                Address = reader["Point"].ToString()
            };
        }

    }
}