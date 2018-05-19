using System;
using System.Xml.Linq;
using Gradual.OMS.TesouroDireto.App_Codigo;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.CadastroInvestidor;
using log4net;

namespace Gradual.OMS.TesouroDireto.AcessoWS
{
    public class CadastroInvestidor : ObjetosBase
    {
        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HabilitarInvestidorResponse HabilitarInvestidor(HabilitarInvestidorRequest pParametros)
        {
            var lRetorno = new HabilitarInvestidorResponse();

            try
            {
                string lXml = ConexaoWS.WsInvestidor.HabilitarInvestidor(pParametros.CPF.DBToInt64(), pParametros.DataNascimento.DBToDateTimeString(), pParametros.CodigoInvestidor, pParametros.DigitoInvestidor, pParametros.Email, pParametros.IdentificacaoOperacao, pParametros.TaxaCustodia.DBToInt32());

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("HabilitarInvestidor", ex);
            }

            return lRetorno;
        }

        public IncluirInvestidorResponse IncluirInvestidor(IncluirInvestidorRequest pParametro)
        {
            var lRetorno = new IncluirInvestidorResponse();

            try
            {
                string lXml = ConexaoWS.WsInvestidor.IncluirInvestidor(pParametro.Investidor.CPF.DBToInt64()
                                                                      , pParametro.Investidor.DataNascimento.DBToDateTimeString()
                                                                      , pParametro.Investidor.CodigoInvestidor
                                                                      , pParametro.Investidor.DigitoInvestidor
                                                                      , pParametro.Investidor.Email
                                                                      , pParametro.Investidor.IdentificacaoDaOperacao
                                                                      , pParametro.Investidor.TaxaCustodia.DBToInt32()
                                                                      , pParametro.Investidor.NomeInvestidor
                                                                      , pParametro.Investidor.CodigoDocumento
                                                                      , pParametro.Investidor.TipoDocumento
                                                                      , pParametro.Investidor.OrgaoEmissor
                                                                      , pParametro.Investidor.SexoInvestidor
                                                                      , pParametro.Investidor.EstadoCivil
                                                                      , pParametro.Investidor.NomeConjuge
                                                                      , pParametro.Investidor.Capacidade.CodigoCapacidade
                                                                      , pParametro.Investidor.NomeResponsavel
                                                                      , pParametro.Investidor.QualificacaoResponsavel
                                                                      , pParametro.Investidor.CodigoAtividade
                                                                      , pParametro.Investidor.CodigoNacionalidade
                                                                      , pParametro.Investidor.Endereco.Rua
                                                                      , pParametro.Investidor.Endereco.Numero
                                                                      , pParametro.Investidor.Endereco.Complemento
                                                                      , pParametro.Investidor.Endereco.Bairro
                                                                      , pParametro.Investidor.Endereco.Cidade
                                                                      , pParametro.Investidor.Endereco.UF
                                                                      , pParametro.Investidor.Endereco.CEP
                                                                      , pParametro.Investidor.Telefone.PrefixoTelefone
                                                                      , pParametro.Investidor.Telefone.NumeroTelefone
                                                                      , pParametro.Investidor.PessoaPoliticamenteExposta);
                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("DADOS_CADASTRADOS") != null)
                {
                    XElement elem = root.Element("DADOS_CADASTRADOS");

                    lRetorno.Objeto.NomeInvestidor = elem.Element("NOME_INVESTIDOR") != null ? elem.Element("NOME_INVESTIDOR").Value : "";
                    lRetorno.Objeto.DocumentoIdentificacao.CodigoDocumento = elem.Element("CODIGO") != null ? elem.Element("CODIGO").Value.DBToInt32() : 0;
                    lRetorno.Objeto.DocumentoIdentificacao.TipoDocumento = elem.Element("TIPO_DOCUMENTO") != null ? elem.Element("TIPO_DOCUMENTO").Value.DBToInt32() : 0;
                    lRetorno.Objeto.DocumentoIdentificacao.OrgaoEmissorDocumento = elem.Element("ORGAO_EMISSOR_DOCUMENTO") != null ? elem.Element("ORGAO_EMISSOR_DOCUMENTO").Value : "";
                    lRetorno.Objeto.SexoInvestidor = elem.Element("SEXO_INVESTIDOR") != null ? elem.Element("SEXO_INVESTIDOR").Value : "";
                    lRetorno.Objeto.EstadoCivil = elem.Element("ESTADO_CIVIL") != null ? elem.Element("ESTADO_CIVIL").Value : "";
                    lRetorno.Objeto.NomeConjuge = elem.Element("NOME_CONJUGE") != null ? elem.Element("NOME_CONJUGE").Value : "";
                    lRetorno.Objeto.Capacidade.CodigoCapacidade = elem.Element("CODIGO_CAPACIDADE") != null ? elem.Element("CODIGO_CAPACIDADE").Value.DBToInt32() : 0;
                    lRetorno.Objeto.Capacidade.NomeResponsavel = elem.Element("NOME_RESPONSAVEL") != null ? elem.Element("NOME_RESPONSAVEL").Value : "";
                    lRetorno.Objeto.Capacidade.QualificacaoResponsavel = elem.Element("QUALIFICACAO_RESPONSAVEL") != null ? elem.Element("QUALIFICACAO_RESPONSAVEL").Value : "";
                    lRetorno.Objeto.CodigoNacionalidade = elem.Element("CODIGO_NACIONALIDADE") != null ? elem.Element("CODIGO_NACIONALIDADE").Value.DBToInt32() : 0;
                    lRetorno.Objeto.CodigoInvestidor = elem.Element("CODIGO_INVESTIDOR") != null ? elem.Element("CODIGO_INVESTIDOR").Value.DBToInt32() : 0;
                    lRetorno.Objeto.Email = elem.Element("EMAIL") != null ? elem.Element("EMAIL").Value : "";
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("IncluirInvestidor", ex);
            }

            return lRetorno;
        }
    }
}
