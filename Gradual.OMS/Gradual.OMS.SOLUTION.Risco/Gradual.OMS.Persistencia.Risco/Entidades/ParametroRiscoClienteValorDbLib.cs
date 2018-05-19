using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.Data;
using Gradual.OMS.Persistencia.Risco.DB;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia.Risco.Entidades
{
    public class ParametroRiscoClienteValorDbLib : IEntidadeDbLib<ParametroRiscoClienteValorInfo>
    {
        private SqlDbLib _dbLib = new SqlDbLib("RISCO");

        #region IEntidadeDbLib<ParametroRiscoClienteValorInfo> Members

        public ConsultarObjetosResponse<ParametroRiscoClienteValorInfo> ConsultarObjetos(ConsultarObjetosRequest<ParametroRiscoClienteValorInfo> lRequest)
        {
            ConsultarObjetosResponse<ParametroRiscoClienteValorInfo> lRetorno = new ConsultarObjetosResponse<ParametroRiscoClienteValorInfo>();
            try
            {

                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_parametro_valor_lst", "@id_cliente_parametro", lRequest.Condicoes[0].Valores[0]);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lRetorno.Resultado.Add(MontarObjeto(ds.Tables[0].Rows[i]));
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public ReceberObjetoResponse<ParametroRiscoClienteValorInfo> ReceberObjeto(ReceberObjetoRequest<ParametroRiscoClienteValorInfo> lRequest)
        {
            ReceberObjetoResponse<ParametroRiscoClienteValorInfo> lRetorno = new ReceberObjetoResponse<ParametroRiscoClienteValorInfo>();
            DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_parametro_valor_sel", "@id_cliente_parametro_valor", int.Parse(lRequest.CodigoObjeto));
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lRetorno.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public RemoverObjetoResponse<ParametroRiscoClienteValorInfo> RemoverObjeto(RemoverObjetoRequest<ParametroRiscoClienteValorInfo> lRequest)
        {
            RemoverObjetoResponse<ParametroRiscoClienteValorInfo> lRetorno = new RemoverObjetoResponse<ParametroRiscoClienteValorInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_parametro_valor_del", "@id_cliente_parametro_valor", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<ParametroRiscoClienteValorInfo> SalvarObjeto(SalvarObjetoRequest<ParametroRiscoClienteValorInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<ParametroRiscoClienteValorInfo> lResponse = new SalvarObjetoResponse<ParametroRiscoClienteValorInfo>();

            paramsProc.Add("@id_cliente_parametro", lRequest.Objeto.ParametroCliente.CodigoParametroCliente);
            paramsProc.Add("@id_cliente_parametro_valor", lRequest.Objeto.CodigoParametroClienteValor);
            paramsProc.Add("@vl_alocado", lRequest.Objeto.ValorAlocado);
            paramsProc.Add("@vl_disponivel", lRequest.Objeto.ValorDisponivel);

            paramsProc.Add("@ds_historico", lRequest.Objeto.Descricao);

            paramsProc.Add("@dt_movimento", lRequest.Objeto.DataMovimento);


            DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_parametro_valor_salvar", paramsProc);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
            }

            return lResponse;
        }

        #endregion

        private ParametroRiscoClienteValorInfo MontarObjeto(DataRow dr)
        {
            ParametroRiscoClienteValorInfo lRetorno = new ParametroRiscoClienteValorInfo();

            lRetorno.CodigoParametroClienteValor = (int)dr["id_cliente_parametro_valor"];
            lRetorno.Descricao = dr["ds_historico"].ToString();
            lRetorno.ValorAlocado = (decimal)dr["vl_alocado"];
            lRetorno.ValorDisponivel = (decimal)dr["vl_disponivel"];
            lRetorno.DataMovimento = (DateTime)dr["dt_movimento"];

            return lRetorno;
        }
    }
}
