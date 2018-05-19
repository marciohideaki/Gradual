
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdensAdm.Lib;
namespace Gradual.OMS.ServicoRoteador
{
    public class CanalInfo
    {
        public string Exchange { get; set; }
        
        public string ChannelID { get; set; }
        
        public string RoteadorAddress { get; set; }
        
        public string AssinaturaAddress { get; set; }

        public IRoteadorOrdens roteador;

        public IAssinaturasRoteadorOrdensCallback assinatura;

        public bool Conectado { get; set; }

        public long LastHeartbeat { get; set; }

        public string RoteadorAdmAddress { get; set; }

        public IRoteadorOrdensAdmin roteadorAdm { get; set; }
    }
}
