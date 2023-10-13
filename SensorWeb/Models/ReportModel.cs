using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SensorWeb.Models
{
    public class ReportModel : BaseModel
    {        
        [Display(Name = "ACIONAMENTO")]
        public string Acionamento { get; set; }

        [Display(Name = "ACIONADO")]
        public string Acionado { get; set; }

        [Display(Name = "ALARME")]
        public Alarme AlarmeAcionamento { get; set; }

        [Display(Name = "ALARME")]
        public Alarme AlarmeAcionado { get; set; }

        [Display(Name = "VELOCIDADE")]
        public string Velocidade { get; set; }

        [Display(Name = "PONTO MAX")]
        public string PontoMaxVel1 { get; set; }

        [Display(Name = "PONTO MAX")]
        public string PontoMaxVel2 { get; set; }
        [Display(Name = "VEL.")]
        public string Vel1 { get; set; }

        [Display(Name = "VEL.")]
        public string Vel2 { get; set; }

        [Display(Name = "ACELERAÇÃO")]
        public string Aceleracao { get; set; }

        [Display(Name = "PONTO MAX")]
        public string PontoMaxAce1 { get; set; }

        [Display(Name = "PONTO MAX")]
        public string PontoMaxAce2 { get; set; }

        [Display(Name = "ACEL.")]
        public string Acel1 { get; set; }

        [Display(Name = "ACEL.")]
        public string Acel2 { get; set; }

        [Display(Name = "OBSERVAÇÕES RELEVANTES")]
        public string ObservacoesRelevantes { get; set; }

        [Display(Name = "GRÁFICO")]
        public string Grafico { get; set; }

        [Display(Name = "PONTO")]
        public string Ponto { get; set; }

        [Display(Name = "UNIDADE")]
        public string Unidade { get; set; }

        [Display(Name = "DIAGNÓSTICO")]
        public string Diagnostico { get; set; }

        [Display(Name = "AÇÃO RECOMENDADA")]
        public string AcaoRecomendada { get; set; }

        [Display(Name = "Nome do Arquivo")]
        public string NomeArquivo { get; set; }


        [Display(Name = "EQUIPAMENTO")]
        public int MotorId { get; set; }
        public string MotorIdName { get; set; }

        [Display(Name = "SENSOR")]
        public int DeviceId { get; set; }
        public string DeviceIdName { get; set; }

        [Display(Name = "TIPO")]
        public int TipoRelatorio { get; set; }
        public string TipoRelatorioName { get; set; }

        [Display(Name = "DATA INICIAL")]
        public DateTime StartDate { get; set; }

        [Display(Name = "DATA FINAL")]
        public DateTime EndDate { get; set; }

        [Display(Name = "EIXO")]
        public int Eixo { get; set; }
        public string EixoName { get; set; }

        [Display(Name = "DADOS")]
        public int DataDeviceId { get; set; }
        public string DataDeviceIdName { get; set; }
    }

    public enum Alarme
    {
        Verde,
        Amarelo,
        Vermelho
    }
}
