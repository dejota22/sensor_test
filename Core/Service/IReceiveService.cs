using Core.ApiModel.Response;
using Core.DTO;
using System;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IReceiveService
    {
        int InsertGlobal(ReceiveGlobal receiveGlobal);

        int InsertData(ReceiveData receiveData, Sensor sensor);

        ReceiveGlobal GetGlobal(int id);

        ReceiveData GetData(int id);

        IEnumerable<ReceiveGlobal> ListGlobal();

        IEnumerable<ReceiveGlobal> ListGlobalLastAlarm();

        IEnumerable<ReceiveData> ListDataLastAlarm();

        IEnumerable<ReceiveData> GetAllData();

        IEnumerable<ReceiveData> GetDataByDeviceMotor(int? deviceId, int? motorId);

        IEnumerable<ReceiveDataDado> GetDataDadoByDataReceiveId(int dataId);

        IEnumerable<RMSCristaModelResponse> GetDataUnionGlobalByDateType(int deviceId, int motorId, DateTime startDate, DateTime endDate, int reportType, int eixo);
        IEnumerable<DataGlobalModel> ListDeviceCodeAlarme(int? deviceId, int? motorId, DateTime? startDate,
            DateTime? endDate, string gravidade, int skip = 0);
        int ListDeviceCodeAlarmeCount(int? deviceId, int? motorId, DateTime? startDate,
            DateTime? endDate, string gravidade);

        IEnumerable<DataGlobalHomeModel> ListDeviceAlarmes(int? deviceId, int? motorId);
        IEnumerable<DataGlobalHomeModel> ListDeviceAlarmesAgregado(int? deviceId, int? motorId);
        IEnumerable<DataDeviceExport> GetDataDadoExportByDataReceiveId(int idDataReceive);
    }
}
