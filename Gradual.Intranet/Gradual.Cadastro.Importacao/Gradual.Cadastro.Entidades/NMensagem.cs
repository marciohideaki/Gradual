using System;
using System.Collections.Generic;
using System.Data;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Text;
using System.ComponentModel;
using Gradual.Generico.Geral;


namespace Gradual.Cadastro.Negocios
{
    /// <summary>
    /// Informa qual a Mensagem
    /// </summary>
    public enum EMensagem 
    {
        CadastroSenhaDesc = 1024,
        CadastroSenhaTit = 1025,
        CadastroTelefoneDesc = 1034,
        CadastroTelefoneTit = 1035,
        CadastroEnderecoDesc = 1044,
        CadastroEnderecoTit = 1045,
        CadastroContaBancariaDesc = 1054,
        CadastroContaBancariaTit = 1055,
        CadastroTotBensImoveisDesc = 1064,
        CadastroTotBensImoveisTit = 1065,
        CadastroSalarioProLaboreDesc = 1074,
        CadastroSalarioProLaboreTit = 1075,
        CadastroOperaContaPropriaDesc = 1084,
        CadastroOperaContaPropriaTit = 1085,
        CadastroRepresentanteLegalDesc = 1094,
        CadastroRepresentanteLegalTit = 1095,
        CadastroPessoaVinculadaDesc = 1104,
        CadastroPessoaVinculadaTit = 1105,
        CadastroPPEDesc = 1114,
        CadastroPPETit = 1115,
        //CadastroIntermediacaoCustoria = 1124,
        CadastroCVM387Desc = 1134,
        CadastroCVM387Tit = 1135,
        CadastroProcuradorDesc = 1144,
        CadastroProcuradorTit = 1145,
        CadastroBensImoveisDesc = 1164,
        CadastroBensImoveisTit = 1165,
        CadastroBensOutrosDesc = 1174,
        CadastroBensOutrosTit = 1175,
        CVMRegrasParametrosDesc = 1184,
        CVMRegrasParametrosTit = 1185,
        CadastroCVM301Desc = 1194,
        CadastroCVM301Tit = 1195,
        CadastroCPFDesc = 2004,
        CadastroCPFTit = 2005,
        CadastroDataNascimentoDesc = 2014,
        CadastroDataNascimentoTit = 2015,
        CadastroAssinaturaEletronicaDesc = 2024,
        CadastroAssinaturaEletronicaTit = 2025,
    }

    /// <summary>
    /// Classe para pegar o Texto das Mensagens
    /// </summary>
    public class NMensagem
    {
        /// <summary>
        /// Seleciona uma determinada Mensagem
        /// </summary>
        /// <param name="enumMensagem">Mensagem a ser Selecionada</param>
        /// <returns>Texto da Mensagem</returns>
        public string Selecionar(EMensagem enumMensagem)
        {
            try
            {
                string retorno = string.Empty;
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendFormat("SELECT mensagem FROM mensagem WHERE id_mensagem = {0}", ((int)enumMensagem).ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
                if (_table.Rows.Count > 0)
                    retorno = Conversao.ToString(_table.Rows[0]["Mensagem"]);
                //else
                //    throw new Exception(CFormatacao.REGISTRONAOENCONTRADO);
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
