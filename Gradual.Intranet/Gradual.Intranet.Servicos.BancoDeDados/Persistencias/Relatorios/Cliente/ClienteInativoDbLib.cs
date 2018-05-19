using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using System.Data.Common;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {



        public static ConsultarObjetosResponse<ClienteInativoInfo> ConsultarClienteInativo(ConsultarEntidadeRequest<ClienteInativoInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteInativoInfo> lResposta =
                new ConsultarObjetosResponse<ClienteInativoInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_inativo_lst_sp"))
            {

                lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametros.Objeto.IdAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.AnsiString, pParametros.Objeto.TipoPessoa);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteInativo(lDataTable.Rows[i]));

            }
            return lResposta;
        }

        private static ClienteInativoInfo CriarRegistroClienteInativo(DataRow linha)
        {
            return new ClienteInativoInfo()
                {
                    CdConta = linha["CdConta"].DBToString(),
                    DsConta = linha["DsConta"].DBToString(),
                    DsCpfCnpj = linha["DsCpfCnpj"].DBToString(),
                    DsEmail = linha["DsEmail"].DBToString(),
                    DsNomeCliente = linha["DsNomeCliente"].DBToString(),
                    DtCadastro = linha["DtCadastro"].DBToDateTime(),
                    IdAssessor = linha["IdAssessor"].DBToInt32(),
                    IdCliente = linha["IdCliente"].DBToInt32(),
                    TipoPessoa = linha["TipoPessoa"].DBToString()
                };


         
        }


    }
}
