using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Gradual.OMS.Library;
using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Implementação do servico de persistencia de layouts para XML.
    /// </summary>
    public class ServicoIntegracaoBVMFPersistenciaLayoutsBin : IServicoIntegracaoBVMFPersistenciaLayouts
    {
        private ServicoIntegracaoBVMFPersistenciaLayoutsBinConfig _config = 
            GerenciadorConfig.ReceberConfig<ServicoIntegracaoBVMFPersistenciaLayoutsBinConfig>();
        private List<LayoutBVMFInfo> _layouts = null;
        
        public ServicoIntegracaoBVMFPersistenciaLayoutsBin()
        {
            carregarColecao();
        }
        
        #region IServicoIntegracaoBVMFPersistenciaLayouts Members

        public List<LayoutBVMFInfo> ListarLayouts()
        {
            return _layouts;
        }

        public List<LayoutBVMFInfo> ListarLayouts(Type conversorTipo)
        {
            var r = from l in _layouts
                    where l.ConversorTipo == conversorTipo
                    select l;
            return r.ToList<LayoutBVMFInfo>();
        }

        public LayoutBVMFInfo ReceberLayout(string nomeLayout)
        {
            // Faz a consulta e retorna
            LayoutBVMFInfo ret =
                (from l in _layouts
                 where l.Nome == nomeLayout
                 select l).FirstOrDefault();
            return ret;
        }

        public LayoutBVMFInfo ReceberLayout(Type conversorTipo, string nomeLayout)
        {
            // Faz a consulta e retorna
            LayoutBVMFInfo ret = 
                (from l in _layouts
                 where l.ConversorTipo == conversorTipo && l.Nome == nomeLayout
                 select l).FirstOrDefault();
            return ret;
        }

        public void RemoverLayout(Type conversorTipo, string nomeLayout)
        {
            // Verifica se este layout já existe e remove
            LayoutBVMFInfo layoutExistente =
                (from l in _layouts
                 where l.ConversorTipo == conversorTipo && l.Nome == nomeLayout
                 select l).FirstOrDefault();
            if (layoutExistente != null)
                _layouts.Remove(layoutExistente);

            // Salva
            salvarColecao();
        }

        public void SalvarLayout(LayoutBVMFInfo layout)
        {
            // Verifica se este layout já existe e remove
            this.RemoverLayout(layout.ConversorTipo, layout.Nome);

            // Adiciona novo layout
            _layouts.Add(layout);

            // Salva
            salvarColecao();
        }

        #endregion

        private void carregarColecao()
        {
            // Carrega o arquivo
            if (File.Exists(_config.CaminhoArquivo))
            {
                FileStream stream = new FileStream(_config.CaminhoArquivo, FileMode.Open, FileAccess.Read);
                BinaryFormatter serializer = new BinaryFormatter();
                _layouts = (List<LayoutBVMFInfo>)serializer.Deserialize(stream);
                stream.Close();
            }
            else
            {
                _layouts = new List<LayoutBVMFInfo>();
            }
        }

        private void salvarColecao()
        {
            // Salva o arquivo
            FileStream stream = new FileStream(_config.CaminhoArquivo, FileMode.Create, FileAccess.Write);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, _layouts);
            stream.Close();
        }
    }
}
