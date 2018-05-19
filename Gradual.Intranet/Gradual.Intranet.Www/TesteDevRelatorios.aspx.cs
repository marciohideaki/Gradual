using System;
using Gradual.Intranet;
using Gradual.Intranet.Servicos.Mock;
using System.Data;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;

namespace Gradual.Intranet.Www
{
    public partial class TesteDevRelatorios : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ListarRelatorioClienteCadastradoPeriodoInfo();
                //ListarRelatorioClienteEmailDisparadoPeriodo();
                //ListarRelatorioClienteClienteSemEmailInfo();
                //ListarRelatorioClienteSuspeitoInfo();
                ListarRelatorioClientePendenciaCadastralInfo();
                //ListarRelatorioClientesExportadosSinacorInfo();
            }
        }

        #region ListarRelatorioClienteCadastradoPeriodoInfo
        private void ListarRelatorioClienteCadastradoPeriodoInfo()
        {
            ConsultarEntidadeCadastroResponse<ClienteCadastradoPeriodoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteCadastradoPeriodoInfo>)
            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteCadastradoPeriodoInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteCadastradoPeriodoInfo>()
                {
                    EntidadeCadastro =
                        new ClienteCadastradoPeriodoInfo()
                        {
                            DtDe = new DateTime(2000, 01, 01),
                            DtAte = new DateTime(2010, 05, 21)
                        }
                });
        }
        #endregion

        #region ListarRelatorioClienteEmailDisparadoPeriodo
        private void ListarRelatorioClienteEmailDisparadoPeriodo()
        {
            ConsultarEntidadeCadastroResponse<EmailDisparadoPeriodoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<EmailDisparadoPeriodoInfo>)
            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<EmailDisparadoPeriodoInfo>(
                new ConsultarEntidadeCadastroRequest<EmailDisparadoPeriodoInfo>()
                {
                    EntidadeCadastro = new EmailDisparadoPeriodoInfo() 
                    {
                        DtDe = new DateTime(2000, 01, 01),
                        DtAte = new DateTime(2010, 05, 21),
                        ETipoEmailDisparo = eTipoEmailDisparo.Compliance
                    }
                });
        }
        #endregion

        #region ListarRelatorioClienteClienteSemEmailInfo
        private void ListarRelatorioClienteClienteSemEmailInfo()
        {
            ConsultarEntidadeCadastroResponse<ClienteSemLoginInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteSemLoginInfo>)
            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSemLoginInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteSemLoginInfo>()
                {
                    EntidadeCadastro = new ClienteSemLoginInfo()
                    {
                        
                        DtDe = new DateTime(2010, 01, 01),
                        DtAte = new DateTime(2010, 05, 21),

                        //ETipoEmailDisparo = eTipoEmailDisparo.Compliance
                    }
                });
        }
        #endregion

        #region ListarRelatorioClienteSuspeitoInfo
        private void ListarRelatorioClienteSuspeitoInfo()
        {
            ConsultarEntidadeCadastroResponse<ClienteSuspeitoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteSuspeitoInfo>)
            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuspeitoInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteSuspeitoInfo>()
                {
                    EntidadeCadastro = new ClienteSuspeitoInfo()
                    {
                        
                        DtDe = new DateTime(2010, 01, 01),
                        DtAte = new DateTime(2010, 05, 21),
                        //CdPais = "BRA",
                        CdAtividade = 5
                        //CodigoBolsa
                        
                    }
                });
        }
        #endregion

        #region ListarRelatorioClientePendenciaCadastralRelInfo
        private void ListarRelatorioClientePendenciaCadastralInfo()
        {
            ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralRelInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralRelInfo>)
            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePendenciaCadastralRelInfo>(
                new ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralRelInfo>()
                {
                    EntidadeCadastro = new ClientePendenciaCadastralRelInfo()
                    {
                        
                        DtDe = new DateTime(2010, 01, 01),
                        DtAte = new DateTime(2010, 05, 21),
                        //CodigoAssessor = 22
                        

                    }
                });
        }
        #endregion

        #region ListarRelatorioClientesExportadosSinacorInfo
        private void ListarRelatorioClientesExportadosSinacorInfo()
        {
            ConsultarEntidadeCadastroResponse<ClientesExportadosSinacorInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClientesExportadosSinacorInfo>)
            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientesExportadosSinacorInfo>(
                new ConsultarEntidadeCadastroRequest<ClientesExportadosSinacorInfo>()
                {
                    EntidadeCadastro = new ClientesExportadosSinacorInfo()
                    {
                        
                        DtDe = new DateTime(2010, 01, 01),
                        DtAte = new DateTime(2010, 05, 21),
                        //CodigoAssessor = 22
                    }
                });
        }
        #endregion
    }
}
