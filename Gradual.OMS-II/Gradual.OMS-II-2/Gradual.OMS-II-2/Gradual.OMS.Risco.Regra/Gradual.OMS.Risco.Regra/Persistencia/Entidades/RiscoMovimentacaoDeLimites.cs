using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using System.Data;
using log4net;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class RiscoMovimentacaoDeLimites
    {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                #region | Métodos CRUD

        public ConsultarObjetosResponse<ClienteLimiteInfo> ConsultarLimitesDoCliente(ConsultarObjetosRequest<ClienteLimiteInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteLimiteInfo> lRetorno = new ConsultarObjetosResponse<ClienteLimiteInfo>();

            RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            lRetorno.Resultado = new List<ClienteLimiteInfo>();

            ///*
            // * 
            // * CodBovespa  = 0,
            // * CpfCnpj     = 1,
            // * NomeCliente = 2
            // * 
            // */

            try
            {
                paramsProc.Add("@id_parametro", pParametros.Objeto.ConsultaIdParametro);

                if (!string.IsNullOrEmpty(pParametros.Objeto.ConsultaClienteParametro))
                    switch (pParametros.Objeto.ConsultaClienteTipo)
                    {
                        case 0:
                            if (0.Equals(pParametros.Objeto.ConsultaClienteParametro.DBToInt32())) return lRetorno;
                            paramsProc.Add("@cd_bovespa", pParametros.Objeto.ConsultaClienteParametro.DBToInt32());
                            break;
                        case 1:
                            paramsProc.Add("@ds_cpfcnpj", pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", "").Replace("-", "").Replace("/", ""));
                            break;
                        case 2:
                            paramsProc.Add("@ds_nome", pParametros.Objeto.ConsultaClienteParametro.Trim());
                            break;
                    }

                DataTable lDataTable = _dbLib.ExecutarProcedure("prc_relatorio_cliente_limite_sp", paramsProc).Tables[0];

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeClienteLimiteInfo(lDataTable.Rows[i]));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<ClienteLimiteMovimentoInfo> ConsultarMovimentacaoDosLimitesDoCliente(ConsultarObjetosRequest<ClienteLimiteMovimentoInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteLimiteMovimentoInfo> lRetorno = new ConsultarObjetosResponse<ClienteLimiteMovimentoInfo>();
            RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            lRetorno.Resultado = new List<ClienteLimiteMovimentoInfo>();
            ///*
            // * 
            // * CodBovespa  = 0,
            // * CpfCnpj     = 1,
            // * NomeCliente = 2
            // * 
            // */
            try 
            {
                if (!string.IsNullOrEmpty(pParametros.Objeto.ConsultaClienteParametro))
                    switch (pParametros.Objeto.ConsultaClienteTipo)
                    {
                        case 0:
                            if (0.Equals(pParametros.Objeto.ConsultaClienteParametro.DBToInt32())) return lRetorno;
                            paramsProc.Add("@cd_bovespa", pParametros.Objeto.ConsultaClienteParametro.DBToInt32());
                            break;
                        case 1:
                            paramsProc.Add("@ds_cpfcnpj", pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", "").Replace("-", "").Replace("/", ""));
                            break;
                        case 2:
                            paramsProc.Add("@ds_nome", pParametros.Objeto.ConsultaClienteParametro.Trim());
                            break;
                    }
    

                DataTable lDataTable = _dbLib.ExecutarProcedure("prc_relatorio_cliente_limite_movimento_sp", paramsProc).Tables[0];

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeClienteLimiteMovimentoInfo(lDataTable.Rows[i]));

            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            
            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private ClienteLimiteInfo CarregarEntidadeClienteLimiteInfo(DataRow pLinha)
        {
            return new ClienteLimiteInfo()
            {
                Nome = pLinha["ds_nome"].DBToString(),
                CpfCnpj = pLinha["ds_cpfcnpj"].DBToString(),
                Parametro = pLinha["ds_parametro"].DBToString(),
                ValorLimite = pLinha["vl_parametro"].DBToDecimal(),
                ValorAlocado = pLinha["vl_alocado"].DBToDecimal(),
                IdClienteParametro = pLinha["id_cliente_parametro"].DBToInt32(),
                DataValidade = pLinha["dt_validade"].DBToDateTime(),
                IdParametro = pLinha["id_parametro"].DBToInt32()
                
            };
        }

        private ClienteLimiteMovimentoInfo CarregarEntidadeClienteLimiteMovimentoInfo(DataRow pLinha)
        {
            return new ClienteLimiteMovimentoInfo()
            {
                DataMovimento = pLinha["dt_movimento"].DBToDateTime(),
                Historico = pLinha["ds_historico"].DBToString(),
                IdClienteParametro = pLinha["id_cliente_parametro"].DBToInt32(),
                IdClienteParametroValor = pLinha["id_cliente_parametro_valor"].DBToInt32(),
                ValorAlocado = pLinha["vl_alocado"].DBToDecimal(),
                ValorDisponivel = pLinha["vl_disponivel"].DBToDecimal(),
                ValorMovimento = pLinha["vl_movimento"].DBToDecimal(),
            };
        }

        #endregion
    }
}
