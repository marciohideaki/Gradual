using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencias.Seguranca.Entidades;
using System.Data;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class ItemPermissaoDbLibBase
    {
        private string _NomeProcSel, _NomeProcIns, _NomeProcDel;

        private DbLib _dbLib;

        public string NomeProcSel
        {
            get
            {
                return _NomeProcSel; 
            }
            set
            {
                _NomeProcSel = value;
            }
        }

        public string NomeProcIns
        {
            get
            {
                return _NomeProcIns;
            }
            set
            {
                _NomeProcIns = value;
            }
        }
        
        public string NomeProcDel
        {
            get
            {
                return _NomeProcDel;            
            }
            set{
                _NomeProcDel = value;
            }
        }

        public virtual DbLib DbLib
        {
            get
            {
                return _dbLib;
            }
            set
            {
                _dbLib = value;
            }
        }

        public virtual List<PermissaoAssociadaInfo> ConsultarObjetos(List<CondicaoInfo> parametros)
        {
            List<PermissaoAssociadaInfo> lRetorno = new List<PermissaoAssociadaInfo>();

            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            foreach (CondicaoInfo condicao in parametros)
            {
                paramsProc.Add("@" + condicao.Propriedade, condicao.Valores[0].ToString());
            }

            // Faz a consulta no banco
            DataSet ds =
                DbLib.ExecutarProcedure(NomeProcSel, paramsProc, new List<string>());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lRetorno.Add(MontarObjeto(dr));
            }

            return lRetorno;
        }

        public virtual void RemoverObjeto(string pCodigoItem, string codigoPermissao)
        {

            object itemParametro;
            int lCodigoitem = 0;

            if (int.TryParse(pCodigoItem, out lCodigoitem))
                itemParametro = lCodigoitem;
            else
                itemParametro = pCodigoItem;


            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            paramsProc.Add("@CodigoItem", itemParametro);
            paramsProc.Add("@CodigoPermissao", codigoPermissao);

            DbLib.ExecutarProcedure(
                NomeProcDel, paramsProc, new List<string>());

        }

        public virtual void SalvarObjeto(PermissaoAssociadaInfo permissaoAssociada, string codigoItem)
        {
            // Monta a execução da procedure

            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            paramsProc.Add("@CodigoItem", codigoItem);
            paramsProc.Add("@CodigoPermissao", permissaoAssociada.CodigoPermissao);
            paramsProc.Add("@Status", permissaoAssociada.Status);

            DbLib.ExecutarProcedure(
                NomeProcIns, paramsProc, new List<string>());

        }

        private PermissaoAssociadaInfo MontarObjeto(DataRow dr)
        {
            return new PermissaoAssociadaInfo()
            {
                CodigoPermissao = dr["CodigoPermissao"].ToString(),
                Status = (PermissaoAssociadaStatusEnum)int.Parse(dr["Status"].ToString())
                /*PermissaoInfo = new PermissaoInfo()
                {
                    CodigoPermissao = dr["CodigoPermissao"].ToString(),
                    DescricaoPermissao = dr["DescricaoPermissao"].ToString(),
                    NomePermissao = dr["NomePermissao"].ToString()
                }*/

            };
        }
    }
}
