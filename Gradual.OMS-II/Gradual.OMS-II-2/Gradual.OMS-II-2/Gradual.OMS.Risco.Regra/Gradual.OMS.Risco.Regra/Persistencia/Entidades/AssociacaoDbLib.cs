using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using log4net;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;
using System.Data;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class AssociacaoDbLib : IEntidadeDbLib<AssociacaoClienteRiscoInfo>
    {

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public OMS.Persistencia.ConsultarObjetosResponse<AssociacaoClienteRiscoInfo> ConsultarObjetos(OMS.Persistencia.ConsultarObjetosRequest<AssociacaoClienteRiscoInfo> lRequest)
        {
            ConsultarObjetosResponse<AssociacaoClienteRiscoInfo> lRetorno = new ConsultarObjetosResponse<AssociacaoClienteRiscoInfo>();
            try
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                foreach (CondicaoInfo ci in lRequest.Condicoes)
                {
                    parametros.Add(ci.Propriedade, ci.Valores[0]);
                }

                DataSet ds = _dbLib.ExecutarConsulta("prc_associacao_lst", parametros);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lRetorno.Resultado.Add(MontarObjeto(ds.Tables[0].Rows[i]));
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public OMS.Persistencia.ReceberObjetoResponse<AssociacaoClienteRiscoInfo> ReceberObjeto(OMS.Persistencia.ReceberObjetoRequest<AssociacaoClienteRiscoInfo> lRequest)
        {
            ReceberObjetoResponse<AssociacaoClienteRiscoInfo> lRetorno = new ReceberObjetoResponse<AssociacaoClienteRiscoInfo>();
            try
            {
                string[] lParametros = lRequest.CodigoObjeto.Split('.');
                int lTipoAssociacao = int.Parse(lParametros[0]); // Prametro = 0, Permissao = 1
                int lId = int.Parse(lParametros[1]);

                Dictionary<string, object> paramsProc = new Dictionary<string, object>();
                paramsProc.Add("@tipo", lTipoAssociacao);  // Parametro = 1, Permissao = 2
                paramsProc.Add("@id", lId);

                DataSet ds = _dbLib.ExecutarProcedure("prc_associacao_sel", paramsProc);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lRetorno.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }


                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public OMS.Persistencia.RemoverObjetoResponse<AssociacaoClienteRiscoInfo> RemoverObjeto(OMS.Persistencia.RemoverObjetoRequest<AssociacaoClienteRiscoInfo> lRequest)
        {
            RemoverObjetoResponse<AssociacaoClienteRiscoInfo> lRetorno = new RemoverObjetoResponse<AssociacaoClienteRiscoInfo>();
            try
            {
                string[] lParametros = lRequest.CodigoObjeto.Split('.');
                int lTipoAssociacao = int.Parse(lParametros[0]); // Prametro = 1, Permissao = 2
                int lId = int.Parse(lParametros[1]);

                Dictionary<string, object> paramsProc = new Dictionary<string, object>();
                paramsProc.Add("@tipo", lTipoAssociacao);  // Parametro = 1, Permissao = 2
                paramsProc.Add("@id", lId);

                DataSet ds = _dbLib.ExecutarProcedure("prc_associacao_del", paramsProc);

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public OMS.Persistencia.SalvarObjetoResponse<AssociacaoClienteRiscoInfo> SalvarObjeto(OMS.Persistencia.SalvarObjetoRequest<AssociacaoClienteRiscoInfo> lRequest)
        {
            SalvarObjetoResponse<AssociacaoClienteRiscoInfo> lRetorno = new SalvarObjetoResponse<AssociacaoClienteRiscoInfo>();
            try
            {
                Dictionary<string, object> paramsProc = new Dictionary<string, object>();

                paramsProc.Add("@tipo_associacao", (int)lRequest.Objeto.TipoAssociacao);
                paramsProc.Add("@id_cliente", lRequest.Objeto.CodigoCliente);
                paramsProc.Add("@id_grupo", lRequest.Objeto.CodigoGrupo);
                //PARAMETRO
                paramsProc.Add("@id_cliente_parametro", lRequest.Objeto.CodigoClienteParametro);
                paramsProc.Add("@id_parametro", lRequest.Objeto.CodigoParametro);
                paramsProc.Add("@vl_parametro", lRequest.Objeto.ValorParametro);
                paramsProc.Add("@dt_validade", lRequest.Objeto.DataValidadeParametro);
                //PERMISSAO
                paramsProc.Add("@id_cliente_permissao", lRequest.Objeto.CodigoClientePermissao);
                paramsProc.Add("@id_permissao", lRequest.Objeto.CodigoPermissao);

                DataSet ds = _dbLib.ExecutarProcedure("prc_associacao_salvar", paramsProc);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lRetorno.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }



        private AssociacaoClienteRiscoInfo MontarObjeto(DataRow lRequest)
        {

            AssociacaoClienteRiscoInfo lRetorno = new AssociacaoClienteRiscoInfo();

            lRetorno.TipoAssociacao = (AssociacaoClienteRiscoInfo.eTipoAssociacao)(ToInt(lRequest["tipo_associacao"]));
            lRetorno.CodigoCliente = ToInt(lRequest["id_cliente"]);
            lRetorno.CodigoGrupo = ToInt(lRequest["id_grupo"]);
            //PARAMETRO
            lRetorno.CodigoClienteParametro = ToInt(lRequest["id_cliente_parametro"]);
            lRetorno.CodigoParametro = ToInt(lRequest["id_parametro"]);
            lRetorno.ValorParametro = ToDecimal(lRequest["vl_parametro"]);
            lRetorno.DataValidadeParametro = (DateTime)lRequest["dt_validade"];
            lRetorno.DescricaoParametro = lRequest["ds_parametro"].ToString();
            //PERMISSAO
            lRetorno.CodigoClientePermissao = ToInt(lRequest["id_cliente_permissao"]);
            lRetorno.CodigoPermissao = ToInt(lRequest["id_permissao"]);         
            lRetorno.DescricaoPermissao = lRequest["ds_permissao"].ToString();
            return lRetorno;

        }


        private int ToInt(object pEntrada)
        {
            int retorno = default(int);

            if (pEntrada != null && int.TryParse(pEntrada.ToString(), out retorno))
                return retorno;
            else
                return 0;
        }

        private decimal ToDecimal(object pEntrada)
        {
            decimal retorno = default(decimal);

            if (pEntrada != null && decimal.TryParse(pEntrada.ToString(), out retorno))
                return retorno;
            else
                return 0;
        }

    }
}
