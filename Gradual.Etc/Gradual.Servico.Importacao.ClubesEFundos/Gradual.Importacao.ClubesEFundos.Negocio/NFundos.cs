using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Gradual.MinhaConta.Dados;
using Gradual.MinhaConta.Entidade;
using System.Globalization;

namespace Gradual.MinhaConta.Negocio
{
    public class NFundos
    { 
        #region ProcessarArquivos
        public void ProcessarArquivos(string arquivo)
        {
            try
            {
                XDocument document = XDocument.Load(arquivo);

                List<EFundos> lstFundos = new List<EFundos>();
                EFundos eFundos         = new EFundos();
                EFundos eFundosAux      = new EFundos();
                lstFundos.Clear();

                var lListaCarteiras = from carteira in document.Descendants("carteira")
                                      select new
                                      {
                                          CARTEIRA  = carteira.Element("identificacao").Element("codigo").Value,
                                          NOMEFUNDO = carteira.Element("identificacao").Element("nome-completo").Value,
                                          DATAATU   = carteira.Element("identificacao").Element("data-atual").Value,
                                      };

                foreach (var lCarteira in lListaCarteiras)
                {
                    eFundosAux.Carteira  = int.Parse(lCarteira.CARTEIRA);
                    eFundosAux.DataAtu   = DateTime.Parse(lCarteira.DATAATU);
                    eFundosAux.NomeFundo = lCarteira.NOMEFUNDO;
                }

                var lListaClientes = from cliente in document.Descendants("cliente")
                                     select new
                                     {
                                         CLIENTE      = cliente.Element("identificacao").Element("codigo").Value,
                                         NOMECLIENTE  = cliente.Element("identificacao").Element("nome").Value,
                                         COTA         = cliente.Element("saldo").Element("valor-da-cota").Value,
                                         QUANTIDADE   = cliente.Element("saldo").Element("saldo-de-cotas").Value,
                                         VALORBRUTO   = cliente.Element("saldo").Element("valor-bruto").Value,
                                         IR           = cliente.Element("saldo").Element("ir").Value,
                                         IOF          = cliente.Element("saldo").Element("iof").Value,
                                         VALORLIQUIDO = cliente.Element("saldo").Element("valor-liquido").Value,
                                         CPFCNPJ      = cliente.Element("identificacao").Element("cpf-cnpj").Value,
                                     };

                foreach (var lCliente in lListaClientes)
                {
                    eFundos              = new EFundos();
                    eFundos.Carteira     = eFundosAux.Carteira;
                    eFundos.DataAtu      = eFundosAux.DataAtu;
                    eFundos.NomeFundo    = eFundosAux.NomeFundo;
                    eFundos.Cliente      = int.Parse(lCliente.CLIENTE);
                    eFundos.NomeCliente  = lCliente.NOMECLIENTE;
                    eFundos.Cota         = decimal.Parse(lCliente.COTA, new CultureInfo("pt-BR") );
                    eFundos.Quantidade   = decimal.Parse(lCliente.QUANTIDADE, new CultureInfo("pt-BR"));
                    eFundos.ValorBruto   = decimal.Parse(lCliente.VALORBRUTO, new CultureInfo("pt-BR"));
                    eFundos.IR           = decimal.Parse(lCliente.IR, new CultureInfo("pt-BR"));
                    eFundos.IOF          = decimal.Parse(lCliente.IOF, new CultureInfo("pt-BR"));
                    eFundos.ValorLiquido = decimal.Parse(lCliente.VALORLIQUIDO, new CultureInfo("pt-BR"));
                    eFundos.CpfCnpj      = lCliente.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", "");
                    lstFundos.Add(eFundos);
                }

                new DFundos().AtualizarFundos(lstFundos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        public BindingList<EFundos> ConsultarFundos(string _CPFCNPJ)
        {
            return new DFundos().ConsultarFundos(_CPFCNPJ);
        }
    }
}
