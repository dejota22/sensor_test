using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Core;
using Core.ApiModel.Request;
using Core.ApiModel.Response;
using Core.DTO;
using Core.Service;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Renci.SshNet.Messages;

namespace SensorService
{
    public class ReceiveService : IReceiveService
    {
        private readonly SensorContext _context;
        private readonly string _connectionString;

        public ReceiveService(SensorContext context)
        {
            _context = context;
            _connectionString = context.Database.GetDbConnection().ConnectionString;
        }

        #region Globals

        public int InsertGlobal(ReceiveGlobal receiveGlobal)
        {
            receiveGlobal.DataReceive = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            _context.ReceiveGlobal.Add(receiveGlobal);
            _context.SaveChanges();

            return receiveGlobal.IdReceiveGlobal;
        }

        public IEnumerable<ReceiveGlobal> ListGlobal()
        {
            return GetQueryGlobal();
        }

        public IEnumerable<ReceiveGlobal> ListGlobalLastAlarm()
        {
            List<ReceiveGlobal> globals = new List<ReceiveGlobal>();

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    globals = conn.Query<ReceiveGlobal>(@"SELECT rd1.id, rd1.DataReceive, rd1.alrm 
                        FROM sensorDB.Receive_Global rd1 
                        JOIN (
	                        SELECT id, max(DataReceive) as DataReceive, alrm
                            FROM sensorDB.Receive_Global
                            GROUP BY id) as rd2
                        on rd1.id = rd2.id and rd1.DataReceive = rd2.DataReceive;")
                    .ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return globals;
        }

        public ReceiveGlobal GetGlobal(int id)
        {
            return GetQueryGlobal().Where(x => x.IdReceiveGlobal.Equals(id)).FirstOrDefault();
        }

        private IQueryable<ReceiveGlobal> GetQueryGlobal()
        {
            IQueryable<ReceiveGlobal> tb_receiveglobal = _context.ReceiveGlobal;
            var query = tb_receiveglobal.Include(g => g.DeviceConfiguration);

            return query;
        }

        private IList<ReceiveGlobal> RawQueryGlobal()
        {
            List<ReceiveGlobal> globals = new List<ReceiveGlobal>();

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    globals = conn.Query<ReceiveGlobal>("SELECT * FROM sensorDB.Receive_Global;").ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return globals;
        }

        #endregion

        private IQueryable<ReceiveData> GetQueryData()
        {
            IQueryable<ReceiveData> tb_receivedata = _context.ReceiveData;
            var query = tb_receivedata.Include(d => d.DeviceConfiguration);

            return query;
        }

        private IQueryable<ReceiveDataDado> GetQueryDataDados()
        {
            IQueryable<ReceiveDataDado> tb_receivedatadado = _context.ReceiveDataDado;
            var query = from receiveDataDado in tb_receivedatadado
                        select receiveDataDado;

            return query;
        }

        public ReceiveData GetData(int id)
        {
            return GetQueryData().Where(x => x.IdReceiveData.Equals(id)).FirstOrDefault();
        }

        public int InsertData(ReceiveData receiveData)
        {
            receiveData.DataReceive = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            var rawDados = MontaListaDado(receiveData.dado);
            var dados = ListaDadoFiltrado(rawDados, receiveData);

            foreach (var dado in dados)
            {
                receiveData.ReceiveDataDados.Add(dado);
            }

            _context.ReceiveData.Add(receiveData);
            _context.SaveChanges();

            return receiveData.IdReceiveData;
        }

        

        public IEnumerable<ReceiveData> GetAllData()
        {
            return GetQueryData();
        }

        public IEnumerable<ReceiveData> ListDataLastAlarm()
        {
            List<ReceiveData> globals = new List<ReceiveData>();

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    globals = conn.Query<ReceiveData>(@"SELECT rd1.id, rd1.DataReceive, rd1.alarme 
                        FROM sensorDB.Receive_Data rd1 
                        JOIN (
	                        SELECT id, max(DataReceive) as DataReceive, alarme
                            FROM sensorDB.Receive_Data
                            GROUP BY id) as rd2
                        on rd1.id = rd2.id and rd1.DataReceive = rd2.DataReceive;")
                    .ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return globals;
        }

        public IEnumerable<ReceiveData> GetDataByDeviceMotor(int? deviceId, int? motorId)
        {
            var receiveDataList = GetQueryData().Where(d => d.DeviceConfiguration.DeviceId == deviceId 
                && d.DeviceConfiguration.MotorId == motorId).ToList();

            return receiveDataList;
        }

        public IEnumerable<ReceiveDataDado> GetDataDadoByDataReceiveId(int dataId)
        {
            return GetQueryDataDados().Where(d => d.IdReceiveData == dataId).OrderBy(d => d.seq);
        }

        public IEnumerable<RMSCristaModelResponse> GetDataUnionGlobalByDateType(int deviceId, int motorId, DateTime startDate, DateTime endDate, int reportType, int eixo)
        {
            int qReportType = reportType;
            if (reportType == 3)
                qReportType = 1;

            var listaData = GetQueryData().Where(d => d.DeviceConfiguration.DeviceId == deviceId && d.DeviceConfiguration.MotorId == motorId && 
                d.DataReceive >= startDate && d.DataReceive <= endDate && d.tipo == qReportType && d.setup_eixo == eixo).ToList();

            var listaGlobal = GetQueryGlobal().Where(d => d.DeviceConfiguration.DeviceId == deviceId && d.DeviceConfiguration.MotorId == motorId &&
                d.DataReceive >= startDate && d.DataReceive <= endDate).ToList();

            return MontaListaReportRMSCrista(listaData, listaGlobal, reportType, eixo);
        }

        public IEnumerable<DataGlobalModel> ListDeviceCodeAlarme(int? deviceId, int? motorId, DateTime? startDate,
            DateTime? endDate, string gravidade, int skip = 0)
        {
            List<DataGlobalModel> receiveDataAndGlobal = new List<DataGlobalModel>();
            string queryConditionData = " WHERE 1 = 1 ";
            string queryConditionGlobal = " WHERE 1 = 1 ";
            if (motorId.HasValue)
            {
                queryConditionData += $" AND m.id = {motorId} ";
            }
            if (deviceId.HasValue)
            {
                queryConditionData += $" AND d.id = {deviceId} ";
            }
            if (startDate.HasValue)
            {
                queryConditionData += $" AND DataReceive >= '{startDate.Value.ToString("yyyy-MM-dd")}' ";
            }
            if (endDate.HasValue)
            {
                queryConditionData += $" AND DataReceive <= '{endDate.Value.ToString("yyyy-MM-dd")}' ";
            }

            queryConditionGlobal = queryConditionData;

            queryConditionData += $" AND rd.alarme >= 1 ";
            queryConditionGlobal += $" AND rg.alrm >= 1 ";

            if (gravidade != null)
            {
                switch (gravidade)
                {
                    case "amarelo":
                        queryConditionData += $" AND rd.alarme < 7 ";
                        queryConditionGlobal += $" AND rg.alrm < 7 ";
                        break;
                    case "vermelho":
                        queryConditionData += $" AND rd.alarme >= 7 ";
                        queryConditionGlobal += $" AND rg.alrm >= 7 ";
                        break;
                    default:
                        break;
                }
            }

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    string query = $@"(SELECT
	                    rg.id, rg.alrm AS alarm, rg.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
                        m.id AS motorId, d.id AS deviceId,
                        CASE WHEN  rg.alrm > 0 and (rg.rms_spd_X > dc.rms_spd_red or rg.rms_spd_X > dc.rms_spd_yel or rg.rms_spd_Y > dc.rms_spd_red or rg.rms_spd_Y > dc.rms_spd_yel or rg.rms_spd_Z > dc.rms_spd_red or rg.rms_spd_Z > dc.rms_spd_yel) THEN ""RMS SPD"" 
                        WHEN rg.alrm > 0 and (rg.rms_acc_X > dc.rms_acc_red or rg.rms_acc_X > dc.rms_acc_yel or rg.rms_acc_Y > dc.rms_acc_red or rg.rms_acc_Y > dc.rms_acc_yel or rg.rms_acc_Z > dc.rms_acc_red or rg.rms_acc_Z > dc.rms_acc_yel) THEN ""RMS ACC"" ELSE """" END AS tipo
                    FROM sensorDB.Receive_Global rg
                    JOIN sensorDB.Device_Configuration dc on rg.IdDeviceConfiguration = dc.id
                    JOIN sensorDB.device d on dc.device_id = d.id
                    JOIN sensorDB.motor m on dc.motor_id = m.id
                    {queryConditionGlobal})
                    UNION
                    (SELECT
	                    rd.id, rd.alarme AS alarm, rd.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
                        m.id AS motorId, d.id AS deviceId, CASE WHEN rd.tipo = 2 THEN ""RMS SPD"" WHEN rd.tipo = 1 THEN ""RMS ACC"" ELSE """" END AS tipo
                    FROM sensorDB.Receive_Data rd
                    JOIN sensorDB.Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
                    JOIN sensorDB.device d on dc.device_id = d.id
                    JOIN sensorDB.motor m on dc.motor_id = m.id
                    {queryConditionData})
                     order by dataReceive desc
                    LIMIT {skip},10;";

                    receiveDataAndGlobal = conn.Query<DataGlobalModel>(query).ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return receiveDataAndGlobal;
        }

        public int ListDeviceCodeAlarmeCount(int? deviceId, int? motorId, DateTime? startDate,
            DateTime? endDate, string gravidade)
        {
            int receiveDataAndGlobalCount = 0;
            string queryConditionData = " WHERE 1 = 1 ";
            string queryConditionGlobal = " WHERE 1 = 1 ";
            if (motorId.HasValue)
            {
                queryConditionData += $" AND m.id = {motorId} ";
            }
            if (deviceId.HasValue)
            {
                queryConditionData += $" AND d.id = {deviceId} ";
            }
            if (startDate.HasValue)
            {
                queryConditionData += $" AND DataReceive >= '{startDate.Value.ToString("yyyy-MM-dd")}' ";
            }
            if (endDate.HasValue)
            {
                queryConditionData += $" AND DataReceive <= '{endDate.Value.ToString("yyyy-MM-dd")}' ";
            }

            queryConditionGlobal = queryConditionData;

            queryConditionData += $" AND rd.alarme >= 1 ";
            queryConditionGlobal += $" AND rg.alrm >= 1 ";

            if (gravidade != null)
            {
                switch (gravidade)
                {
                    case "amarelo":
                        queryConditionData += $" AND rd.alarme < 7 ";
                        queryConditionGlobal += $" AND rg.alrm < 7 ";
                        break;
                    case "vermelho":
                        queryConditionData += $" AND rd.alarme >= 7 ";
                        queryConditionGlobal += $" AND rg.alrm >= 7 ";
                        break;
                    default:
                        break;
                }
            }

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    string query = $@"SELECT COUNT(*)
	                FROM
	                (
                        (SELECT
	                        rg.id, rg.alrm AS alarm, rg.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
                            m.id AS motorId, d.id AS deviceId
                        FROM sensorDB.Receive_Global rg
                        JOIN sensorDB.Device_Configuration dc on rg.IdDeviceConfiguration = dc.id
                        JOIN sensorDB.device d on dc.device_id = d.id
                        JOIN sensorDB.motor m on dc.motor_id = m.id
                        {queryConditionGlobal})
                        UNION
                        (SELECT
	                        rd.id, rd.alarme AS alarm, rd.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
                            m.id AS motorId, d.id AS deviceId
                        FROM sensorDB.Receive_Data rd
                        JOIN sensorDB.Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
                        JOIN sensorDB.device d on dc.device_id = d.id
                        JOIN sensorDB.motor m on dc.motor_id = m.id
                        {queryConditionData})
                    ) x;";

                    receiveDataAndGlobalCount = conn.Query<int>(query).First();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return receiveDataAndGlobalCount;
        }

        private List<String> MontaListaDado(string dados)
        {
            var x = dados.Split(4).ToList();
            List<String> novaLista = new List<String>();

            foreach (string a in x)
            {
                string valor = "";
                string inicio = a.Substring(2, 2);
                string fim = a.Substring(0, 2);
                valor = String.Concat(inicio, fim);
                novaLista.Add(valor);
            }

            return novaLista;
        }

        private List<ReceiveDataDado> ListaDadoFiltrado(List<string> dadosRaw, ReceiveData dadoSensor)
        {
            List<ReceiveDataDado> dadosTransitorios =  ListaDadoTransitorio(dadosRaw, dadoSensor);
            int tipo = dadoSensor.tipo;

            const double alpha = 0.998;
            const double delta_t = 1/3333.375;
            const double filtro_2 = 0.95;

            double[] vals_F = new double[dadosTransitorios.Count];
            double[] vals_H = new double[dadosTransitorios.Count];
            double[] vals_K = new double[tipo == 2 ? dadosTransitorios.Count : 0];
            double[] vals_L = new double[tipo == 2 ? dadosTransitorios.Count : 0];
            double[] vals_M = new double[tipo == 2 ? dadosTransitorios.Count : 0];

            double maxDataDadoValor = 0;

            foreach (var dadoT in dadosTransitorios.OrderBy(d => d.seq))
            {
                int currentIndex = dadosTransitorios.FindIndex(d => d == dadoT);

                if (currentIndex > 0)
                {
                    //Conversão para K
                    double val_F_prev = vals_F[currentIndex - 1];
                    double val_F = dadoT.valor;

                    double val_H_prev = vals_H[currentIndex - 1];
                    double val_H = (val_F - val_F_prev) + alpha * val_H_prev;

                    vals_F[currentIndex] = val_F;
                    vals_H[currentIndex] = val_H;

                    if (tipo == 2)
                    {
                        double val_K_prev = vals_K[currentIndex - 1];
                        double val_K = val_K_prev + (val_H + val_H_prev) * delta_t / 2;

                        double val_L_prev = vals_L[currentIndex - 1];
                        double val_L = (val_K - val_K_prev) + filtro_2 * val_L_prev;

                        double val_M_prev = vals_M[currentIndex - 1];
                        double val_M = val_L * 1000;

                        vals_K[currentIndex] = val_K;
                        vals_L[currentIndex] = val_L;
                        vals_M[currentIndex] = val_M;

                        dadoT.valor = val_M;
                    }
                    else
                    {
                        dadoT.valor = val_H;
                    }
                }
                else
                {
                    vals_F[0] = dadoT.valor;
                    vals_H[0] = 0;

                    if (tipo == 2)
                    {
                        vals_K[0] = 0;
                        vals_L[0] = 0;
                        vals_M[0] = 0;
                    }

                    dadoT.valor = 0;
                }

                if (Math.Abs(dadoT.valor) > maxDataDadoValor)
                {
                    maxDataDadoValor = Math.Abs(dadoT.valor);
                }
            }

            dadoSensor.ftr_crista = tipo == 2 ? (maxDataDadoValor / dadoSensor.rms_spd) : (maxDataDadoValor / dadoSensor.rms_acc);

            return dadosTransitorios;
        }

        private List<ReceiveDataDado> ListaDadoTransitorio(List<string> dadosRaw, ReceiveData dadoSensor)
        {
            List<ReceiveDataDado> dados = new List<ReceiveDataDado>();
            int posicao = 1;

            foreach (String item in dadosRaw)
            {
                double Valor_em_G;

                Int16 numero = Int16.Parse(item.ToString(), System.Globalization.NumberStyles.HexNumber);

                if (numero >= 32768)
                {
                    short numeroNegativo = (Int16)~numero;
                }

                int FS = dadoSensor.setup_fs;
                double[] Fator_FS = { 0.061, 0.488, 0.122, 0.244 };

                Valor_em_G = ((numero * Fator_FS[FS]) / 1000) * 9.81;

                int ODR = dadoSensor.setup_odr;
                int[] div_dec = { 1, 8, 16, 24, 32 };
                double tempo = (1 / (dadoSensor.dec / div_dec[ODR]));

                var dado = new ReceiveDataDado()
                {
                    seq = posicao,
                    valor = Valor_em_G,
                    tempo = (posicao - 1) * tempo
                };

                dados.Add(dado);
                posicao++;
            }

            return dados;
        }

        private IEnumerable<RMSCristaModelResponse> MontaListaReportRMSCrista(IList<ReceiveData> listData, IList<ReceiveGlobal> listGlobal, int reportType, int eixo)
        {
            var listaReport = new List<RMSCristaModelResponse>();

            var listDataMod = listData.Select(d => new RMSCristaModelResponse()
            {
                DataReceive = (long)(d.DataReceive.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                Value = reportType == 1 ? d.rms_acc : reportType == 2 ? d.rms_spd : reportType == 3 ? d.ftr_crista : 0,
                Origem = "Completo",
                DataDevice = d.IdReceiveData
            }).ToList();

            var listGlobalMod = new List<RMSCristaModelResponse>();
            if (reportType != 3)
            {
                foreach (var data in listGlobal)
                {
                    var globalMod = new RMSCristaModelResponse();
                    globalMod.DataReceive = (long)(data.DataReceive.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
                    globalMod.Origem = "Global";

                    if (reportType == 1 && eixo == 1)
                        globalMod.Value = data.rms_acc_X;
                    if (reportType == 1 && eixo == 2)
                        globalMod.Value = data.rms_acc_Y;
                    if (reportType == 1 && eixo == 3)
                        globalMod.Value = data.rms_acc_Z;
                    if (reportType == 2 && eixo == 1)
                        globalMod.Value = data.rms_spd_X;
                    if (reportType == 2 && eixo == 2)
                        globalMod.Value = data.rms_spd_Y;
                    if (reportType == 2 && eixo == 3)
                        globalMod.Value = data.rms_spd_Z;

                    listGlobalMod.Add(globalMod);
                }
            }

            listaReport = listDataMod.Union(listGlobalMod).OrderBy(u => u.DataReceive).ToList();

            return listaReport;
        }
    }

    public static class Extensions
    {
        public static IEnumerable<string> Split(this string str, int n)
        {
            if (String.IsNullOrEmpty(str) || n < 1)
            {
                throw new ArgumentException();
            }

            return Enumerable.Range(0, str.Length / n)
                            .Select(i => str.Substring(i * n, n));
        }
    }

    //public static IEnumerable<T> Select<T>(this IDataReader reader,
    //                                   Func<IDataReader, T> projection)
    //{
    //    while (reader.Read())
    //    {
    //        yield return projection(reader);
    //    }
    //}
}
