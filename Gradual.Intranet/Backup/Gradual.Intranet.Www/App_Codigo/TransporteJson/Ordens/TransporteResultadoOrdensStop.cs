using Gradual.Intranet.Contratos.Mensagens;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    //public class TransporteResultadoOrdensStop
    //{
    //    #region | Propriedades

    //    public string Id { get; set; }

    //    public string Tipo { get; set; }

    //    public string Papel { get; set; }

    //    public string Hora { get; set; }

    //    public string PrecoStop { get; set; }

    //    public string PrecoLim { get; set; }

    //    public string Env { get; set; }

    //    public string Status { get; set; }

    //    public string LossGain { get; set; }

    //    public string Cliente { get; set; }

    //    public string Validade { get; set; }

    //    public string InicioMovel { get; set; }

    //    public string Ajuste { get; set; }

    //    #endregion

    //    #region | Construtores

    //    public TransporteResultadoOrdensStop() { }

    //    public TransporteResultadoOrdensStop(OrdemStartStopResponse pInfo)
    //    {
    //        this.Id = pInfo.CodigoOperacao;
    //        this.Tipo = pInfo.TipoOperacao;
    //        this.Papel = pInfo.Instrumento;
    //        this.Hora = pInfo.Hora.ToString("HH:mm:ss");
    //        this.PrecoStop = pInfo.PrecoStop.ToString("N2");
    //        this.PrecoLim = pInfo.PrecoLimite.ToString("N2");
    //        this.Env = pInfo.EnviadoBolsa;
    //        this.Status = pInfo.StatusOrdem;
    //        this.LossGain = pInfo.StopGainLoss;
    //        this.Cliente = pInfo.ClienteCodigo;
    //        this.Validade = pInfo.Validade.ToString("dd/MM/yyyy");
    //        this.InicioMovel = pInfo.InicioMovel.ToString("N2");
    //        this.Ajuste = pInfo.InicioAjuste.ToString("N2");
    //    }

    //    #endregion

    //    #region | Métodos Apoio

    //    public List<TransporteResultadoOrdensStop> ToListTransporteResultadoOrdensStop(ListarOrdemStartStopInfo pInfo)
    //    {
    //        var lRetorno = new List<TransporteResultadoOrdensStop>();

    //        pInfo.OrdemStarStopResponse.ForEach(delegate(OrdemStartStopResponse osso)
    //        {
    //            lRetorno.Add(new TransporteResultadoOrdensStop(osso));
    //        });

    //        return lRetorno;
    //    }

    //    #endregion
    //}
}
