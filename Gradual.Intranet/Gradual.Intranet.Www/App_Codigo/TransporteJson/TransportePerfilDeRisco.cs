using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransportePerfilDeRisco
    {
        #region Propriedades

        /// <summary>
        /// ID do Perfil
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição do Perfil
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// (get) Tipo de Objeto para o javascript
        /// </summary>
        public string TipoDeObjeto { get; set; }

        /// <summary>
        /// Regras definidas para esse perfil
        /// </summary>
        public List<TransportePerfilDeRiscoRegra> Regras { get; set; }

        #endregion

        #region Construtores

        public TransportePerfilDeRisco() { }

        //TODO: Aqui em vez de object é o Tipo do objeto de perfil que vem direto do serviço
        public TransportePerfilDeRisco(object pPerfilInfo) 
        { 
            //this.Id = pPerfilInfo.IdPerfil;
            //this.Descricao = pPerfilInfo.Descricao;

            if (pPerfilInfo.GetType() == typeof(TransportePerfilDeRisco))
            {
                this.Id = ((TransportePerfilDeRisco)pPerfilInfo).Id;
                this.Descricao = ((TransportePerfilDeRisco)pPerfilInfo).Descricao;
            }
        }

        #endregion
    }

    public class TransportePerfilDeRiscoRegra
    {
        #region Propriedades

        //ID do Perfil de Risco que é "pai" dessa regra no banco
        public int ParentId { get; set; }

        public int IdRegra { get; set; }

        public string IdRegraDesc { get; set; }

        public string IdSistema { get; set; }

        public string IdSistemaDesc { get; set; }

        public string Ativo { get; set; }

        public string Prefixo { get; set; }

        public string IdBolsa { get; set; }

        public string IdBolsaDesc { get; set; }

        public List<TransportePerfilDeRiscoValorDeParametro> ValoresDosParametros { get; set; }

        #endregion
    }

    public class TransportePerfilDeRiscoValorDeParametro
    {
        #region Propriedades

        public int IdParametro { get; set; }

        public string Valor { get; set; }

        #endregion
    }
}