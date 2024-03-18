using Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public interface IConfigService
    {
        void EditParam(ConfigParams config);
        ConfigParams GetParam(int idConfig);
        ConfigParams GetByNameParam(string name);
        IEnumerable<ConfigParams> GetAllParam();

        int InsertLog(ConfigLog log);
        IEnumerable<ConfigLog> GetLogByPrimary(int idPrimary);
    }
}
