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

        [Display(Name = "Equipamento")]
        public string Equipamento { get; set; }

        [Display(Name = "PONTO")]
        public string Ponto { get; set; }

        [Display(Name = "TIPO")]
        public string Tipo { get; set; }

        [Display(Name = "UNIDADE")]
        public string Unidade { get; set; }

        [Display(Name = "DIAGNÓSTICO")]
        public string Diagnostico { get; set; }

        [Display(Name = "AÇÃO RECOMENDADA")]
        public string AcaoRecomendada { get; set; }

        [Display(Name = "Nome do Arquivo")]
        public string NomeArquivo { get; set; }
    }

    public enum Alarme
    {
        Verde,
        Amarelo,
        Vermelho
    }
}
