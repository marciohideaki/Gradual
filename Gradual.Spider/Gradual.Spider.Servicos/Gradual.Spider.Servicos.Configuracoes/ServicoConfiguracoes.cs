using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Gradual.Spider.Servicos.Configuracoes
{
    public class ServicoConfiguracoes : Gradual.Spider.Servicos.Configuracoes.Lib.IServicoConfiguracoes, Gradual.OMS.Library.Servicos.IServicoControlavel
    {
        private Gradual.OMS.Library.Servicos.ServicoStatus gServicoStatus = Gradual.OMS.Library.Servicos.ServicoStatus.Indefinido;

        public void IniciarServico()
        {
            try
            {
                Gradual.Utils.Logger.Initialize();
                
                gServicoStatus = Gradual.OMS.Library.Servicos.ServicoStatus.EmExecucao;

                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Iniciando servico."), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, String.Format("{0}", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void PararServico()
        {
            try
            {
                gServicoStatus = Gradual.OMS.Library.Servicos.ServicoStatus.Parado;

                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Parando servico."), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, String.Format("{0}", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public Gradual.OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            return gServicoStatus;
        }

        public Lib.Mensageria.BuscarParametrosResponse BuscarParametros(Lib.Mensageria.BuscarParametrosRequest pRequest)
        {
            Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "BuscarParametros"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            Gradual.Generico.Dados.AcessaDados lDados = null;
            System.Data.DataTable lTable = null;
            System.Data.Common.DbCommand lCommand = null;
            Gradual.Spider.Servicos.Configuracoes.Lib.Mensageria.BuscarParametrosResponse lRetorno = new Gradual.Spider.Servicos.Configuracoes.Lib.Mensageria.BuscarParametrosResponse();

            try
            {
                lDados = new Gradual.Generico.Dados.AcessaDados();
                lDados.ConnectionStringName = "SPIDER";

                lCommand = lDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_spider_buscar_parametros");

                lTable = lDados.ExecuteDbDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    lRetorno.Parametros = new List<Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro>();

                    foreach (System.Data.DataRow drParametro in lTable.Rows)
                    {
                        Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro lParametro = new Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro();

                        lParametro.Parameter = (Gradual.Spider.Servicos.Configuracoes.Lib.Classes.ParameterType) Enum.Parse(typeof(Gradual.Spider.Servicos.Configuracoes.Lib.Classes.ParameterType), drParametro["Tipo"].DBToString());
                        lParametro.Value = drParametro["Valor"].DBToString();
                        lParametro.Description = drParametro["Descricao"].DBToString();

                        lRetorno.Parametros.Add(lParametro);
                    }
                }

                lRetorno.StatusResposta = Gradual.OMS.Library.MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lRetorno;
        }
    }
}
