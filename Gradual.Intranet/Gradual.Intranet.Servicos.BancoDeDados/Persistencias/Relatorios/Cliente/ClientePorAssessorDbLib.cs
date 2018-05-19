using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Cliente
{
    public class ClientePorAssessorDbLib
    {
        public ConsultarObjetosResponse<ClientePorAssessorInfo> ConsultarListaClientePorAssessor(ConsultarEntidadeRequest<ClientePorAssessorInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ClientePorAssessorInfo>() { Resultado = new List<ClientePorAssessorInfo>() };
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_por_assessor_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.ConsultaCdAssessor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new ClientePorAssessorInfo()
                        {
                            CdAssessor = lLinha["cd_assessor"].DBToInt32(),
                            CdCodigoBovespa = lLinha["cd_bovespa"].DBToInt32(),
                            CdCodigoBMF = lLinha["cd_bmf"].DBToInt32(),
                            CdSexo = lLinha["cd_sexo"].DBToChar(),
                            DsCpfCnpj = lLinha["ds_cpfcnpj"].DBToString(),
                            DsNome = lLinha["ds_nome"].DBToString(),
                            DtNascimentoFundacao = lLinha["dt_nascimentofundacao"].DBToDateTime(),
                            DtPasso1 = lLinha["dt_passo1"].DBToDateTime(),
                            IdCliente = lLinha["id_cliente"].DBToInt32(),
                            StPasso = lLinha["st_passo"].DBToInt32(),
                            TpPessoa = lLinha["tp_pessoa"].DBToChar()
                        });
            }

            return lRetorno;
        }
    }
}
