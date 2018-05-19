using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao.Arena.App_Codigo
{
    public static class ConfiguracoesValidadas
    {
        #region Atributos
        private static bool gIniciar = false;

        private static string _ArenaKey;

        private static string _ArenaUri;
        #endregion

        #region Propriedades
        public static string ArenaKey
        {
            get
            {
                Iniciar();

                return _ArenaKey;
            }
        }

        public static string ArenaUri
        {
            get
            {
                Iniciar();

                return _ArenaUri;
            }
        }
        #endregion

        #region Métodos Private
        private static void Iniciar()
        {
            try
            {
                if (!gIniciar)
                {
                    gIniciar = true;

                    string lValor;

                    //
                    // Arena Key: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["ArenaKey"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception(string.Format("Valor [{0}] para ArenaKey inválido!", lValor));

                    _ArenaKey = lValor;

                    //
                    // Arena Uri: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["ArenaUri"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception(string.Format("Valor [{0}] para ArenaUri inválido!", lValor));

                    _ArenaUri = lValor;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}