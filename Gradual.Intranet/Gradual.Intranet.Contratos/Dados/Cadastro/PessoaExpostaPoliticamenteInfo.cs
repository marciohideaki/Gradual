using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PessoaExpostaPoliticamenteInfo : ICodigoEntidade
    {
        public Nullable<int> IdPEP { get; set; }
        
        public string DsNome { get; set; }

        public string DsIdentificacao { get; set; }

        public string DsDocumento { get; set; }

        public DateTime DtImportacao { get; set; }
        
        public string DsLinha { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Construtores
        
        public PessoaExpostaPoliticamenteInfo() { }
        
        public PessoaExpostaPoliticamenteInfo(string pLinhaDoArquivo)
        {
            /*
                   seq  início   tam. indice   X/N    registro detalhe (tipo 110) – pessoa titular)
                    01     001   003       0     N     Tipo de registro (fixo = 110)
                    02     004   001       3     X     Tipo de pessoa (F = Física ; J = Jurídica)
                    03     005   009       4     N     Documento base do CPF ou CNPJ
                    04     014   004      13     N     Filial (quando CPF será formatado com zeros)
                    05     018   002      17     N     Dígito de controle do CPF ou CNPJ
                    06     020   009      19     N     Nosso número ( Identificação da Pessoa)
                    07     029   015      28     X     Identificação (fixo = ‘titular’)
                    08     044   100      43     X     Nome
                    09     144   010     143     X     Data de nascimento (formato dd/mm/aaaa) ou data de aniversário (formato dd/mm/0001)
                    10     154   001     144     X     Sexo (masculino = ‘M’ , feminino = ‘F’ ou não informado = spaces)
                    11     155   050     145     X     Endereço (logradouro)
                    12     205   005     195     N     Número
                    13     210   045     200     X     Complemento do endereço
                    14     255   030     245     X     Bairro
                    15     285   008     275     N     Cep
                    16     293   050     283     X     Municipio
                    17     343   002     333     x     UF
                    18     345   003     335     N     DDD
                    19     348   012     338     N     Telefone
                    20     360   005     350     N     Ramal
                    21     365   050     355     X     Email
                    22     415   036     405     X     Filler (spaces)
                                            * 
                X = ALFANUMÉRICO N = NUMÉRICO
                */

            this.DsLinha = pLinhaDoArquivo;

            if (pLinhaDoArquivo.Length > 430)
            {
                this.DsIdentificacao = pLinhaDoArquivo.Substring(28, 15);

                this.DsNome = pLinhaDoArquivo.Substring(43, 100);

                this.DsDocumento = pLinhaDoArquivo.Substring(4, 15);
            }
        }

        #endregion
    }
}
