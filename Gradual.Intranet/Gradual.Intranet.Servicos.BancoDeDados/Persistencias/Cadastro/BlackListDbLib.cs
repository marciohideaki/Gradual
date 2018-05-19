using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static SalvarEntidadeResponse<PaisesBlack2ListInfo> SalvarBlackList2(SalvarObjetoRequest<PaisesBlack2ListInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_pais", DbType.String, pParametros.Objeto.CdPais);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_pais_blacklist", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<PaisesBlack2ListInfo> response = new SalvarEntidadeResponse<PaisesBlack2ListInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_pais_blacklist"].Value)
                    };

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }
    }
}
