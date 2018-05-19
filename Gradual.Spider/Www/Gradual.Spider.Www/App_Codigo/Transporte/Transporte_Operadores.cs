using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Spider.DbLib;
using Gradual.Spider.Lib;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.Www.App_Codigo.Transporte
{
    public class Transporte_Operadores
    {
        #region Propriedades

        public string Nome                      { get; set; }

        public string Email                     { get; set; }

        public string Id                        { get; set; }

        public string Senha                     { get; set; }

        public string CodAssessor               { get; set; }

        public string CodAssessorAssociado      { get; set; }

        public string TipoAcesso                { get; set; }

        public string CodigoOperador            { get; set; }

        public string Sigla                     { get; set; }

        public string Sessao                    { get; set; }

        public string Data                      { get; set; }

        public string AcessaPlataforma          { get; set; }

        public string CodigoLocalidade          { get; set; }

        public string Localidade                { get; set; }

        public List<Transporte_Operadores> rows { get; set; }

        public string records                   { get; set; }

        public string total                     { get; set; }

        public string page                      { get; set; }
        /// <summary>
        /// (get) Descrição completa para aparecer na listagem
        /// </summary>
        public string Descricao
        {
            get
            {
                return string.Format("Nome: {0}, Email: {1}", this.Nome, this.Email);
            }
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "Usuario"; } }

        #endregion
        #region Construtores

        public Transporte_Operadores() { }

        public Transporte_Operadores(UsuarioInfo pUsuarioInfo, AssessorInfo AssessorComplementar)
        {
            this.Id         = pUsuarioInfo.CodigoUsuario;
            this.Nome       = pUsuarioInfo.Nome;
            this.Email      = pUsuarioInfo.Email;
            this.Senha      = pUsuarioInfo.Senha;
            this.TipoAcesso = pUsuarioInfo.CodigoTipoAcesso.ToString();

            if ((eTipoAcesso)pUsuarioInfo.CodigoTipoAcesso == eTipoAcesso.Assessor)
            {
                this.CodAssessor          = pUsuarioInfo.CodigoAssessor.ToString();
                this.CodAssessorAssociado = pUsuarioInfo.CodigosFilhoAssessor;
            }

            if (AssessorComplementar != null)
            {
                this.CodigoOperador       = AssessorComplementar.CodigoOperador;
                this.Sigla                = AssessorComplementar.CodigoSigla;
                this.Localidade           = AssessorComplementar.DsLocalidade;
                this.Data                 = AssessorComplementar.DtAtualizacao.ToString("dd/MM/yyyy");
                this.AcessaPlataforma     = AssessorComplementar.StAtivo.ToString();
                this.Sessao               = AssessorComplementar.CodigoSessao;
                this.CodAssessorAssociado = AssessorComplementar.ListaAssessoresFilhos;
                this.CodigoLocalidade     = AssessorComplementar.CodigoLocalidade.DBToString();
            }
        }

        public List<Transporte_Operadores> TraduzirLista(List<UsuarioInfo> pListaUsuarios, List<AssessorInfo> pListaAssessores)
        {
            var lRetorno = new List<Transporte_Operadores>();

            foreach (var user in pListaUsuarios)
            {
                var AssessorComplementar = pListaAssessores.Find(assessor => { return assessor.IdLoginAssessor == int.Parse(user.CodigoUsuario); });

                if (AssessorComplementar == null) continue;

                var lTrans = new Transporte_Operadores();

                lTrans.Id         = user.CodigoUsuario;
                lTrans.Nome       = user.Nome;
                lTrans.Email      = user.Email;
                lTrans.Senha      = user.Senha;
                lTrans.TipoAcesso = user.CodigoTipoAcesso.ToString();

                if ((eTipoAcesso)user.CodigoTipoAcesso == eTipoAcesso.Assessor)
                {
                    lTrans.CodAssessor          = user.CodigoAssessor.ToString();
                    lTrans.CodAssessorAssociado = user.CodigosFilhoAssessor;
                }

                if (AssessorComplementar != null)
                {
                    lTrans.CodigoOperador       = AssessorComplementar.CodigoOperador;
                    lTrans.Sigla                = AssessorComplementar.CodigoSigla;
                    lTrans.Localidade           = AssessorComplementar.DsLocalidade;
                    lTrans.Data                 = AssessorComplementar.DtAtualizacao.ToString("dd/MM/yyyy");
                    lTrans.AcessaPlataforma     = AssessorComplementar.StAtivo.ToString();
                    lTrans.Sessao               = AssessorComplementar.CodigoSessao;
                    lTrans.CodAssessorAssociado = AssessorComplementar.ListaAssessoresFilhos;
                    lTrans.CodigoLocalidade     = AssessorComplementar.CodigoLocalidade.DBToString();
                }

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }

        #endregion

        public Transporte_Operadores TraduzirLista(List<AssessorInfo> pLista , Dictionary<int,string> pListaSessao)
        {
            var lRetorno = new Transporte_Operadores();

            lRetorno.rows = new List<Transporte_Operadores>();

            pLista.ForEach(assessor =>
            {
                var lOperador = new Transporte_Operadores();

                lOperador.Id                   = assessor.IdLoginAssessor.ToString();
                lOperador.Email                = assessor.Email;
                lOperador.Nome                 = assessor.NomeAssessor;

                if (!string.IsNullOrEmpty(assessor.CodigoSessao))
                {
                    if (pListaSessao.ContainsKey(assessor.CodigoSessao.DBToInt32()))
                    {
                        lOperador.Sessao = pListaSessao[assessor.CodigoSessao.DBToInt32()];
                    }
                    else
                    {
                        lOperador.Sessao = assessor.CodigoSessao;
                    }
                }
                else
                {
                    lOperador.Sessao = "";
                }

                lOperador.Sigla                = assessor.CodigoSigla;
                lOperador.CodigoOperador       = assessor.CodigoOperador;
                lOperador.CodAssessorAssociado = assessor.ListaAssessoresFilhos;
                lOperador.AcessaPlataforma     = assessor.StAtivo.ToString();
                lOperador.CodAssessor          = assessor.CodigoAssessor.ToString();
                lOperador.Data                 = assessor.DtAtualizacao.ToString("dd/MM/yyyy");
                lOperador.Localidade           = assessor.DsLocalidade;

                lRetorno.rows.Add(lOperador);
            });

            lRetorno.records = lRetorno.rows.Count.ToString();

            lRetorno.page = "1";
            //lRetorno.total = 

            return lRetorno;
        }

        public UsuarioInfo ToUsuarioInfo()
        {
            UsuarioInfo lRetorno = new UsuarioInfo();

            lRetorno.CodigosFilhoAssessor = this.CodAssessorAssociado;
            lRetorno.Nome                 = this.Nome;
            lRetorno.Email                = this.Email;
            lRetorno.CodigoUsuario        = this.Id;
            //lRetorno.Senha                = Criptografia.CalculateMD5Hash(this.Senha);
            int lCodAssessor              = -1;

            if (int.TryParse(this.CodAssessor, out lCodAssessor))
            {
                lRetorno.CodigoAssessor = lCodAssessor;
            }

            int lTipoAcesso = -1;

            if (int.TryParse(this.TipoAcesso, out lTipoAcesso))
            {
                lRetorno.CodigoTipoAcesso = lTipoAcesso;
            }

            return lRetorno;
        }
    }
}