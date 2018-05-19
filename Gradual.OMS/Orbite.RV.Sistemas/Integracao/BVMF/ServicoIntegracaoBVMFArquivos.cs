using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    public class ServicoIntegracaoBVMFArquivos : IServicoIntegracaoBVMFArquivos
    {
        #region IServicoIntegracaoBVMFArquivos Members

        public DataSet InterpretarArquivo(string arquivo)
        {
            FileStream file = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
            DataSet ds = this.InterpretarArquivo(file);
            file.Close();
            return ds;
        }

        public DataSet InterpretarArquivo(Stream arquivo)
        {
            // Inicializa
            DataSet retorno = null;

            // Pega lista de layouts da persistencia
            IServicoIntegracaoBVMFPersistenciaLayouts servicoPersistenciaLayouts = 
                Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();

            // Infere o layout
            ArquivoBVMFInfo arquivoBVMF = this.InferirLayout(servicoPersistenciaLayouts.ListarLayouts(), arquivo);

            // A inferencia de layout muda posicao do stream, volta para o inicio
            arquivo.Position = 0;

            // Se achou, interpreta o arquivo
            if (arquivoBVMF != null)
                retorno = this.InterpretarArquivo(arquivoBVMF.Layout, arquivo);

            // Retorna
            return retorno;
        }

        public DataSet InterpretarArquivo(LayoutBVMFInfo layout, string arquivo)
        {
            FileStream file = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
            DataSet ds = this.InterpretarArquivo(layout, file, arquivo);
            file.Close();
            return ds;
        }

        public DataSet InterpretarArquivo(LayoutBVMFInfo layout, Stream stream)
        {
            return this.InterpretarArquivo(layout, stream, null);
        }

        public DataSet InterpretarArquivo(LayoutBVMFInfo layout, Stream stream, string nomeArquivo)
        {
            // Cria o conversor do layout
            IConversorLayoutBase conversor = 
                (IConversorLayoutBase)
                    Activator.CreateInstance(layout.ConversorTipo);

            // Pede a interpretacao do arquivo para o conversor
            DataSet ds = conversor.InterpretarArquivo(layout, stream);

            // Retorna
            return ds;
        }

        public DataSet CriarSchema(LayoutBVMFInfo layout)
        {
            // Cria dataset vazio
            DataSet ds = new DataSet(layout.Nome);

            // Varre as tabelas
            foreach (LayoutBVMFTabelaInfo tabela in layout.Tabelas)
            {
                // Cria tabela
                DataTable tb = ds.Tables.Add(tabela.Nome);

                // Adiciona os campos
                foreach (LayoutBVMFCampoInfo campo in tabela.Campos)
                    tb.Columns.Add(campo.Nome, campo.Tipo);
            }

            // Retorna 
            return ds;
        }

        public ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, string arquivo)
        {
            // Abre o arquivo
            FileStream stream = File.Open(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Repassa a chamada
            ArquivoBVMFInfo arquivoBVMF = InferirLayout(layouts, stream);
            stream.Close();
            if (arquivoBVMF != null)
                arquivoBVMF.NomeArquivo = arquivo;

            // Retorna
            return arquivoBVMF;
        }

        public ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, Stream stream)
        {
            // Inicializa
            ArquivoBVMFInfo arquivoInferido = null;
            
            // Separa os layouts por conversor
            var conversores = from l in layouts
                              group l by l.ConversorTipo into g
                              select new { ConversorTipo = g.Key, Layouts = g };

            // Para cada grupo, pede para inferir os layouts
            foreach (var conversor in conversores)
            {
                // Cria o conversor
                IConversorLayoutBase conversorInstancia = 
                    (IConversorLayoutBase)
                        Activator.CreateInstance(conversor.ConversorTipo);

                // Transforma a lista de layouts num list
                List<LayoutBVMFInfo> layouts2 = new List<LayoutBVMFInfo>(
                    from c in conversor.Layouts
                    select c);

                // Pede a inferencia
                arquivoInferido = conversorInstancia.InferirLayout(layouts2, stream);

                // Se achou, sai do loop
                if (arquivoInferido != null)
                    break;
            }

            // Retorna
            return arquivoInferido;
        }

        public List<ArquivoBVMFInfo> ListarDiretorio(string diretorio)
        {
            // Pega lista de arquivos
            string[] arquivos = Directory.GetFiles(diretorio);

            // Retorna
            return ListarDiretorio(arquivos);
        }

        public List<ArquivoBVMFInfo> ListarDiretorio(string diretorio, string condicoes)
        {
            // Pega lista de arquivos
            string[] arquivos = Directory.GetFiles(diretorio, condicoes);

            // Retorna
            return ListarDiretorio(arquivos);
        }

        public List<ArquivoBVMFInfo> ListarDiretorio(string[] arquivos)
        {
            // Inicializa
            List<ArquivoBVMFInfo> retorno = new List<ArquivoBVMFInfo>();

            // Pega lista de layouts da persistencia
            IServicoIntegracaoBVMFPersistenciaLayouts servicoPersistenciaLayouts =
                Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();
            List<LayoutBVMFInfo> layouts = servicoPersistenciaLayouts.ListarLayouts();

            // Varre a lista de arquivos do diretorio
            foreach (string arquivo in arquivos)
            {
                // Infere
                ArquivoBVMFInfo arquivoBVMF = this.InferirLayout(layouts, arquivo);

                // Se achou, insere na colecao
                if (arquivoBVMF != null)
                    retorno.Add(arquivoBVMF);
            }

            // Retorna
            return retorno;
        }

        #endregion
    }
}
