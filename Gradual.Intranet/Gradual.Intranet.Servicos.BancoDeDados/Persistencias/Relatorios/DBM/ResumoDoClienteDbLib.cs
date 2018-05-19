using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class ResumoDoClienteDbLib
    {
        public ReceberObjetoResponse<ResumoDoClienteDadosCadastraisInfo> ReceberDadosCadastrais(ReceberEntidadeRequest<ResumoDoClienteDadosCadastraisInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoDoClienteDadosCadastraisInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Objeto = new ResumoDoClienteDadosCadastraisInfo();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_DBM_DadosCadastrais"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.ConsultaCdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pnm_cliente", DbType.String, pParametro.Objeto.ConsultaNmCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametro.Objeto.ConsultaCodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lRetorno = new ReceberObjetoResponse<ResumoDoClienteDadosCadastraisInfo>()
                    {
                        Objeto = new ResumoDoClienteDadosCadastraisInfo()
                        {
                            CdCliente = lDataTable.Rows[0]["CD_CLIENTE"].DBToInt32(),
                            DsTipoCliente = lDataTable.Rows[0]["DS_TIPO_CLIENTE"].DBToString(),
                            DsUF = lDataTable.Rows[0]["SG_ESTADO"].DBToString(),
                            DtCriacao = lDataTable.Rows[0]["DT_CRIACAO"].DBToDateTime(),
                            DtUltimaOperacao = lDataTable.Rows[0]["DT_ULT_OPER"].DBToDateTime(),
                            NmCidade = lDataTable.Rows[0]["NM_CIDADE"].DBToString(),
                            NmCliente = lDataTable.Rows[0]["NM_CLIENTE"].DBToString(),
                            CdDddCelular1 = lDataTable.Rows[0]["CD_DDD_CELULAR1"].DBToString(),
                            CdDddCelular2 = lDataTable.Rows[0]["CD_DDD_CELULAR2"].DBToString(),
                            CDDddTel = lDataTable.Rows[0]["CD_DDD_TEL"].DBToString(),
                            NmBairro = lDataTable.Rows[0]["NM_BAIRRO"].DBToString(),
                            NmComplementoEndereco = lDataTable.Rows[0]["NM_COMP_ENDE"].DBToString(),
                            NmEmail = lDataTable.Rows[0]["NM_E_MAIL"].DBToString(),
                            NrCelular1 = lDataTable.Rows[0]["NR_CELULAR1"].DBToString(),
                            NrCelular2 = lDataTable.Rows[0]["NR_CELULAR2"].DBToString(),
                            NmLogradouro = lDataTable.Rows[0]["NM_LOGRADOURO"].DBToString(),
                            NrPredio = lDataTable.Rows[0]["NR_PREDIO"].DBToString(),
                            NrRamal = lDataTable.Rows[0]["NR_RAMAL"].DBToString(),
                            NrTelefone = lDataTable.Rows[0]["NR_TELEFONE"].DBToString(),
                        }
                    };
            }

            return lRetorno;
        }

        public ReceberObjetoResponse<ResumoDoClienteCorretagemInfo> ReceberCorretagem(ReceberEntidadeRequest<ResumoDoClienteCorretagemInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoDoClienteCorretagemInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno = new ReceberObjetoResponse<ResumoDoClienteCorretagemInfo>();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_DBM_Corretagem_ResCliente"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametros.Objeto.ConsultaCdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pnm_cliente", DbType.String, pParametros.Objeto.ConsultaNmCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametros.Objeto.ConsultaCodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new ResumoDoClienteCorretagemInfo();
                    lRetorno.Objeto.VlVolumeMes = lDataTable.Rows[0]["vl_VolumeMes"].DBToDecimal();
                    lRetorno.Objeto.VlDisponivel = lDataTable.Rows[0]["vl_Disponivel"].DBToDecimal();
                    lRetorno.Objeto.VlCorretagemMes = lDataTable.Rows[0]["vl_CorretagemMes"].DBToDecimal();
                    lRetorno.Objeto.VlVolumeMediaAno = lDataTable.Rows[0]["vl_VolumeMediaAno"].DBToDecimal();
                    lRetorno.Objeto.VlVolumeEm12Meses = lDataTable.Rows[0]["vl_VolumeEm12Meses"].DBToDecimal();
                    lRetorno.Objeto.VlCorretagemMediaAno = lDataTable.Rows[0]["vl_CorretagemMediaAno"].DBToDecimal();
                    lRetorno.Objeto.VlCorretagemEm12Meses = lDataTable.Rows[0]["vl_CorretagemEm12Meses"].DBToDecimal();
                }
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<ResumoDoClienteCarteiraInfo> ConsultarCarteira(ConsultarEntidadeRequest<ResumoDoClienteCarteiraInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ResumoDoClienteCarteiraInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Resultado = new List<ResumoDoClienteCarteiraInfo>();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_DBM_ResCliente_PosCustodia"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pnm_cliente", DbType.String, pParametros.Objeto.ConsultaNomeCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametros.Objeto.ConsultaCodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametros.Objeto.ConsultaCodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new ResumoDoClienteCarteiraInfo()
                        {
                            DsCarteira = lLinha["CD_NEGOCIO"].DBToString(),
                            QtQuantidade = lLinha["QTDE_DISP"].DBToInt32(),
                            VlCotacao = lLinha["CUSTODIA"].DBToDecimal(),
                        });
            }

            return lRetorno;
        }
    }
}
