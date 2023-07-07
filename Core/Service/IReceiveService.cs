using Core.ApiModel.Response;
using Core.DTO;
using System;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IReceiveService
    {
        int InsertGlobal(ReceiveGlobal receiveGlobal);

        int InsertData(ReceiveData receiveData);

        ReceiveGlobal GetGlobal(int id);

        ReceiveData GetData(int id);

        IEnumerable<ReceiveGlobal> GetAllGlobal();

        IEnumerable<ReceiveData> GetAllData();

        IEnumerable<ReceiveData> GetDataByDeviceMotor(int? deviceId, int? motorId);

        IEnumerable<ReceiveDataDado> GetDataDadoByDataReceiveId(int dataId);

        IEnumerable<RMSCristaModelResponse> GetDataUnionGlobalByDateType(int deviceId, int motorId, DateTime startDate, DateTime endDate, int reportType, int eixo);
    }
}
