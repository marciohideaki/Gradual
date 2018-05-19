using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Monitoramento
{
    public class TransporteRelatorio_001 : ICodigoEntidade
    {
        public string CodCliente { get; set; }

        public string NomeCliente { get; set; }

        public string CodAssessor { get; set; }

        public string Instrumento { get; set; }

        public string Quantidade { get; set; }

        public string Vencimento { get; set; }

        public string Carteira { get; set; }

        public string QuantidadeAtual { get; set; }

        public string QuantidadeAbertura { get; set; }

        public string QuantidadeD1 { get; set; }

        public string NomeAssessor { get; set; }

        public string DtStrike { get; set; }

        public string PrecoExercicio { get; set; }

        public List<TransporteRelatorio_001> TraduzirLista(List<ClientePosicaoDeOpcaoRelInfo> pParametro, List<int> lstAssessores)
        {
            var lRetorno = new List<TransporteRelatorio_001>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(
                    cpo =>
                    {
                        if (lstAssessores.Contains(cpo.CdAssessor))
                        {
                            lRetorno.Add(new TransporteRelatorio_001()
                            {
                                Carteira           = cpo.IdCarteira.DBToString(),
                                CodAssessor        = cpo.CdAssessor.DBToString(),
                                CodCliente         = cpo.CdCliente.DBToString(),
                                Instrumento        = cpo.DsPapel,
                                NomeCliente        = cpo.NmCliente.ToStringFormatoNome(),
                                Quantidade         = cpo.QtQuantidade.DBToString(),
                                Vencimento         = cpo.DtVencimento.ToString("dd/MM/yyyy"),
                                QuantidadeAtual    = cpo.QtQunatidadeAtual.DBToString(),
                                QuantidadeD1       = cpo.QtQuandidadeD1.DBToString(),
                                QuantidadeAbertura = cpo.QtQuantidadeAbertura.DBToString(),
                                DtStrike           = cpo.DtStrike.ToString("dd/MM/yyyy"),
                                NomeAssessor       = cpo.NomeAssessor,
                                PrecoExercicio     = cpo.PrecoExercicio.ToString("N2")
                            });

                            CodAssessor = cpo.CdAssessor.DBToString();
                        }
                    });

            lRetorno.Sort((s1, s2) => { return s1.NomeCliente.CompareTo(s2.NomeCliente); }); //--> Ordenando por nome.

            return lRetorno;
        }

        public List<TransporteRelatorio_001> TraduzirLista(List<ClientePosicaoDeOpcaoRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_001>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(
                    cpo =>
                    {
                        lRetorno.Add(new TransporteRelatorio_001()
                        {
                            Carteira           = cpo.IdCarteira.DBToString(),
                            CodAssessor        = cpo.CdAssessor.DBToString(),
                            CodCliente         = cpo.CdCliente.DBToString(),
                            Instrumento        = cpo.DsPapel,
                            NomeCliente        = cpo.NmCliente.ToStringFormatoNome(),
                            Quantidade         = cpo.QtQuantidade.DBToString(),
                            Vencimento         = cpo.DtVencimento.ToString("dd/MM/yyyy"),
                            QuantidadeAtual    = cpo.QtQunatidadeAtual.DBToString(),
                            QuantidadeD1       = cpo.QtQuandidadeD1.DBToString(),
                            QuantidadeAbertura = cpo.QtQuantidadeAbertura.DBToString(),
                            DtStrike           = cpo.DtStrike.ToString("dd/MM/yyyy"),
                            NomeAssessor       = cpo.NomeAssessor,
                            PrecoExercicio     = cpo.PrecoExercicio.ToString("N2")
                        });

                        CodAssessor = cpo.CdAssessor.DBToString();
                        
                    });

            lRetorno.Sort((s1, s2) => { return s1.NomeCliente.CompareTo(s2.NomeCliente); }); //--> Ordenando por nome.

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }

    public class TransporteRelatorio_002 : ICodigoEntidade
    {

        public string CodCliente { get; set; }

        public string NomeCliente { get; set; }

        public string CodAssessor { get; set; }

        public string Instrumento { get; set; }

        public string Quantidade { get; set; }

        public string Vencimento { get; set; }

        public string Carteira { get; set; }

        public string QuantidadeAtual { get; set; }

        public string QuantidadeAbertura { get; set; }

        public string QuantidadeD1 { get; set; }

        public string NomeAssessor { get; set; }

        public string Strike { get; set; }

        public string OperacoesDia { get; set; }

        public string QtdeExercicio { get; set; }

        public List<TransporteRelatorio_002> TraduzirLista(List<ClientePosicaoOpcaoExercidaRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_002>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(
                    cpo =>
                    {
                        lRetorno.Add(new TransporteRelatorio_002()
                        {
                            Carteira           = cpo.IdCarteira.DBToString(),
                            CodAssessor        = cpo.CdAssessor.DBToString(),
                            CodCliente         = cpo.CdCliente.DBToString(),
                            Instrumento        = cpo.DsPapel,
                            NomeCliente        = cpo.NmCliente.ToStringFormatoNome(),
                            Quantidade         = cpo.QtQuantidade.DBToString(),
                            Vencimento         = cpo.DtVencimento.ToString("dd/MM/yyyy"),
                            QuantidadeAtual    = cpo.QtQunatidadeAtual.DBToString(),
                            QuantidadeD1       = cpo.QtQuandidadeD1.DBToString(),
                            QuantidadeAbertura = cpo.QtQuantidadeAbertura.DBToString(),
                            Strike             = cpo.DsPapel.Substring(5),
                            NomeAssessor       = cpo.NomeAssessor,
                            OperacoesDia       = cpo.OperacoesDia,
                            QtdeExercicio      = cpo.QtdeExercicio.ToString(),
                        });

                        CodAssessor = cpo.CdAssessor.DBToString();

                    });

            lRetorno.Sort((s1, s2) => { return s1.NomeCliente.CompareTo(s2.NomeCliente); }); //--> Ordenando por nome.

            return lRetorno;
        }
        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}