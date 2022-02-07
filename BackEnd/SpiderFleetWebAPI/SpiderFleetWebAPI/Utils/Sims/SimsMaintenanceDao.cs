using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Sims;
using CredencialSpiderFleet.Models.Sims.Credit;
using CredencialSpiderFleet.Models.Sims.XMLSim;
using SpiderFleetWebAPI.Models.Response.Sims;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace SpiderFleetWebAPI.Utils.Sims
{
    public class SimsMaintenanceDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private VariableConfiguration configuration = new VariableConfiguration();
        public SimsMaintenanceDao() { }


        public SimMaintenanceResponse bulking()
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();

            string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";

            List<string> simsBd = new List<string>();
            try
            {
                simsBd = ReadListSimsAvailable(1);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(url);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                Records record = new Records();
                XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                int item = 0;
                List<string> simsWeb = new List<string>();
                foreach (XmlElement nodo in xLista)
                {
                    Records xml = new Records();
                    xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                    XmlNode da;
                    da = xLista.Item(item);
                    if (da.HasChildNodes)
                    {
                        for (int i = 0; i < da.ChildNodes.Count; i++)
                        {
                            string dato = da.ChildNodes[i].Name;
                            dato = dato.ToLower();

                            if (dato.Equals(msisdns.inum.ToString()))
                            {
                                xml.Card.Inum = da.ChildNodes[i].InnerText;
                                break;
                            }
                        }
                    }

                    item++;
                    simsWeb.Add(xml.Card.Inum);
                }

                List<string> simsDif = new List<string>();
                simsDif = simsWeb.Except(simsBd).ToList();

                foreach (string sim in simsDif)
                {
                    CredencialSpiderFleet.Models.Sims.SimsMaintenance sims = new CredencialSpiderFleet.Models.Sims.SimsMaintenance();
                    sims.Sim = sim;
                    sims.Status = Convert.ToInt32(statussims.available);
                    sims.LastUploadDate = null;

                    try
                    {
                        response = Create(sims);
                        response.messages.Add("response :" + response.success + " sims: " + sims);
                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        public SimMaintenanceResponse Create(CredencialSpiderFleet.Models.Sims.SimsMaintenance sims)
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@sim", Convert.ToString(sims.Sim)));
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(sims.Status)));
                    if (!string.IsNullOrEmpty(sims.LastUploadDate.ToString())) { cmd.Parameters.Add(new SqlParameter("@lastUploadDate", Convert.ToString(sims.LastUploadDate.ToString()))); }
                    else { cmd.Parameters.Add(new SqlParameter("@lastUploadDate", DBNull.Value)); }

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.VarChar;
                    sqlParameter.Size = 60;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al tratar de actualizar el registro");
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

        public List<string> ReadListSims()
        {
            List<string> listSim = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_status_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listSim.Add(Convert.ToString(reader["sim"]));
                        }
                    }
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
            return listSim;
        }

        //sims disponibles
        public List<string> ReadListSimsAvailable(int status)
        {
            List<string> listSim = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_status_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(status)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listSim.Add(Convert.ToString(reader["sim"]));
                        }
                    }
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
            return listSim;
        }

        //Para formatar el texto seleccionado: Ctrl+K, Ctrl+F

        /// <summary>
        /// Metodo que pone credito a los sims con estatus 2
        /// </summary>
        /// <returns></returns>
        public SimMaintenanceResponse Credit()
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();
            try
            {

                List<string> simsBd = new List<string>();
                try
                {
                    simsBd = ReadListSimsAvailable(2);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }


                Dictionary<string, string> val = new Dictionary<string, string>();
                if (simsBd.Count > 0)
                {
                    try
                    {

                        string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
                        }

                        Records record = new Records();
                        XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                        XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                        int item = 0;
                        foreach (XmlElement nodo in xLista)
                        {
                            Records xml = new Records();
                            xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                            XmlNode da;
                            da = xLista.Item(item);
                            if (da.HasChildNodes)
                            {
                                for (int i = 0; i < da.ChildNodes.Count; i++)
                                {
                                    string dato = da.ChildNodes[i].Name;
                                    dato = dato.ToLower();

                                    if (dato.Equals(msisdns.tsimid.ToString()))
                                    {
                                        xml.Card.Tsimid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.aserviceid.ToString()))
                                    {
                                        xml.Card.Aserviceid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.inum.ToString()))
                                    {
                                        xml.Card.Inum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.onum.ToString()))
                                    {
                                        xml.Card.Onum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.prepayed.ToString()))
                                    {
                                        xml.Card.Prepayed = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.blocked.ToString()))
                                    {
                                        xml.Card.Blocked = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.balance.ToString()))
                                    {
                                        xml.Card.Balance = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.curr.ToString()))
                                    {
                                        xml.Card.Curr = da.ChildNodes[i].InnerText;
                                    }
                                }
                            }

                            val.Add(xml.Card.Inum, xml.Card.Balance);

                            item++;
                        }

                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }


                ListCredits listaCredit = new ListCredits();

                if (val.Count > 0)
                {
                    listaCredit.credits = new List<Credits>();

                    foreach (string simNum in simsBd)
                    {
                        if (val.ContainsKey(simNum))
                        {
                            double valor = Convert.ToDouble(val[simNum]);
                            if (valor < 2.00)// 0.50)
                            {
                                Credits credits = new Credits();
                                credits.Msisdn = simNum;
                                credits.Amount = "1.00";
                                listaCredit.credits.Add(credits);
                            }
                        }
                    }
                }

                List<string> listSim = new List<string>();
                List<Sbalance> listBalance = new List<Sbalance>();
                if (listaCredit.credits.Count > 0)
                {
                    foreach (Credits credits in listaCredit.credits)
                    {

                        //Obtiene  Id Order
                        string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";
                        XmlDocument docId = new XmlDocument();
                        try
                        {
                            docId.Load(urlId);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
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
                            return response;
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

                        listBalance.Add(dataBalance);
                        listSim.Add(credits.Msisdn.Trim());
                    }

                    //(new Message.Message()).sendEmail("Guillermo ", listSim, "fecha", "puesto");
                    response.listBalance = listBalance;
                    response.success = true;
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        /// <summary>
        /// Metodo que quita credito a los sims con estatus 3
        /// </summary>
        /// <returns></returns>
        public TransferCreditResponse TransferCreditDebitAccount()
        {
            TransferCreditResponse response = new TransferCreditResponse();
            List<string> simsBd = new List<string>();
            try
            {
                try
                {
                    simsBd = ReadListSimsAvailable(3);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }


                Dictionary<string, string> val = new Dictionary<string, string>();
                if (simsBd.Count > 0)
                {
                    try
                    {
                        string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
                        }

                        Records record = new Records();
                        XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                        XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                        int item = 0;
                        foreach (XmlElement nodo in xLista)
                        {
                            Records xml = new Records();
                            xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                            XmlNode da;
                            da = xLista.Item(item);
                            if (da.HasChildNodes)
                            {
                                for (int i = 0; i < da.ChildNodes.Count; i++)
                                {
                                    string dato = da.ChildNodes[i].Name;
                                    dato = dato.ToLower();

                                    if (dato.Equals(msisdns.tsimid.ToString()))
                                    {
                                        xml.Card.Tsimid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.aserviceid.ToString()))
                                    {
                                        xml.Card.Aserviceid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.inum.ToString()))
                                    {
                                        xml.Card.Inum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.onum.ToString()))
                                    {
                                        xml.Card.Onum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.prepayed.ToString()))
                                    {
                                        xml.Card.Prepayed = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.blocked.ToString()))
                                    {
                                        xml.Card.Blocked = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.balance.ToString()))
                                    {
                                        xml.Card.Balance = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.curr.ToString()))
                                    {
                                        xml.Card.Curr = da.ChildNodes[i].InnerText;
                                    }
                                }
                            }

                            val.Add(xml.Card.Inum, xml.Card.Balance);

                            item++;
                        }

                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }


                ListCredits listaCredit = new ListCredits();

                try
                {
                    if (val.Count > 0)
                    {
                        listaCredit.credits = new List<Credits>();

                        foreach (string simNum in simsBd)
                        {
                            if (val.ContainsKey(simNum))
                            {
                                double valor = Convert.ToDouble(val[simNum]);

                                if (valor > 2)
                                {
                                    Credits credits = new Credits();
                                    credits.Msisdn = simNum;

                                    string quitaMoney = Convert.ToString(Convert.ToDecimal(val[simNum]) - Convert.ToDecimal("1.00"));

                                    credits.Amount = "-" + quitaMoney; //val[simNum];
                                    listaCredit.credits.Add(credits);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                List<string> listSim = new List<string>();
                List<Sbalance> listBalance = new List<Sbalance>();
                if (listaCredit.credits.Count > 0)
                {
                    int count = 0;
                    foreach (Credits credits in listaCredit.credits)
                    {

                        //Obtiene  Id Order
                        string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";
                        XmlDocument docId = new XmlDocument();
                        try
                        {
                            docId.Load(urlId);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
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
                            return response;
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

                        listBalance.Add(dataBalance);
                        listSim.Add(credits.Msisdn.Trim());
                    }
                }

                //(new Message.Message()).sendEmail("Guillermo ", listSim, "fecha", "quitado");
                response.listBalance = listBalance;
                response.success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
                return response;
            }
        }

        /// <summary>
        /// Metodo que pone credito a los sims por empresa
        /// </summary>
        /// <returns></returns>
        public SimMaintenanceResponse CreditCompany(string hierarchy)
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();
            try
            {

                List<string> simsBd = new List<string>();
                try
                {
                    simsBd = ReadListSimsCompany(hierarchy);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                Dictionary<string, string> val = new Dictionary<string, string>();
                if (simsBd.Count > 0)
                {
                    try
                    {

                        string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";
                        //string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";

                        response.messages.Add("Lista Sims " + url);

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
                        }

                        Records record = new Records();
                        XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                        XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                        int item = 0;
                        foreach (XmlElement nodo in xLista)
                        {
                            Records xml = new Records();
                            xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                            XmlNode da;
                            da = xLista.Item(item);
                            if (da.HasChildNodes)
                            {
                                for (int i = 0; i < da.ChildNodes.Count; i++)
                                {
                                    string dato = da.ChildNodes[i].Name;
                                    dato = dato.ToLower();

                                    if (dato.Equals(msisdns.tsimid.ToString()))
                                    {
                                        xml.Card.Tsimid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.aserviceid.ToString()))
                                    {
                                        xml.Card.Aserviceid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.inum.ToString()))
                                    {
                                        xml.Card.Inum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.onum.ToString()))
                                    {
                                        xml.Card.Onum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.prepayed.ToString()))
                                    {
                                        xml.Card.Prepayed = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.blocked.ToString()))
                                    {
                                        xml.Card.Blocked = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.balance.ToString()))
                                    {
                                        xml.Card.Balance = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.curr.ToString()))
                                    {
                                        xml.Card.Curr = da.ChildNodes[i].InnerText;
                                    }
                                }
                            }

                            val.Add(xml.Card.Inum, xml.Card.Balance);

                            item++;
                        }

                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }


                ListCredits listaCredit = new ListCredits();

                if (val.Count > 0)
                {
                    listaCredit.credits = new List<Credits>();

                    foreach (string simNum in simsBd)
                    {
                        if (val.ContainsKey(simNum))
                        {
                            double valor = Convert.ToDouble(val[simNum]);

                            if (valor <= 0.80)
                            {
                                Credits credits = new Credits();
                                credits.Msisdn = simNum;
                                credits.Amount = "1.00";
                                listaCredit.credits.Add(credits);
                            }
                        }
                    }
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
                            return response;
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

                        response.messages.Add("Recarga " + urlId);

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
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

                        HistorialSims(hierarchy, dataBalance.Onum, saldo.Trim());

                        listBalance.Add(dataBalance);
                        listSim.Add(credits.Msisdn.Trim());
                    }
                    response.listBalance = listBalance;
                    response.success = true;
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        public SimMaintenanceResponse CreditSimsAllCompany()
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();
            try
            {
                List<string> listNode = new List<string>();
                List<string> listSims = new List<string>();
                try
                {
                    listNode = ReadListAllCompany();
                    if(listNode.Count > 0)
                    {
                        foreach(var node in listNode)
                        {
                            ReadListSimsAllCompany(node, listSims);
                        }
                    }
                    else
                    {
                        response.success = false;
                        response.messages.Add("No hay Compañias para cargar saldo");
                        return response;
                    }
                    
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                Dictionary<string, string> val = new Dictionary<string, string>();
                if (listSims.Count > 0)
                {
                    try
                    {

                        string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";
                        //string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
                        }

                        Records record = new Records();
                        XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                        XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                        int item = 0;
                        foreach (XmlElement nodo in xLista)
                        {
                            Records xml = new Records();
                            xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                            XmlNode da;
                            da = xLista.Item(item);
                            if (da.HasChildNodes)
                            {
                                for (int i = 0; i < da.ChildNodes.Count; i++)
                                {
                                    string dato = da.ChildNodes[i].Name;
                                    dato = dato.ToLower();

                                    if (dato.Equals(msisdns.tsimid.ToString()))
                                    {
                                        xml.Card.Tsimid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.aserviceid.ToString()))
                                    {
                                        xml.Card.Aserviceid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.inum.ToString()))
                                    {
                                        xml.Card.Inum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.onum.ToString()))
                                    {
                                        xml.Card.Onum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.prepayed.ToString()))
                                    {
                                        xml.Card.Prepayed = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.blocked.ToString()))
                                    {
                                        xml.Card.Blocked = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.balance.ToString()))
                                    {
                                        xml.Card.Balance = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.curr.ToString()))
                                    {
                                        xml.Card.Curr = da.ChildNodes[i].InnerText;
                                    }
                                }
                            }

                            val.Add(xml.Card.Inum, xml.Card.Balance);

                            item++;
                        }

                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }
                else
                {
                    response.success = false;
                    response.messages.Add("No hay Sims para cargar saldo");
                    return response;
                }


                ListCredits listaCredit = new ListCredits();

                if (val.Count > 0)
                {
                    listaCredit.credits = new List<Credits>();

                    foreach (string simNum in listSims)
                    {
                        if (val.ContainsKey(simNum))
                        {
                            double valor = Convert.ToDouble(val[simNum]);

                            if (valor <= 0.50)
                            {
                                Credits credits = new Credits();
                                credits.Msisdn = simNum;
                                credits.Amount = "0.80";
                                listaCredit.credits.Add(credits);
                            }
                        }
                    }
                }

                List<string> listSim = new List<string>();
                List<Sbalance> listBalance = new List<Sbalance>();

                if (listaCredit.credits.Count > 0)
                {
                    foreach (Credits credits in listaCredit.credits)
                    {

                        //Obtiene  Id Order
                        string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";

                        XmlDocument docId = new XmlDocument();
                        try
                        {
                            docId.Load(urlId);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
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
                            return response;
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

                        HistorialSims("/", dataBalance.Onum, saldo.Trim());

                        listBalance.Add(dataBalance);
                        listSim.Add(credits.Msisdn.Trim());
                    }
                    response.listBalance = listBalance;
                    response.success = true;
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }



        public SimMaintenanceResponse TransferCreditCompany(string hierarchy)
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();
            try
            {

                List<string> simsBd = new List<string>();
                try
                {
                    simsBd = ReadListSimsCompany(hierarchy);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }


                Dictionary<string, string> val = new Dictionary<string, string>();
                if (simsBd.Count > 0)
                {
                    try
                    {

                        string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
                        }

                        Records record = new Records();
                        XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                        XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                        int item = 0;
                        foreach (XmlElement nodo in xLista)
                        {
                            Records xml = new Records();
                            xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                            XmlNode da;
                            da = xLista.Item(item);
                            if (da.HasChildNodes)
                            {
                                for (int i = 0; i < da.ChildNodes.Count; i++)
                                {
                                    string dato = da.ChildNodes[i].Name;
                                    dato = dato.ToLower();

                                    if (dato.Equals(msisdns.tsimid.ToString()))
                                    {
                                        xml.Card.Tsimid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.aserviceid.ToString()))
                                    {
                                        xml.Card.Aserviceid = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.inum.ToString()))
                                    {
                                        xml.Card.Inum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.onum.ToString()))
                                    {
                                        xml.Card.Onum = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.prepayed.ToString()))
                                    {
                                        xml.Card.Prepayed = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.blocked.ToString()))
                                    {
                                        xml.Card.Blocked = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.balance.ToString()))
                                    {
                                        xml.Card.Balance = da.ChildNodes[i].InnerText;
                                    }
                                    else if (dato.Equals(msisdns.curr.ToString()))
                                    {
                                        xml.Card.Curr = da.ChildNodes[i].InnerText;
                                    }
                                }
                            }

                            val.Add(xml.Card.Inum, xml.Card.Balance);

                            item++;
                        }

                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }



                ListCredits listaCredit = new ListCredits();

                try
                {
                    if (val.Count > 0)
                    {
                        listaCredit.credits = new List<Credits>();

                        foreach (string simNum in simsBd)
                        {
                            if (val.ContainsKey(simNum))
                            {
                                double valor = Convert.ToDouble(val[simNum]);

                                Credits credits = new Credits();
                                credits.Msisdn = simNum;

                                string quitaMoney = Convert.ToString(Convert.ToDecimal(val[simNum]));

                                if (!quitaMoney.Equals("0.00"))
                                {
                                    credits.Amount = "-" + quitaMoney;
                                    response.messages.Add("sim : " + credits.Msisdn + " monto :" + credits.Amount);
                                    listaCredit.credits.Add(credits);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                List<string> listSim = new List<string>();
                List<Sbalance> listBalance = new List<Sbalance>();
                if (listaCredit.credits.Count > 0)
                {
                    int count = 0;
                    foreach (Credits credits in listaCredit.credits)
                    {

                        //Obtiene  Id Order
                        string urlId = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=account&plain=1";
                        XmlDocument docId = new XmlDocument();
                        try
                        {
                            docId.Load(urlId);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
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

                        string url = configuration.url + configuration.user + "&upass=" + configuration.password +
                            "&plain=1&command=sbalance&onum=" + credits.Msisdn + "&amount=" + credits.Amount + "&curr=" + configuration.currency + "&orderid=" + orderId;

                        response.messages.Add("ejecuta url : " + url);

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(url);
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.messages.Add(ex.Message);
                            return response;
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

                        listBalance.Add(dataBalance);
                        listSim.Add(credits.Msisdn.Trim());
                    }
                }

                //(new Message.Message()).sendEmail("Guillermo ", listSim, "fecha", "quitado");
                response.listBalance = listBalance;
                response.success = true;


                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }





        public List<string> ReadListSimsCompany(string hierarchy)
        {
            List<string> listSim = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_sims_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listSim.Add(Convert.ToString(reader["sim"]));
                        }
                    }
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
            return listSim;
        }

        public List<string> ReadListAllCompany()
        {
            List<string> listNode = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_company_credit", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@active", Convert.ToInt32(1)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listNode.Add(Convert.ToString(reader["node"]));
                        }
                    }
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
            return listNode;
        }


        public void ReadListSimsAllCompany(string node, List<string> listSim)
        {
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_sims_by_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listSim.Add(Convert.ToString(reader["sim"]));
                        }
                    }
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
        }

        /// <summary>
        /// Metodo que lleva el historial de los sim de credito
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="sim"></param>
        /// <param name="saldo"></param>
        /// <returns></returns>
        public int HistorialSims(string hierarchy, string sim, string saldo)
        {
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_history_amount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@sim", Convert.ToString(sim)));
                    cmd.Parameters.Add(new SqlParameter("@saldo", Convert.ToDecimal(saldo)));

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

        public ReportCreditSimsResponse ReporteCreditoSims()
        {
            ReportCreditSimsResponse response = new ReportCreditSimsResponse();
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                try
                {

                    string url = configuration.url + configuration.user + "&upass=" + configuration.password + "&plain=1&command=gbalance";
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(url);
                        response.messages.Add("Url");
                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }

                    Records record = new Records();
                    XmlNodeList xPersonas = doc.GetElementsByTagName("records");
                    XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName("card");
                    int item = 0;
                    foreach (XmlElement nodo in xLista)
                    {
                        Records xml = new Records();
                        xml.Card = new CredencialSpiderFleet.Models.Sims.XMLSim.Card();
                        XmlNode da;
                        da = xLista.Item(item);
                        if (da.HasChildNodes)
                        {
                            for (int i = 0; i < da.ChildNodes.Count; i++)
                            {
                                string dato = da.ChildNodes[i].Name;
                                dato = dato.ToLower();

                                if (dato.Equals(msisdns.tsimid.ToString()))
                                {
                                    xml.Card.Tsimid = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.aserviceid.ToString()))
                                {
                                    xml.Card.Aserviceid = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.inum.ToString()))
                                {
                                    xml.Card.Inum = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.onum.ToString()))
                                {
                                    xml.Card.Onum = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.prepayed.ToString()))
                                {
                                    xml.Card.Prepayed = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.blocked.ToString()))
                                {
                                    xml.Card.Blocked = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.balance.ToString()))
                                {
                                    xml.Card.Balance = da.ChildNodes[i].InnerText;
                                }
                                else if (dato.Equals(msisdns.curr.ToString()))
                                {
                                    xml.Card.Curr = da.ChildNodes[i].InnerText;
                                }
                            }
                        }

                        val.Add(xml.Card.Inum, xml.Card.Balance);

                        item++;
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                List<ReportSims> ListReportSims = new List<ReportSims>();
                ReportSims report = new ReportSims();

                if (val.Count > 0)
                {
                    foreach (var item in val)
                    {
                        report = new ReportSims();
                        report = ReadInfoSims(item.Key);

                        if (report.Sim == null)
                        {
                            report.Sim = item.Key;
                            report.HierarchyEmpresa = string.Empty;
                            report.Empresa = string.Empty;
                            report.HierarchySubEmpresa = string.Empty;
                            report.SubEmpresa = string.Empty;
                            report.Device = string.Empty;
                        }
                        report.Saldo = item.Value;
                        ListReportSims.Add(report);
                    }

                    response.success = true;
                    response.listReportSims = ListReportSims;
                    response.messages.Add("count " + ListReportSims.Count);
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }


        public ReportSims ReadInfoSims(string sim)
        {
            ReportSims data = new ReportSims();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_report_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sim", Convert.ToString(sim)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = new ReportSims();
                            data.HierarchyEmpresa = Convert.ToString(reader["hierarchy_empresa"]);
                            data.Empresa = Convert.ToString(reader["nombre_empresa"]);
                            data.HierarchySubEmpresa = Convert.ToString(reader["hierarchy_subempresa"]);
                            data.SubEmpresa = Convert.ToString(reader["nombre_sub_empresa"]);
                            data.Device = Convert.ToString(reader["idDevice"]);
                            data.Sim = Convert.ToString(reader["sim"]);
                            data.Saldo = string.Empty;
                        }
                    }
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
            return data;
        }
    }
}