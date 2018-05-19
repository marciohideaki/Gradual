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
    public class ConversorLayoutBovespa : IConversorLayoutBase
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

            // Dicionario de conversao
            Dictionary<Type, ConversorCampoBase> dicionarioConversao =
                (GerenciadorConfig.ReceberConfig<ConversorLayoutBovespaConfig>()).CriarDicionario();

            // Varre o arquivo
            StreamReader reader = new StreamReader(stream);
            string linha = reader.ReadLine();
            while (linha != null)
            {
                // Inicializa
                pos = 0;

                // Pega ID da linha
                int idLinha = int.Parse(linha.Substring(0, 2));

                // Verifica se ID é diferente da linha anterior
                if (idLinha != ultimoIdLinha)
                {
                    // Acha objeto de tabela correspondente
                    tabelaAtual = (
                                from c in layout.Tabelas
                                where ((ConversorLayoutBovespaParametroTabela)c.Parametros).RegistroTipo == idLinha
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

        public ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, Stream stream)
        {
            // Inicializa
            LayoutBVMFInfo layoutEncontrado = null;
            DateTime? dataMovimento = null;

            // Dicionario de conversao
            Dictionary<Type, ConversorCampoBase> dicionarioConversao =
                (GerenciadorConfig.ReceberConfig<ConversorLayoutBovespaConfig>()).CriarDicionario();

            // Le a primeira linha do arquivo
            StreamReader reader = new StreamReader(stream);
            string linha = reader.ReadLine();

            // Testa a linha com cada layout
            foreach (LayoutBVMFInfo layout in layouts)
            {
                // Pega tabela header
                LayoutBVMFTabelaInfo tabela = (from t in layout.Tabelas
                                           where t.Parametros != null &&
                                           ((ConversorLayoutBovespaParametroTabela)t.Parametros).TabelaTipo ==
                                              ConversorLayoutBovespaParametroTabelaTipoEnum.Header
                                           select t).FirstOrDefault();

                // Apenas se achou a tabela
                if (tabela != null)
                {

                    // Acha campo que descreve o layout
                    LayoutBVMFCampoInfo campo = (from c in tabela.Campos
                                             where c.Parametros != null &&
                                             ((ConversorCampoBovespaColunaParametro)c.Parametros).ColunaTipo ==
                                                ConversorCampoBovespaColunaTipoEnum.TipoLayout
                                             select c).FirstOrDefault();

                    // Apenas se achou o campo
                    if (campo != null)
                    {
                        // Calcula a posicao do campo para leitura
                        int pos = 0;
                        foreach (LayoutBVMFCampoInfo campo2 in tabela.Campos)
                            if (campo2 != campo)
                                pos += campo2.Tamanho;
                            else
                                break;

                        // Continua apenas se puder extrair o valor
                        if (pos < linha.Length)
                        {
                            // Faz a extracao
                            string tipoLayout = linha.Substring(pos, campo.Tamanho);

                            // Compara
                            if (tipoLayout == layout.Nome)
                            {
                                // Sinaliza o layout encontrado
                                layoutEncontrado = layout;

                                // Caso exista, faz a leitura da data do movimento
                                LayoutBVMFCampoInfo dataM = (from c in tabela.Campos
                                                         where c.Parametros != null &&
                                                         ((ConversorCampoBovespaColunaParametro)c.Parametros).ColunaTipo ==
                                                            ConversorCampoBovespaColunaTipoEnum.DataMovimento
                                                         select c).FirstOrDefault();

                                // Apenas se achou o campo
                                if (dataM != null)
                                {
                                    // Calcula a posicao do campo para leitura
                                    int pos2 = 0;
                                    foreach (LayoutBVMFCampoInfo campo2 in tabela.Campos)
                                        if (campo2 != dataM)
                                            pos2 += campo2.Tamanho;
                                        else
                                            break;

                                    // Faz a extracao
                                    ConversorCampoBase conversorCampo = dicionarioConversao[typeof(DateTime)];
                                    dataMovimento =
                                        (DateTime)
                                            conversorCampo.DeTextoParaObjeto(
                                                typeof(DateTime),
                                                linha.Substring(pos2, dataM.Tamanho),
                                                new ConversorCampoDataParametro() { FormatoData = "ddmmyy" });
                                }

                                // Sai do loop
                                break;
                            }
                        }
                    }
                }
            }

            // Retorna
            if (layoutEncontrado != null)
                return new ArquivoBVMFInfo() 
                { 
                    Layout = layoutEncontrado, 
                    DataMovimento = dataMovimento 
                };
            else
                return null;
        }

        #endregion
    }
}
