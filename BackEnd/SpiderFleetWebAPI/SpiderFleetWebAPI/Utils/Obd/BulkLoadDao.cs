using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Sims.Credit;
using CredencialSpiderFleet.Models.Sims.XMLSim;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.Sims;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace SpiderFleetWebAPI.Utils.Obd
{
    public class BulkLoadDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();
        private VariableConfiguration configuration = new VariableConfiguration();

        public BulkLoadResponse BulkLoadObds(StreamReader reader, string empresa)
        {
            BulkLoadResponse response = new BulkLoadResponse();
            List<string> listSims = new List<string>();
            try
            {
                string Line;
                
                while ((Line = reader.ReadLine()) != null)
                {
                    string[] data = Line.Split(',');
                    if (data.Length > 0)
                    {
                        CredencialSpiderFleet.Models.Obd.BulkLoad bulk = new CredencialSpiderFleet.Models.Obd.BulkLoad();
                        bulk.Node = empresa;
                        bulk.Device = data[0].ToString();                        
                        bulk.IdSpider = data[2].ToString();
                        bulk.Name = data[2].ToString();
                        bulk.Dongle = Convert.ToInt32(data[3].ToString());
                        bulk.Empresa = UseFul.NumberEmpresa(empresa);
                        bulk.Sim = data[1].ToString();

                        CreateRegistry(bulk, response, listSims);
                    }
                }

                if (listSims.Count > 0)
                {
                    CreditCompany(empresa, listSims, response);
                }
                return response;
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        private void CreateRegistry(CredencialSpiderFleet.Models.Obd.BulkLoad bulk, BulkLoadResponse response, List<string> sims)
        {
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_bulk_load_obds", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(bulk.Device)));
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(bulk.Node)));
                    cmd.Parameters.Add(new SqlParameter("@idspider", Convert.ToString(bulk.IdSpider)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(bulk.Name)));
                    cmd.Parameters.Add(new SqlParameter("@dongle", Convert.ToInt32(bulk.Dongle)));
                    cmd.Parameters.Add(new SqlParameter("@empresa", Convert.ToString(bulk.Empresa)));
                    cmd.Parameters.Add(new SqlParameter("@sim", Convert.ToString(bulk.Sim)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("El numero de dispositivo " + bulk.Device + " ya existe, favor de verificar");
                    }
                    else if (respuesta == 5)
                    {
                        response.success = false;
                        response.messages.Add("Ocurrio un error al dar de alta el registro " + bulk.Device +", favor de verificar");
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("El numero de Sim " + bulk.Sim + " no existe favor dfe verificar, favor de verificar");
                    }
                    else if (respuesta == 4)
                    {
                        response.success = false;
                        response.messages.Add("El numero de Sim " + bulk.Sim + " ya esta siendo utilizado, favor de verificar");
                    }
                    else if (respuesta == 1)
                    {
                        sims.Add(bulk.Sim);
                        response.success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        public void CreditCompany(string empresa,List<string> listSims, BulkLoadResponse response)
        {
            try
            {
                ListCredits listaCredit = new ListCredits();

                foreach (string simNum in listSims)
                {
                    Credits credits = new Credits();
                    credits.Msisdn = simNum;
                    credits.Amount = "1.00";
                    listaCredit.credits.Add(credits);
                }

                List<string> listSim = new List<string>();
                List<Sbalance> listBalance = new List<Sbalance>();

                if (listaCredit.credits.Count > 0)
                {
                    foreach (Credits credits in listaCredit.credits)
                    {

                        //Obtiene  Id Order
                        string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";
                        response.messages.Add("Id Order " + urlId);

                        XmlDocument docId = new XmlDocument();
                        try
                        {
                            docId.Load(urlId);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                        }

                        Account account = new Account();
                        XmlNodeList listAccount = docId.SelectNodes("account");
                        XmlNode accounts = listAccount.Item(0);

                        account.Name = accounts["name"].InnerText;
                        account.Active = accounts["active"].InnerText;
                        account.Expire = accounts["expire"].InnerText;
                        account.Balance = accounts["balance"].InnerText;
                        account.Currency = accounts["currency"].InnerText;
                        account.Orderid = accounts["orderid"].InnerText.Trim();  //muestra el ultimo ejecutado 

                        int orderId = Convert.ToInt32(account.Orderid);
                        orderId++;

                        //Asigna Credito al numero sim correspondiente
                        string url = configuration.url + configuration.user + "&upass=" + configuration.password +
                            "&plain=1&command=sbalance&onum=" + credits.Msisdn + "&amount=" + credits.Amount + "&curr=" + configuration.currency + "&orderid=" + orderId;

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                        }

                        Sbalance dataBalance = new Sbalance();
                        XmlNodeList listData = doc.SelectNodes("sbalance");
                        XmlNode balance;

                        for (int i = 0; i < listData.Count; i++)
                        {
                            balance = listData.Item(i);
                            dataBalance.Aserviceid = balance.SelectSingleNode("aserviceid").InnerText;
                            dataBalance.Inum = balance.SelectSingleNode("inum").InnerText;
                            dataBalance.Onum = balance.SelectSingleNode("onum").InnerText;
                            dataBalance.Amount = balance.SelectSingleNode("amount").InnerText;
                            dataBalance.Orderid = balance.SelectSingleNode("orderid").InnerText;

                            XmlNodeList card = balance.SelectNodes("card");
                            XmlNode cards = card.Item(0);
                            dataBalance.Card = new CredencialSpiderFleet.Models.Sims.Credit.Card();
                            dataBalance.Card.Balance = cards.FirstChild.InnerText;
                            dataBalance.Card.Amount = cards.LastChild.InnerText;

                            XmlNodeList client = balance.SelectNodes("client");
                            XmlNode clients = client.Item(0);
                            dataBalance.Client = new Client();
                            dataBalance.Client.Balance = clients.FirstChild.InnerText;
                            dataBalance.Client.Amount = clients.LastChild.InnerText;
                        }

                        string saldo = dataBalance.Amount.Replace(configuration.currency, "");
                        (new SimsMaintenanceDao()).HistorialSims(empresa, dataBalance.Onum, saldo.Trim());
                    }
                    response.success = true;
                }
                else
                {
                    response.success = false;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }
        }
    }
}