using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using System.Data;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Newtonsoft.Json;

namespace Gradual.Intranet.Servicos.Mock
{
    public class ServicoPersistenciaCadastro : IServicoPersistenciaCadastro
    {
        #region Métodos Private

        private List<ClienteResumidoInfo> ConsultarEntidadeCadastroClienteResumidoInfo(ClienteResumidoInfo pParametros)
        {
            List<ClienteResumidoInfo> lResposta = new List<ClienteResumidoInfo>();

            string lFiltro = "";

            if(!pParametros.TermoDeBusca.Contains('*'))
            {
                pParametros.TermoDeBusca = string.Format("*{0}*", pParametros.TermoDeBusca);    //implementado como a versão real, que inclui * automaticamente
            }

            switch (pParametros.OpcaoBuscarPor)
            {
                case OpcoesBuscarPor.CodBovespa:

                    lFiltro = string.Format("CodBovespa LIKE '{0}'", pParametros.TermoDeBusca);

                    break;
                case OpcoesBuscarPor.CpfCnpj:

                    lFiltro = string.Format("CPF LIKE '{0}'", pParametros.TermoDeBusca);

                    break;
                case OpcoesBuscarPor.NomeCliente:

                    lFiltro = string.Format("NomeCliente LIKE '{0}'", pParametros.TermoDeBusca);

                    break;
            }


            if (pParametros.OpcaoStatus == OpcoesStatus.Ativo)
            {
                lFiltro += " AND Status = 'Ativo'";
            }
            else if (pParametros.OpcaoStatus == OpcoesStatus.Inativo)
            {
                lFiltro += " AND Status = 'Inativo'";
            }
            else
            {
                //os dois valores
            }

            switch (pParametros.OpcaoPasso)
            {
                case OpcoesPasso.Visitante:

                    lFiltro += " AND Passo = 'Visitante'";

                    break;
                case OpcoesPasso.Cadastrado:

                    lFiltro += " AND Passo = 'Cadastrado'";

                    break;
                case OpcoesPasso.Exportado:

                    lFiltro += " AND Passo = 'Exportado'";

                    break;
                case OpcoesPasso.Visitante | OpcoesPasso.Cadastrado:

                    lFiltro += " AND (Passo = 'Visitante' OR Passo = 'Cadastrado')";

                    break;
                case OpcoesPasso.Visitante | OpcoesPasso.Exportado:

                    lFiltro += " AND (Passo = 'Visitante' OR Passo = 'Exportado')";

                    break;
                case OpcoesPasso.Cadastrado | OpcoesPasso.Exportado:

                    lFiltro += " AND (Passo = 'Cadastrado' OR Passo = 'Exportado')";

                    break;

                default:

                    //todos então deixa

                    break;
            }

            if (pParametros.OpcaoPendencia == OpcoesPendencia.ComPendenciaCadastral)
            {
                lFiltro += " AND FlagPendencia = 'Cadastral'";
            }
            else if (pParametros.OpcaoPendencia == OpcoesPendencia.ComSolicitacaoAlteracao)
            {
                lFiltro += " AND FlagPendencia = 'Alteracao'";
            }
            else
            {
                //os dois valores
            }

            DataView lView = new DataView(ArmazenDeMocks.TabelaDeClientes);

            lView.RowFilter = lFiltro;

            DataTable lTable = lView.ToTable();

            foreach (DataRow lRow in lTable.Rows)
            {
                lResposta.Add(FabricaDeMocks.GerarResumoDoCliente(lRow));
            }

            return lResposta;
        }

        private ClienteInfo RetornarClienteInfo(string pCodCliente)
        {
            int lId;

            ClienteInfo lRetorno = new ClienteInfo();

            if(Int32.TryParse(pCodCliente, out lId))
            {
                foreach (DataRow lRow in ArmazenDeMocks.TabelaDeClientes.Rows)
                {
                    if (lId == lRow[0].DBToInt32())
                    {
                        lRetorno = FabricaDeMocks.GerarClienteInfo(lRow);

                        break;
                    }
                }
            }

            return lRetorno;
        }

        private ClienteSituacaoFinanceiraPatrimonialInfo RetornarClienteSituacaoFinanceiraPatrimonialInfo(string pIdCliente)
        {
            return FabricaDeMocks.GerarClienteSituacaoFinanceiraPatrimonialInfo(pIdCliente.DBToInt32(), null);
        }

