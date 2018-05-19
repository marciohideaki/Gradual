using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {



          /* int IdCliente = Convert.ToInt32(pCliente.IdCliente);
            var lRetorno = new EFichaPessoaFisica();
            ESituacaoFinanceiraPatrimonial eSituacaoFinanceiraPatrimonial = new ESituacaoFinanceiraPatrimonial();
            EBanco eBanco = new EBanco();
            EControladora eControladora = new EControladora();
            EDiretor eDiretor = new EDiretor();
            EEmitente eEmitente = new EEmitente();
            EClienteEndereco eEndereco = new EClienteEndereco();
            EProcuradorRepresentante eProcurador = new EProcuradorRepresentante();
            ETelefone eTelefone = new ETelefone();

            eSituacaoFinanceiraPatrimonial.IdCliente = idCliente;
            eBanco.IdCliente = idCliente;
            eControladora.IdCliente = idCliente;
            eDiretor.IdCliente = idCliente;
            eEmitente.IdCliente = idCliente;
            eEndereco.IdCliente = idCliente;
            eProcurador.IdCliente = idCliente;

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexao;

            lRetorno.ECliente                   = new DCliente().ClienteCompletoSelecionar(pCliente);
            lRetorno.LstSituacaoFinanceira      = new DSituacaoFinanceiraPatrimonial().SituacaoFinanceiraListar(eSituacaoFinanceiraPatrimonial);
            lRetorno.LstEndereco                = new DClienteEndereco().ListarEndereco(eEndereco);
            lRetorno.LstInfoBancaria            = new DBanco().Listar(eBanco);
            lRetorno.LstProcuradorRepresentante = new DProcuradorRepresentante().ListarProcuradorRepresentante(eProcurador);*/

        /*
       public static ConsultarObjetosResponse<ClienteFichaPessoaFisicaInfo> ConsultarClienteEndereco(ConsultarEntidadeRequest<ClienteFichaPessoaFisicaInfo> pParametros)
       {

        ConsultarObjetosResponse<ClienteFichaPessoaFisicaInfo> resposta =
        new ConsultarObjetosResponse<ClienteFichaPessoaFisicaInfo>();

        int IdCliente = Convert.ToInt32(pParametros.Objeto.ECliente.IdCliente);   




        ClienteSituacaoFinanceiraPatrimonialInfo eSituacaoFinanceiraPatrimonial = new ClienteSituacaoFinanceiraPatrimonialInfo();
        ClienteBancoInfo eBanco = new ClienteBancoInfo();
        ClienteControladoraInfo eControladora = new ClienteControladoraInfo();
        ClienteDiretorInfo eDiretor = new ClienteDiretorInfo();
        ClienteEmitenteInfo eEmitente = new ClienteEmitenteInfo();
        ClienteEnderecoInfo eEndereco = new ClienteEnderecoInfo();
        ClienteProcuradorRepresentanteInfo eProcurador = new ClienteProcuradorRepresentanteInfo();
        ClienteTelefoneInfo eTelefone = new ClienteTelefoneInfo();

        ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

        lAcessaDados.ConnectionStringName = gNomeConexao;





        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_lst_sp"))
        {
        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

        if (null != lDataTable && lDataTable.Rows.Count > 0)
        {
        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
        {
        DataRow linha = lDataTable.NewRow();

        linha["ds_bairro"] = (lDataTable.Rows[i]["ds_bairro"]).DBToString();
        linha["ds_cidade"] = (lDataTable.Rows[i]["ds_cidade"]).DBToString();
        linha["ds_complemento"] = (lDataTable.Rows[i]["ds_complemento"]).DBToString();
        linha["ds_logradouro"] = (lDataTable.Rows[i]["ds_logradouro"]).DBToString();
        linha["ds_numero"] = (lDataTable.Rows[i]["ds_numero"]).DBToInt32();
        linha["cd_pais"] = (lDataTable.Rows[i]["cd_pais"]).DBToInt32();
        linha["cd_uf"] = (lDataTable.Rows[i]["cd_uf"]).DBToString();
        linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
        linha["id_endereco"] = (lDataTable.Rows[i]["id_endereco"]).DBToInt32();
        linha["id_tipo_endereco"] = (lDataTable.Rows[i]["id_tipo_endereco"]).DBToInt32();
        linha["cd_cep"] = (lDataTable.Rows[i]["cd_cep"]).DBToInt32();
        linha["cd_cep_ext"] = (lDataTable.Rows[i]["cd_cep_ext"]).DBToInt32();
        linha["st_principal"] = bool.Parse(lDataTable.Rows[i]["st_principal"].ToString());

        resposta.Resultado.Add(CriarRegistroClienteEnderecoInfo(linha));
        }

        }
        }

        return resposta;
        }


        private static ClienteEnderecoInfo CriarRegistroClienteEnderecoInfo(DataRow linha)
        {
            ClienteEnderecoInfo lClienteEnderecoInfo = new ClienteEnderecoInfo();


            lClienteEnderecoInfo.DsBairro = linha["ds_bairro"].DBToString();
            lClienteEnderecoInfo.DsCidade = linha["ds_cidade"].DBToString();
            lClienteEnderecoInfo.DsComplemento = linha["ds_complemento"].DBToString();
            lClienteEnderecoInfo.DsLogradouro = linha["ds_logradouro"].DBToString();
            lClienteEnderecoInfo.DsNumero = linha["ds_numero"].DBToString();
            lClienteEnderecoInfo.CdPais = linha["cd_pais"].DBToString();
            lClienteEnderecoInfo.CdUf = linha["cd_uf"].DBToString();
            lClienteEnderecoInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteEnderecoInfo.IdEndereco = linha["id_endereco"].DBToInt32();
            lClienteEnderecoInfo.IdTipoEndereco = linha["id_tipo_endereco"].DBToInt32();
            lClienteEnderecoInfo.NrCep = linha["cd_cep"].DBToInt32();
            lClienteEnderecoInfo.NrCepExt = linha["cd_cep_ext"].DBToInt32();
            lClienteEnderecoInfo.StPrincipal = bool.Parse(linha["st_principal"].ToString());

            return lClienteEnderecoInfo;

        } 
        */
    }
}
