using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static SalvarEntidadeResponse<ClienteDocumentacaoEntregueInfo> SalvarClienteDocumentacaoEntregue(SalvarObjetoRequest<ClienteDocumentacaoEntregueInfo> pParametros)
        {
            var lRetorno = new SalvarEntidadeResponse<ClienteDocumentacaoEntregueInfo>();
            var lAcessaDados = new AcessaDados() { ConnectionStringName = gNomeConexaoCadastro };

            lRetorno.Objeto = new ClienteDocumentacaoEntregueInfo();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_documentacaoentregue_ins"))
            {
                pParametros.Objeto.DtAdesaoDocumento = DateTime.Now;

                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_documentacaoentregue", DbType.Int32, pParametros.Objeto.IdDocumentacaoEntregue);
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@id_login_usuario_logado", DbType.Int32, pParametros.Objeto.IdLoginUsuarioLogado);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_adesaodocumento", DbType.DateTime, pParametros.Objeto.DtAdesaoDocumento);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_observacao", DbType.String, pParametros.Objeto.DsObservacao);

                var lNovoId = lAcessaDados.ExecuteScalar(lDbCommand);

                if (null == pParametros.Objeto.IdDocumentacaoEntregue)
                    pParametros.Objeto.IdDocumentacaoEntregue = (int)lNovoId;
            }

            return new SalvarEntidadeResponse<ClienteDocumentacaoEntregueInfo>() { Objeto = pParametros.Objeto };
        }

        public static RemoverObjetoResponse<ClienteDocumentacaoEntregueInfo> RemoverClienteDocumentacaoEntregue(RemoverEntidadeRequest<ClienteDocumentacaoEntregueInfo> pParametro)
        {
            var lRetorno = new RemoverObjetoResponse<ClienteDocumentacaoEntregueInfo>();
            var lAcessaDados = new AcessaDados() { ConnectionStringName = gNomeConexaoCadastro };

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_documentacaoentregue_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_documentacaoentregue", DbType.Int32, pParametro.Objeto.IdDocumentacaoEntregue);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }

        public static ReceberObjetoResponse<ClienteDocumentacaoEntregueInfo> ReceberClienteDocumentacaoEntregue(ReceberEntidadeRequest<ClienteDocumentacaoEntregueInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ClienteDocumentacaoEntregueInfo>();
            var lAcessaDados = new AcessaDados() { ConnectionStringName = gNomeConexaoCadastro };

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_documentacaoentregue_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_documentacaoentregue", DbType.Int32, pParametro.Objeto.IdDocumentacaoEntregue);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto.DsObservacao = lDataTable.Rows[0]["ds_observacao"].DBToString();
                    lRetorno.Objeto.DtAdesaoDocumento = lDataTable.Rows[0]["dt_adesaodocumento"].DBToDateTime();
                    lRetorno.Objeto.IdCliente = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                    lRetorno.Objeto.IdDocumentacaoEntregue = lDataTable.Rows[0]["id_cliente_documentacaoentregue"].DBToInt32();
                    lRetorno.Objeto.IdLoginUsuarioLogado = lDataTable.Rows[0]["id_login_usuario_logado"].DBToInt32();
                }
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<ClienteDocumentacaoEntregueInfo> ConsultarClienteDocumentacaoEntregue(ConsultarEntidadeRequest<ClienteDocumentacaoEntregueInfo> pParametro)
        {
            var lRetorno = new ConsultarObjetosResponse<ClienteDocumentacaoEntregueInfo>();
            var lAcessaDados = new AcessaDados() { ConnectionStringName = gNomeConexaoCadastro };

            lRetorno.Resultado = new List<ClienteDocumentacaoEntregueInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_documentacaoentregue_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new ClienteDocumentacaoEntregueInfo() 
                        {
                            DsObservacao = lLinha["ds_observacao"].DBToString(),
                            DtAdesaoDocumento = lLinha["dt_adesaodocumento"].DBToDateTime(),
                            IdCliente = lLinha["id_cliente"].DBToInt32(),
                            IdDocumentacaoEntregue = lLinha["id_cliente_documentacaoentregue"].DBToInt32(),
                            IdLoginUsuarioLogado = lLinha["id_login_usuario_logado"].DBToInt32(),
                        });
            }

            return lRetorno;
        }
    }
}
