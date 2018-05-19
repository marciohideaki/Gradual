using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.Lib.Dados;
using System.Data;
using System.Data.Common;
using Gradual.Spider.Lib;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using Gradual.Spider.Lib.Dados;
using System.Security.Cryptography;

namespace Gradual.Spider.DbLib
{
    public class EsqueciSenhaDbLib
    {
        public const string NomeConexaoCadastro = "GradualCadastro";

        public EsqueciSenhaInfo ReceberEsqueciSenha(EsqueciSenhaInfo pParametros)
        {
            var lRetorno = new EsqueciSenhaInfo();

            try
            {
                
                lRetorno = pParametros;

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = NomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_sel_esqueci_senha_sp"))
                {
                    string lSenha = GerarSenha();

                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.DsEmail);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_Senha", DbType.String, CalculateMD5Hash(lSenha));
                    lAcessaDados.AddInParameter(lDbCommand, "@st_alteracao_funcionario", DbType.String, pParametros.StAlteracaoFuncionario ? "1" : "0");

                    if (!pParametros.StAlteracaoFuncionario)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_CpfCnpj", DbType.String, pParametros.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lDbCommand, "@dt_NascimentoFundacao", DbType.Date, pParametros.DtNascimentoFundacao);
                    }

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    lRetorno.CdSenha = lSenha;
                }
                
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.FalhaLogin);
                //throw new Exception(ERRONEGOCIO.ToString() + ex.Message + ERRONEGOCIO.ToString());
            }
            return lRetorno;

        }

        #region Métodos auxiliares
        
        public static String GerarSenha()
        {
            string carac = "abcdefhijkmnopqrstuvxwyz123456789";
            char[] caracter = carac.ToCharArray();
            embaralhar(ref caracter, 3);

            string novaSenha = string.Empty;

            for (int i = 0; i < 8; i++)
                novaSenha += caracter[i];

            return novaSenha;
        }

        private static void embaralhar(ref char[] array, int qtd)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < qtd; i++)
            {
                for (int j = 0; j <= array.Length; j++)
                {
                    swap(ref array[random.Next(0, array.Length)], ref array[random.Next(0, array.Length)]);
                }
            }
        }

        private static void swap(ref char arg1, ref char arg2)
        {
            char temp = arg1; arg1 = arg2; arg2 = temp;
        }
        public static string CalculateMD5Hash(string input)
        {
            try
            {
                //Primeiro passo, validar o input
                if (string.IsNullOrEmpty(input))
                    input = "gradual123*";

                // Segundo passo, calcular o MD5 hash a partir da string
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

                byte[] hash = md5.ComputeHash(inputBytes);

                // Terceiro passo, converter o array de bytes em uma string haxadecimal
                StringBuilder _HashBuilder = new StringBuilder();

                for (int intX = 0; intX < hash.Length; intX++)
                {
                    _HashBuilder.Append(hash[intX].ToString("X2"));
                }
                return _HashBuilder.ToString();

            }

            catch
            {
                throw new Exception("Ocorreu um erro ao Criptografar a senha.");

            }
        }
        #endregion
    }
}
