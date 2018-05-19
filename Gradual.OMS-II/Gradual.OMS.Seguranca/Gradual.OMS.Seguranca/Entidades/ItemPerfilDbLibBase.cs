using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{

    public class ItemPerfilDbLibBase
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

            if (int.TryParse(pCodigoItem, out lCodigoitem))
                itemParametro = lCodigoitem;
            else
                itemParametro = pCodigoItem;


            List<string> lRetorno = new List<string>();
            // Lista de parametros para a procedure
            // Faz a consulta no banco
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcSel, "@CodigoItem", itemParametro);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lRetorno.Add(dr["CodigoPerfil"].ToString());
            }
            return lRetorno;
        }

        public List<PerfilInfo> ConsultarObjetos2(string pCodigoItem)
        {

            object itemParametro;
            int lCodigoitem = 0;

            if (int.TryParse(pCodigoItem, out lCodigoitem))
                itemParametro = lCodigoitem;
            else
                itemParametro = pCodigoItem;

            List<PerfilInfo> lRetorno = new List<PerfilInfo>();
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

        public void RemoverObjeto(string pCodigoItem, string codigoPerfil)
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
            paramsProc.Add("@CodigoPerfil", int.Parse(codigoPerfil));

            DbLib.ExecutarProcedure(
                NomeProcDel, paramsProc, new List<string>());

        }

        public void SalvarObjeto(string codigoItem, string codigoPerfil)
        {
            // Monta a execução da procedure

            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            paramsProc.Add("@CodigoItem", int.Parse(codigoItem));
            paramsProc.Add("@CodigoPerfil", int.Parse(codigoPerfil));

            DbLib.ExecutarProcedure(
                NomeProcIns, paramsProc, new List<string>());

        }

        private PerfilInfo MontarObjeto(DataRow dr)
        {
            return new PerfilInfo()
            {
                CodigoPerfil = dr["CodigoPerfil"].ToString(),
                NomePerfil = dr["NomePerfil"].ToString()
            };
        }

    }
}
