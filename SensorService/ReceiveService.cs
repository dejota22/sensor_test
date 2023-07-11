using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Core;
using Core.ApiModel.Request;
using Core.ApiModel.Response;
using Core.DTO;
using Core.Service;
using Google.Protobuf.WellKnownTypes;
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

        private IQueryable<ReceiveDataDado> GetQueryDataDados()
        {
            IQueryable<ReceiveDataDado> tb_receivedatadado = _context.ReceiveDataDado;
            var query = from receiveDataDado in tb_receivedatadado
                        select receiveDataDado;

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
            var dados = ListaDadoFiltrado(rawDados, receiveData);

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

        public IEnumerable<ReceiveData> GetDataByDeviceMotor(int? deviceId, int? motorId)
        {
            return GetQueryData().Where(d => d.DeviceConfiguration.DeviceId == deviceId && d.DeviceConfiguration.MotorId == motorId && d.ReceiveDataDados.Any() == true);
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
            }

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
                DataReceive = (d.DataReceive.Ticks / TimeSpan.TicksPerMillisecond),
                Value = reportType == 1 ? d.rms_acc : reportType == 2 ? d.rms_spd : reportType == 3 ? d.ftr_crista : 0
            });

            var listGlobalMod = new List<RMSCristaModelResponse>();
            foreach (var data in listGlobal)
            {
                var globalMod = new RMSCristaModelResponse();
                globalMod.DataReceive = (data.DataReceive.Ticks / TimeSpan.TicksPerMillisecond);

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
            }

            listaReport = listDataMod.Union(listGlobalMod).ToList();

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