        private List<ClienteTelefoneInfo> ConsultarEntidadeCadastroClienteTelefoneInfo(ClienteTelefoneInfo pParametros)
        {
            List<ClienteTelefoneInfo> lRetorno = new List<ClienteTelefoneInfo>();

            lRetorno.Add(new ClienteTelefoneInfo()
            {
                DsDdd = "11",
                DsNumero = "33445566",
                IdCliente = pParametros.IdCliente,
                IdTelefone = 100,
                IdTipoTelefone = 1,
                StPrincipal = true
            });

            lRetorno.Add(new ClienteTelefoneInfo()
            {
                DsDdd = "11",
                DsNumero = "99887766",
                IdCliente = pParametros.IdCliente,
                IdTelefone = 101,
                IdTipoTelefone = 2,
                StPrincipal = false
            });

            return lRetorno;
        }
        
        private List<ClienteEnderecoInfo> ConsultarEntidadeCadastroClienteEnderecoInfo(ClienteEnderecoInfo pParametros)
        {
            List<ClienteEnderecoInfo> lRetorno = new List<ClienteEnderecoInfo>();

            lRetorno.Add(new ClienteEnderecoInfo()
            {
                  CdPais = "BRA"
                , CdUf = "SP"
                , DsBairro = "Santo Amaro"
                , DsCidade = "São Paulo"
                , DsComplemento = "Fds"
                , DsLogradouro = "Av Sobe e Desce"
                , DsNumero = "369"
                , IdCliente = pParametros.IdCliente
                , IdEndereco = 1
                , IdTipoEndereco = 1
                , NrCep = 14788
                , NrCepExt = 222
                , StPrincipal = true
            });

            return lRetorno;
        }

        private List<TipoEnderecoInfo> ConsultarEntidadeCadastroTipoEnderecoInfo()
        {
            List<TipoEnderecoInfo> lRetorno = new List<TipoEnderecoInfo>();
            
            lRetorno.Add(new TipoEnderecoInfo() { IdTipoEndereco = 1, DsEndereco = "Comercial" });
            lRetorno.Add(new TipoEnderecoInfo() { IdTipoEndereco = 2, DsEndereco = "Residencial" });
            lRetorno.Add(new TipoEnderecoInfo() { IdTipoEndereco = 3, DsEndereco = "Outros" });

            return lRetorno;
        }
        
        private List<ClienteBancoInfo> ConsultarEntidadeCadastroClienteBancoInfo(ClienteBancoInfo pParametros)
        {
            List<ClienteBancoInfo> lRetorno = new List<ClienteBancoInfo>();

            lRetorno.Add(new ClienteBancoInfo() { CdBanco = "341", DsAgencia = "700", DsConta = "700700", DsContaDigito = "1", IdBanco = 56751, IdCliente = pParametros.IdCliente, StPrincipal = true,  TpConta = "CC" });

            lRetorno.Add(new ClienteBancoInfo() { CdBanco = "341", DsAgencia = "701", DsConta = "700701", DsContaDigito = "2", IdBanco = 56751, IdCliente = pParametros.IdCliente, StPrincipal = false, TpConta = "CC" });

            return lRetorno;
        }

        private List<ClienteCadastradoPeriodoInfo> ConsultarRelatorio_ClienteCadastradoPeriodoInfo(ClienteCadastradoPeriodoInfo pParametros)
        {
            List<ClienteCadastradoPeriodoInfo> lResposta = new List<ClienteCadastradoPeriodoInfo>();

            foreach (DataRow lRow in ArmazenDeMocks.TabelaDeClientes.Rows)
            {
                lResposta.Add(FabricaDeMocks.GerarRelatorio_ClienteCadastradoPorPeriodo(lRow));
            }

            return lResposta;
        }

        private List<ClienteProcuradorRepresentanteInfo> ConsultarEntidadeCadastroClienteProcuradorRepresentanteInfo(ClienteProcuradorRepresentanteInfo pParametros)
        {
            List<ClienteProcuradorRepresentanteInfo> lRetorno = new List<ClienteProcuradorRepresentanteInfo>();

            ClienteProcuradorRepresentanteInfo lInfo = new ClienteProcuradorRepresentanteInfo();

            lInfo.IdCliente = pParametros.IdCliente;
            lInfo.IdProcuradorRepresentante = 1;
            lInfo.NrCpfCnpj = "11111111111";
            lInfo.DsNome = "Marcos Mockado da Silva";
            lInfo.DsNumeroDocumento = "22222222";
            lInfo.TpDocumento = "RG";
            lInfo.TpSituacaoLegal = 1;
            lInfo.CdOrgaoEmissor = "SSP";
            lInfo.CdUfOrgaoEmissor = "SP";
            lInfo.DtNascimento = DateTime.Now.AddYears(-42);

            lRetorno.Add(lInfo);

            return lRetorno;
        }
        
