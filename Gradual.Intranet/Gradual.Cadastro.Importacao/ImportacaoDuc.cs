using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Gradual.Cadastro.Entidades;
using Gradual.Cadastro.Negocios;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;

namespace Gradual.Cadastro.Importacao
{
    public class ClienteAntigo
    {
        public int IdCliente { get; set; }
        public Gradual.Cadastro.Entidades.ECliente Cliente { get; set; }
        public int IdAssessor { get; set; }
        public BindingList<Gradual.Cadastro.Entidades.EAlteracao> Alteracao { get; set; }
        public BindingList<Gradual.Cadastro.Entidades.EBensImoveis> BensImoveis { get; set; }
        public BindingList<Gradual.Cadastro.Entidades.EBensOutros> BensOutros { get; set; }
        public BindingList<Gradual.Cadastro.Entidades.EConta> Banco { get; set; }
        public BindingList<Gradual.Cadastro.Entidades.EEndereco> Endereco { get; set; }
        public Gradual.Cadastro.Entidades.ELogin Login { get; set; }
        public Gradual.Cadastro.Entidades.ERepresentante Representante { get; set; }
        public BindingList<Gradual.Cadastro.Entidades.ETelefone> Telefone { get; set; }
    }

    public class ClienteNovo
    {
        public ClienteInfo Cliente { get; set; }
        public List<ClienteAlteracaoInfo> Alteracao { get; set; }
        public ClienteSituacaoFinanceiraPatrimonialInfo Sfp { get; set; }
        public List<ClienteBancoInfo> Banco { get; set; }
        public List<ClienteEnderecoInfo> Endereco { get; set; }
        public LoginInfo Login { get; set; }
        public List<ClienteProcuradorRepresentanteInfo> Representante { get; set; }
        public List<ClienteTelefoneInfo> Telefone { get; set; }
        public List<ContratoInfo> Contrato { get; set; }
    }

    public class ImportacaoDuc
    {
        public IServicoPersistenciaCadastro ServicoPersistenciaCadastro
        {
            get
            {
                return Ativador.Get<IServicoPersistenciaCadastro>();
            }
        }

        public List<int> GetIdClientes()
        {
            NCliente _NCliente = new NCliente();
            return _NCliente.ListarPasso123();
        }

        public ClienteAntigo GetClienteDuc(int pIdClienteDuc)
        {
            ClienteAntigo lRetorno = new ClienteAntigo();

            lRetorno.IdCliente = pIdClienteDuc;
            lRetorno.Cliente = new NCliente().Listar(pIdClienteDuc);
            lRetorno.Alteracao = new NAlteracao().Listar(pIdClienteDuc, NAlteracao.eStatus.Todas);
            lRetorno.Banco = new NConta().Listar(pIdClienteDuc);
            lRetorno.BensImoveis = new NBensImoveis().Listar(pIdClienteDuc);
            lRetorno.BensOutros = new NBensOutros().Listar(pIdClienteDuc);
            lRetorno.Endereco = new NEndereco().Listar(pIdClienteDuc);
            lRetorno.IdAssessor = new NCliente().GetIdAssessor(lRetorno.Cliente.ID_AssessorFilial.Value);
            lRetorno.Login = new NLogin().Selecionar(lRetorno.Cliente.ID_Login.Value);
            lRetorno.Representante = new NRepresentante().Listar(pIdClienteDuc);
            lRetorno.Telefone = new NTelefone().Listar(pIdClienteDuc);

            return lRetorno;
        }

        public ClienteNovo Conversao(ClienteAntigo pClienteDuc, int pIdLoginAlteracao)
        {
            ClienteNovo lRetorno = new ClienteNovo();

            lRetorno.Alteracao = GetAlteracao(pClienteDuc, pIdLoginAlteracao);
            lRetorno.Banco = GetBanco(pClienteDuc);
            lRetorno.Cliente = GetCliente(pClienteDuc);
            lRetorno.Contrato = GetContrato();
            lRetorno.Endereco = GetEndereco(pClienteDuc);
            lRetorno.Login = GetLogin(pClienteDuc);
            lRetorno.Representante = GetRepresentante(pClienteDuc);
            lRetorno.Sfp = GetSfp(pClienteDuc);
            lRetorno.Telefone = GetTelefone(pClienteDuc);

            return lRetorno;
        }

