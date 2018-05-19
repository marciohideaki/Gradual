using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;
using Orbite.RV.Contratos.Integracao.BVMF.Legado;
using Orbite.RV.Sistemas.Integracao.BVMF;

using VF.SAC.Consultas.Library.Comum;

using Orbite.RV.Contratos.Integracao.BVMF.Legado.Mensagens;

namespace Orbite.RV.Sistemas.Integracao.BVMF.Legado
{
    /// <summary>
    /// Implementação do servico de integração com o sistema legado de arquivos BVMF.
    /// Converte os arquivos utilizando o serviço de persistencia dos layouts BVMF, ou seja,
    /// o arquivo que será salvo está configurado no serviço de persistencia BVMF.
    /// </summary>
    public class ServicoIntegracaoBVMFLegado : IServicoIntegracaoBVMFLegado
    {
        #region IServicoIntegracaoBVMFLegado Members

        /// <summary>
        /// Operação para realizar a conversão do arquivo legado no arquivo atual.
        /// Converte os arquivos utilizando o serviço de persistencia dos layouts BVMF, ou seja,
        /// o arquivo que será salvo está configurado no serviço de persistencia BVMF.
        /// </summary>
        /// <param name="caminhoArquivoLegado">Caminho do arquivo a ser convertido</param>
        public ConverterBVMFLegadoResponse ConverterArquivo(ConverterBVMFLegadoRequest parametros)
        {
            // Pega o servico de layouts
            IServicoIntegracaoBVMFPersistenciaLayouts servicoIntegracaoBVMFPersistenciaLayouts =
                Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();

            // Pega o dataset
            VF.SAC.Consultas.Library.DataSets.DbMain ds = new VF.SAC.Consultas.Library.DataSets.DbMain();
            ds.ReadXml(parametros.CaminhoArquivo);

            // Para cada layout de Bovespa e BMF (idImpArqLayoutTipo = 1,2)
            foreach (DataRow drLayout in ds.Tables["TbImpArqLayout"].Select("idImpArqLayoutTipo in (1, 2)"))
            {
                // Cria Layout
                LayoutBVMFInfo layout = new LayoutBVMFInfo();

                // Preenche layout
                layout.Nome = (string)drLayout["nmImpArqLayout"];
                layout.Descricao = (string)drLayout["dsImpArqLayout"];
                
                // Informa o conversor de acordo com o tipo de layout
                layout.ConversorTipo = 
                    (int)drLayout["idImpArqLayoutTipo"] == 1 ? typeof(ConversorLayoutBovespa) : typeof(ConversorLayoutBMF);

                // Parametros do layout
                if (drLayout["vlParametros"] != DBNull.Value)
                    layout.Parametros = traduzirParametro(Serializador.DesserializaParametro((string)drLayout["vlParametros"]));

                // Varre as tabelas
                foreach (DataRow drTabela in ds.Tables["TbImpArqTabela"].Select("idImpArqLayout = " + drLayout["idImpArqLayout"].ToString()))
                {
                    // Cria tabela
                    LayoutBVMFTabelaInfo tabela = new LayoutBVMFTabelaInfo();
                    layout.Tabelas.Add(tabela);

                    // Preenche tabela
                    tabela.Nome = (string)drTabela["nmImpArqTabela"];
                    tabela.Descricao = (string)drTabela["dsImpArqTabela"];
                    tabela.Ordem = int.Parse(drTabela["idImpArqTabela"].ToString());

                    // Parametros da tabela
                    if (drTabela["vlParametros"] != DBNull.Value)
                        tabela.Parametros = traduzirParametro(Serializador.DesserializaParametro((string)drTabela["vlParametros"]));

                    // Varre as colunas
                    foreach (DataRow drColuna in ds.Tables["TbImpArqColuna"].Select("idImpArqLayout = " + drLayout["idImpArqLayout"].ToString() + " and idImpArqTabela = " + drTabela["idImpArqTabela"].ToString()))
                    {
                        // Cria coluna
                        LayoutBVMFCampoInfo campo = new LayoutBVMFCampoInfo();
                        tabela.Campos.Add(campo);

                        // Preenche coluna
                        campo.Nome = (string)drColuna["nmImpArqColuna"];
                        campo.Descricao = (string)drColuna["dsImpArqColuna"];
                        campo.Tipo = Type.GetType((string)drColuna["nmClasseTipo"]);
                        campo.Tamanho = int.Parse(drColuna["vlTamanho"].ToString());
                        campo.Ordem = int.Parse(drColuna["idImpArqColuna"].ToString());
                        campo.Parametros = null;
                        campo.ParametrosTipo = null;

                        // Parametros da coluna
                        if (drColuna["vlParametros"] != DBNull.Value)
                            campo.Parametros = traduzirParametro(Serializador.DesserializaParametro((string)drColuna["vlParametros"]));
                        if (drColuna["vlParametrosTipo"] != DBNull.Value)
                            campo.ParametrosTipo = traduzirParametro(Serializador.DesserializaParametro((string)drColuna["vlParametrosTipo"]));
                    }
                }

                // Salva o layout, apenas se não existir
                servicoIntegracaoBVMFPersistenciaLayouts.SalvarLayout(layout);
            }

            // Retorna
            return
                new ConverterBVMFLegadoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        #endregion

        /// <summary>
        /// Rotina auxiliar para converter o paramêtro legado no parâmetro atual
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        private object traduzirParametro(object parametro)
        {
            // Inicializa
            object retorno = null;

            // Traduz de acordo com o tipo
            if (parametro.GetType() == typeof(VF.SAC.Consultas.Library.Parser.ParametrosColuna))
            {
                VF.SAC.Consultas.Library.Parser.ParametrosColuna parametro2 = (VF.SAC.Consultas.Library.Parser.ParametrosColuna)parametro;
                ConversorCampoBovespaColunaParametro parametro3 = new ConversorCampoBovespaColunaParametro();
                parametro3.ColunaTipo =
                    (ConversorCampoBovespaColunaTipoEnum)
                        Enum.Parse(
                            typeof(ConversorCampoBovespaColunaTipoEnum),
                            Enum.GetName(
                                typeof(VF.SAC.Consultas.Library.Parser.ParametrosColuna.TipoColunaEnumerator), parametro2.TipoColuna));
                retorno = parametro3;
            }
            else if (parametro.GetType() == typeof(VF.SAC.Consultas.Library.Parser.ParametrosColunaData))
            {
                VF.SAC.Consultas.Library.Parser.ParametrosColunaData parametro2 = (VF.SAC.Consultas.Library.Parser.ParametrosColunaData)parametro;
                ConversorCampoDataParametro parametro3 = new ConversorCampoDataParametro();
                parametro3.FormatoData = parametro2.FormatoData;
                retorno = parametro3;
            }
            else if (parametro.GetType() == typeof(VF.SAC.Consultas.Library.Parser.ParametrosColunaNumero))
            {
                VF.SAC.Consultas.Library.Parser.ParametrosColunaNumero parametro2 = (VF.SAC.Consultas.Library.Parser.ParametrosColunaNumero)parametro;
                ConversorCampoNumeroParametro parametro3 = new ConversorCampoNumeroParametro();
                parametro3.NumeroDecimais = parametro2.NumeroDecimais;
                parametro3.Separador = parametro2.Separador;
                retorno = parametro3;
            }
            else if (parametro.GetType() == typeof(VF.SAC.Consultas.Library.Parser.ParametrosLayout))
            {
                VF.SAC.Consultas.Library.Parser.ParametrosLayout parametro2 = (VF.SAC.Consultas.Library.Parser.ParametrosLayout)parametro;
                ConversorLayoutBVMFParametro parametro3 = new ConversorLayoutBVMFParametro();
                parametro3.TipoLinhaPosicaoInicial = parametro2.TipoLinhaPosicaoInicial;
                parametro3.TipoLinhaQuantidade = parametro2.TipoLinhaQuantidade;
                retorno = parametro3;
            }
            else if (parametro.GetType() == typeof(VF.SAC.Consultas.Library.Parser.ParametrosTabela))
            {
                VF.SAC.Consultas.Library.Parser.ParametrosTabela parametro2 = (VF.SAC.Consultas.Library.Parser.ParametrosTabela)parametro;
                ConversorLayoutBVMFParametroTabela parametro3 = new ConversorLayoutBVMFParametroTabela();
                parametro3.RegistroTipo = parametro2.TipoRegistro;
                parametro3.TabelaTipo =
                    (ConversorLayoutBVMFParametroTabelaTipoEnum)
                        Enum.Parse(
                            typeof(ConversorLayoutBVMFParametroTabelaTipoEnum),
                            Enum.GetName(
                                typeof(VF.SAC.Consultas.Library.Parser.ParametrosTabela.TipoTabelaEnumerator), parametro2.TipoTabela));
                retorno = parametro3;
            }
            else if (parametro.GetType() == typeof(VF.SAC.Consultas.Library.Parser.ParametrosTabelaBMF))
            {
                VF.SAC.Consultas.Library.Parser.ParametrosTabela parametro2 = (VF.SAC.Consultas.Library.Parser.ParametrosTabelaBMF)parametro;
                ConversorLayoutBVMFParametroTabela parametro3 = new ConversorLayoutBVMFParametroTabela();
                parametro3.RegistroTipo = parametro2.TipoRegistro;
                parametro3.TabelaTipo =
                    (ConversorLayoutBVMFParametroTabelaTipoEnum)
                        Enum.Parse(
                            typeof(ConversorLayoutBVMFParametroTabelaTipoEnum),
                            Enum.GetName(
                                typeof(VF.SAC.Consultas.Library.Parser.ParametrosTabela.TipoTabelaEnumerator), parametro2.TipoTabela));
                retorno = parametro3;
            }

            // Retorna
            return retorno;
        }
    }
}
