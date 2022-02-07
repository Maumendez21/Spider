using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.Alarms;
using SpiderFleetWebAPI.Models.Mongo.Login;
using SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderFleetWebAPI.Utils.Main.LastPositionDevice
{
    public class LastPositionDevicesDao
    {
        public LastPositionDevicesDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private const string SLEEP = "Sleep";
        private const string GPS = "GPS";
        private MongoDBContext mongoDBContext = new MongoDBContext();

        /// <summary>
        /// Spider
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public List<LastPositionDevices> ReadLastPositionDevice(string tipo, string valor)
        {
            List<LastPositionDevices> listLastPosition = new List<LastPositionDevices>();
            LastPositionDevices position = new LastPositionDevices();
            Dictionary<string, string> map = new Dictionary<string, string>(); 
            //LastPositionDevicesResponse response = new LastPositionDevicesResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_last_position_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Convert.ToString(tipo)));
                    cmd.Parameters.Add(new SqlParameter("@Valor", Convert.ToString(valor)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Events = string.Empty;

                            position = new LastPositionDevices();
                            position.dispositivo = Convert.ToString(reader["device"]);
                            Events = Convert.ToString(reader["event"]);
                            position.nombre = Convert.ToString(reader["name"]);
                            position.fecha = Convert.ToDateTime(reader["date"]);
                            position.latitud = Convert.ToString(reader["latitude"]);
                            position.longitud = Convert.ToString(reader["longitude"]);
                            //position.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            position.empresa = Convert.ToString(reader["empresa"]);
                            
                            position.operador = string.Empty;
                            position.velocidad = string.Empty;
                            position.direccion = string.Empty;
                            position.odo = string.Empty;

                            int secs = (int)DateTime.Now.Subtract(Convert.ToDateTime(position.fecha)).TotalSeconds;

                            int horas = VerifyUser.VerifyUser.GetHours();
                            int calculo = 0;

                            if (horas == 6)
                            {
                                calculo = 21950;
                            }
                            else
                            {
                                calculo = 18350;
                            }

                            if (Events.Equals(SLEEP))
                            {
                                position.estado = false;
                            }
                            else
                            {
                                //Horario de Invierno   21950   
                                //Horario de Verano 18350
                                if (secs >= calculo)// 21950) 18350
                                { 
                                    position.estado = false; 
                                }
                                else
                                { 
                                    position.estado = true;
                                }
                            }

                            if (!map.ContainsKey(position.dispositivo))
                            {
                                map.Add(position.dispositivo, position.dispositivo);
                                listLastPosition.Add(position);
                            }
                        }
                        //response.wp = listLastPosition;
                        //response.success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //response.success = false;
                //response.messages.Add(ex.Message);
                //return response;
            }
            finally
            {
                cn.Close();
            }
            return listLastPosition;
        }

        private string coordenadas()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            string[] coordenadas = {
            "19.614123,-99.18028",
"19.201043,-96.137013",
"25.824733,-100.409017",
"19.528037,-99.08396",
"25.793072,-100.58353",
"18.656418,-91.833547",
"25.722205,-100.541558",
"23.188457,-102.874872",
"25.588075,-101.863663",
"19.529275,-99.08366",
"18.113197,-94.408683",
"19.013708,-98.172653",
"19.29957,-99.115588",
"19.451243,-99.22025",
"32.51863,-116.992607",
"24.047778,-104.620063",
"14.793897,-92.32984",
"19.570195,-99.257292",
"18.035695,-99.76394",
"19.383297,-99.268122",
"19.553133,-99.12832",
"19.471467,-99.187758",
"28.388277,-106.90232",
"20.326872,-96.871398",
"19.4714,-99.187803",
"20.207752,-101.12413",
"19.47141,-99.187895",
"25.747627,-100.311478",
"19.471675,-99.187785",
"19.351178,-99.16169",
"19.5213,-99.202998",
"18.981135,-98.195233",
"19.40784,-99.070497",
"20.654667,-101.410463",
"19.470135,-99.189638",
"19.471698,-99.1877",
"19.737658,-98.852475",
"19.471757,-99.187763",
"19.529298,-99.083577",
"16.75033,-93.075472",
"19.369812,-99.012412",
"19.471475,-99.18773",
"19.39615,-99.108473",
"18.992973,-98.276458",
"25.762158,-100.11905",
"25.82445,-100.408927",
"25.825852,-100.300117",
"25.649337,-100.359273",
"19.471495,-99.187757",
"19.050372,-98.234147",
"19.292327,-99.236553",
"25.751715,-100.38159",
"19.52752,-99.08252",
"19.471225,-99.187918",
"19.412442,-99.277667",
"20.538862,-100.48191",
"19.525622,-99.17324",
"25.764628,-100.295298",
"19.388578,-99.115578",
"19.390932,-99.058753",
"19.47167,-99.187733",
"19.566767,-99.206455",
"19.41155,-99.082485",
"27.6544799804688,-99.603401184082",
"20.709688,-103.36301",
"19.21618,-96.179218",
"27.560977935791,-99.5990600585938",
"25.6622505187988,-100.27417755127",
"19.800668,-98.948667",
"19.613425,-99.312067",
"19.471617,-99.187767",
"25.726667,-100.276872",
"25.711602,-100.313393",
"19.495535,-99.142123",
"25.711365,-100.31343",
"20.65581,-103.372163",
"19.47461,-99.186647",
"25.71119,-100.313285",
"20.121535,-98.759115",
"19.829663,-99.204922",
"19.529393,-99.083728",
"19.697617,-99.200127",
"18.992712,-98.275907",
"19.471752,-99.187813",
"25.61377,-100.263822",
"19.412343,-99.27721",
"19.01538,-98.25595",
"19.471273,-99.187842",
"19.471297,-99.187843",
"18.981625,-98.187115",
"19.04888,-98.22821",
"19.471452,-99.187698",
"19.471593,-99.187617",
"19.414568,-99.296327",
"19.08378,-98.238718",
"19.413125,-99.27628",
"19.381788,-99.030133",
"19.050092,-98.234303",
"25.711463,-100.31336",
"25.786597,-100.302862",
"19.52511,-99.087545",
"19.40926,-99.241252",
"19.471363,-99.187825",
"25.711468,-100.313337",
"25.711372,-100.31352",
"19.253047,-99.57192",
"25.78903,-100.302282",
"25.751875,-100.28843",
"19.653898,-99.206683",
"25.714485,-100.312202",
"19.326192,-98.87334",
"25.824673,-100.40907",
"19.431203,-99.226512",
"19.373148,-99.017357",
"25.714463,-100.312243",
"25.641063,-100.090005",
"19.46119,-99.173142",
"19.423623,-99.161145",
"19.529482,-99.08356",
"19.516678,-99.099733",
"19.471265,-99.187838",
"19.47175,-99.187622",
"25.768222,-100.292778",
"25.671035,-100.198912",
"25.711293,-100.313512",
"19.407483,-99.255048",
"25.70523,-100.162963",
"25.711362,-100.31334",
"19.41029,-99.25009",
"25.681567,-100.32071",
"25.6800193786621,-100.263526916504",
"25.706585,-100.350493",
"19.471357,-99.18809",
"25.71378,-100.160913",
"25.7601808,-100.4333209",
"19.471748,-99.187935",
"19.47135,-99.187897",
"19.47161,-99.187725",
"19.005783,-98.236002",
"19.387062,-99.263463",
"19.43494,-99.212215",
"18.39962,-93.215273",
"19.397582,-99.057665",
"19.47123,-99.18785",
"19.512732,-99.150527",
"19.74065,-99.198922",
"19.306167,-99.142428",
"19.431007,-99.239023",
"19.424242,-99.210418",
"19.048552,-98.22838",
"19.332765,-99.155815",
"19.01527,-98.22873",
"19.432313,-99.191065",
"19.42951,-99.21079",
"19.283302,-98.436938",
"19.37617,-98.978928",
"19.347347,-99.178825",
"25.74429,-100.38273",
"19.413965,-99.276125",
"19.06485,-98.23356",
"19.06485,-98.23356",
"25.84134,-100.41464",
"25.78138,-100.29701",
"25.74754,-100.38561",
"19.52973,-99.08334",
"20.575292,-103.363648",
"19.730012,-99.460963",
"19.709355,-98.818052",
"19.47133,-99.18793",
"25.74195,-100.25810",
"25.6710605621338,-100.199028015137",
"25.7522106170654,-100.391929626465",
"25.7115898132324,-100.313339233398",
"25.711593,-100.31341",
"25.688208,-100.235577",
"19.38940,-99.70765",
"25.714592,-100.312328",
"25.6800308227539,-100.263542175293",
"25.6799907684326,-100.263511657715",
"25.6800098419189,-100.263496398926",
"19.07322,-98.21420",
"19.10375,-98.27489",
"22.16293,-100.94532",
"25.6799392700195,-100.263519287109",
"19.40708,-99.08024",
"25.685788,-100.321053",
"19.52755,-99.08271",
"25.6802269,-100.2615917",
"19.043407,-98.163998",
"19.28810,-99.68806",
"25.6801885,-100.2618655",
"19.73774,-98.85257",
"25.6803069,-100.2618986",
"25.658028,-100.223103",
"19.471308,-99.187772",
"27.654972076416,-99.6043167114258",
"19.529707,-99.083492",
"19.296053,-99.132537",
"19.527562,-99.082643",
"19.103823,-98.27486",
"19.383222,-99.27466",
"19.257665,-98.152133",
"19.541307,-99.057315",
"20.244353,-99.050993",
"24.767697,-107.698315",
"18.998925,-98.250007",
"19.216183,-96.179155",
"19.304868,-99.169965",
"19.494698,-99.236507",
"19.376757,-99.266122",
"19.52756,-99.08258",
"19.398943,-99.281975",
"24.839618,-107.428168",
"19.664568,-98.871718",
"25.76002,-100.42434",
"18.01158,-99.748633",
"19.52758,-99.08263",
"19.47165,-99.187635",
"25.711397,-100.313357",
"25.7118396759033,-100.313179016113",
"25.7115802764893,-100.313438415527",
"25.7003593444824,-100.27758026123",
"25.7128391265869,-100.349891662598",
"25.6711006164551,-100.318176269531",
"20.58297,-100.40169",
"25.7115802764893,-100.313346862793",
"25.7043895721436,-100.331588745117",
"19.59277,-99.24815",
"25.7054691314697,-100.169372558594",
"25.7680797576904,-100.416053771973",
"25.7115802764893,-100.313377380371",
"0,1.23119335906918E-11",
"25.7027893066406,-100.23754119873",
"25.7521896362305,-100.391899108887",
"25.738920211792,-100.378059387207",
"20.58295,-100.40158",
"25.6709594726563,-100.199089050293",
"25.7130298614502,-100.350059509277",
"25.723413,-100.21639",
"19.427482,-99.349967",
"25.781372,-100.460628",
"19.062155,-98.120668",
"19.52745,-99.08247",
"21.052532,-86.782742",
"19.40078,-99.280938",
"22.10837,-102.27632",
"24.8305,-107.41023",
"19.50337,-99.262095",
"31.72881,-106.43818",
"18.11333,-94.408612",
"19.471698,-99.187742",
"19.362105,-99.144028",
"19.471555,-99.187725",
"19.471443,-99.187938",
"18.113357,-94.408663",
"18.07377,-92.935237",
"19.429802,-99.211043",
"19.277623,-99.704578",
"20.96924,-89.58067",
"19.43296,-99.224083",
"19.64691,-99.21405",
"19.423973,-99.21009",
"19.471248,-99.187578",
"19.200653,-96.137635",
"19.424012,-99.210055",
"25.80796,-100.19541",
"21.20991,-101.68345",
"19.363308,-99.219553",
"19.70788,-98.83079",
"25.825995,-100.403318",
"28.397713,-106.862913",
"19.529425,-99.083798",
"19.53089,-99.088348",
"19.47140,-99.18778",
"19.438153,-99.202562",
"25.77784,-100.18553",
"19.409832,-99.076722",
"19.471828,-99.187702",
"25.766478,-100.218308",
"25.711508,-100.313348",
"19.049988,-98.234352",
"25.6806063,-100.261841",
"25.6803354,-100.2618442",
"19.457838,-99.200328",
"19.424418,-99.21015",
"19.08314,-104.28784",
"19.52555,-99.176603",
"24.746922,-107.698565",
"24.068082,-104.603962",
"19.536167,-99.06202",
"18.113272,-94.408625",
"19.529447,-99.083833",
"25.739792,-100.376792",
"19.033997,-98.191358",
"19.071048,-98.218387",
"18.981847,-98.186472",
"19.402338,-99.281932",
"19.52917,-99.083115",
"19.471745,-99.187873",
"19.471212,-99.18772",
"19.01556,-98.22833",
"20.621712,-103.407322",
"19.83157,-99.095727",
"19.737555,-98.852492",
"18.97346,-98.20665",
"19.471958,-99.187725",
"19.39783,-99.29229",
"19.320142,-98.881753",
"22.189863,-101.006012",
"21.03279,-89.627378",
"19.080153,-97.966188",
"25.711235,-100.313263",
"25.663017,-100.158582",
"25.71142,-100.313505",
"25.7115592956543,-100.313522338867",
"25.713847,-100.30159",
"25.779163,-100.331858",
"18.981727,-98.186432",
"19.370172,-99.249602",
"19.430472,-99.211357",
"19.47138,-99.187813",
"19.389597,-99.221685",
"19.365687,-99.175398",
"19.47106,-99.187667",
"19.05038,-98.234325",
"25.711413,-100.313535",
"19.471303,-99.18778",
"19.37401,-99.265947",
"19.321847,-99.208587",
"19.412343,-99.277187",
"18.986413,-98.18719",
"19.529358,-99.08371",
"19.527545,-99.082693",
"20.287043,-102.550657",
"25.748978,-100.362477",
"19.47197,-99.178087",
"19.52949,-99.083503",
"19.411782,-99.277235",
"25.80802,-100.195253",
"19.06485,-98.23356",
"19.06485,-98.23356",
"25.74785,-100.29927",
"25.762162,-100.119148",
"25.711472,-100.313355",
"25.711498,-100.313358",
"25.68466,-100.318097",
"25.711357,-100.313385",
"19.0955488,-98.2646938",
"19.389612,-99.736837",
"20.9435901641846,-101.375259399414",
"25.6754608154297,-100.39688873291",
"25.7096405029297,-100.242462158203",
"25.817042,-100.212978",
"25.7100582122803,-100.241767883301",
"27.5966663360596,-99.5317459106445",
"19.2941971,-99.6211697",
"19.003212,-98.195588"
            };

            string gen = coordenadas[rnd.Next(0, 356)];
            return gen;
        }

        private string nombres ()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            string[] nombres = {
                "71",
"R04BAW-04",
"MPC-078 Everardo Rodruiguez Carranza",
"NSB7275 CONTROL VEHICULAR",
"SF-530",
"TF-0799-F",
"SF 423",
"FZP-894-B",
"ZHT-420-B",
"LB92720 CV CARLOS DURAN",
"XM-4398-A",
"455",
"12F-765",
"76",
"LA74364 CEDIS Tijuana",
"FZP-893-B",
"DLA-010-B",
"NEW8898",
"NW91039",
"67",
"83",
"108",
"ENM-776-1",
"GKS148B",
"HONDA",
"GKS-146-B",
"72",
"Reclu Hyundai",
"105",
"577",
"116",
"F97PX",
"84",
"GLF-760-B",
"57",
"65",
"LB46226 PLANTA DINA",
"120",
"LE21677 CV LUIS ALBERTO",
"YEV985A",
"82_Nuevo",
"123",
"131",
"SF-444",
"SF-00584",
"CPC-054   Jose Villanueva Vazquez",
"AS Trucking-62",
"PRUEBA JCG",
"80",
"H - 100",
"124",
"Rafael Rodriguez -13",
"NAU2759 ASESORIA TECNICA",
"51",
"55",
"LB36211 CEDIS QUERETARO",
"TOYOTA",
"ENM7321",
"132",
"130",
"129",
"TSuru",
"122",
"MCR-01",
"JRJ-821-1",
"LB00660 CEDIS VERACRUZ",
"EKC01",
"EKR01",
"126_Nuevo",
"UMJ448B",
"113_Nuevo",
"A 0139",
"AETV19-124",
"283",
"AETV20-125",
"LB00653 CEDIS GUADALAJARA",
"SF-277",
"AETV09-114",
"LB14642 CEDIS PACHUCA",
"3N6AD35AXLK841250",
"KY77763 CV FABIAN HDZ",
"53",
"Bora VW",
"93",
"Amaro Flores 2",
"VW Camioneta",
"March",
"69_Nuevo",
"85",
"LB92759 EDUARDO GARRIDO",
"563",
"127",
"114_Nuevo",
"12F - 763",
"NP-2016",
"Mazda",
"81",
"NP-2013",
"AETV17-122",
"AETV32-137",
"G24AWS ALEJANDRO HDZ",
"109",
"102_Nuevo",
"AETV49-206",
"AETV14-119",
"59",
"AETV23-128",
"AETV29-134",
"12F - 766",
"AETV50-207",
"82",
"MPC-079   Edgar Najera Silva",
"103_Nuevo",
"119_Nuevo",
"AETV10-115",
"AETV30-135",
"77_Nuevo",
"96_Nuevo",
"NSB7275 CONTROL VEHICULAR_Nuevo",
"94_Nuevo",
"84",
"100",
"AETV18-123",
"AETV51-208",
"AETV13-118",
"66_Nuevo",
"AETV21-126",
"AETV33-138",
"110_Nuevo",
"AETV08",
"208",
"18609-201",
"79",
"AETV35",
"PHONE JC",
"95",
"111_Nuevo",
"106",
"479",
"97",
"98_Nuevo",
"XM-4399-A",
"58",
"92_Nuevo",
"S14-APW",
"99_Nuevo",
"90",
"104",
"R55-APV",
"76",
"Z63-AVB",
"Eurovan",
"101",
"V49-AUM",
"18075",
"NSZ3865 CREDITO_Nuevo",
"12F - 765_Nuevo",
"A94",
"Jetta",
"Attitude",
"Attitude",
"P 0013",
"C 0100",
"C 282",
"MKW1539 IMEX CARLOS ",
"JRJ-794-0_Nuevo",
"B15BFS",
"3C7WRAHT4LG296258",
"51",
"C 148",
"AETV43-M06",
"AETV60-M14",
"AETV59-M13",
"AETV47-204",
"AETV06",
"LB14628",
"15678",
"201",
"205",
"230",
"SF 180",
"LC36510 CEDIS PUEBLA",
"Reclu - Pto Interior",
"206",
"LC70148 ALEJANDRO ZINZU",
"276",
"LC23601 CV ",
"PHONE ERICK SOLIS",
"SF-00633",
"KW14615",
"PHONE OSKAR ORNELAS",
"LD47605 PLANTA EMILIO",
"PHONE EDER RDZ",
"AET-120",
"75_Nuevo",
"EKC02",
"LA92753 CV JUAN CARLOS",
"61",
"LB72759 CEDIS VIA MORELOS",
"NSZ3875 CEDIS PUEBLA",
"Toyota",
"F96PX",
"Ines Movil",
"VM-74-190",
"LSGHD52H5JD145282",
"NO USAR MA",
"LB00660 CEDIS VERACRUZ",
"08F - 141",
"UMJ448B",
"L42 - AYH",
"LD47600 CV (NO TIENE)",
"60",
"VTB-998-A",
"NUT5189 IMEX ",
"Hyundai",
"Nuevo",
"LB72758 CV IVAN MORALES",
"121",
"AETV22-127",
"AETV36-M01",
"AETV44-M07",
"AETV41-M08",
"AETV54-M17",
"AETV61-M15",
"Reclu Leon II",
"AETV45-M02",
"AETV57-M16",
"LC38331 CEDIS ATIZAPAN",
"AETV53-M19",
"AETV55-M11",
"AETV42-M04",
"AETV56-M12",
"AETV38-M03",
"AETV58-M20",
"AETV62-M18",
"Reclu Queretaro",
"AETV39-M09",
"AETV40-M10",
"AETV34",
"107_Nuevo",
"18608-202",
"Jannet jmz",
"LB52650 CEDIS AV CENTRAL",
"600",
"80_Nuevo",
"LB80936 CEDIS LEON",
"VTB-143-A",
"112",
"Reclu Mochis - Walbro",
"XP-1248-A",
"91_Nuevo",
"14F - 731",
"01_Nuevo",
"78_Nuevo",
"XM-4401-A",
"KZ07912 (Eduardo)",
"12F - 768",
"T96-APW",
"YYN-300-B",
"86",
"Reclu SLP",
"VTB-999-A",
"63",
"YEV-833-A",
"ZHT-280-B",
"CUC-038 J Refugio Medellin Davila",
"Reclu Leon",
"K31 - BCX",
"LD29645 TRADE POLYMERS",
"CPC-055 Saul Rodriguez Cardenas",
"ENM-776-2",
"NCU2315 TRADE POLYMERS",
"Nissa 2018",
"111",
"54",
"Reclu Lenovo ",
"65_Nuevo",
"125",
"AETV48-205",
"AETV25-130",
"NP2009",
"PHONE TOMAS MIRELES",
"PHONE AARON MIRANDA",
"87",
"NVR-1419",
"JU-95-586",
"ENM7397",
"VTB-145-A_Nuevo",
"HF-25-398",
"LD18950 CEDIS COACALCO",
"XM-4402-A",
"MJN7447 ASESORIA TECNICA CRISTIAN SALINAS",
"CM08",
"551",
"594",
"Endeavor",
"78_Nuevo",
"NSZ3877 CV MECANICOS",
"74_Nuevo",
"117",
"Np300",
"JRJ-821-0",
"118_Nuevo",
"LD47448 PLANTA PITUFO",
"GLI",
"128",
"111",
"LD76520 IXTAPALUCA",
"G99 -ANC",
"G56AWS CEDIS MERIDA",
"SF-0530",
"AETV07-112",
"AETV31-136",
"AETV11-116",
"AETV37-M05",
"VAN ",
"18610-109",
"SF-00639",
"75",
"F13-AMA",
"64_Nuevo",
"15F - 082",
"K83 - BCX",
"70",
"SF 167",
"SFK 000 015",
"78",
"T60 - AVS",
"U24-AXR",
"Chofer",
"LB92759 EDUARDO GARRIDO_Nuevo",
"LB92759 EDUARDO GARRIDO_Nuevo",
"3C7WRAHT2LG296274",
"AC-1673-B",
"AETV12-117",
"MDZ3998",
"NSZ3865 CREDITO ",
"Pilot",
"MPC-074   Jose Pablo Diaz Segura",
"Attitude",
"Attitude",
"P 0052",
"SF-00590 Hector",
"AETV24-129",
"AETV16-121",
"AETV26-131",
"AETV27-132",
"Memo Test",
"NVR-13-88",
"UPHZ161742",
"Equipo Portatil",
"MC01",
"AETV46-203",
"EKCR01",
"6182",
"TAG",
"20043"
            };
            
            string gen = nombres[rnd.Next(0, 356)];
            return gen;
        }

        private int status()
        {
            Random rnd = new Random();
            int numero = rnd.Next(7);
            return numero;
        }

        private string Dispositivo()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int numero = rnd.Next(50000);
            return numero.ToString();
        }

        public LastPositionDeviceResponse ReadDemo(string tipo, string busqueda)
        {
            LastPositionDeviceResponse response = new LastPositionDeviceResponse();
            List<CurrentPositionDevice> listLastPosition = new List<CurrentPositionDevice>();
            CurrentPositionDevice position = new CurrentPositionDevice();
            Dictionary<string, string> map = new Dictionary<string, string>();

            try
            {
                //1. Activo
                //2. Inactivo
                //3. Warning
                //4. Falla
                //5. Activo sin Movimiento
                //6. Paro de Motor
                //7. Panico
                //8. Desconexion  E0

                List<CurrentPositionDevice> currentPositionDevices = new List<CurrentPositionDevice>
            {
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "1MU-GI", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1, latitud = "19.201043", longitud = "-96.137013" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "60H-HB", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.824733", longitud = "-100.409017" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ERP-6K", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.528037", longitud = "-99.08396" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "QFF-32", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.793072", longitud = "-100.58353" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "0FH-L8", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.656502", longitud = "-91.833615" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "P5U-VY", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.722205", longitud = "-100.541558" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "F0R-LS", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "24.284357", longitud = "-103.404753" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "76M-0E", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.57771", longitud = "-103.56398" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "W4U-UV", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.529275", longitud = "-99.08366" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "KCZ-13", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.113295", longitud = "-94.408643" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "M6K-5I", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.013708", longitud = "-98.172653" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "WFJ-I2", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.29957", longitud = "-99.115588" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "AFW-S5", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.451162", longitud = "-99.220362" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "NAE-ON", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "32.51863", longitud = "-116.992607" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "U2P-ZG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "24.047745", longitud = "-104.620098" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5UW-ZY", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "14.793887", longitud = "-92.329878" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MF3-1L", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.529432", longitud = "-99.08371" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IQW-C6", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.250825", longitud = "-99.661665" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "1XE-7D", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.47134", longitud = "-99.18787" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "QIC-YI", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.553218", longitud = "-99.128372" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "K8L-NJ", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.47143", longitud = "-99.187725" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "URU-YN", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "28.388277", longitud = "-106.90232" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BTT-UP", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.343005", longitud = "-96.8885" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5M4-GX", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.4714", longitud = "-99.187803" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6OB-SV", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.212147", longitud = "-101.136793" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7XS-VZ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.435683", longitud = "-99.192202" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "WS7-RN", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.747627", longitud = "-100.311478" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "O3P-S1", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "37.85591958089602", longitud = "-92.0363584725735" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "157-Z4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.351178", longitud = "-99.16169" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IEU-RH", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.47173", longitud = "-99.187793" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DO5-BL", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.981135", longitud = "-98.195233" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "673-2M", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.43483", longitud = "-99.21127" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "798-80", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.654692", longitud = "-101.41045" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "SVB-GB", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.471282", longitud = "-99.18772" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "B6L-P6", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.471698", longitud = "-99.1877" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BRL-XN", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.3143394321992", longitud = "-91.23426937959681" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "2CA-NW", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.751413", longitud = "-98.972978" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "HW6-GR", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.529298", longitud = "-99.083577" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "QAX-F4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "16.765447", longitud = "-93.095272" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "168-9L", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.434617", longitud = "-99.151992" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "WCO-PG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.43277", longitud = "-99.23643" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "GCQ-FJ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.470445", longitud = "-99.189393" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "PVG-OT", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.992973", longitud = "-98.276458" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "48V-MS", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.762158", longitud = "-100.11905" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6Z4-ZV", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.82445", longitud = "-100.408927" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BJG-UE", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.825852", longitud = "-100.300117" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ESM-L0", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.649337", longitud = "-100.359273" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZXU-HP", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471495", longitud = "-99.187757" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FG0-7C", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.050372", longitud = "-98.234147" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "44I-W9", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.400087", longitud = "-99.23509" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "2LV-QP", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.751715", longitud = "-100.38159" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7IZ-4E", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.52766", longitud = "-99.082448" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "765-AE", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.58146", longitud = "-99.264257" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6J4-Y3", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.413653", longitud = "-99.278445" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "O8X-D0", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.446107", longitud = "-99.191602" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8LC-D7", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "27.65447998", longitud = "-99.60340118" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7MK-ZL", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "20.709792", longitud = "-103.362963" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8N8-5M", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.21618", longitud = "-96.179218" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IXK-PE", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "26.07522011", longitud = "-100.1760788" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OJE-RE", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.66225052", longitud = "-100.2741776" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "HP1-26", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.461515", longitud = "-99.190123" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5J2-F3", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.613452", longitud = "-99.312088" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "G5W-FT", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471507", longitud = "-99.187792" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5AA-RG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.726667", longitud = "-100.276872" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IAY-QT", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711468", longitud = "-100.313385" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "1XH-8L", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.495535", longitud = "-99.142123" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZGV-9Z", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711457", longitud = "-100.313468" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "RVS-2P", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "20.65583", longitud = "-103.3722" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8WC-L0", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711532", longitud = "-100.31339" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "D01-SA", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.76443", longitud = "-100.376547" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XP2-9N", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.52511", longitud = "-99.087545" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZP6-78", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.409253", longitud = "-99.241235" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "SR2-NZ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711552", longitud = "-100.313275" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YBL-K5", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711347", longitud = "-100.313553" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JHM-OA", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.253195", longitud = "-99.571617" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YBA-ZN", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.778245", longitud = "-100.180557" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BFS-O4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.751923", longitud = "-100.288267" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "GOJ-LX", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.666688", longitud = "-99.222048" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "0HG-GH", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.764467", longitud = "-100.376607" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "V7O-2L", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.326192", longitud = "-98.87334" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "WER-DE", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.824673", longitud = "-100.40907" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "SDS-7K", Hierarchy = "/83/", statusEvent = 3, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471897", longitud = "-99.187798" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4HZ-RE", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.438943", longitud = "-99.213435" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "N8O-TS", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711543", longitud = "-100.313408" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "K1A-XF", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.641063", longitud = "-100.090005" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CAG-GP", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471313", longitud = "-99.187953" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "0XH-O1", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471745", longitud = "-99.187568" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7XP-KM", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.529482", longitud = "-99.08356" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "D5M-A2", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.405643", longitud = "-99.228125" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TRZ-C3", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471265", longitud = "-99.187838" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XT4-JA", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471773", longitud = "-99.18775" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4D4-8B", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.768247", longitud = "-100.292783" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "S7L-RT", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.671138", longitud = "-100.198987" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "G9L-ZH", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711355", longitud = "-100.313538" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "0SD-HP", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471378", longitud = "-99.187937" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OO9-4V", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.70523", longitud = "-100.162963" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "UKE-N1", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.711417", longitud = "-100.313383" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "33R-I3", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.681748", longitud = "-100.320777" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FU4-RW", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.68001938", longitud = "-100.2635269" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TG9-4D", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.70664", longitud = "-100.350425" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "PD9-MR", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.713837", longitud = "-100.160995" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "H28-0N", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.7601808", longitud = "-100.4333209" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "NC7-2N", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471745", longitud = "-99.187852" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6B5-NJ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471223", longitud = "-99.187975" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4US-HB", Hierarchy = "/83/", statusEvent = 3, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "35.19217483240336", longitud = "-101.99885383646982" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OLJ-JX", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.005747", longitud = "-98.23597" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "Z3E-1W", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471755", longitud = "-99.187823" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "NVE-DI", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "18.093907", longitud = "-93.16429" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "RJG-RG", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.451185", longitud = "-99.190438" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CUK-U0", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471345", longitud = "-99.187835" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5V1-LC", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "26.597627390682387", longitud = "-104.1338572192629" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5TY-CW", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.402732", longitud = "-99.238212" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "I8F-LX", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.30091", longitud = "-99.057623" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "D6D-06", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.471593", longitud = "-99.18782" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IPO-9T", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.277972", longitud = "-99.560115" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FE1-DU", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.048552", longitud = "-98.22838" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "F4T-AV", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.332722", longitud = "-99.155843" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "EB3-Y0", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.01527", longitud = "-98.22873" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "37C-Q9", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.355238", longitud = "-99.260867" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TD4-3D", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.42951", longitud = "-99.21079" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "GQT-XS", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.283302", longitud = "-98.436938" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "PG2-17", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.37617", longitud = "-98.978928" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "11V-RQ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.3635", longitud = "-99.180787" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OPE-ZT", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.74429", longitud = "-100.38273" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BDO-AD", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.413347", longitud = "-99.274867" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "LN8-9A", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.06485", longitud = "-98.23356" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BJ8-AO", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.06485", longitud = "-98.23356" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YK6-SF", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.84134", longitud = "-100.41464" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7JB-19", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.78138", longitud = "-100.29701" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CLU-AV", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "25.74754", longitud = "-100.38561" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "E8E-KB", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 4,latitud = "19.52973", longitud = "-99.08334" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CBT-UG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "20.575385", longitud = "-103.363655" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CI3-15", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.729875", longitud = "-99.461123" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "R3V-C1", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.709407", longitud = "-98.818103" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DSV-TV", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.47133", longitud = "-99.18793" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "1BX-BB", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.74195", longitud = "-100.2581" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "928-N6", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.67106056", longitud = "-100.199028" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DFY-8L", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.76421928", longitud = "-100.3756714" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "W1L-MD", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71158981", longitud = "-100.3133392" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZQK-5R", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.668057", longitud = "-100.319658" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "I0R-IR", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.817013", longitud = "-100.213073" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "VMU-YJ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.3894", longitud = "-99.70765" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BOS-TG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.714592", longitud = "-100.312328" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JKP-42", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.68003082", longitud = "-100.2635422" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MFW-1E", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.67999077", longitud = "-100.2635117" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MHJ-Q6", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.68000984", longitud = "-100.2634964" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "L02-T2", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.07322", longitud = "-98.2142" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ILZ-6L", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.10375", longitud = "-98.27489" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "1U9-YK", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "22.16293", longitud = "-100.94532" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "II2-F4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.67993927", longitud = "-100.2635193" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OAP-DF", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.40708", longitud = "-99.08024" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CD0-DE", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.686267", longitud = "-100.321572" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OCE-18", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.52754", longitud = "-99.08255" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "KW9-07", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.6802269", longitud = "-100.2615917" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8UT-TB", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.043407", longitud = "-98.163998" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "2F8-R6", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.2881", longitud = "-99.68806" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DOD-WR", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.6801885", longitud = "-100.2618655" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "KKY-NZ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.73774", longitud = "-98.85257" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FUW-16", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.6803069", longitud = "-100.2618986" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YPI-09", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.648487", longitud = "-100.067472" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "WJC-H5", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.405635", longitud = "-99.253015" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4R5-CN", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "27.65497208", longitud = "-99.60431671" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7FJ-GB", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.529593", longitud = "-99.083738" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "99S-KX", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.363437", longitud = "-99.180657" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "97V-B2", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.527563", longitud = "-99.082682" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "N71-WN", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.103682", longitud = "-98.274815" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "M7I-RJ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.375207", longitud = "-99.295797" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4M5-0X", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.257665", longitud = "-98.152133" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "3PV-92", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.541307", longitud = "-99.057315" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "9LQ-Z7", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "20.244353", longitud = "-99.050993" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "Z77-D9", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "24.767697", longitud = "-107.698315" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YXB-PS", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "18.998925", longitud = "-98.250007" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6ZJ-ZB", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.216183", longitud = "-96.179155" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "971-7H", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.304893", longitud = "-99.170032" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XD1-W4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.494698", longitud = "-99.236507" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ULW-I2", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.376857", longitud = "-99.26625" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BRB-ZY", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.52756", longitud = "-99.08258" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "P9F-SU", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.398943", longitud = "-99.281975" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "GLQ-I0", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "24.839635", longitud = "-107.428115" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DF6-82", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.664592", longitud = "-98.871847" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CUS-SJ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.76002", longitud = "-100.42434" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "VFL-JO", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "18.035752", longitud = "-99.764003" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "NB2-WQ", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.52756", longitud = "-99.08258" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "92I-MA", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.711358", longitud = "-100.313388" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MWT-60", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71183968", longitud = "-100.313179" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OPX-Q0", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71158028", longitud = "-100.3134384" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "Q8T-QC", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.70465088", longitud = "-100.1668091" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JY6-EH", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71299934", longitud = "-100.3498917" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FS4-1L", Hierarchy = "/83/", statusEvent = 3, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.68358994", longitud = "-100.3171921" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "PW5-M2", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "20.58297", longitud = "-100.40169" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "KMG-R7", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71158028", longitud = "-100.3133469" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "9VO-S9", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.76408958", longitud = "-100.3751068" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7NM-69", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.59277", longitud = "-99.24815" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "QED-9M", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.76288986", longitud = "-100.372673" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "L6E-E5", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71430969", longitud = "-100.3497314" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "322-VL", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71158028", longitud = "-100.3133774" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "2UL-XP", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "28.413790301412458", longitud = "-107.46345317439524" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "E2B-TC", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.70656967", longitud = "-100.165947" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YJN-EK", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.76276016", longitud = "-100.3723297" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "1G7-14", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.7641201", longitud = "-100.3751907" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "KT5-AR", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "20.58295", longitud = "-100.40158" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OHC-VD", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.67095947", longitud = "-100.1990891" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "H23-GH", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.71302986", longitud = "-100.3500595" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "NCY-Q5", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.711322", longitud = "-100.313457" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YDT-6Y", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.472883", longitud = "-99.198803" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "BSA-4E", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "25.788363", longitud = "-100.584985" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "W4B-0B", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.062155", longitud = "-98.120668" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IYK-GB", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.52745", longitud = "-99.08247" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6ZP-P5", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "21.052532", longitud = "-86.782742" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5QH-76", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.471307", longitud = "-99.18795" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YSR-5G", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "22.10837", longitud = "-102.27632" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "G98-MQ", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "24.830357", longitud = "-107.41034" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5HI-QQ", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.503413", longitud = "-99.262135" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "M1C-E8", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "31.72881", longitud = "-106.43818" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "Z2S-P8", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "17.983053", longitud = "-93.013155" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8XE-B2", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.471697", longitud = "-99.187757" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "UKC-7Q", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "21.03538544099496", longitud = "-87.10087884406558" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ENF-U4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 5,latitud = "19.471797", longitud = "-99.187475" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JZ8-0F", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.43223", longitud = "-99.283078" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZR9-8T", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.131448", longitud = "-94.371717" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DNC-8G", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.047023", longitud = "-94.081385" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "69B-7H", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.429802", longitud = "-99.211043" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "T57-T5", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.277623", longitud = "-99.704578" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "VEK-AM", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.969363", longitud = "-89.580622" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8WS-KY", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.471247", longitud = "-99.187693" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "932-JJ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.64691", longitud = "-99.21405" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "RYF-SZ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.423973", longitud = "-99.21009" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "HIE-1T", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.42879", longitud = "-99.19432" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ELI-ML", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.207572", longitud = "-96.138753" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "O1O-4U", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.423952", longitud = "-99.210145" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "UH8-WU", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.80795", longitud = "-100.19535" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "R42-JH", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "21.20991", longitud = "-101.68345" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CH2-Y0", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.363242", longitud = "-99.219577" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5ZP-4X", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.70788", longitud = "-98.83079" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MAT-RH", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.825995", longitud = "-100.403318" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "WOG-9K", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "28.39775", longitud = "-106.862927" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "IP7-ZZ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.529425", longitud = "-99.083798" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "J0T-7H", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.53089", longitud = "-99.088348" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MP4-TQ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.4714", longitud = "-99.18778" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7TU-WP", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.438153", longitud = "-99.202562" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "YLU-HH", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.77784", longitud = "-100.18553" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "DY3-98", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.473298", longitud = "-99.200975" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XJC-8R", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.711475", longitud = "-100.31326" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FFR-WL", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.711518", longitud = "-100.313358" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6G9-AK", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.062775", longitud = "-98.191752" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "C3U-TV", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.6806063", longitud = "-100.261841" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "640-RF", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.6803354", longitud = "-100.2618442" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "UJF-AE", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.471333", longitud = "-99.187957" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XRV-EK", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.08304", longitud = "-104.2878" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "S48-6K", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.525595", longitud = "-99.176602" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MZQ-D4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "24.746828", longitud = "-107.698642" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JEK-NC", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "24.068033", longitud = "-104.60389" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "3TV-36", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.536167", longitud = "-99.06202" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5AR-SW", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.113263", longitud = "-94.408703" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "KMN-HW", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.529447", longitud = "-99.083833" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "VIP-IC", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.739792", longitud = "-100.376792" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "C54-G3", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.071183", longitud = "-98.21853" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "53B-F4", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "16.166661", longitud = "-97.116791" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TMR-S9", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.981787", longitud = "-98.186485" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "E51-IO", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.402338", longitud = "-99.281932" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "SMQ-JA", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.52917", longitud = "-99.083232" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ALJ-QI", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.471745", longitud = "-99.187873" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "7YP-AB", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.01556", longitud = "-98.22833" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "C8M-XD", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.621717", longitud = "-103.407308" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "855-HS", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.799807", longitud = "-99.10112" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "0QG-3U", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.737555", longitud = "-98.852492" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZPV-F2", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.973433", longitud = "-98.206725" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TUU-64", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.380607", longitud = "-99.278233" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4KG-SC", Hierarchy = "/83/", statusEvent = 2, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.39783", longitud = "-99.29229" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "PQ1-HM", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.319985", longitud = "-98.88161" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TA3-GC", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "22.189863", longitud = "-101.006012" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FQB-8S", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "21.03279", longitud = "-89.627378" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "TSH-0K", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.080153", longitud = "-97.966188" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "SDJ-HR", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.711318", longitud = "-100.313372" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "SNP-3I", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.648607", longitud = "-100.067608" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XYM-FC", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.71142", longitud = "-100.313462" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JY9-RB", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.7115593", longitud = "-100.3135223" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "ZWP-DW", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.71385", longitud = "-100.301583" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CSW-OK", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.789702", longitud = "-100.581045" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MDP-IT", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.981727", longitud = "-98.186432" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "LHU-Z4", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.370172", longitud = "-99.249602" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FU9-7O", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.430472", longitud = "-99.211357" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "QD6-6I", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.436175", longitud = "-99.200627" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "4S0-ZU", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.389757", longitud = "-99.221855" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "5YF-UP", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "32.242256", longitud = "-83.745161" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "UJE-V0", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.05038", longitud = "-98.234325" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CEK-X5", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.711413", longitud = "-100.313535" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "FWR-YG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.471303", longitud = "-99.18778" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "8V0-FP", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.37401", longitud = "-99.265947" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "3NZ-XX", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.321847", longitud = "-99.208587" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XC9-K4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.412343", longitud = "-99.277187" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "NTQ-B4", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "18.986413", longitud = "-98.18719" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "H9L-FG", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.529358", longitud = "-99.08371" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "0IC-3P", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.527687", longitud = "-99.08251" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JHQ-OQ", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.314107", longitud = "-102.516692" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OB4-DC", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.749057", longitud = "-100.362462" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "GTR-7D", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.47234", longitud = "-99.178253" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "AE9-3R", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.52949", longitud = "-99.083503" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "VGQ-39", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.399152", longitud = "-99.275568" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "9JU-9Z", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.80802", longitud = "-100.195253" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "UK6-IO", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.06485", longitud = "-98.23356" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "PVV-4P", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.06485", longitud = "-98.23356" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "MYM-8X", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.74785", longitud = "-100.29927" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "XSO-BG", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.762162", longitud = "-100.119148" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "VIJ-M3", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "32.079035", longitud = "-97.790767" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "D0W-OQ", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.711615", longitud = "-100.313425" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "EQ5-OS", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.685008", longitud = "-100.316912" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "6XI-SX", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "33.648017", longitud = "-101.397225" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "OSF-NF", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.0955488", longitud = "-98.2646938" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "63S-GL", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.311363", longitud = "-99.730218" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "R5T-3D", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "20.94359016", longitud = "-101.3752594" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "LBU-PL", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "31.813233", longitud = "-115.581857" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "Y9G-OK", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.7096405", longitud = "-100.2424622" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "538-Q1", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.81707", longitud = "-100.212877" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "9J6-N1", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "25.71005821", longitud = "-100.2417679" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "CD5-G6", Hierarchy = "/83/", statusEvent = 1, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "27.59181976", longitud = "-99.54445648" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "JIQ-D8", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.2941971", longitud = "-99.6211697" },
new CurrentPositionDevice() {dispositivo = "213WP2019020144", direccion = "", empresa = "83", estado = false, Events = "GPS", fecha = new DateTime(), nombre = "J7C-KG", Hierarchy = "/83/", statusEvent = 5, odo ="", nombreEmpresa="DemoExhibicion", operador = "", velocidad = "", typeDevice = 1,latitud = "19.003212", longitud = "-98.195588" }

            };
                response.ListLastPosition = currentPositionDevices;
                response.success = true;
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

        #region Nuevo Spider
        /// <summary>
        /// Nuevo Spider
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public LastPositionDeviceResponse ReadCurrentPositionDevices(string tipo, string valor,string busqueda)
        {
            LastPositionDeviceResponse response = new LastPositionDeviceResponse();
            List<CurrentPositionDevice> listLastPosition = new List<CurrentPositionDevice>();
            CurrentPositionDevice position = new CurrentPositionDevice();
            Dictionary<string, string> map = new Dictionary<string, string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    //SqlCommand cmd = new SqlCommand("ad.sp_consult_list_last_position_device", cn);
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_last_position_device_spider", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Convert.ToString(tipo)));
                    cmd.Parameters.Add(new SqlParameter("@Valor", Convert.ToString(valor)));
                    //cmd.Parameters.Add(new SqlParameter("@search", Convert.ToString(busqueda)));
                    if (!string.IsNullOrEmpty(busqueda)) { cmd.Parameters.Add(new SqlParameter("@search", Convert.ToString(busqueda))); }
                    else { cmd.Parameters.Add(new SqlParameter("@search", "")); }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Events = string.Empty;

                            position = new CurrentPositionDevice();
                            position.dispositivo = Convert.ToString(reader["device"]);
                            position.Events = Convert.ToString(reader["event"]);
                            position.nombre = Convert.ToString(reader["name"]);
                            position.fecha = Convert.ToDateTime(reader["date"]);
                            position.latitud = Convert.ToString(reader["latitude"]);
                            position.longitud = Convert.ToString(reader["longitude"]);
                            position.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            position.empresa = Convert.ToString(reader["empresa"]);
                            position.nombreEmpresa = Convert.ToString(reader["nombre_empresa"]);

                            position.operador = string.Empty;
                            position.velocidad = string.Empty;
                            position.direccion = string.Empty;
                            position.odo = string.Empty;

                            if (!map.ContainsKey(position.dispositivo))
                            {
                                map.Add(position.dispositivo, position.dispositivo);
                                listLastPosition.Add(position);
                            }
                        }

                        reader.Close();
                        response.ListLastPosition = listLastPosition;
                        response.success = true;
                    }
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
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

        public string LastStatusAlarm(string device)
        {
            string alarm = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_last_status_alarms_by_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                             
                            string date = Convert.ToString(reader["date"]);
                            string alarms = Convert.ToString(reader["alarm"]).Trim();
                            if(alarms.Equals("17"))
                            {
                                alarm = "Ignition Off";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //finally
            //{
            //    //cn.Close();
            //}
            return alarm;
        }

        private int StatusEventNotificationsPriority(string device)
        {
            int alarm = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_status_event_notifications_priority", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alarm = Convert.ToInt32(reader["count"]);
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
            return alarm;
        }

        private List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> NotificationsPriority(List<CurrentPositionDevice> listLastPosition,
            DateTime startDate, DateTime endDate)
        {
            List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> listNotifications = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();
            CredencialSpiderFleet.Models.Itineraries.NotificationsPriority notifications = new CredencialSpiderFleet.Models.Itineraries.NotificationsPriority();
            
            foreach (var data in listLastPosition)
            {
                try
                {
                    if (sql.IsConnection)
                    {
                        cn = sql.Connection();
                        SqlCommand cmd = new SqlCommand("ad.sp_consult_notifications_priority", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(data.dispositivo)));
                        cmd.Parameters.Add(new SqlParameter("@startdate", Convert.ToDateTime(startDate)));
                        cmd.Parameters.Add(new SqlParameter("@enddate", Convert.ToDateTime(endDate)));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications = new CredencialSpiderFleet.Models.Itineraries.NotificationsPriority();
                                notifications.Id = Convert.ToInt32(reader["id"]);
                                notifications.Device = Convert.ToString(reader["device"]);
                                notifications.Alarm = Convert.ToString(reader["alarm"]).Trim();
                                notifications.Name = Convert.ToString(reader["name"]).Trim();
                                notifications.Latitud = Convert.ToString(reader["latitude"]).Trim();
                                notifications.Longitude = Convert.ToString(reader["longitude"]).Trim();
                                notifications.DateGenerated = Convert.ToDateTime(reader["date_generated"]);
                                notifications.View = Convert.ToInt32(reader["band"]);
                                string mongo = Convert.ToString(reader["mongo"]);

                                if (notifications.Alarm.Equals("S.O.S."))
                                {
                                    notifications.Description = "Se ha activado el botón de Panico de la unidad " + notifications.Name +",ejecutar el protocolo para esta Alarma";
                                }
                                else if (notifications.Alarm.Equals("Desconexion"))
                                {
                                    notifications.Description = "Se ha desconectado el dispositivo de la unidad " + notifications.Name + "";
                                }
                                else if (notifications.Alarm.Equals("Geo Cerca"))
                                {
                                    CoordinatesGeoFence coordinates = new CoordinatesGeoFence();
                                    coordinates = GetNameGeoCerca(mongo);
                                    notifications.Description = "La Unidad " + notifications.Name + " se ha salido de la Geo Cerca " + coordinates.name;
                                    notifications.Coordinates = coordinates.Coordinates;
                                }

                                listNotifications.Add(notifications);
                            }
                            reader.Close();
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
          
            return listNotifications;
        }

        private CoordinatesGeoFence GetNameGeoCerca(string id)
        {
            CoordinatesGeoFence coordinates = new CoordinatesGeoFence();
            string nameGeoCerca = string.Empty;

            try
            {
                var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter;
                var filter = build.Eq(x => x.Id, id);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = StoredTripData.Find(filter).FirstOrDefault();

                if (result != null)
                {
                    coordinates.name = result.Name;
                    coordinates.Coordinates = result.Polygon.Coordinates[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return coordinates;
        }

        public string LastAlarm(string device)
        {
            string evento = string.Empty;

            try
            {
                var build = Builders<SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms>.Filter;
                var filter = build.Eq("device", device);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms>("Alarms");
                var result = StoredTripData.Find(filter).Sort("{date:-1}").FirstOrDefault();

                if (result != null)
                {
                    evento = result.Alarmsn[0].Type;                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return evento;
        }

        public string[] LastLogin(string device)
        {
            string[] evento = new string[2];

            try
            {
                var build = Builders<Login>.Filter;
                var filter = build.Eq("device", device);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Login>("Login");
                var result = StoredTripData.Find(filter).Sort("{date:-1}").FirstOrDefault();

                if (result != null)
                {
                    evento[0] = result.Version_sw;
                    evento[1] = result.Version_hw;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return evento;
        }

        #endregion

        #region Nuevo Spider SP
        /// <summary>
        /// Nuevo Spider
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public async Task<LastPositionDeviceResponse> ReadCurrentPositionDevicesHierarchy(string hierarchy, string busqueda)
        {
            LastPositionDeviceResponse response = new LastPositionDeviceResponse();
            List<CurrentPositionDevice> listLastPosition = new List<CurrentPositionDevice>();
            CurrentPositionDevice position = new CurrentPositionDevice();
            Dictionary<string, string> map = new Dictionary<string, string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_last_position_device_spider_pruebas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(hierarchy)));
                    if (!string.IsNullOrEmpty(busqueda)) { cmd.Parameters.Add(new SqlParameter("@search", Convert.ToString(busqueda))); }
                    else { cmd.Parameters.Add(new SqlParameter("@search", "")); }

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string Events = string.Empty;

                            position = new CurrentPositionDevice();
                            position.dispositivo = Convert.ToString(reader["device"]);
                            position.Events = Convert.ToString(reader["event"]);
                            position.nombre = Convert.ToString(reader["name"]);
                            position.fecha = Convert.ToDateTime(reader["date"]);
                            position.latitud = Convert.ToString(reader["latitude"]);
                            position.longitud = Convert.ToString(reader["longitude"]);
                            position.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            //position.empresa = Convert.ToString(reader["empresa"]);
                            position.nombreEmpresa = Convert.ToString(reader["nombre_empresa"]);
                            position.statusEvent = Convert.ToInt32(reader["status_event"]);
                            position.typeDevice = Convert.ToInt32(reader["type_device"]);

                            position.operador = string.Empty;
                            position.velocidad = string.Empty;
                            position.direccion = string.Empty;
                            position.odo = string.Empty;

                            DateTime timeUtc = DateTime.UtcNow;
                            //TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                            int secs = (int)cstTime.Subtract(Convert.ToDateTime(position.fecha)).TotalSeconds;
                            secs = secs - 3600;
                            //int secs = (int)DateTime.Now.Subtract(Convert.ToDateTime(position.fecha)).TotalSeconds;

                            if (position.Events.Equals(SLEEP))
                            {
                                position.estado = false;
                            }
                            else
                            {
                                if (secs >= 21950)//18350)// 21950)21600
                                {
                                    position.estado = false;
                                }
                                else
                                {
                                    position.estado = true;
                                }
                            }


                            if (!map.ContainsKey(position.dispositivo))
                            {
                                map.Add(position.dispositivo, position.dispositivo);
                                listLastPosition.Add(position);
                            }
                        }

                        reader.Close();
                        response.ListLastPosition = listLastPosition;
                        
                        int red = listLastPosition.Count(x => x.statusEvent == 7 );
                        red += listLastPosition.Count(x => x.statusEvent == 4);
                        red += listLastPosition.Count(x => x.statusEvent == 10);

                        if (red > 0)
                        {
                            response.View = NotificationsPriorityCount(hierarchy);
                        }

                        response.success = true;
                    }
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
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }


        private int NotificationsPriorityCount(string node)
        {
            int result = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_notifications_priority_count", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node ", Convert.ToString(node)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@count";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    result = Convert.ToInt32(sqlParameter.Value.ToString());

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
            return result;
        }


        public NotificationsResponse NotificationsPriority(string node)
        {

            NotificationsResponse response = new NotificationsResponse();
            List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> listNotifications = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();
            CredencialSpiderFleet.Models.Itineraries.NotificationsPriority notifications = new CredencialSpiderFleet.Models.Itineraries.NotificationsPriority();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_notifications_priority_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications = new CredencialSpiderFleet.Models.Itineraries.NotificationsPriority();
                            notifications.Id = Convert.ToInt32(reader["id"]);
                            notifications.Device = Convert.ToString(reader["device"]);
                            notifications.Alarm = Convert.ToString(reader["alarm"]).Trim();
                            notifications.Name = Convert.ToString(reader["name"]).Trim();
                            notifications.Latitud = Convert.ToString(reader["latitude"]).Trim();
                            notifications.Longitude = Convert.ToString(reader["longitude"]).Trim();
                            notifications.DateGenerated = Convert.ToDateTime(reader["date_generated"]);
                            notifications.View = Convert.ToInt32(reader["band"]);
                            string mongo = Convert.ToString(reader["mongo"]);

                            if (notifications.Alarm.Equals("S.O.S."))
                            {
                                notifications.Description = "Se ha activado el botón de Panico de la unidad " + notifications.Name + ",ejecutar el protocolo para esta Alarma";
                            }
                            else if (notifications.Alarm.Equals("Desconexion"))
                            {
                                notifications.Description = "Se ha desconectado el dispositivo de la unidad " + notifications.Name + "";
                            }
                            else if (notifications.Alarm.Equals("Geo Cerca"))
                            {
                                CoordinatesGeoFence coordinates = new CoordinatesGeoFence();
                                coordinates = GetNameGeoCerca(mongo);
                                notifications.Description = "La Unidad " + notifications.Name + " se ha salido de la Geo Cerca " + coordinates.name;
                                notifications.Coordinates = coordinates.Coordinates;
                            }

                            listNotifications.Add(notifications);
                        }
                        reader.Close();
                        response.success = true;
                        response.listNotifications = listNotifications;
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

        #endregion
    }
}