        private List<ClienteEmitenteInfo> ConsultarEntidadeCadastroClienteProcuradorRepresentanteInfo(ClienteEmitenteInfo pParametros)
        {
            List<ClienteEmitenteInfo> lRetorno = new List<ClienteEmitenteInfo>();

            ClienteEmitenteInfo lInfo = new ClienteEmitenteInfo();

            lInfo.CdSistema = "BOL";
            lInfo.DsData = DateTime.Now.AddDays(-1);
            lInfo.DsEmail = "mock@teste.com.br";
            lInfo.DsNome = "Maurício Mockado da Silva";
            lInfo.DsNumeroDocumento = "11111111111";
            lInfo.DtNascimento = DateTime.Now.AddYears(-28);
            lInfo.IdCliente = pParametros.IdCliente;
            lInfo.IdPessoaAutorizada = pParametros.IdPessoaAutorizada;
            lInfo.NrCpfCnpj = "22222222222";
            lInfo.StPrincipal = true;

            lRetorno.Add(lInfo);

            return lRetorno;
        }
        
        private List<ClienteContratoInfo> ConsultarEntidadeCadastroClienteContratoInfo(ClienteContratoInfo pParametros)
        {
            List<ClienteContratoInfo> lRetorno = new List<ClienteContratoInfo>();

            ClienteContratoInfo lInfo = new ClienteContratoInfo();

            lInfo.IdCliente = pParametros.IdCliente;
            lInfo.DtAssinatura = DateTime.Now.AddDays(-42);
            lInfo.IdContrato = 1;
            lInfo.LstIdContrato = new List<int>();
            
            lInfo.LstIdContrato.Add(45);
            lInfo.LstIdContrato.Add(46);

            lRetorno.Add(lInfo);

            return lRetorno;
        }
        
        private List<ContratoInfo> ConsultarEntidadeCadastroContratoInfo()
        {
            List<ContratoInfo> lRetorno = new List<ContratoInfo>();

            lRetorno = JsonConvert.DeserializeObject<List<ContratoInfo>>("[{\"IdContrato\":45,\"DsContrato\":\"CVM 301\",\"DsPath\":\"1\",\"StObrigatorio\":false},{\"IdContrato\":46,\"DsContrato\":\"CVM 387\",\"DsPath\":\"1\",\"StObrigatorio\":false},{\"IdContrato\":47,\"DsContrato\":\"Regras e Parâmetros de Atuação\",\"DsPath\":\"1\",\"StObrigatorio\":false},{\"IdContrato\":44,\"DsContrato\":\"Contrato de Intermediação e Custódia\",\"DsPath\":\"2\",\"StObrigatorio\":true}]");

            return lRetorno;
        }

        private List<ClienteAlteracaoInfo> ConsultarEntidadeCadastroClienteAlteracaoInfo(ClienteAlteracaoInfo pParametros)
        {
            List<ClienteAlteracaoInfo> lRetorno = new List<ClienteAlteracaoInfo>();

            lRetorno.Add(new ClienteAlteracaoInfo()
            {
                CdTipo='A',
                 DsDescricao = "Descrição",
                  DsInformacao = "Dados Cadastrais",
                   DtRealizacao = null,
                    DtSolicitacao = DateTime.Now,
                     IdAlteracao = 1,
                      IdLoginRealizacao = null,
                IdCliente = pParametros.IdCliente

            });

            return lRetorno;
        }

        #endregion

        #region IServicoPersistenciaCadastro Members

