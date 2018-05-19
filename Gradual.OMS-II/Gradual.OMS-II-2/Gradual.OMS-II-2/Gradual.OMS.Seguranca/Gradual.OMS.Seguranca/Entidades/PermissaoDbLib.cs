using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Persistencia;
using System.Data;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class PermissaoDbLib : IEntidadeDbLib<PermissaoInfo>
    {
        private DbLib _dbLib = new DbLib("Seguranca");

        public const string NomeProcSalvar = "prc_Permissao_salvar";
        public const string NomeProcSel = "prc_Permissao_sel";
        public const string NomeProcLst = "prc_Permissao_sel";
        public const string NomeProcDel = "prc_Permissao_del";




        #region IEntidadeDbLib<PermissaoInfo> Members

        public ConsultarObjetosResponse<PermissaoInfo> ConsultarObjetos(ConsultarObjetosRequest<PermissaoInfo> parametros)
        {
            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            // Monta a execução da procedure e executa

            foreach (CondicaoInfo condicao in parametros.Condicoes)
            {
                paramsProc.Add("@" + condicao.Propriedade, condicao.Valores[0]);
            }

            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcLst, paramsProc, new List<string>());

            // Preenche a coleção resultado
            List<PermissaoInfo> resultado = new List<PermissaoInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                resultado.Add(this.MontarObjeto(dr));

            // Retorna
            return
                new ConsultarObjetosResponse<PermissaoInfo>()
                {
                    Resultado = resultado
                };
        }

        public ReceberObjetoResponse<PermissaoInfo> ReceberObjeto(ReceberObjetoRequest<PermissaoInfo> parametros)
        {
            // Faz a consulta no banco
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcSel, "@CodigoPermissao", parametros.CodigoObjeto);

            // Monta o objeto
            PermissaoInfo permissao = ds.Tables[0].Rows.Count > 0 ? this.MontarObjeto(ds.Tables[0].Rows[0]) : null;

            // Retorna
            return
                new ReceberObjetoResponse<PermissaoInfo>()
                {
                    Objeto = permissao
                };

        }

        public RemoverObjetoResponse<PermissaoInfo> RemoverObjeto(RemoverObjetoRequest<PermissaoInfo> parametros)
        {
            // Monta a execução da procedure
            _dbLib.ExecutarProcedure(
                NomeProcDel,
                "@CodigoPermissao", parametros.CodigoObjeto);

            // Retorna
            return new RemoverObjetoResponse<PermissaoInfo>();
        }

        public SalvarObjetoResponse<PermissaoInfo> SalvarObjeto(SalvarObjetoRequest<PermissaoInfo> parametros)
        {
            DataSet ds = null;

            // Monta parametros
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            string codigoPermissao = string.Empty;

            codigoPermissao = parametros.Objeto.CodigoPermissao;

            if (codigoPermissao == string.Empty)
                codigoPermissao = Guid.NewGuid().ToString();

            paramsProc.Add("@CodigoPermissao", codigoPermissao);
            paramsProc.Add("@NomePermissao", parametros.Objeto.NomePermissao);
            paramsProc.Add("@DescricaoPermissao", parametros.Objeto.DescricaoPermissao);

            ds = _dbLib.ExecutarProcedure(NomeProcSalvar, paramsProc, new List<string>());

            // Execução a procedure

            // Monta o objeto
            PermissaoInfo permissao =
                this.ReceberObjeto(new ReceberObjetoRequest<PermissaoInfo>()
                {
                    CodigoObjeto = this.MontarObjeto(ds.Tables[0].Rows[0]).CodigoPermissao
                }).Objeto;



            // Retorna
            return
                new SalvarObjetoResponse<PermissaoInfo>()
                {
                    Objeto = permissao
                };
        }

        public PermissaoInfo MontarObjeto(DataRow dr)
        {
            PermissaoInfo lRetorno = new PermissaoInfo();
            lRetorno.CodigoPermissao = dr["CodigoPermissao"].ToString();
            lRetorno.DescricaoPermissao = dr["DescricaoPermissao"].ToString();
            lRetorno.NomePermissao = dr["NomePermissao"].ToString();
            return lRetorno;
        }

        #endregion
    }
}
