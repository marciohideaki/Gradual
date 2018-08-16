using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.ConsolidadorRelatorioCCLib;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_006 : ICodigoEntidade
    {
        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string Assessor { get; set; }

        public string D0 { get; set; }

        public string D1 { get; set; }

        public string D2 { get; set; }

        public string D3 { get; set; }

        public string CM { get; set; }

        public double D0Total { get; set; }

        public double D1Total { get; set; }

        public double D2Total { get; set; }

        public double D3Total { get; set; }

        public double CMTotal { get; set; }


        public List<TransporteRelatorio_006> TraduzirLista(List<ContaCorrenteRiscoInfo> pParametro, string pOrdernarPor)
        {
            var lRetorno = new List<TransporteRelatorio_006>();
            var lConsultarEntidadeCadastroRequest = new ConsultarEntidadeCadastroRequest<SinacorListaInfo>() { EntidadeCadastro = new SinacorListaInfo(eInformacao.Assessor), };
            var lListaAssessor = Ativador.Get<IServicoPersistenciaCadastro>().ConsultarEntidadeCadastro<SinacorListaInfo>(lConsultarEntidadeCadastroRequest);
            var lAsessor = new SinacorListaInfo();

            if (pParametro != null)
                pParametro.ForEach(delegate(ContaCorrenteRiscoInfo ccr)
                {
                    lAsessor = lListaAssessor.Resultado.Find(delegate(SinacorListaInfo sli) { return sli.Id == ccr.IdAssessor.Value.DBToString(); });

                    D0Total += ccr.SaldoD0.DBToDouble();
                    D1Total += ccr.SaldoD1.DBToDouble();
                    D2Total += ccr.SaldoD2.DBToDouble();
                    D3Total += ccr.SaldoD3.DBToDouble();
                    CMTotal += (null != ccr.SaldoContaMargem && ccr.SaldoContaMargem.HasValue) ? ccr.SaldoContaMargem.Value.DBToDouble() : 0D;

                    lRetorno.Add(new TransporteRelatorio_006()
                    {
                        Assessor = null != lAsessor ? lAsessor.Value : string.Empty,
                        CM = (null != ccr.SaldoContaMargem && ccr.SaldoContaMargem.HasValue) ? ccr.SaldoContaMargem.Value.ToString("N2") : "0,00",
                        CpfCnpj = ccr.DsCpfCnpj.ToCpfCnpjString(),
                        D0 = ccr.SaldoD0.ToString("N2"),
                        D1 = ccr.SaldoD1.ToString("N2"),
                        D2 = ccr.SaldoD2.ToString("N2"),
                        D3 = ccr.SaldoD3.ToString("N2"),
                        NomeCliente = ccr.NomeCliente.ToStringFormatoNome(),
                    });
                });

            switch (pOrdernarPor) //--> Realizando as ordenações.
            {
                case "D0":
                    lRetorno.Sort((tr1, tr2) => Comparer<decimal>.Default.Compare(tr1.D0.DBToDecimal(), tr2.D0.DBToDecimal()));
                    break;
                case "D1":
                    lRetorno.Sort((tr1, tr2) => Comparer<decimal>.Default.Compare(tr1.D1.DBToDecimal(), tr2.D1.DBToDecimal()));
                    break;
                case "D2":
                    lRetorno.Sort((tr1, tr2) => Comparer<decimal>.Default.Compare(tr1.D2.DBToDecimal(), tr2.D2.DBToDecimal()));
                    break;
                case "D3":
                    lRetorno.Sort((tr1, tr2) => Comparer<decimal>.Default.Compare(tr1.D3.DBToDecimal(), tr2.D3.DBToDecimal()));
                    break;
                case "CM":
                    lRetorno.Sort((tr1, tr2) => Comparer<decimal>.Default.Compare(tr1.CM.DBToDecimal(), tr2.CM.DBToDecimal()));
                    break;
                case "assessor":
                    lRetorno.Sort((tr1, tr2) => Comparer<string>.Default.Compare(tr1.Assessor, tr2.Assessor));
                    break;
                default:
                    lRetorno.Sort((tr1, tr2) => Comparer<string>.Default.Compare(tr1.NomeCliente, tr2.NomeCliente));
                    break;
            }

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}