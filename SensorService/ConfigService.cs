using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
  public  class ConfigService : IConfigService
    {
        private readonly SensorContext _context;

        public ConfigService(SensorContext context)
        {
            _context = context;
        }

        private IQueryable<ConfigParams> GetQueryParams()
        {
            IQueryable<ConfigParams> tbConfigParams = _context.ConfigParams;
            var query = from c in tbConfigParams
                        select c;

            return query;
        }

        private IQueryable<ConfigLog> GetQueryLog()
        {
            IQueryable<ConfigLog> tbConfigLog = _context.ConfigLog;
            var query = from c in tbConfigLog
                        select c;

            return query;
        }

        void IConfigService.EditParam(ConfigParams config)
        {
            _context.Update(config);
            _context.SaveChanges();
        }

        ConfigParams IConfigService.GetParam(int idConfig)
        {
            return GetQueryParams().Where(x => x.Id.Equals(idConfig)).FirstOrDefault();
        }

        ConfigParams IConfigService.GetByNameParam(string name)
        {
            return GetQueryParams().Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        IEnumerable<ConfigParams> IConfigService.GetAllParam()
        {
            return GetQueryParams();
        }

        

        int IConfigService.InsertLog(Core.ConfigLog log)
        {
            log.CreatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            _context.Add(log);
            _context.SaveChanges();

            return log.Id;
        }

        IEnumerable<ConfigLog> IConfigService.GetLogByPrimary(int idPrimary)
        {
            return GetQueryLog().Where(x => x.PrimaryId.Equals(idPrimary));
        }
    }
}
