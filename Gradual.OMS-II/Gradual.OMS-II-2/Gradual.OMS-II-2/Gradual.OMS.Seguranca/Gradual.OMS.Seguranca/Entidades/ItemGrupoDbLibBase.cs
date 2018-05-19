using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{

    public class ItemGrupoDbLibBase
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
            set
            {
                _NomeProcDel = value;
            }
        }

        public DbLib DbLib
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

        public List<string> ConsultarObjetos(string pCodigoItem)
        {
            object itemParametro;
            int lCodigoitem = 0;
            List<string> lRetorno = new List<string>();
            // Lista de parametros para a procedure
            // Faz a consulta no banco

            if(int.TryParse(pCodigoItem, out lCodigoitem))
                itemParametro = lCodigoitem;
            else
                itemParametro = pCodigoItem;


            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcSel, "@CodigoItem", itemParametro);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lRetorno.Add(dr["CodigoGrupo"].ToString());
            }
            return lRetorno;
        }

        public List<UsuarioGrupoInfo> ConsultarObjetos2(string pCodigoItem)
        {

            object itemParametro;
            int lCodigoitem = 0;
            if (int.TryParse(pCodigoItem, out lCodigoitem))
                itemParametro = lCodigoitem;
            else
                itemParametro = pCodigoItem;

            List<UsuarioGrupoInfo> lRetorno = new List<UsuarioGrupoInfo>();
            // Lista de parametros para a procedure
            // Faz a consulta no banco
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcSel, "@CodigoItem", itemParametro);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lRetorno.Add(MontarObjeto(dr));
            }
            return lRetorno;
        }

        public void RemoverObjeto(string pCodigoItem, string codigoGrupo)
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
            paramsProc.Add("@CodigoGrupo", int.Parse(codigoGrupo));

            DbLib.ExecutarProcedure(
                NomeProcDel, paramsProc, new List<string>());

        }

        public void SalvarObjeto(string pCodigoItem, string codigoGrupo)
        {

            object itemParametro;
            int lCodigoitem = 0;
            if (int.TryParse(pCodigoItem, out lCodigoitem))
                itemParametro = lCodigoitem;
            else
                itemParametro = pCodigoItem;
            // Monta a execução da procedure

            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            paramsProc.Add("@CodigoItem", itemParametro);
            paramsProc.Add("@CodigoGrupo", int.Parse(codigoGrupo));

            DbLib.ExecutarProcedure(
                NomeProcIns, paramsProc, new List<string>());

        }

        private UsuarioGrupoInfo MontarObjeto(DataRow dr)
        {
            return new UsuarioGrupoInfo()
            {
                CodigoUsuarioGrupo = dr["CodigoGrupo"].ToString(),
                NomeUsuarioGrupo = dr["NomeGrupo"].ToString()
            };
        }

    }
}
