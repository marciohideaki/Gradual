using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;

namespace Gradual.OMS.Risco.Regra
{

    public static class LogRisco
    {

        public static string DBToString(object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "";

            return pObject.ToString();
        }


        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum eAcao
        {
            Salvar,
            Listar,
            Receber,
            Excluir
        }


        private static string GetCabacalho(object Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao)
        {
            return
            "[Entidade] " + Entidade.GetType().Name +
                " ; [Ação] " + Acao.ToString() +
                " ; [IdUsuarioLogado (ID_LOGIN)] " + DBToString(IdUsuarioLogado) +
                " ; [DescricaoUsuarioLogado] " + DBToString(DescricaoUsuarioLogado);
        }



        public static void Logar(object Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao, Exception Ex = null)
        {
            string lCabecalho = GetCabacalho(Entidade, IdUsuarioLogado, DescricaoUsuarioLogado, Acao);

            string lConteudo = "";

            try
            {

                lConteudo = Entidade.ToString();

     

                if (null == Ex)
                    logger.Info(lCabecalho + lConteudo);
                else
                    logger.Error(lCabecalho + lConteudo, Ex);

            }
            catch (Exception ex)
            {
                logger.Error("[Erro ao Salvar Log] ", ex);
            }

        }

    }
}
