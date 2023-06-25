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
            receiveGlobal.DataReceive = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            _context.ReceiveGlobal.Add(receiveGlobal);
            _context.SaveChanges();

            return receiveGlobal.IdReceiveGlobal;
        }

        public int InsertData(ReceiveData receiveData)
        {
            receiveData.DataReceive = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            var rawDados = MontaListaDado(receiveData.dado);
            var dados = ListaDadoConvertido(rawDados, receiveData);

            foreach (var dado in dados)
            {
                receiveData.ReceiveDataDados.Add(dado);
            }

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

        private List<ReceiveDataDado> ListaDadoConvertido(List<string> dadosRaw, ReceiveData dadoSensor)
        {
            List<ReceiveDataDado> dados = new List<ReceiveDataDado>();
            int posicao = 1;

            foreach (String item in dadosRaw)
            {
                double Valor_em_G;

                int numero = int.Parse(item.ToString(), System.Globalization.NumberStyles.HexNumber);

                if (numero >= 32768)
                {
                    int numeroNegativo = (int)~numero;
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
