using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Core;
using Core.ApiModel.Request;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace SensorService
{
    public class ReceiveService : IReceiveService
    {
        private readonly SensorContext _context;

        public ReceiveService(SensorContext context)
        {
            _context = context;
        }

        private IQueryable<ReceiveGlobal> GetQueryGlobal()
        {
            IQueryable<ReceiveGlobal> tb_receiveglobal = _context.ReceiveGlobal;
            var query = from receiveGlobal in tb_receiveglobal
                        select receiveGlobal;

            return query;
        }

        private IQueryable<ReceiveData> GetQueryData()
        {
            IQueryable<ReceiveData> tb_receivedata = _context.ReceiveData;
            var query = from receiveData in tb_receivedata
                        select receiveData;

            return query;
        }

        public ReceiveGlobal GetGlobal(int id)
        {
            return GetQueryGlobal().Where(x => x.IdReceiveGlobal.Equals(id)).FirstOrDefault();
        }

        public ReceiveData GetData(int id)
        {
            return GetQueryData().Where(x => x.IdReceiveData.Equals(id)).FirstOrDefault();
        }

        public int InsertGlobal(ReceiveGlobal receiveGlobal)
        {
            receiveGlobal.DataReceive = DateTime.Now;
            _context.ReceiveGlobal.Add(receiveGlobal);
            _context.SaveChanges();

            return receiveGlobal.IdReceiveGlobal;
        }

        public int InsertData(ReceiveData receiveData)
        {
            receiveData.DataReceive = DateTime.Now;
            _context.ReceiveData.Add(receiveData);
            _context.SaveChanges();

            return receiveData.IdReceiveData;
        }

        public IEnumerable<ReceiveGlobal> GetAllGlobal()
        {
            return GetQueryGlobal();
        }

        public IEnumerable<ReceiveData> GetAllData()
        {
            return GetQueryData();
        }
    }
}