        public ConsultarEntidadeCadastroResponse<T> ConsultarEntidadeCadastro<T>(ConsultarEntidadeCadastroRequest<T> pParametros) where T : ICodigoEntidade
        {
            ConsultarEntidadeCadastroResponse<T> lResposta = new ConsultarEntidadeCadastroResponse<T>();

            if (typeof(T) == typeof(ClienteResumidoInfo))
            {
                List<ClienteResumidoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteResumidoInfo(pParametros.EntidadeCadastro as ClienteResumidoInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteTelefoneInfo))
            {
                List<ClienteTelefoneInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteTelefoneInfo(pParametros.EntidadeCadastro as ClienteTelefoneInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteEnderecoInfo))
            {
                List<ClienteEnderecoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteEnderecoInfo(pParametros.EntidadeCadastro as ClienteEnderecoInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(TipoEnderecoInfo))
            {
                List<TipoEnderecoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroTipoEnderecoInfo();

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteBancoInfo))
            {
                List<ClienteBancoInfo> lRespostaBusca;
                
                lRespostaBusca = ConsultarEntidadeCadastroClienteBancoInfo(pParametros.EntidadeCadastro as ClienteBancoInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteCadastradoPeriodoInfo))
            {
                List<ClienteCadastradoPeriodoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarRelatorio_ClienteCadastradoPeriodoInfo(pParametros.EntidadeCadastro as ClienteCadastradoPeriodoInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteSuitabilityInfo))
            {
                List<ClienteSuitabilityInfo> lRespostaBusca;

                lRespostaBusca = new List<ClienteSuitabilityInfo>();

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteProcuradorRepresentanteInfo))
            {
                List<ClienteProcuradorRepresentanteInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteProcuradorRepresentanteInfo(pParametros.EntidadeCadastro as ClienteProcuradorRepresentanteInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteEmitenteInfo))
            {
                List<ClienteEmitenteInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteProcuradorRepresentanteInfo(pParametros.EntidadeCadastro as ClienteEmitenteInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteContratoInfo))
            {
                List<ClienteContratoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteContratoInfo(pParametros.EntidadeCadastro as ClienteContratoInfo);

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if(typeof(T) == typeof(ContratoInfo))
            {
                List<ContratoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroContratoInfo();

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(PessoaExpostaPoliticamenteInfo))
            {
                List<PessoaExpostaPoliticamenteInfo> lRespostaBusca;

                lRespostaBusca = FabricaDeMocks.GerarListaDePEP();

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(ClienteAlteracaoInfo))
            {
                List<ClienteAlteracaoInfo> lRespostaBusca;

                lRespostaBusca = ConsultarEntidadeCadastroClienteAlteracaoInfo(pParametros.EntidadeCadastro as ClienteAlteracaoInfo);
    

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else if (typeof(T) == typeof(SinacorListaInfo))
            {
                List<SinacorListaInfo> lRespostaBusca;

                string lChave = ( pParametros as ConsultarEntidadeCadastroRequest<SinacorListaInfo>).EntidadeCadastro.Informacao.ToString();

                //string lChave = pParametros.EntidadeCadastro.GetType().ToString();

                if(!ArmazenDeMocks.ListaDoSinacor.ContainsKey(lChave))
                    throw new Exception(string.Format("Sem Lista do Sinacor disponível no Armazém de Mocks para chave [{0}]", lChave));

                lRespostaBusca = ArmazenDeMocks.ListaDoSinacor[lChave];

                lResposta.Resultado = lRespostaBusca.OfType<T>().ToList<T>();

                return lResposta;
            }
            else
            {
                throw new NotImplementedException(string.Format("Mock ainda não implementado para [{0}]", typeof(T)));
            }
        }

        public SalvarEntidadeCadastroResponse SalvarEntidadeCadastro<T>(SalvarEntidadeCadastroRequest<T> pParametros) where T : ICodigoEntidade
        {
            SalvarEntidadeCadastroResponse lResposta = new SalvarEntidadeCadastroResponse();

            //lResposta.Status = MensagemStatusEnum.Respondida;
            lResposta.StatusResposta = MensagemResponseStatusEnum.OK;

            /*
             TODO: Aqui temos um problema: nós sabemos se é inclusão ou alteração se o ID do objeto for nulo ou não; sendo inclusão,
                   temos que responder um ID novo (como o Random() abaixo); se não, não retornamos nada. Porém, somente via ICodigoEntidade
                   não temos como saber o ID, teríamos que fazer um switch com todos os tipos aqui. Para testar alteração, basta descomentar 
                   abaixo, retornando uma DescricaoResposta => Luciano
             */
            //lResposta.DescricaoResposta = new Random().Next(25, 100).ToString();


            return lResposta;
        }

        public ReceberEntidadeCadastroResponse<T> ReceberEntidadeCadastro<T>(ReceberEntidadeCadastroRequest<T> pParametros) where T : ICodigoEntidade
        {
            if (typeof(T) == typeof(ClienteInfo))
            {
                ReceberEntidadeCadastroResponse<ClienteInfo> lResponse = new ReceberEntidadeCadastroResponse<ClienteInfo>();

                lResponse.EntidadeCadastro = RetornarClienteInfo(pParametros.CodigoEntidadeCadastro);
                
                lResponse.CodigoMensagemRequest = pParametros.CodigoMensagem;
                
                return lResponse as ReceberEntidadeCadastroResponse<T>;
            }
            else if (typeof(T) == typeof(ClienteSituacaoFinanceiraPatrimonialInfo))
            {
                ReceberEntidadeCadastroResponse<ClienteSituacaoFinanceiraPatrimonialInfo> lResponse = new ReceberEntidadeCadastroResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();

                lResponse.EntidadeCadastro = RetornarClienteSituacaoFinanceiraPatrimonialInfo(pParametros.CodigoEntidadeCadastro);

                lResponse.CodigoMensagemRequest = pParametros.CodigoMensagem;

                return lResponse as ReceberEntidadeCadastroResponse<T>;
            }
            else
            {
                return null;
            }
        }

        public RemoverEntidadeCadastroResponse RemoverEntidadeCadastro<T>(RemoverEntidadeCadastroRequest<T> pParametros) where T : ICodigoEntidade
        {


            throw new NotImplementedException();
        }

        #endregion
    }
}
