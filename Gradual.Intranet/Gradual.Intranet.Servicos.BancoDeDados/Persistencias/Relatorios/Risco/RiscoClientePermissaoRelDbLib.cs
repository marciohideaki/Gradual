using System.Collections.Generic;
using System.Data;
using Gradual.Intranet.Contratos.Dados.Relatorios;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public partial class RiscoRelDbLib
    {
        #region | CRUD
        
        public ConsultarObjetosResponse<RiscoClientePermissaoRelInfo> ConsultarRiscoClientePermissao(ConsultarEntidadeRequest<RiscoClientePermissaoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClientePermissaoRelInfo>();
            var lAcessaDados = new ConexaoDbHelper() { ConnectionStringName = ClienteDbLib.gNomeConexaoRisco };

            lRetorno.Resultado = new List<RiscoClientePermissaoRelInfo>();

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_permissao_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, pParametros.Objeto.ConsultaIdBolsa);
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.Objeto.ConsultaIdGrupo);

                if (!string.IsNullOrEmpty(pParametros.Objeto.ConsultaClienteParametro))
                    switch (pParametros.Objeto.ConsultaClienteTipo)
                    {
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CodBovespa:
                            if (0.Equals(pParametros.Objeto.ConsultaClienteParametro.DBToInt32())) return lRetorno;
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.Int32, pParametros.Objeto.ConsultaClienteParametro.DBToInt32());
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CpfCnpj:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", "").Replace("-", ""));
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.NomeCliente:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim());
                            break;
                    }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CriarRegistroRiscoClientePermissaoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoClientePermissaoRelInfo CriarRegistroRiscoClientePermissaoInfo(DataRow pLinha)
        {
            var lRetorno = new RiscoClientePermissaoRelInfo();

            lRetorno.Bolsa = pLinha["ds_bolsa"].DBToString();
            lRetorno.CpfCnpj = pLinha["ds_cpfcnpj"].DBToString();
            lRetorno.DescricaoGrupo = pLinha["ds_grupo"].DBToString();
            lRetorno.DescricaoPermissao = pLinha["ds_permissao"].DBToString();
            lRetorno.NomeCliente = pLinha["ds_nome"].DBToString();
            
            return lRetorno;
        }

        #endregion
    }
}
