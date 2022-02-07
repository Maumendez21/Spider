using CredencialSpiderFleet.Models.Logical;
using CredencialSpiderFleet.Models.SnappedPoints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CredencialSpiderFleet.Models.Useful
{
    public class UseFul
    {
        private const double EARTHRADIUS = 6371;

        public UseFul()
        { }
        /// <summary>
        /// Metodo que genera un password de 7 digitos
        /// </summary>
        public string RandomPassword()
        {
            Random obj = new Random();
            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = posibles.Length;
            char letra;
            int longitudnuevacadena = 7;
            string nuevacadena = "";
            for (int i = 0; i < longitudnuevacadena; i++)
            {
                letra = posibles[obj.Next(longitud)];
                nuevacadena += letra.ToString();
            }
            return nuevacadena;
        }

        /// <summary>
        /// Metodo que Encripta una cadena
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string MD5Cifrar(string word)
        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(word));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Metodo que valida la estructura del Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


        /// <summary>
        /// Metodo que valida numero telefonico de 8 a 10 Digitos
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public bool IsValidTelephone(string strNumber)
        {
            Regex regex = new Regex(@"\A[0-9]{8,10}\z");
            Match match = regex.Match(strNumber);

            if (match.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Metodo que valida la longitud
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="longitud"></param>
        /// <returns></returns>
        public bool IsValidLength(string valor, int longitud)
        {
            if (valor.Length > longitud)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Metodo que valida numero telefonico de 8 a 10 Digitos
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public bool IsValidCaracterEspecial(string strNumber)
        {
            //Regex regex = new Regex(@"~!@#$%^&*()_|+\-=?;:',.<>\{\}\[\]\\\/]/");
            Regex regex = new Regex(@"~!@#$%^&*()_|+\-=?;:',");
            Match match = regex.Match(strNumber);

            if (match.Success)
                return true;
            else
                return false;
        }

        public bool hasSpecialChar(string input)
        {
            string specialChar = @"@\|!#$%&/()=?»«@£§€{}+*^~[]¡¿¬°;'<>,";  //_.
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

        public static int compare(DateTime fechaInicio, DateTime fechaFin)
        {
            int respuesta = DateTime.Compare(fechaInicio, fechaFin);
            return respuesta;

            //if (result < 0)
            //    relationship = "is earlier than";
            //else if (result == 0)
            //    relationship = "is the same time as";
            //else
            //    relationship = "is later than";
        }

        public static double diferencia(DateTime fechaInicio, DateTime fechaFin)
        {            
            TimeSpan t = fechaInicio - fechaFin;
            double NrOfDays = t.TotalDays;
            return NrOfDays;
        }

        public static double diferenciaSeconds(DateTime fechaInicio, DateTime fechaFin)
        {
            TimeSpan t = fechaInicio - fechaFin;
            double NrOfSeconds = t.TotalSeconds;
            int segundos = t.Seconds;

            return NrOfSeconds;
        }

        public int compare(string fechaInicio, string fechaFin)
        {
            int respuesta = 0;
            DateTime dtInicio = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
            DateTime dtFin = DateTime.ParseExact(fechaFin, "yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));

            respuesta = DateTime.Compare(dtInicio, dtFin);

            return respuesta;
        }

        public string formatddMMyyyyToyyyyMMdd(string fecha)
        {
            string respuesta = string.Empty;
            DateTime fechaDate = DateTime.ParseExact(fecha, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
            respuesta = fechaDate.ToString("yyyy-MM-dd hh:mm:ss");

            return respuesta;
        }

        public string formatddMMyyyyToyyyyMMdd(DateTime fecha)
        {
            string respuesta = string.Empty;
            //DateTime fechaDate = DateTime.ParseExact(fecha, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
            respuesta = fecha.ToString("yyyy-MM-dd HH:mm:ss");

            return respuesta;
        }

        /// <summary>
        /// Metodo que regresa los meses entre dos rangos de fechas
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int MonthDiff(DateTime startDate, DateTime endDate)
        {
            int months = ((endDate.Year * 12) + endDate.Month) - ((startDate.Year * 12) + startDate.Month);

            //if (endDate.Day >= startDate.Day)
            //{
            //    months++;
            //}

            return months;
        }

        public string CalcularTiempo(int tsegundos)
        {
            int horas = (tsegundos / 3600);
            int minutos = ((tsegundos - horas * 3600) / 60);
            int segundos = tsegundos - (horas * 3600 + minutos * 60);

            string resultHoras = (horas < 10) ? ("0" + horas) : horas.ToString();
            string resultMin = (minutos < 10) ? ("0" + minutos) : minutos.ToString();
            string resultSeg = (segundos < 10) ? ("0" + segundos) : segundos.ToString();

            return resultHoras + ":" + resultMin + ":" + resultSeg;
            //return resultMin + ":" + resultSeg;
        }

        public string metrosKilometros(int metros)
        {
            string respuesta = string.Empty;
            double value = Math.Round((metros / 1000.0), 2, MidpointRounding.ToEven);
            return respuesta = value.ToString();
        }

        public double litros(double litros)
        {
            string respuesta = string.Empty;
            double value = Math.Round(litros, 2, MidpointRounding.ToEven);
            return value;
        }

        public string hierarchyPrincipal(string hierarchy)
        {
            string respuesta = string.Empty;
            string [] data = hierarchy.Split('-');
            respuesta = "/" + data[0] + "/";
            return respuesta;
        }

        public string getNode(string hierarchy)
        {
            string respuesta = string.Empty;
            string[] data = hierarchy.Split('-');


            if(data.Length > 1)
            {
                string x = string.Empty;
                foreach(string item in data)
                {
                    if(string.IsNullOrEmpty(x))
                    {
                        x = "/" + item;
                    }
                    else
                    {
                        x =  x + "/" + item;
                    }
                }

                respuesta = x + "/";
            }
            else
            {
                respuesta = "/" + data[0] + "/";
            }

            
            return respuesta;
        }

        public string nodePrincipal(string hierarchy)
        {
            string respuesta = string.Empty;
            string[] data = hierarchy.Split('/');
            respuesta = "/" + data[1] + "/";
            return respuesta;
        }

        public string hierarchyPrincipalToken(string hierarchy)
        {
            string respuesta = string.Empty;
            string[] data = hierarchy.Split('/');
            respuesta = "/" + data[1] + "/";
            return respuesta;
        }

        public static string NumberEmpresa(string hierarchy)
        {
            string respuesta = string.Empty;
            respuesta = hierarchy.Replace("/", "");
            return respuesta;
        }

        public static bool IsPrincipal(string hierarchy)
        {
            bool band = false;
            string[] data = hierarchy.Split('/');

            if(data.Length == 3)
            {
                band = true;
            }

            return band;
        }

        public static string CalcularTime(int segundos)
        {
            int minutosH = segundos / 60;
            int Rsegundo = segundos % 60;
            int Rhora = minutosH / 60;
            int Rminutos = minutosH % 60;
            
            return (Rhora >= 10 ? Rhora.ToString() : "0" + Rhora) + ":" + (Rminutos >= 10 ? Rminutos.ToString() : "0" + Rminutos) + ":" + (Rsegundo >= 10? Rsegundo.ToString() : "0" + Rsegundo);
        }

        public static string CalcularTime(string time)
        {
            string[] convert = time.Split(':');

            int horas = 0;
            int minutos = 0;
            int segundos = 0;
            int total = 0;

            if (!convert[0].Equals(""))
            {
                horas = 3600 * Convert.ToInt32(convert[0]);
            }
            
            if (!convert[1].Equals(""))
            {
                minutos = 60 * Convert.ToInt32(convert[1]);
            }

            if (!convert[2].Equals(""))
            {
                segundos =Convert.ToInt32(convert[2]);
            }

            total = (horas + minutos + segundos);

            return total.ToString();
        }

        public SnappedPoints.SnappedPoints SnappedAPI(string path)
        {

            string URL = "https://roads.googleapis.com/v1/snapToRoads?path=" + path + "&interpolate=true&key=AIzaSyDEw9Cw96wQgmfLqZF6nWMGXdnHVv9azd0";

            var salida = GetReleases(URL);
            /*
            JObject jObject = JObject.Parse(salida);
            //string displayName = (string)jObject.SelectToken("displayName");
            return jObject;//.SelectToken("snappedPoints");
            */

            SnappedPoints.SnappedPoints snappedPoints = JsonConvert.DeserializeObject<SnappedPoints.SnappedPoints>(salida);

            return snappedPoints;

        }

        private string GetReleases(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            //request.UserAgent = RequestConstants.UserAgentValue;
            //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }

            return content;
        }

        public static string GenerarPassword(int longitud)
        {
            string contraseña = string.Empty;
            string[] letras = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            Random EleccionAleatoria = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int LetraAleatoria = EleccionAleatoria.Next(0, 100);
                int NumeroAleatorio = EleccionAleatoria.Next(0, 9);

                if (LetraAleatoria < letras.Length)
                {
                    contraseña += letras[LetraAleatoria];
                }
                else
                {
                    contraseña += NumeroAleatorio.ToString();
                }
            }
            return contraseña;
        }

        public static string ToUTF8(string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }

        public static string GetAlarm(string code)
        {
            string labelAlarm = string.Empty;
            switch (code)
            {
                case "01":
                    labelAlarm = "Speeding";
                    break;
                case "02":
                    labelAlarm = "Low Voltage";
                    break;
                case "03":
                    labelAlarm = "High Engine Coolant Temperature";
                    break;
                case "04":
                    labelAlarm = "Hard Acceleration";
                    break;
                case "05":
                    labelAlarm = "Hard Deceleration";
                    break;
                case "06":
                    labelAlarm = "Idle Engine";
                    break;
                case "07":
                    labelAlarm = "Towing";
                    break;
                case "08":
                    labelAlarm = "High RPM";
                    break;
                case "09":
                    labelAlarm = "Power On";
                    break;
                case "0A":
                    labelAlarm = "Exhaust Emission";
                    break;
                case "0B":
                    labelAlarm = "Quick Lane Change";
                    break;
                case "0C":
                    labelAlarm = "Sharp Turn";
                    break;
                case "0D":
                    labelAlarm = "Fatigue Driving";
                    break;
                case "0E":
                    labelAlarm = "Power Off";
                    break;
                case "0F":
                    labelAlarm = "Geo-fence";
                    break;
                case "10":
                    labelAlarm = "Exhaust Emission";
                    break;
                case "11":
                    labelAlarm = "Emergency";
                    break;
                case "12":
                    labelAlarm = "Tamper";
                    break;
                case "13":
                    labelAlarm = "Illegal Enter";
                    break;
                case "14":
                    labelAlarm = "Illegal Ignition";
                    break;
                case "15":
                    labelAlarm = "OBD Communication Error";
                    break;
                case "16":
                    labelAlarm = "Ignition On";
                    break;
                case "17":
                    labelAlarm = "Ignition Off";
                    break;
                case "18":
                    labelAlarm = "MIL alarm";
                    break;
                case "19":
                    labelAlarm = "Unlock Alarm";
                    break;
                case "1A":
                    labelAlarm = "No Card Presented";
                    break;
                case "1B":
                    labelAlarm = "Dangerous Driving";
                    break;
                case "1C":
                    labelAlarm = "Vibration";
                    break;
                case "EX":
                    labelAlarm = "Speed Excess";
                    break;
                case "WT":
                    labelAlarm = "Wait Time";
                    break;
                case "ENG":
                    labelAlarm = "ENG";
                    break;
                default:
                    labelAlarm = "Unknow Alarm";
                    break;
            }
            return labelAlarm;
        }

        public static double GetDistanceDouble(GeoCoordinate point1, GeoCoordinate point2)
        {
            double distance = 0;
            double Lat = (point2.Latitude - point1.Latitude) * (Math.PI / 180);
            double Lon = (point2.Longitude - point1.Longitude) * (Math.PI / 180);
            double a = Math.Sin(Lat / 2) * Math.Sin(Lat / 2) + 
                Math.Cos(point1.Latitude * (Math.PI / 180)) * 
                Math.Cos(point2.Latitude * (Math.PI / 180)) * 
                Math.Sin(Lon / 2) * Math.Sin(Lon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            distance = EARTHRADIUS * c;
            return distance;
        }

        public static int GetDistanceInt(GeoCoordinate point1, GeoCoordinate point2)
        {
            double distance = 0;
            try
            {
                double Lat = (point2.Latitude - point1.Latitude) * (Math.PI / 180);
                double Lon = (point2.Longitude - point1.Longitude) * (Math.PI / 180);
                double a = Math.Sin(Lat / 2) * Math.Sin(Lat / 2) + Math.Cos(point1.Latitude * (Math.PI / 180)) * Math.Cos(point2.Latitude * (Math.PI / 180)) * Math.Sin(Lon / 2) * Math.Sin(Lon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                distance = EARTHRADIUS * c;
            }
            catch (Exception)
            {
                distance = -1;
            }
            
            return (int)Math.Round(distance, MidpointRounding.AwayFromZero); ;
        }

        public static double GetDistanceOther(float x1, float y1, float x2, float y2)
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            return distance;
        }


        public static int GetDiferenceDates(DateTime inicio, DateTime final)
        {
            TimeSpan duracion = final - inicio;

            int hours = duracion.Hours;
            int minutes = duracion.Minutes;
            int segundos = duracion.Seconds;

            int totalSegundos = 0;

            if (hours > 0)
            {
                totalSegundos = (hours * 60) * 60;
            }

            if (minutes > 0)
            {
                totalSegundos += (minutes * 60);
            }

            totalSegundos += segundos;

            return totalSegundos;
        }

        public static string DateFormatdddd_ddMMMMyyyy(DateTime date)
        {
            CultureInfo ci = new CultureInfo("es-MX");
            ci = new CultureInfo("es-MX");

            string dateFormat = new CultureInfo("es-MX", true).TextInfo.ToTitleCase(date.ToString("dddd, dd MMMM yyyy HH:mm", ci));

            return dateFormat;
        }



        public static DateTime DateFormatMX(DateTime date)
        {
            var dt = date.ToUniversalTime();
            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            DateTime nzDateTime = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, nzTimeZone);

            return nzDateTime;
        }

        

    }
}