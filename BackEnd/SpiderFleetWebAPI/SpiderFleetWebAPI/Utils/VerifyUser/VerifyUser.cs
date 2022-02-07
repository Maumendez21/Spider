using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Utils.DataSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SpiderFleetWebAPI.Utils.VerifyUser
{
    public class VerifyUser
    {
        public string verifyTokenUser(IPrincipal User)
        {
            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;
                var identity = (ClaimsIdentity)User.Identity;
                var claim = identity.Claims.ToList();
                username = claim?.FirstOrDefault(x => x.Type.Equals("username", StringComparison.OrdinalIgnoreCase))?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("El username se encuentra vacio");
                }

                return username;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int GetHours()
        {
            try
            {
                int horas = 0;

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                //DateTime cstTime = new DateTime(2021, 04, 4, 2, 0, 0);
                //DateTime cstTime = new DateTime(2021, 10, 31, 2, 59, 58);
                //DateTime cstTime = new DateTime(2021, 10, 31, 3, 0, 1);

                //Mes
                int mesInicio = Convert.ToInt32((new DataSystemDao()).GetDataSystem("MIV"));
                int mesFin = Convert.ToInt32((new DataSystemDao()).GetDataSystem("MFV"));
                //Dia
                int diaInicio = Convert.ToInt32((new DataSystemDao()).GetDataSystem("DIV"));
                int diaFin = Convert.ToInt32((new DataSystemDao()).GetDataSystem("DFV"));
                //Hora
                int horaInicio = Convert.ToInt32((new DataSystemDao()).GetDataSystem("HIV"));
                int horaFin = Convert.ToInt32((new DataSystemDao()).GetDataSystem("HVF"));

                DateTime dateStart = new DateTime(cstTime.Year, mesInicio, diaInicio, horaInicio, 0, 0);
                DateTime dateEnd = new DateTime(cstTime.Year, mesFin, diaFin, horaFin, 0, 0);

                DateTime dateNow = new DateTime(cstTime.Year, cstTime.Month, cstTime.Day, cstTime.Hour, cstTime.Minute, cstTime.Second);

                int compareHI = 0;
                int compareHV = 0;
                compareHI = UseFul.compare(dateStart, dateNow);
                compareHV = UseFul.compare(dateNow, dateEnd);
                if (compareHI >= 0)
                {
                    horas = Convert.ToInt32((new DataSystemDao()).GetDataSystem("HI"));
                }
                else if (compareHI < 0)
                {
                    if (compareHV <= 0)
                    {
                        horas = Convert.ToInt32((new DataSystemDao()).GetDataSystem("HV"));
                    }
                    else
                    {
                        horas = Convert.ToInt32((new DataSystemDao()).GetDataSystem("HI"));
                    }
                }

                return horas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}