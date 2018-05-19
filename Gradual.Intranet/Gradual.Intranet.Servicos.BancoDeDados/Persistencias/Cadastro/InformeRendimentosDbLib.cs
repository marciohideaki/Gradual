using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.OMS.Persistencia;
using System.Collections.Generic;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<InformeRendimentosInfo> ConsultarInformeRendimentos(ConsultarObjetosRequest<InformeRendimentosInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ConsultarObjetosResponse<InformeRendimentosInfo>();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_sel_rendimento"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "CPF", DbType.Int64, Int64.Parse(pParametros.Objeto.ConsultaCpfCnpj.Replace("-", "").Replace(".", "")));
                lAcessaDados.AddInParameter(lDbCommand, "NASCIMENTO", DbType.Date, pParametros.Objeto.ConsultaDataNascimento);
                lAcessaDados.AddInParameter(lDbCommand, "DEPENDENTE", DbType.Int16, pParametros.Objeto.ConsultaCondicaoDeDependente);
                lAcessaDados.AddInParameter(lDbCommand, "DATAINICIO", DbType.Date, pParametros.Objeto.ConsultaDataInicio);
                lAcessaDados.AddInParameter(lDbCommand, "DATAFIM", DbType.Date, pParametros.Objeto.ConsultaDataFim);
                lAcessaDados.AddInParameter(lDbCommand, "CODIGORETENCAO", DbType.Int16, pParametros.Objeto.ConsultaTipoInforme);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow item in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new InformeRendimentosInfo()
                        {
                            Data = item["Data"].DBToString(),
                            Imposto = item["Imposto"].DBToDecimal(),
                            Rendimento = item["Rendimento"].DBToDecimal(),
                        });
                    }
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<InformeRendimentosTesouroDiretoInfo> ConsultarInformeRendimentosTesouroDireto(ConsultarObjetosRequest<InformeRendimentosTesouroDiretoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<InformeRendimentosTesouroDiretoInfo>();
            lRetorno.Resultado = new List<InformeRendimentosTesouroDiretoInfo>();
            AcessaDados lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_rendimento_tesouro"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "CPF", DbType.Int64, Int64.Parse(pParametros.Objeto.ConsultaCpfCnpj.Replace("-", "").Replace(".", "").Replace("/", "")));
                lAcessaDados.AddInParameter(lDbCommand, "NASCIMENTO", DbType.Date, pParametros.Objeto.ConsultaDataNascimento);
                lAcessaDados.AddInParameter(lDbCommand, "DEPENDENTE", DbType.Int16, pParametros.Objeto.ConsultaCondicaoDeDependente);
                lAcessaDados.AddInParameter(lDbCommand, "ANO", DbType.Date, pParametros.Objeto.ConsultaAno);
                lAcessaDados.AddInParameter(lDbCommand, "ANOANTERIOR", DbType.Date, pParametros.Objeto.ConsultaAnoAnterior);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && null != lDataTable.Rows && lDataTable.Rows.Count > 0)
                    foreach (DataRow item in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new InformeRendimentosTesouroDiretoInfo()
                        {
                            Posicao = item["Posicao"].DBToString(),
                            Quantidade = item["qtd"].DBToDecimal(),
                            QuantidadeAnoAnterior = item["qtdAnoAnterior"].DBToDecimal(),
                            Valor = item["vlr"].DBToDecimal(),
                            ValorAnoAnterior = item["vlrAnoAnterior"].DBToDecimal()
                        });
                    }
            }

            return lRetorno;
        }
    }
}
