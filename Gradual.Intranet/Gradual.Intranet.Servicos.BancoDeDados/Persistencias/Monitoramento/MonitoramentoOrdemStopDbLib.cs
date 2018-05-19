using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Dados.Enum;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public class MonitoramentoOrdemStopDbLib
    {
        #region Propriedades
        public static string gNomeConexaoRisco
        {
            get { return "Risco"; }
        }
        #endregion

        /// <summary>
        /// Busca as ordens stop start 
        /// </summary>
        /// <param name="pInfo">Entidade do tipo BuscarOrdensStopStartRequest</param>
        /// <returns>Retorna um List do tipo OrdemStopStartInfo</returns>
        public BuscarOrdensStopStartResponse BuscarOrdensStopStart(BuscarOrdensStopStartRequest pInfo)
        {
            OrdemStopStartInfo _TOrdem = new OrdemStopStartInfo();

            AcessaDados lAcessaDados = new AcessaDados();

            BuscarOrdensStopStartResponse _ListOrdem = new BuscarOrdensStopStartResponse();

            OrdemStopStartInfoDetalhe lDetalhe;

            lAcessaDados.ConnectionStringName = gNomeConexaoRisco;

            _ListOrdem.OrdensStartStop = new List<OrdemStopStartInfo>();

            string lUltimoId = "";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_buscar_ordens_stop_start"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.Int32, pInfo.Account);
                lAcessaDados.AddInParameter(lDbCommand, "@CodBmf", DbType.Int32, pInfo.CodBmf);
                lAcessaDados.AddInParameter(lDbCommand, "@DataDe", DbType.DateTime, pInfo.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DataAte", DbType.DateTime, pInfo.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "@Symbol", DbType.AnsiString, pInfo.Symbol);
                lAcessaDados.AddInParameter(lDbCommand, "@StopStartStatusID", DbType.Int32, pInfo.OrderStatusId);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pInfo.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@id_sistema", DbType.Int32, pInfo.IdSistema);

                DataTable dtDados = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (dtDados.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDados.Rows.Count - 1; i++)
                    {
                        if (lUltimoId != dtDados.Rows[i]["StopStartID"].ToString())
                        {
                            _TOrdem = new OrdemStopStartInfo();
                            _TOrdem.Details = new List<OrdemStopStartInfoDetalhe>();

                            _TOrdem.Account = int.Parse(dtDados.Rows[i]["Account"].ToString());
                            _TOrdem.StopStartID = int.Parse(dtDados.Rows[i]["StopStartID"].ToString());
                            _TOrdem.Symbol = dtDados.Rows[i]["Symbol"].ToString();
                            _TOrdem.IdStopStartTipo = (StopStartTipoEnum)dtDados.Rows[i]["StopStartTipoEnum"];
                            _TOrdem.OrdTypeID = int.Parse(dtDados.Rows[i]["OrdTypeID"].ToString());
                            _TOrdem.IdBolsa = dtDados.Rows[i]["id_Bolsa"].ToString().DBToInt32();
                            _TOrdem.InitialMovelPrice = dtDados.Rows[i]["InitialMovelPrice"].DBToDecimal();
                            _TOrdem.OrderQty = int.Parse(dtDados.Rows[i]["OrderQty"].ToString());
                            _TOrdem.ReferencePrice = dtDados.Rows[i]["ReferencePrice"].DBToDecimal();
                            _TOrdem.SendStartPrice = dtDados.Rows[i]["SendStartPrice"].DBToDecimal();
                            _TOrdem.SendStopGainPrice = dtDados.Rows[i]["SendStopGainPrice"].DBToDecimal();
                            _TOrdem.SendStopLossValuePrice = dtDados.Rows[i]["SendStopLossValuePrice"].DBToDecimal();
                            _TOrdem.StartPriceValue = dtDados.Rows[i]["StartPriceValue"].DBToDecimal();
                            _TOrdem.StopGainValuePrice = dtDados.Rows[i]["StopGainValuePrice"].DBToDecimal();
                            _TOrdem.StopLossValuePrice = dtDados.Rows[i]["StopLossValuePrice"].DBToDecimal();
                            _TOrdem.StopStartStatusID = int.Parse(dtDados.Rows[i]["StopStartStatusID"].ToString());
                            _TOrdem.RegisterTime = dtDados.Rows[i]["RegisterTime"].DBToDateTime(eDateNull.Permite);
                            _TOrdem.ExecutionTime = dtDados.Rows[i]["ExecutionTime"].DBToDateTime(eDateNull.Permite);
                            _TOrdem.ExpireDate = dtDados.Rows[i]["ExpireDate"].DBToDateTime(eDateNull.Permite);
                            _TOrdem.AdjustmentMovelPrice = dtDados.Rows[i]["AdjustmentMovelPrice"].DBToDecimal();

                            _ListOrdem.OrdensStartStop.Add(_TOrdem);

                            lUltimoId = _TOrdem.StopStartID.ToString();
                        }

                        lDetalhe = new OrdemStopStartInfoDetalhe();

                        lDetalhe.OrderStatusDescription = dtDados.Rows[i]["OrderStatusDescription"].DBToString();
                        lDetalhe.RegisterTime = dtDados.Rows[i]["RegisterTimeDetail"].DBToDateTime(eDateNull.Permite);
                        lDetalhe.StopStartStatusID = dtDados.Rows[i]["OrderStatusID"].DBToInt32();

                        _ListOrdem.OrdensStartStop[_ListOrdem.OrdensStartStop.Count - 1].Details.Add(lDetalhe);
                    }
                }


                return _ListOrdem;

            }

        }
    }
}