        public int ImportarCliente(ClienteNovo pClienteNovo)
        {
            //Criar Transação
            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = "Cadastro";
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();

            try
            {   //Inserir Login
                SalvarObjetoRequest<LoginInfo> lLogin = new SalvarObjetoRequest<LoginInfo>();
                lLogin.Objeto = pClienteNovo.Login;
                pClienteNovo.Login.IdLogin = ClienteDbLib.SalvarLogin(trans, lLogin, false).Codigo;

                //Colocar idLogin no cliente
                pClienteNovo.Cliente.IdLogin = pClienteNovo.Login.IdLogin.Value;

                //Inserir Cliente
                SalvarObjetoRequest<ClienteInfo> lCliente = new SalvarObjetoRequest<ClienteInfo>();
                lCliente.Objeto = pClienteNovo.Cliente;
                pClienteNovo.Cliente.IdCliente = ClienteDbLib.SalvarCliente(trans, lCliente, false).Codigo;

                //colocar idCliente nas outras entidades
                //Inserir outras entidades

                SalvarObjetoRequest<ClienteTelefoneInfo> lTelefone;
                foreach (ClienteTelefoneInfo item in pClienteNovo.Telefone)
                {
                    item.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                    lTelefone = new SalvarObjetoRequest<ClienteTelefoneInfo>();
                    lTelefone.Objeto = item;
                    ClienteDbLib.SalvarClienteTelefone(trans, lTelefone);
                }

                SalvarObjetoRequest<ClienteEnderecoInfo> lEndereco;
                foreach (ClienteEnderecoInfo item in pClienteNovo.Endereco)
                {
                    item.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                    lEndereco = new SalvarObjetoRequest<ClienteEnderecoInfo>();
                    lEndereco.Objeto = item;
                    ClienteDbLib.SalvarClienteEndereco(trans, lEndereco);
                }

                SalvarObjetoRequest<ClienteBancoInfo> lBanco;
                foreach (ClienteBancoInfo item in pClienteNovo.Banco)
                {
                    item.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                    lBanco = new SalvarObjetoRequest<ClienteBancoInfo>();
                    lBanco.Objeto = item;
                    ClienteDbLib.SalvarClienteBanco(trans, lBanco);
                }

                //Implementar
                //List<ClienteAlteracaoInfo> Alteracao - OK
                //List<ClienteProcuradorRepresentanteInfo> Representante - TODO
                //List<ContratoInfo> Contrato - TODO

                SalvarObjetoRequest<ClienteAlteracaoInfo> lAlteracao;
                foreach (ClienteAlteracaoInfo item in pClienteNovo.Alteracao)
                {
                    item.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                    lAlteracao = new SalvarObjetoRequest<ClienteAlteracaoInfo>();
                    lAlteracao.Objeto = item;
                    ClienteDbLib.SalvarClienteAlteracaoImportacao(trans, lAlteracao);
                }

                SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo> lRepresentante;
                foreach (ClienteProcuradorRepresentanteInfo item in pClienteNovo.Representante)
                {
                    item.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                    lRepresentante = new SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo>();
                    lRepresentante.Objeto = item;
                    ClienteDbLib.SalvarClienteProcuradorRepresentante(trans, lRepresentante);
                }

                SalvarObjetoRequest<ClienteContratoInfo> lObjSalvar = new SalvarObjetoRequest<ClienteContratoInfo>();
                lObjSalvar.Objeto = new ClienteContratoInfo();
                lObjSalvar.Objeto.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                lObjSalvar.Objeto.DtAssinatura = DateTime.Now;
                lObjSalvar.Objeto.LstIdContrato = new List<int>();

                foreach (ContratoInfo item in pClienteNovo.Contrato)
                    lObjSalvar.Objeto.LstIdContrato.Add(item.IdContrato.Value);

                ClienteDbLib.SalvarClienteContrato(trans, lObjSalvar);

                pClienteNovo.Sfp.IdCliente = pClienteNovo.Cliente.IdCliente.Value;
                SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lSfp = new SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
                lSfp.Objeto = pClienteNovo.Sfp;
                ClienteDbLib.SalvarClienteSituacaoFinanceiraPatrimonial(trans, lSfp);

                trans.Commit();

                return pClienteNovo.Cliente.IdCliente.Value;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
                if (!ConnectionState.Closed.Equals(conn.State)) conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        #region Conversão

        private ClienteInfo GetCliente(ClienteAntigo pClienteDuc)
        {
            ClienteInfo lRetorno = new ClienteInfo();
            lRetorno.CdAtividadePrincipal = 0;
            lRetorno.CdEscolaridade = null;
            lRetorno.CdEstadoCivil = pClienteDuc.Cliente.EstadoCivil;
            lRetorno.CdNacionalidade = pClienteDuc.Cliente.Nacionalidade;
            lRetorno.CdNire = null;
            lRetorno.CdOrgaoEmissorDocumento = pClienteDuc.Cliente.OrgaoEmissorDocumento;
            lRetorno.CdPaisNascimento = pClienteDuc.Cliente.PaisNascimento;
            lRetorno.CdProfissaoAtividade = pClienteDuc.Cliente.Profissao;
            lRetorno.CdSenha = pClienteDuc.Login.Senha;
            lRetorno.CdSexo = pClienteDuc.Cliente.Sexo;
            lRetorno.CdUfEmissaoDocumento = pClienteDuc.Cliente.EstadoEmissaoDocumento;
            lRetorno.CdUfNascimento = pClienteDuc.Cliente.UFNascimento;
            lRetorno.DsAutorizadoOperar = "";
            lRetorno.DsCargo = pClienteDuc.Cliente.Cargo;
            lRetorno.DsConjugue = pClienteDuc.Cliente.Conjugue;
            lRetorno.DsCpfCnpj = pClienteDuc.Cliente.CPF;
            lRetorno.DsEmail = pClienteDuc.Login.Email;
            lRetorno.DsEmailComercial = pClienteDuc.Cliente.EmailComercial;
            lRetorno.DsEmpresa = pClienteDuc.Cliente.Empresa;
            lRetorno.DsFormaConstituicao = "";
            lRetorno.DsNaturalidade = pClienteDuc.Cliente.Naturalidade;
            lRetorno.DsNome = pClienteDuc.Login.Nome;
            lRetorno.DsNomeFantasia = pClienteDuc.Login.Nome;
            lRetorno.DsNomeMae = pClienteDuc.Cliente.NomeMae;
            lRetorno.DsNomePai = pClienteDuc.Cliente.NomePai;
            lRetorno.DsNumeroDocumento = pClienteDuc.Cliente.NumeroDocumento;
            lRetorno.DsOrigemCadastro = "Importação DUC";
            lRetorno.DsUfnascimentoEstrangeiro = pClienteDuc.Cliente.UFNascimentoEstrangeiro;
            lRetorno.DtAtivacaoInativacao = null;
            lRetorno.DtEmissaoDocumento = pClienteDuc.Cliente.DataEmissaoDocumento;
            lRetorno.DtNascimentoFundacao = pClienteDuc.Cliente.DataNascimento;
            lRetorno.DtPasso1 = pClienteDuc.Cliente.DataCadastroInicial.Value;
            lRetorno.DtPasso2 = null;
            if (pClienteDuc.Cliente.Passo > 1) lRetorno.DtPasso2 = lRetorno.DtPasso1;
            lRetorno.DtPasso3 = pClienteDuc.Cliente.DataAprovacaoFinal;
            lRetorno.DtPrimeiraExportacao = null;
            lRetorno.DtUltimaAtualizacao = DateTime.Now;
            lRetorno.DtUltimaExportacao = null;
            lRetorno.IdAssessorInicial = pClienteDuc.IdAssessor;
            lRetorno.IdCliente = 0;
            lRetorno.IdLogin = 0;
            lRetorno.NrInscricaoEstadual = "";
            lRetorno.StAtivo = toBool(pClienteDuc.Login.Ativo, eTipoRetorno.True).Value;
            lRetorno.StCadastroPortal = null;
            lRetorno.StCarteiraPropria = toBool(pClienteDuc.Cliente.CarteiraPropria, eTipoRetorno.True);
            lRetorno.StCVM387 = toBool(pClienteDuc.Cliente.CVM387, eTipoRetorno.True);
            if (pClienteDuc.Cliente.Emancipado.Length == 0)
                lRetorno.StEmancipado = false;
            else
                lRetorno.StEmancipado = toBool(pClienteDuc.Cliente.Emancipado[0], eTipoRetorno.False);
            lRetorno.StInterdito = false;
            lRetorno.StPasso = pClienteDuc.Cliente.Passo.Value;
            lRetorno.StPessoaVinculada = (pClienteDuc.Cliente.PessoaVinculada == 'S') ? 1 : 0; //toBool(pClienteDuc.Cliente.PessoaVinculada, eTipoRetorno.False).Value;
            lRetorno.StPPE = toBool(pClienteDuc.Cliente.PPE, eTipoRetorno.False);
            lRetorno.StSituacaoLegalOutros = null;
            lRetorno.TpCliente = pClienteDuc.Cliente.Tipo.Value;
            lRetorno.TpDocumento = pClienteDuc.Cliente.TipoDocumento;
            lRetorno.TpPessoa = 'F';

            return lRetorno;
        }

        private List<ClienteAlteracaoInfo> GetAlteracao(ClienteAntigo pClienteDuc, int pIdLoginAlteracao)
        {
            List<ClienteAlteracaoInfo> lRetorno = new List<ClienteAlteracaoInfo>();
            foreach (EAlteracao item in pClienteDuc.Alteracao)
            {
                //ClienteAlteracaoInfo lAlteracao = new ClienteAlteracaoInfo();
                //lAlteracao.CdTipo = item.Tipo;
                //lAlteracao.DsDescricao = item.Descricao;
                //lAlteracao.DsInformacao = item.Campo;
                //lAlteracao.DtRealizacao = item.DataRealizada;
                //lAlteracao.DtSolicitacao = item.Data.Value;
                //lAlteracao.IdAlteracao = 0;
                //lAlteracao.IdCliente = 0;
                //lAlteracao.IdLogin = (null == item.DataRealizada) ? new Nullable<int>() : 1;
                //lRetorno.Add(lAlteracao);
                lRetorno.Add(GetAlteracao(item, pIdLoginAlteracao));
            }
            return lRetorno;
        }

        public ClienteAlteracaoInfo GetAlteracao(EAlteracao pAlteracaoAntiga, int pIdLoginAlteracao)
        {
            ClienteAlteracaoInfo lRetorno = new ClienteAlteracaoInfo();
            lRetorno.CdTipo = pAlteracaoAntiga.Tipo;
            lRetorno.DsDescricao = pAlteracaoAntiga.Descricao;
            lRetorno.DsInformacao = pAlteracaoAntiga.Campo;
            lRetorno.DtRealizacao = pAlteracaoAntiga.DataRealizada;
            lRetorno.DtSolicitacao = pAlteracaoAntiga.Data.Value;
            lRetorno.IdAlteracao = 0;
            lRetorno.IdCliente = 0;
            lRetorno.IdLoginRealizacao = (null == pAlteracaoAntiga.DataRealizada) ? new Nullable<int>() : pIdLoginAlteracao;
            return lRetorno;
        }

        private ClienteSituacaoFinanceiraPatrimonialInfo GetSfp(ClienteAntigo pClienteDuc)
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lRetorno = new ClienteSituacaoFinanceiraPatrimonialInfo();
            lRetorno.IdCliente = 0;
            lRetorno.IdSituacaoFinanceiraPatrimonial = 0;
            lRetorno.DtAtualizacao = DateTime.Now;
            lRetorno.DtCapitalSocial = null;
            lRetorno.DtPatrimonioLiquido = null;
            lRetorno.VlTotalAplicacaoFinanceira = 0;
            lRetorno.VlTotalBensImoveis = 0;
            lRetorno.VlTotalBensMoveis = 0;
            lRetorno.VlTotalPatrimonioLiquido = 0;
            lRetorno.VTotalCapitalSocial = 0;
            lRetorno.VlTotalSalarioProLabore = pClienteDuc.Cliente.Salario;
            lRetorno.VlTotalOutrosRendimentos = pClienteDuc.Cliente.OutrosRendimentosValor;
            lRetorno.DsOutrosRendimentos = pClienteDuc.Cliente.OutrosRendimentosDescricao;

            foreach (EBensImoveis item in pClienteDuc.BensImoveis)
                lRetorno.VlTotalBensImoveis += item.Valor.Value;

            foreach (EBensOutros item in pClienteDuc.BensOutros)
            {
                switch (item.Tipo)
                {
                    case 6:
                    case 13:
                    case 15:
                        //Aplicação
                        lRetorno.VlTotalAplicacaoFinanceira += item.Valor.Value;
                        break;
                    case 7:
                    case 8:
                    case 17:
                        //Outros
                        lRetorno.VlTotalOutrosRendimentos += item.Valor.Value;
                        break;
                    case 12:
                        //Bens Moveis
                        lRetorno.VlTotalBensMoveis += item.Valor.Value;
                        break;
                    case 18:
                        //Patrimonio Lioquido
                        lRetorno.VlTotalPatrimonioLiquido += item.Valor.Value;
                        break;
                    default:
                        lRetorno.VlTotalOutrosRendimentos += item.Valor.Value;
                        break;
                }
            }
            return lRetorno;
        }

        private List<ClienteBancoInfo> GetBanco(ClienteAntigo pClienteDuc)
        {
            List<ClienteBancoInfo> lRetorno = new List<ClienteBancoInfo>();
            ClienteBancoInfo lBanco;

            foreach (EConta item in pClienteDuc.Banco)
            {
                lBanco = new ClienteBancoInfo();
                lBanco.CdBanco = item.Banco.ToString();
                lBanco.DsAgencia = item.Agencia.ToString();
                lBanco.DsConta = item.Conta;
                lBanco.DsContaDigito = item.ContaDigito;
                lBanco.IdBanco = 0;
                lBanco.IdCliente = 0;
                lBanco.StPrincipal = toBool(item.Principal).Value;
                lBanco.TpConta = "C" + item.Tipo.ToString().ToUpper();
                lRetorno.Add(lBanco);
            }
            return lRetorno;
        }

        private List<ClienteEnderecoInfo> GetEndereco(ClienteAntigo pClienteDuc)
        {
            List<ClienteEnderecoInfo> lRetorno = new List<ClienteEnderecoInfo>();
            ClienteEnderecoInfo lEndereco;
            foreach (EEndereco item in pClienteDuc.Endereco)
            {
                lEndereco = new ClienteEnderecoInfo();
                lEndereco.CdPais = item.Pais;
                lEndereco.CdUf = item.UF;
                lEndereco.DsBairro = item.Bairro;
                lEndereco.DsCidade = item.Cidade;
                lEndereco.DsComplemento = item.Complemento;
                lEndereco.DsLogradouro = item.Logradouro;
                lEndereco.DsNumero = item.Numero;
                lEndereco.IdCliente = 0;
                lEndereco.IdEndereco = 0;
                lEndereco.StPrincipal = toBool(item.Correspondencia).Value;
                try
                {
                    lEndereco.NrCep = int.Parse(item.CEP.Replace(".", "").Replace("-", "").PadLeft(8, '0').Substring(0, 5));
                    lEndereco.NrCepExt = int.Parse(item.CEP.Replace(".", "").Replace("-", "").PadLeft(8, '0').Substring(5, 3));
                }
                catch
                {
                    lEndereco.NrCep = 0;
                    lEndereco.NrCepExt = 0;
                }
                switch (item.Tipo.ToString().ToUpper())
                {
                    case "C"://Comercial
                        lEndereco.IdTipoEndereco = 1;
                        break;
                    case "R"://Residencial
                        lEndereco.IdTipoEndereco = 2;
                        break;
                    case "O"://Outros
                        lEndereco.IdTipoEndereco = 3;
                        break;
                    default:
                        break;
                }

                lRetorno.Add(lEndereco);
            }
            return lRetorno;
        }

        private LoginInfo GetLogin(ClienteAntigo pClienteDuc)
        {
            LoginInfo lRetorno = new LoginInfo();
            lRetorno.CdAssessor = null;
            lRetorno.CdAssinaturaEletronica = pClienteDuc.Login.Assinatura;
            lRetorno.CdSenha = pClienteDuc.Login.Senha;
            lRetorno.DsEmail = pClienteDuc.Login.Email;
            lRetorno.DsNome = "";
            lRetorno.DsRespostaFrase = "";
            lRetorno.DtUltimaExpiracao = DateTime.Now;
            lRetorno.IdFrase = null;
            lRetorno.IdLogin = 0;
            lRetorno.NrTentativasErradas = 0;
            lRetorno.TpAcesso = 0;
            return lRetorno;
        }

        private List<ClienteProcuradorRepresentanteInfo> GetRepresentante(ClienteAntigo pClienteDuc)
        {
            List<ClienteProcuradorRepresentanteInfo> lRetorno = new List<ClienteProcuradorRepresentanteInfo>();
            if (null != pClienteDuc.Representante && null != pClienteDuc.Representante.CPF && pClienteDuc.Representante.CPF.ToString().Trim().Length > 0)
            {
                ClienteProcuradorRepresentanteInfo lRepresentante = new ClienteProcuradorRepresentanteInfo();
                lRepresentante.DsNome = pClienteDuc.Representante.Nome;
                lRepresentante.CdOrgaoEmissor = pClienteDuc.Representante.OrgaoEmissorDocumento;
                lRepresentante.CdUfOrgaoEmissor = pClienteDuc.Representante.UFEmissaoDocumento;
                lRepresentante.DsNumeroDocumento = pClienteDuc.Representante.NumeroDocumento;
                lRepresentante.DtNascimento = pClienteDuc.Representante.DataNascimento.Value;
                lRepresentante.IdCliente = 0;
                lRepresentante.IdProcuradorRepresentante = 0;
                lRepresentante.NrCpfCnpj = pClienteDuc.Representante.CPF.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");
                lRepresentante.TpDocumento = pClienteDuc.Representante.TipoDocumento;
                lRepresentante.TpSituacaoLegal = pClienteDuc.Representante.SituacaoLegal.Value;
                lRetorno.Add(lRepresentante);
            }
            return lRetorno;
        }

        private List<ClienteTelefoneInfo> GetTelefone(ClienteAntigo pClienteDuc)
        {
            List<ClienteTelefoneInfo> lRetorno = new List<ClienteTelefoneInfo>();
            ClienteTelefoneInfo lTelefone;
            foreach (ETelefone item in pClienteDuc.Telefone)
            {
                lTelefone = new ClienteTelefoneInfo();
                lTelefone.DsDdd = item.DDD;
                lTelefone.DsNumero = item.Telefone;
                lTelefone.DsRamal = item.Ramal;
                lTelefone.IdCliente = 0;
                lTelefone.IdTelefone = 0;
                lTelefone.StPrincipal = toBool(item.Principal).Value;
                switch (item.Tipo.Value.ToString().ToUpper())
                {
                    case "C": //Comercial
                        lTelefone.IdTipoTelefone = 2;
                        break;
                    case "P": //Particular (Celular)
                        lTelefone.IdTipoTelefone = 3;
                        break;
                    case "R": //Residencial
                        lTelefone.IdTipoTelefone = 1;
                        break;
                    default:
                        break;
                }
                lRetorno.Add(lTelefone);
            }
            return lRetorno;
        }

        private List<ContratoInfo> GetContrato()
        {
            ConsultarEntidadeCadastroRequest<ContratoInfo> EntradaContrato = new ConsultarEntidadeCadastroRequest<ContratoInfo>();
            ConsultarEntidadeCadastroResponse<ContratoInfo> RetornoContrato = new ConsultarEntidadeCadastroResponse<ContratoInfo>();
            EntradaContrato.EntidadeCadastro = new ContratoInfo();
            RetornoContrato = ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ContratoInfo>(EntradaContrato);
            return RetornoContrato.Resultado;
        }

        #endregion

        #region AUX

        private enum eTipoRetorno
        {
            True,
            False,
            Null
        }

        private bool? toBool(char? pEntrada, eTipoRetorno pRetornoDefault = eTipoRetorno.Null)
        {
            bool? lRetorno = null;
            if (null == pEntrada)
            {
                switch (pRetornoDefault)
                {
                    case eTipoRetorno.True:
                        lRetorno = true;
                        break;
                    case eTipoRetorno.False:
                        lRetorno = false;
                        break;
                    case eTipoRetorno.Null:
                        lRetorno = null;
                        break;
                }
            }
            else
            {
                if (pEntrada.Value.ToString().ToUpper().Trim().Equals("S"))
                    lRetorno = true;
                else
                    lRetorno = false;
            }
            return lRetorno;
        }

        #endregion
    }
}
