using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorLayoutBMF : IConversorLayoutBase
    {
        #region IConversorLayoutBase Members

        public DataSet InterpretarArquivo(LayoutBVMFInfo layout, Stream stream)
        {
            // Cria o schema
            DataSet ds =
                Ativador.Get<IServicoIntegracaoBVMFArquivos>().CriarSchema(layout);

            // Variaveis utilizadas na interpretacao
            DataTable tb = null;
            LayoutBVMFTabelaInfo tabelaAtual = null;
            int ultimoIdLinha = -1;
            int pos = 0;
            IEnumerable<LayoutBVMFCampoInfo> camposOrdenados = null;
            ConversorLayoutBVMFParametro parametrosLayout = 
                layout.Parametros as ConversorLayoutBVMFParametro;

            // Dicionario de conversao
            Dictionary<Type, ConversorCampoBase> dicionarioConversao =
                (GerenciadorConfig.ReceberConfig<ConversorLayoutBVMFConfig>()).CriarDicionario();

            // Varre o arquivo
            StreamReader reader = new StreamReader(stream);
            string linha = reader.ReadLine();
            while (linha != null)
            {
                // Inicializa
                pos = 0;

                // Pega ID da linha
                int idLinha = 
                    int.Parse(
                        linha.Substring(
                            parametrosLayout.TipoLinhaPosicaoInicial, 
                            parametrosLayout.TipoLinhaQuantidade));

                // Verifica se ID é diferente da linha anterior
                if (idLinha != ultimoIdLinha)
                {
                    // Acha objeto de tabela correspondente
                    tabelaAtual = (
                                from c in layout.Tabelas
                                where ((ConversorLayoutBVMFParametroTabela)c.Parametros).RegistroTipo == idLinha
                                select c
                            ).FirstOrDefault();
                    if (tabelaAtual != null)
                    {
                        // Pega datatable correspondente
                        tb = ds.Tables[tabelaAtual.Nome];

                        // Cria sequencia de campos ordenados
                        camposOrdenados = from c in tabelaAtual.Campos
                                          orderby c.Ordem
                                          select c;

                        // Indica idLinha atual
                        ultimoIdLinha = idLinha;
                    }
                }

                // Apenas se processa esse tipo de linha
                if (tabelaAtual != null)
                {
                    // Cria nova linha
                    DataRow dr = tb.NewRow();

                    // Varre os campos
                    foreach (LayoutBVMFCampoInfo campo in camposOrdenados)
                    {
                        // Pega a string de valor
                        int tamanho = campo.Tamanho;
                        if (tamanho > linha.Length - pos)
                            tamanho = linha.Length - pos;
                        string strValor = linha.Substring(pos, tamanho);
                        pos += tamanho;

                        // Pega a classe que vai interpretar o valor
                        ConversorCampoBase conversorCampo = dicionarioConversao[campo.Tipo];

                        // Converte o valor
                        object valor = conversorCampo.DeTextoParaObjeto(campo.Tipo, strValor, campo.ParametrosTipo);

                        // Atribui o valor à coluna da linha
                        dr[campo.Nome] = valor == null ? DBNull.Value : valor;
                    }

                    // Adiciona linha na tabela
                    tb.Rows.Add(dr);
                }

                // Proxima linha
                linha = reader.ReadLine();
            }

            // Retorna
            return ds;
        }

        public ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, Stream stream, string nomeArquivo)
        {
            // Inicializa
            ArquivoBVMFInfo arquivoEncontrado = null;

            // Varre a lista de layouts 
            foreach (LayoutBVMFInfo layout in layouts)
            {
                // Verifica se o nome do layout é o inicio do nome do arquivo
                if (nomeArquivo.StartsWith(layout.Nome))
                {
                    // Faz o parse da data
                    DateTime dataReferencia = DateTime.MinValue;
                    string[] tempData = nomeArquivo.Split('_');
                    if (tempData.Length > 1)
                    {
                        string data = tempData[tempData.Length - 1].Split('.')[0];
                        ConversorCampoData parserData =
                            new ConversorCampoData(
                                new ConversorCampoDataParametro() 
                                {
                                    FormatoData = "yyyymmdd"
                                });
                        dataReferencia = (DateTime)parserData.DeTextoParaObjeto(typeof(DateTime), data);
                    }

                    // Cria informacoes do arquivo encontrado
                    arquivoEncontrado =
                        new ArquivoBVMFInfo()
                        {
                            DataMovimento = dataReferencia,
                            Layout = layout,
                            NomeArquivo = nomeArquivo
                        };

                    // Sai do loop
                    break;
                }
            }

            // Retorna
            return arquivoEncontrado;
        }

        #endregion
    }
}
