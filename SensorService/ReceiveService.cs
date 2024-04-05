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
using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Asn1.X509;
using System.Numerics;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using Renci.SshNet.Messages;

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

        public int InsertData(ReceiveData receiveData, Sensor sensor)
        {
            receiveData.DataReceive = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            var rawDados = MontaListaDado(receiveData.dado);
            var dados = ListaDadoFiltrado(rawDados, receiveData);

            int skipSamples = 0;
            var setupAcc = sensor.SetupAcc.FirstOrDefault();
            var setupSpd = sensor.SetupSpd.FirstOrDefault();
            if (receiveData.tipo == 1 && setupAcc != null)
            {
                switch(setupAcc.amostras)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        skipSamples = 1024;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        skipSamples = 2048;
                        break;
                    default:
                        skipSamples = 0; break;
                }
            }
            else if (receiveData.tipo == 2 && setupSpd != null)
            {
                switch (setupSpd.amostras)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        skipSamples = 1024;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        skipSamples = 2048;
                        break;
                    default:
                        skipSamples = 0; break;
                }
            }

            dados = dados.Skip(skipSamples).ToList();
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
                && (d.DeviceConfiguration.MotorId == motorId || d.DeviceConfiguration.Motor.GroupId == motorId)).ToList();

            return receiveDataList;
        }

        public IEnumerable<ReceiveDataDado> GetDataDadoByDataReceiveId(int dataId)
        {
            return GetQueryDataDados().Where(d => d.IdReceiveData == dataId).OrderBy(d => d.seq);
        }

        public IEnumerable<DataDeviceExport> GetDataDadoExportByDataReceiveId(int dataId)
        {
            List<DataDeviceExport> listDataDevice = new List<DataDeviceExport>();

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    string query = $@"SELECT
                          rd.IdReceive_Data, rd.DataReceive as dataReceive, c.trade_name as companyName, 
                          m.name as motor, rd.id as sensor,
                          case
                            when rd.tipo = 2 THEN 'RMS SPD'
                            when rd.tipo = 1 THEN 'RMS ACC' 
                            else ''
                          end as tipo,
                          case 
                            when rd.tipo = 1 then rd.rms_acc
                            when rd.tipo = 2 then rd.rms_spd
                          end as rms,  
                          rd.ftr_crista as fatorCrista,   
                          case
                            when (rd.alarme >= 1 and rd.alarme < 7) then 'Atenção'  
                            when (rd.alarme >= 7) then 'Alerta'
                                                  else 'Normal'
                          end as alarme, 
                          case 
                            when rd.setup_odr = 0 then '26K'  
                            when rd.setup_odr = 1 then '3K3'
                            when rd.setup_odr = 2 then '1K6'
                            when rd.setup_odr = 3 then '1K1'
                            when rd.setup_odr = 4 then '0K8' 
                          end as odr,
                          case 
                            when rd.setup_freq_cut = 0 then '6K6'  
                            when rd.setup_freq_cut = 1 then '2K6'
                            when rd.setup_freq_cut = 2 then '1K3'
                            when rd.setup_freq_cut = 3 then '0K5'
                            when rd.setup_freq_cut = 4 then '0K2'
                            when rd.setup_freq_cut = 5 then '0K1'
                            when rd.setup_freq_cut = 6 then 'K67'
                          end as freqCut,
                          case 
                            when rd.setup_filtro = 0 then 'Filtro Passa Baixas (FPB)'  
                            when rd.setup_filtro = 1 then 'Filtro Passa Altas (FPA)'
                          end as filtro,
                          case 
                            when rd.setup_eixo = 1 then 'X'
                            when rd.setup_eixo = 2 then 'Y'
                            when rd.setup_eixo = 3 then 'Z'
                            when rd.setup_eixo = 4 then 'XY'
                            when rd.setup_eixo = 5 then 'XZ'
                            when rd.setup_eixo = 6 then 'YZ'
                            when rd.setup_eixo = 7 then 'XYZ'
                          end as eixo,    
                          case 
                            when rd.setup_fs = 0 then '02G'
                            when rd.setup_fs = 1 then '16G'
                            when rd.setup_fs = 2 then '04G'
                            when rd.setup_fs = 3 then '08G'
                            when rd.setup_fs = 4 then 'AUTO'  
                          end as fs,
                          case 
                            when rd.setup_amostras = 1 then '1024'
                            when rd.setup_amostras = 2 then '2048'
                            when rd.setup_amostras = 3 then '3072'
                            when rd.setup_amostras = 4 then '4096'
                            when rd.setup_amostras = 5 then '5120'
                            when rd.setup_amostras = 6 then '6144'
                            when rd.setup_amostras = 7 then '7168'
                            when rd.setup_amostras = 8 then '8192'
                          end as amostras,    
                          cu.name as unitName,
                          case
	                        when cus2.id is null then cus.name
                            else cus2.name
                          end as sectorName,
                          case
	                        when cus2.id is not null then cus.name
                          end as subSectorName
                        FROM sensorDB.Receive_Data rd
                             Inner Join sensorDB.Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
                             Inner Join sensorDB.motor m on dc.motor_id = m.id
                             Inner Join sensorDB.company c on m.company_id = c.id
                             left Join sensorDB.company_unit_sector cus on m.company_unit_sector_id = cus.id
                             left Join sensorDB.company_unit_sector cus2 on cus.parent_sector_id = cus2.id
                             left Join sensorDB.company_unit cu on cus.company_unit_id = cu.id
                         where rd.IdReceive_Data = {dataId};";

                    listDataDevice = conn.Query<DataDeviceExport>(query).ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return listDataDevice;
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
	                                    rg.id, 
                                        CASE WHEN (rg.rms_acc_X > dc.rms_acc_red) or (rg.rms_spd_X > dc.rms_spd_red) THEN 7 
		                                    WHEN (rg.rms_acc_X > dc.rms_acc_yel) or (rg.rms_spd_X > dc.rms_spd_yel) THEN 1 
		                                    WHEN (rg.rms_acc_Y > dc.rms_acc_red) or (rg.rms_spd_Y > dc.rms_spd_red) THEN 8 
		                                    WHEN (rg.rms_acc_Y > dc.rms_acc_yel) or (rg.rms_spd_Y > dc.rms_spd_yel) THEN 2 
		                                    WHEN (rg.rms_acc_Z > dc.rms_acc_red) or (rg.rms_spd_Z > dc.rms_spd_red) THEN 9 
		                                    WHEN (rg.rms_acc_Z > dc.rms_acc_yel) or (rg.rms_spd_Z > dc.rms_spd_yel) THEN 3  ELSE 0
	                                    END AS alarm,
                                        rg.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
	                                    m.id AS motorId, d.id AS deviceId,
	                                    CASE WHEN  (rg.rms_spd_X > dc.rms_spd_red or rg.rms_spd_X > dc.rms_spd_yel or rg.rms_spd_Y > dc.rms_spd_red or rg.rms_spd_Y > dc.rms_spd_yel or rg.rms_spd_Z > dc.rms_spd_red or rg.rms_spd_Z > dc.rms_spd_yel) THEN ""RMS SPD""
	                                    WHEN (rg.rms_acc_X > dc.rms_acc_red or rg.rms_acc_X > dc.rms_acc_yel or rg.rms_acc_Y > dc.rms_acc_red or rg.rms_acc_Y > dc.rms_acc_yel or rg.rms_acc_Z > dc.rms_acc_red or rg.rms_acc_Z > dc.rms_acc_yel) THEN ""RMS ACC"" ELSE """" END AS tipo
                                    FROM Receive_Global rg
                                    JOIN Device_Configuration dc on rg.IdDeviceConfiguration = dc.id
                                    JOIN device d on dc.device_id = d.id
                                    JOIN motor m on dc.motor_id = m.id
                                    {queryConditionGlobal}
                                    )
                                    UNION
                                    (SELECT
	                                    rd.id, rd.alarme AS alarm, rd.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
	                                    m.id AS motorId, d.id AS deviceId, CASE WHEN rd.tipo = 2 THEN ""RMS SPD"" WHEN rd.tipo = 1 THEN ""RMS ACC"" ELSE """" END AS tipo
                                    FROM Receive_Data rd
                                    JOIN Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
                                    JOIN device d on dc.device_id = d.id
                                    JOIN motor m on dc.motor_id = m.id
                                    {queryConditionData}
                                    )
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
	                                    rg.id, 
                                        CASE WHEN (rg.rms_acc_X > dc.rms_acc_red) or (rg.rms_spd_X > dc.rms_spd_red) THEN 7 
		                                    WHEN (rg.rms_acc_X > dc.rms_acc_yel) or (rg.rms_spd_X > dc.rms_spd_yel) THEN 1 
		                                    WHEN (rg.rms_acc_Y > dc.rms_acc_red) or (rg.rms_spd_Y > dc.rms_spd_red) THEN 8 
		                                    WHEN (rg.rms_acc_Y > dc.rms_acc_yel) or (rg.rms_spd_Y > dc.rms_spd_yel) THEN 2 
		                                    WHEN (rg.rms_acc_Z > dc.rms_acc_red) or (rg.rms_spd_Z > dc.rms_spd_red) THEN 9 
		                                    WHEN (rg.rms_acc_Z > dc.rms_acc_yel) or (rg.rms_spd_Z > dc.rms_spd_yel) THEN 3  ELSE 0
	                                    END AS alarm,
                                        rg.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
	                                    m.id AS motorId, d.id AS deviceId,
	                                    CASE WHEN  (rg.rms_spd_X > dc.rms_spd_red or rg.rms_spd_X > dc.rms_spd_yel or rg.rms_spd_Y > dc.rms_spd_red or rg.rms_spd_Y > dc.rms_spd_yel or rg.rms_spd_Z > dc.rms_spd_red or rg.rms_spd_Z > dc.rms_spd_yel) THEN ""RMS SPD""
	                                    WHEN (rg.rms_acc_X > dc.rms_acc_red or rg.rms_acc_X > dc.rms_acc_yel or rg.rms_acc_Y > dc.rms_acc_red or rg.rms_acc_Y > dc.rms_acc_yel or rg.rms_acc_Z > dc.rms_acc_red or rg.rms_acc_Z > dc.rms_acc_yel) THEN ""RMS ACC"" ELSE """" END AS tipo
                                    FROM Receive_Global rg
                                    JOIN Device_Configuration dc on rg.IdDeviceConfiguration = dc.id
                                    JOIN device d on dc.device_id = d.id
                                    JOIN motor m on dc.motor_id = m.id
                                    {queryConditionGlobal}
                                    )
                                    UNION
                                    (SELECT
	                                    rd.id, rd.alarme AS alarm, rd.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
	                                    m.id AS motorId, d.id AS deviceId, CASE WHEN rd.tipo = 2 THEN ""RMS SPD"" WHEN rd.tipo = 1 THEN ""RMS ACC"" ELSE """" END AS tipo
                                    FROM Receive_Data rd
                                    JOIN Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
                                    JOIN device d on dc.device_id = d.id
                                    JOIN motor m on dc.motor_id = m.id
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

        public IEnumerable<DataGlobalHomeModel> ListDeviceAlarmes(int? deviceId, int? motorId)
        {
            List<DataGlobalHomeModel> receiveDataAndGlobal = new List<DataGlobalHomeModel>();
            string queryConditionData = " WHERE 1 = 1 ";
            string queryConditionGlobal = " WHERE 1 = 1 ";
            string queryConditionSS = "";
            if (motorId.HasValue)
            {
                queryConditionData += $" AND m.id = {motorId} ";
                queryConditionSS += $" AND m.id = {motorId} ";
            }
            if (deviceId.HasValue)
            {
                queryConditionData += $" AND d.id = {deviceId} ";
                queryConditionSS += $" AND d.id = {deviceId} ";
            }

            queryConditionGlobal = queryConditionData;

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    string query = $@"SELECT U.* FROM
                                    (
                                    SELECT 
	                                    G.dataReceive, G.unitId, G.unitName, G.sectorId, G.sectorName, G.subSectorId, G.subSectorName, 
                                        G.deviceId, G.device, G.motorId, G.motor, G.plan, G.orientation, 
                                        G.alertaAccX, G.valorAccX, G.alertaAccY, G.valorAccY, G.alertaAccZ, G.valorAccZ, 
                                        G.alertaSpdX, G.valorSpdX, G.alertaSpdY, G.valorSpdY, G.alertaSpdZ, G.valorSpdZ
                                    FROM 
	                                    (SELECT
		                                    rg.id, rg.alrm AS alarm, rg.DataReceive AS dataReceive, 
                                            CASE WHEN s.parent_sector_id is null THEN s.id
		                                    ELSE s.parent_sector_id END AS sectorId, 
                                            CASE WHEN s.parent_sector_id is null THEN s.name
		                                    ELSE ps.name END AS sectorName,
                                            CASE WHEN s.parent_sector_id is not null THEN s.id
		                                    ELSE null END AS subSectorId, 
                                            CASE WHEN s.parent_sector_id is not null THEN s.name
		                                    ELSE null END AS subSectorName,
                                            u.id AS unitId, 
                                            u.name AS unitName,
                                            d.tag AS device, m.name AS motor,
		                                    m.id AS motorId, d.id AS deviceId, dm.measurement_plan AS plan, dm.sensor_orientation AS orientation,
                                            CASE WHEN rg.rms_acc_X > dc.rms_acc_red THEN ""Alerta"" 
			                                    WHEN rg.rms_acc_X > dc.rms_acc_yel THEN ""Atenção"" ELSE ""Normal""
		                                    END AS alertaAccX,
                                            rg.rms_acc_X AS valorAccX,
		                                    CASE WHEN rg.rms_acc_Y > dc.rms_acc_red THEN ""Alerta"" 
			                                    WHEN rg.rms_acc_Y > dc.rms_acc_yel THEN ""Atenção"" ELSE ""Normal""
		                                    END AS alertaAccY,
                                            rg.rms_acc_Y AS valorAccY,
                                            CASE WHEN rg.rms_acc_Z > dc.rms_acc_red THEN ""Alerta"" 
			                                    WHEN rg.rms_acc_Z > dc.rms_acc_yel THEN ""Atenção"" ELSE ""Normal""
		                                    END AS alertaAccZ,
                                            rg.rms_acc_Z AS valorAccZ,
                                            CASE WHEN rg.rms_spd_X > dc.rms_spd_red THEN ""Alerta"" 
			                                    WHEN rg.rms_spd_X > dc.rms_spd_yel THEN ""Atenção"" ELSE ""Normal""
		                                    END AS alertaSpdX,
                                            rg.rms_spd_X AS valorSpdX,
		                                    CASE WHEN rg.rms_spd_Y > dc.rms_spd_red THEN ""Alerta"" 
			                                    WHEN rg.rms_spd_Y > dc.rms_spd_yel THEN ""Atenção"" ELSE ""Normal""
		                                    END AS alertaSpdY,
                                            rg.rms_spd_Y AS valorSpdY,
                                            CASE WHEN rg.rms_spd_Z > dc.rms_spd_red THEN ""Alerta"" 
			                                    WHEN rg.rms_spd_Z > dc.rms_spd_yel THEN ""Atenção"" ELSE ""Normal""
		                                    END AS alertaSpdZ,
                                            rg.rms_spd_Z AS valorSpdZ
	                                    FROM Receive_Global rg
	                                    JOIN Device_Configuration dc on rg.IdDeviceConfiguration = dc.id
	                                    JOIN device d on dc.device_id = d.id
	                                    JOIN motor m on dc.motor_id = m.id
                                        JOIN device_motor dm on d.device_motor_id = dm.id and m.id = dm.motor_id
                                        LEFT JOIN company_unit_sector s on m.company_unit_sector_id = s.id
                                        LEFT JOIN company_unit_sector ps on s.parent_sector_id = ps.id
                                        LEFT JOIN company_unit u on s.company_unit_id = u.id
                                        INNER JOIN (
		                                  SELECT IdDeviceConfiguration, MAX(DataReceive) AS DataReceive
		                                  FROM Receive_Global GROUP BY IdDeviceConfiguration
		                                ) AS max USING (IdDeviceConfiguration, DataReceive)
		                                      {queryConditionGlobal}
	                                     order by rg.DataReceive desc limit 20) AS G 
	
                                        UNION
    
                                    SELECT 
	                                    D.dataReceive, D.unitId, D.unitName, D.sectorId, D.sectorName, D.subSectorId, D.subSectorName, 
                                        D.deviceId, D.device, D.motorId, D.motor, D.plan, D.orientation, 
                                        D.alertaAccX, D.valorAccX, D.alertaAccY, D.valorAccY, D.alertaAccZ, D.valorAccZ, 
                                        D.alertaSpdX, D.valorSpdX, D.alertaSpdY, D.valorSpdX, D.alertaSpdY, D.valorSpdZ
                                    FROM 
	                                    (SELECT
		                                    rd.id, rd.alarme AS alarm, rd.DataReceive AS dataReceive, 
                                            CASE WHEN s.parent_sector_id is null THEN s.id
		                                    ELSE s.parent_sector_id END AS sectorId, 
                                            CASE WHEN s.parent_sector_id is null THEN s.name
		                                    ELSE ps.name END AS sectorName,
                                            CASE WHEN s.parent_sector_id is not null THEN s.id
		                                    ELSE null END AS subSectorId, 
                                            CASE WHEN s.parent_sector_id is not null THEN s.name
		                                    ELSE null END AS subSectorName,
                                            u.id AS unitId, 
                                            u.name AS unitName,
                                            d.tag AS device, m.name AS motor,
		                                    m.id AS motorId, d.id AS deviceId, dm.measurement_plan AS plan, dm.sensor_orientation AS orientation,
                                            CASE WHEN rd.alarme >= 7 and rd.tipo = 1 and ((rd.alarme in (1,4,7,10)) or (rd.alarme = 0 and rd.setup_eixo = 1))  THEN ""Alerta"" 
			                                    WHEN (rd.alarme >= 1 and rd.alarme < 7) and rd.tipo = 1 and ((rd.alarme in (1,4,7,10)) or (rd.alarme = 0 and rd.setup_eixo = 1))  THEN ""Atenção"" 
                                               WHEN rd.alarme = 0 and rd.tipo = 1 and rd.rms_acc != 0 THEN ""Normal"" ELSE """"
		                                    END AS alertaAccX,
                                            rd.rms_acc AS valorAccX,
                                            CASE WHEN rd.alarme >= 7 and rd.tipo = 1 and ((rd.alarme in (2,5,8,11)) or (rd.alarme = 0 and rd.setup_eixo = 2))  THEN ""Alerta"" 
			                                    WHEN (rd.alarme >= 1 and rd.alarme < 7) and rd.tipo = 1 and ((rd.alarme in (2,5,8,11)) or (rd.alarme = 0 and rd.setup_eixo = 2))  THEN ""Atenção""
                                                WHEN rd.alarme = 0 and rd.tipo = 1 and rd.rms_acc != 0 THEN ""Normal"" ELSE """"
		                                    END AS alertaAccY,
                                            rd.rms_acc AS valorAccY,
                                            CASE WHEN rd.alarme >= 7 and rd.tipo = 1 and ((rd.alarme in (3,6,9,12)) or (rd.alarme = 0 and rd.setup_eixo = 3))  THEN ""Alerta"" 
			                                    WHEN (rd.alarme >= 1 and rd.alarme < 7) and rd.tipo = 1 and ((rd.alarme in (3,6,9,12)) or (rd.alarme = 0 and rd.setup_eixo = 3))  THEN ""Atenção""
                                                WHEN rd.alarme = 0 and rd.tipo = 1 and rd.rms_acc != 0 THEN ""Normal"" ELSE """"
		                                    END AS alertaAccZ,
                                            rd.rms_acc AS valorAccZ,
                                            CASE WHEN rd.alarme >= 7 and rd.tipo = 2 and ((rd.alarme in (1,4,7,10)) or (rd.alarme = 0 and rd.setup_eixo = 1))  THEN ""Alerta"" 
			                                    WHEN (rd.alarme >= 1 and rd.alarme < 7) and rd.tipo = 2 and ((rd.alarme in (1,4,7,10)) or (rd.alarme = 0 and rd.setup_eixo = 1))  THEN ""Atenção"" 
                                                WHEN rd.alarme = 0 and rd.tipo = 2 and rd.rms_spd != 0 THEN ""Normal"" ELSE """"
		                                    END AS alertaSpdX,
                                            rd.rms_spd AS valorSpdX,
                                            CASE WHEN rd.alarme >= 7 and rd.tipo = 2 and ((rd.alarme in (2,5,8,11)) or (rd.alarme = 0 and rd.setup_eixo = 2))  THEN ""Alerta"" 
			                                    WHEN (rd.alarme >= 1 and rd.alarme < 7) and rd.tipo = 2 and ((rd.alarme in (2,5,8,11)) or (rd.alarme = 0 and rd.setup_eixo = 2))  THEN ""Atenção""
                                                WHEN rd.alarme = 0 and rd.tipo = 2 and rd.rms_spd != 0 THEN ""Normal"" ELSE """"
		                                    END AS alertaSpdY,
                                            rd.rms_spd AS valorSpdY,
                                            CASE WHEN rd.alarme >= 7 and rd.tipo = 2 and ((rd.alarme in (3,6,9,12)) or (rd.alarme = 0 and rd.setup_eixo = 3))  THEN ""Alerta"" 
			                                    WHEN (rd.alarme >= 1 and rd.alarme < 7) and rd.tipo = 2 and ((rd.alarme in (3,6,9,12)) or (rd.alarme = 0 and rd.setup_eixo = 3))  THEN ""Atenção""
                                                WHEN rd.alarme = 0 and rd.tipo = 2 and rd.rms_spd != 0 THEN ""Normal"" ELSE """"
		                                    END AS alertaSpdZ,
                                            rd.rms_spd AS valorSpdZ
	                                    FROM Receive_Data rd
	                                    JOIN Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
	                                    JOIN device d on dc.device_id = d.id
	                                    JOIN motor m on dc.motor_id = m.id
	                                    JOIN device_motor dm on d.device_motor_id = dm.id and m.id = dm.motor_id
                                        LEFT JOIN company_unit_sector s on m.company_unit_sector_id = s.id
                                        LEFT JOIN company_unit_sector ps on s.parent_sector_id = ps.id
                                        LEFT JOIN company_unit u on s.company_unit_id = u.id
                                        INNER JOIN (
		                                  SELECT IdDeviceConfiguration, MAX(DataReceive) AS DataReceive
		                                  FROM Receive_Global GROUP BY IdDeviceConfiguration
		                                ) AS max USING (IdDeviceConfiguration, DataReceive)
		                                      {queryConditionData}
	                                     order by rd.DataReceive desc limit 50) AS D 
	
                                        UNION
    
                                        SELECT 
		                                    SS.dataReceive, SS.unitId, SS.unitName, SS.sectorId, SS.sectorName, SS.subSectorId, SS.subSectorName, 
		                                    SS.deviceId, SS.device, SS.motorId, SS.motor, SS.plan, SS.orientation, 
		                                    SS.alertaAccX, SS.valorAccX, SS.alertaAccY, SS.valorAccY, SS.alertaAccZ, SS.valorAccZ, 
                                            SS.alertaSpdX, SS.valorSpdX,  SS.alertaSpdY, SS.valorSpdY,  SS.alertaSpdZ, SS.valorSpdZ
	                                    FROM
	                                    (SELECT
		                                    DATE(""1000-01-01"") AS dataReceive, u.id AS unitId, u.name AS unitName, 
		                                    CASE WHEN s.parent_sector_id is null THEN s.id
		                                    ELSE s.parent_sector_id END AS sectorId, 
		                                    CASE WHEN s.parent_sector_id is null THEN s.name
		                                    ELSE ps.name END AS sectorName,
		                                    CASE WHEN s.parent_sector_id is not null THEN s.id
		                                    ELSE null END AS subSectorId, 
		                                    CASE WHEN s.parent_sector_id is not null THEN s.name
		                                    ELSE null END AS subSectorName,
		                                    d.id AS deviceId, d.tag AS device, m.id AS motorId, m.name AS motor,
		                                    dm.measurement_plan AS plan, dm.sensor_orientation AS orientation,
		                                    ""Sem Sinal"" AS alertaAccX, ""Sem Sinal"" AS alertaAccY, ""Sem Sinal"" AS alertaAccZ, ""Sem Sinal"" AS alertaSpdX, ""Sem Sinal"" AS alertaSpdY, ""Sem Sinal"" AS alertaSpdZ,
                                            """" AS valorAccX, """" AS valorAccY, """" AS valorAccZ, """" AS valorSpdX, """" AS valorSpdY, """" AS valorSpdZ
	                                    FROM device d
	                                    JOIN device_motor dm on d.device_motor_id = dm.id
	                                    JOIN motor m on dm.motor_id = m.id
	                                    LEFT JOIN company_unit_sector s on m.company_unit_sector_id = s.id
	                                    LEFT JOIN company_unit_sector ps on s.parent_sector_id = ps.id
	                                    LEFT JOIN company_unit u on s.company_unit_id = u.id
	                                    WHERE d.id not in
		                                    (
			                                    SELECT DISTINCT d2.id
				                                    FROM device d2
				                                    JOIN Device_Configuration dc2 on dc2.device_id = d2.id 
				                                    JOIN Receive_Data rd2 on rd2.IdDeviceConfiguration = dc2.id
			                                    UNION
			                                    SELECT DISTINCT d3.id
				                                    FROM device d3
				                                    JOIN Device_Configuration dc3 on dc3.device_id = d3.id 
				                                    JOIN Receive_Global rg3 on rg3.IdDeviceConfiguration = dc3.id
		                                    )  {queryConditionSS} ) AS SS
    
                                     ) AS U
                                    order by U.dataReceive desc;";

                    receiveDataAndGlobal = conn.Query<DataGlobalHomeModel>(query).ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return receiveDataAndGlobal;
        }

        public IEnumerable<DataGlobalHomeModel> ListDeviceAlarmesAgregado(int? deviceId, int? motorId)
        {
            List<DataGlobalHomeModel> receiveDataAndGlobal = new List<DataGlobalHomeModel>();
            string queryConditionData = " WHERE 1 = 1 ";
            string queryConditionGlobal = " WHERE 1 = 1 ";
            string queryConditionSS = "";
            if (motorId.HasValue)
            {
                queryConditionData += $" AND m.id = {motorId} ";
                queryConditionSS += $" AND m.id = {motorId} ";
            }
            else
            {
                queryConditionData += $" AND m.id in (select id from motor) ";
                queryConditionSS += $" AND m.id in (select id from motor) ";
            }

            if (deviceId.HasValue)
            {
                queryConditionData += $" AND d.id = {deviceId} ";
                queryConditionSS += $" AND d.id = {deviceId} ";
            }

            queryConditionGlobal = queryConditionData;

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                try
                {
                    string query = $@"SELECT U.* FROM
                                    (SELECT 
	                                    G.companyId, G.dataReceive, G.unitId, G.unitName, G.sectorId, G.sectorName, G.subSectorId, G.subSectorName, 
                                        G.deviceId, G.device, G.motorId, G.motor, G.alerta
                                    FROM 
	                                    (SELECT
		                                    d.company_id AS companyId, rg.id, rg.alrm AS alarm, rg.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
		                                    m.id AS motorId, d.id AS deviceId,
                                            CASE WHEN s.parent_sector_id is null THEN s.id
		                                    ELSE s.parent_sector_id END AS sectorId, 
                                            CASE WHEN s.parent_sector_id is null THEN s.name
		                                    ELSE ps.name END AS sectorName,
                                            CASE WHEN s.parent_sector_id is not null THEN s.id
		                                    ELSE null END AS subSectorId, 
                                            CASE WHEN s.parent_sector_id is not null THEN s.name
		                                    ELSE null END AS subSectorName,
                                            u.id AS unitId, 
                                            u.name AS unitName,
		                                    CASE WHEN (rg.rms_spd_X > dc.rms_spd_red or rg.rms_spd_Y > dc.rms_spd_red or rg.rms_spd_Z > dc.rms_spd_red or rg.rms_acc_X > dc.rms_acc_red or rg.rms_acc_Y > dc.rms_acc_red or rg.rms_acc_Z > dc.rms_acc_red) THEN ""Alerta""
			                                    WHEN (rg.rms_spd_X > dc.rms_spd_yel or rg.rms_spd_Y > dc.rms_spd_yel or rg.rms_spd_Z > dc.rms_spd_yel or rg.rms_acc_X > dc.rms_acc_yel or rg.rms_acc_Y > dc.rms_acc_yel or rg.rms_acc_Z > dc.rms_acc_yel) THEN ""Atenção""
			                                    ELSE ""Normal"" END AS alerta
	                                    FROM Receive_Global rg
	                                    JOIN Device_Configuration dc on rg.IdDeviceConfiguration = dc.id
	                                    JOIN device d on dc.device_id = d.id
	                                    JOIN motor m on dc.motor_id = m.id
                                        JOIN device_motor dm on d.device_motor_id = dm.id and m.id = dm.motor_id
                                        LEFT JOIN company_unit_sector s on m.company_unit_sector_id = s.id
                                        LEFT JOIN company_unit_sector ps on s.parent_sector_id = ps.id
                                        LEFT JOIN company_unit u on s.company_unit_id = u.id
                                        INNER JOIN (
		                                  SELECT IdDeviceConfiguration, MAX(DataReceive) AS DataReceive
		                                  FROM Receive_Global GROUP BY IdDeviceConfiguration
		                                ) AS max USING (IdDeviceConfiguration, DataReceive)
	                                        {queryConditionGlobal}
                                            order by rg.DataReceive desc limit 20) AS G
                                        UNION
    
                                        SELECT 
	                                    D.companyId, D.dataReceive, D.unitId, D.unitName, D.sectorId, D.sectorName, D.subSectorId, D.subSectorName, 
                                        D.deviceId, D.device, D.motorId, D.motor, D.alerta
                                    FROM 
	                                    (SELECT
		                                    d.company_id AS companyId, rd.id, rd.alarme AS alarm, rd.DataReceive AS dataReceive, d.tag AS device, m.name AS motor,
		                                    m.id AS motorId, d.id AS deviceId, 
                                            CASE WHEN s.parent_sector_id is null THEN s.id
		                                    ELSE s.parent_sector_id END AS sectorId, 
                                            CASE WHEN s.parent_sector_id is null THEN s.name
		                                    ELSE ps.name END AS sectorName,
                                            CASE WHEN s.parent_sector_id is not null THEN s.id
		                                    ELSE null END AS subSectorId, 
                                            CASE WHEN s.parent_sector_id is not null THEN s.name
		                                    ELSE null END AS subSectorName,
                                            u.id AS unitId, 
                                            u.name AS unitName,
		                                    CASE WHEN rd.tipo = 2 THEN ""RMS SPD"" WHEN rd.tipo = 1 THEN ""RMS ACC"" ELSE """" END AS tipo, 
		                                    CASE  WHEN rd.alarme in (1,4,7,10) THEN ""X"" WHEN rd.alarme in (2,5,8,11) THEN ""Y"" WHEN rd.alarme in (3,6,9,12) THEN ""Z"" 
			                                    WHEN rd.alarme = 0 and rd.setup_eixo = 1 THEN ""X"" WHEN rd.alarme = 0 and rd.setup_eixo = 2 THEN ""Y"" WHEN rd.alarme = 0 and rd.setup_eixo = 3 THEN ""Z"" ELSE """" END AS eixo,
		                                    CASE WHEN (rd.alarme >= 1 and rd.alarme < 7) THEN ""Atenção"" WHEN (rd.alarme >= 7) THEN ""Alerta"" ELSE ""Normal"" END AS alerta
	                                    FROM Receive_Data rd
	                                    JOIN Device_Configuration dc on rd.IdDeviceConfiguration = dc.id
	                                    JOIN device d on dc.device_id = d.id
	                                    JOIN motor m on dc.motor_id = m.id
                                        JOIN device_motor dm on d.device_motor_id = dm.id and m.id = dm.motor_id
                                        LEFT JOIN company_unit_sector s on m.company_unit_sector_id = s.id
                                        LEFT JOIN company_unit_sector ps on s.parent_sector_id = ps.id
                                        LEFT JOIN company_unit u on s.company_unit_id = u.id
                                        INNER JOIN (
		                                  SELECT IdDeviceConfiguration, MAX(DataReceive) AS DataReceive
		                                  FROM Receive_Global GROUP BY IdDeviceConfiguration
		                                ) AS max USING (IdDeviceConfiguration, DataReceive)
	                                        {queryConditionData}
                                            order by rd.DataReceive desc limit 50) AS D
        
	                                        UNION
    
                                            SELECT 
	                                        SS.companyId, SS.dataReceive, SS.unitId, SS.unitName, SS.sectorId, SS.sectorName, SS.subSectorId, SS.subSectorName, 
                                            SS.deviceId, SS.device, SS.motorId, SS.motor, SS.alerta
                                        FROM 
	                                        (SELECT
		                                        d.company_id AS companyId, DATE(""1000-01-01"") AS dataReceive, m.id AS motorId, m.name AS motor,
		                                        d.id AS deviceId, d.tag AS device, u.id AS unitId, u.name AS unitName,
		                                        dm.measurement_plan AS plan, dm.sensor_orientation AS orientation,
		                                        CASE WHEN s.parent_sector_id is null THEN s.id
		                                        ELSE s.parent_sector_id END AS sectorId, 
		                                        CASE WHEN s.parent_sector_id is null THEN s.name
		                                        ELSE ps.name END AS sectorName,
		                                        CASE WHEN s.parent_sector_id is not null THEN s.id
		                                        ELSE null END AS subSectorId, 
		                                        CASE WHEN s.parent_sector_id is not null THEN s.name
		                                        ELSE null END AS subSectorName,
		                                        ""Sem Sinal"" AS alerta
	                                        FROM device d
	                                        JOIN device_motor dm on d.device_motor_id = dm.id
	                                        JOIN motor m on dm.motor_id = m.id
	                                        LEFT JOIN company_unit_sector s on m.company_unit_sector_id = s.id
	                                        LEFT JOIN company_unit_sector ps on s.parent_sector_id = ps.id
	                                        LEFT JOIN company_unit u on s.company_unit_id = u.id
	                                        WHERE d.id not in
	                                        (
		                                        SELECT DISTINCT d2.id
			                                        FROM device d2
			                                        JOIN Device_Configuration dc2 on dc2.device_id = d2.id 
			                                        JOIN Receive_Data rd2 on rd2.IdDeviceConfiguration = dc2.id
		                                        UNION
                                                SELECT DISTINCT d3.id
			                                        FROM device d3
			                                        JOIN Device_Configuration dc3 on dc3.device_id = d3.id 
			                                        JOIN Receive_Global rg3 on rg3.IdDeviceConfiguration = dc3.id
                                            ) {queryConditionSS}) AS SS

                                     ) AS U
                                    order by U.dataReceive desc;";

                    receiveDataAndGlobal = conn.Query<DataGlobalHomeModel>(query).ToList();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                }
            }

            return receiveDataAndGlobal;
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

                if (dadoT.valor > 0)
                {
                    dadoT.valor = dadoT.valor / 9.81;
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
}
