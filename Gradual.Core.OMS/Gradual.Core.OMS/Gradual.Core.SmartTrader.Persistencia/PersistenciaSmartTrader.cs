using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Generico.Dados;
using Gradual.Core.OMS.SmartTrader.Lib.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Core.SmartTrader.Persistencia
{
    public class PersistenciaSmartTrader
    {
        private const string gNomeConexao = "RiscoOMS";

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Método responsável por inserir uma nova ordem no banco de dados
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos da ordem do cliente</param>
        /// <returns>bool</returns>
        public OrdemSmart InserirOrdem(OrdemSmart _OrdemSmart)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexao;

                //Cria DBComand atribuindo a storedprocedure PRC_INS_ORDER_ROUTER_OMS_V2                
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_smart_order"))
                {
                    logger.Info("ADICIONA OS PARAMETROS DA ORDEM SMART TRADER.");
                  

                    lAcessaDados.AddInParameter(lDbCommand, "@account", DbType.Int32, _OrdemSmart.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@symbol", DbType.AnsiString, _OrdemSmart.Instrument);
                    lAcessaDados.AddInParameter(lDbCommand, "@quantidade", DbType.AnsiString, _OrdemSmart.Qty);
                    lAcessaDados.AddInParameter(lDbCommand, "@preco_inicio", DbType.Decimal, _OrdemSmart.OperacaoInicio.PrecoOrdem);
                    lAcessaDados.AddInParameter(lDbCommand, "@disparo_inicio", DbType.Decimal, _OrdemSmart.OperacaoInicio.PrecoDisparo);
                    lAcessaDados.AddInParameter(lDbCommand, "@preco_lucro", DbType.Decimal, _OrdemSmart.OperacaoLucro.PrecoOrdem);
                    lAcessaDados.AddInParameter(lDbCommand, "@disparo_lucro", DbType.Decimal, _OrdemSmart.OperacaoLucro.PrecoDisparo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tipo_disp_lucro", DbType.Int32, (int)(_OrdemSmart.OperacaoLucro.PrecoOrdemTipo));
                    lAcessaDados.AddInParameter(lDbCommand, "@valor_tipo_disp_lucro", DbType.Decimal, _OrdemSmart.OperacaoLucro.Valor);
                    lAcessaDados.AddInParameter(lDbCommand, "@preco_perda", DbType.Decimal, _OrdemSmart.OperacaoPerda.PrecoOrdem);
                    lAcessaDados.AddInParameter(lDbCommand, "@disparo_perda", DbType.Decimal, _OrdemSmart.OperacaoPerda.PrecoDisparo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tipo_disp_perda", DbType.Decimal, (int)(_OrdemSmart.OperacaoPerda.PrecoOrdemTipo));
                    lAcessaDados.AddInParameter(lDbCommand, "@valor_tipo_disp_perda", DbType.Decimal, _OrdemSmart.OperacaoPerda.Valor);

                    logger.Info("ENVIA SOLICITAÇÃO PARA A STORED PROCEDURE");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _OrdemSmart.Id = (lDataTable.Rows[i]["SmartOrderID"]).DBToInt32();
                        }
                    }

                    logger.Info("STORED PROCEDURE EXECUTADA COM SUCESSO.");

                    return _OrdemSmart;

                }
            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO ENVIAR UMA ORDER SMARTTRADE PARA O BANCO DE DADOS");
                logger.Info("DESCRICAO DO ERRO:    " + ex.Message);

                throw (ex);
            }

        }

    }
}
