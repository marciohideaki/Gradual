using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.OMS.Seguranca.Lib.Dados;
using Gradual.OMS.Seguranca.Lib;
using System.Data.Common;


namespace Gradual.OMS.Seguranca
{
    public class PersistenciaControleAcesso
    {

        private const string gNomeConexaoSinacor     = "SINACOR";
        private const string gNomeConexaoSQL         = "CONTROLEACESSO";
        private const string gNomeConexaoContaMargem = "CONTAMARGEM";

        public ListarAssessorCompartilhadoResponse ObterClientesCompartilhados(ListarAssessorCompartilhadoRequest pParametros)
        {
            ListarAssessorCompartilhadoResponse _response = new ListarAssessorCompartilhadoResponse();
            ClienteAssessorCompartilhadoInfo info;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_clientes_compartilhados"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametros.CodigoAssessor);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            info = new ClienteAssessorCompartilhadoInfo();

                            info.IdAssessor = (lDataTable.Rows[i]["id_assessor"]).DBToInt32();
                            info.CodigoBolsa = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            info.NomeCliente = (lDataTable.Rows[i]["ds_cliente"]).DBToString();

                            _response.ListaClientes.Add(info);
                        }
                    }

                }

                return _response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        public VerificarTentativaAcessoResponse ControlarTentativasAcesso(VerificarTentativaAcessoRequest pParametros)
        {
            VerificarTentativaAcessoResponse _response = new VerificarTentativaAcessoResponse();      
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_tentativa_acesso"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idUsuario", DbType.Int32, pParametros._TentativaUsuarioInfo.IdUsuario);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsUsuario", DbType.AnsiString, pParametros._TentativaUsuarioInfo.DsUsuario);
                    lAcessaDados.AddInParameter(lDbCommand, "@IP", DbType.AnsiString, pParametros._TentativaUsuarioInfo.IPUsuario);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {                                                
                        _response.DataUltimaTentativa = (lDataTable.Rows[0]["dtUltimaTentativa"]).DBToDateTime();                        
                        _response.DataBloqueio = (lDataTable.Rows[0]["dtBloqueio"]).DBToDateTime();
                        _response.NrTentativas = (lDataTable.Rows[0]["nrTentativas"]).DBToInt32();

                        if (_response.NrTentativas >= 3 )
                        {
                            _response.StBloqueado = true;
                            _response.DescricaoResposta = "Usuário bloqueado por (três) tentativas incorretas de acesso.";
                        }
                        else
                        {
                            _response.StBloqueado = false;
                        }                        
                        _response.DsUsuario = (lDataTable.Rows[0]["dsUsuario"]).DBToString();                               
                    }

                }

                return _response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        public int ObterNumeroTentativaAcesso(int IdUsuario)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            int NrAcessos = 0;


            lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obterNumeroTentativaAcesso"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@idUsuario", DbType.Int32, IdUsuario);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    NrAcessos = (lDataTable.Rows[0]["nrTentativas"]).DBToInt32();
                }

            }


            return NrAcessos;

        }

        public int ObterCodigoAcessoSistema(int idLogin)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            int CodigoAcesso = 0;

            lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_verifica_acesso"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, idLogin);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    CodigoAcesso = (lDataTable.Rows[0]["CodigoRetorno"]).DBToInt32();
                }
            }

            return CodigoAcesso;

        }
    

        public AlterarPermissaoAcessoResponse AlterarPermissaoAcesso(AlterarPermissaoAcessoRequest pParametros)
        {
            AlterarPermissaoAcessoResponse _response = new AlterarPermissaoAcessoResponse();
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_operacao_acesso"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idUsuario", DbType.Int32, pParametros.PermissaoAcessoUsuarioInfo.IdUsuario);

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    _response.Mensagem = "Operacao realizada com sucesso.";
                    _response.Resposta = Library.MensagemResponseStatusEnum.OK;
                }

            }
            catch
            {
                _response.Mensagem = "Ocorreu um erro ao tentar efetuar a transação";
                _response.Resposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return _response;

        }
    }
}
