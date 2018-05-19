using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Collections;
using Gradual.Integracao.Itau.EdiFundos.Lib.Dados;
using System.Globalization;

namespace Gradual.Integracao.Itau.EdiFundos
{
    public class ParserXMLDownloadArquivos : XmlParser
    {
        private List<PosicaoClienteInfo> lstPosicaoCliente = new List<PosicaoClienteInfo>();
        private StringBuilder strTempBuffer;
        private PosicaoClienteInfo posicaoClienteInfo = null;

        /// <summary>
        /// Lista com a posicao em custodia dos clientes (read only)
        /// </summary>
        public List<PosicaoClienteInfo> ListaPosicaoCliente
        {
            get { return lstPosicaoCliente; }
        }

        protected override void Characters(string param1, int param2, int param3)
        {
            strTempBuffer.Append(param1.Substring(param2, param3));
        }

        
        protected override void EndElement(string param1, string param2, string param3)
        {
            if (posicaoClienteInfo != null )
            {
                switch (param2)
                {
                    case "SaldosCotaAberturaD0Bean":
                        {
                            posicaoClienteInfo.IDCotistaItau = posicaoClienteInfo.Agencia + posicaoClienteInfo.Conta + posicaoClienteInfo.DigitoConta;
                            lstPosicaoCliente.Add(posicaoClienteInfo);
                            posicaoClienteInfo = null;
                        }
                        break;
                    case "CDBANC":
                        posicaoClienteInfo.BancoFundo = strTempBuffer.ToString();
                        break;
                    case "CDFDO":
                        posicaoClienteInfo.CodFundo = strTempBuffer.ToString();
                        break;
                    case "CDBANCLI":
                        posicaoClienteInfo.BancoCli = strTempBuffer.ToString();
                        break;
                    case "AGENCIA":
                        posicaoClienteInfo.Agencia = strTempBuffer.ToString();
                        break;
                    case "CDCTA":
                        posicaoClienteInfo.Conta = strTempBuffer.ToString();
                        break;
                    case "DAC10":
                        posicaoClienteInfo.DigitoConta = strTempBuffer.ToString();
                        break;
                    case "SUBCONT":
                        posicaoClienteInfo.SubConta = strTempBuffer.ToString();
                        break;
                    case "DTLANCT": 
                        break;
                    case "LANMOV":
                        break;
                    case "IDTIPREG":
                        posicaoClienteInfo.IDTipoReg = strTempBuffer.ToString();
                        break;
                    case "CDAPL": 
                        break;
                    case "DTAPROCE":
                        posicaoClienteInfo.DataProcessamento = DateTime.ParseExact(strTempBuffer.ToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        break;
                    case "VLCOTAP":
                        break;
                    case "VLORAP":
                        break;
                    case "VLUFIRAP":
                        break;
                    case "QTCOTPAT":
                        posicaoClienteInfo.QtdeCotas = strTempBuffer.ToString().ToDecimal(5);
                        break;
                    case "QTUFIR":
                        break;
                    case "DTACOTA":
                        posicaoClienteInfo.DataReferencia = DateTime.ParseExact(strTempBuffer.ToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        break;
                    case "VLRCOT":
                        posicaoClienteInfo.ValorCota = strTempBuffer.ToString().ToDecimal(7);
                        break;
                    case "VLPATBRU":
                        posicaoClienteInfo.ValorBruto = strTempBuffer.ToString().ToDecimal(2);
                        break;
                    case "VLIRRF":
                        posicaoClienteInfo.ValorIR = strTempBuffer.ToString().ToDecimal(2);
                        break;
                    case "VLRIOF":
                        posicaoClienteInfo.ValorIOF = strTempBuffer.ToString().ToDecimal(2);
                        break;
                    case "VLTXPRF": break;
                    case "VALTXSAI": break;
                    case "VLPATLIQ":
                        posicaoClienteInfo.ValorLiquido = strTempBuffer.ToString().ToDecimal(2);
                        break;
                    case "LUCRPRE":
                        break;
                    case "DEBCRE":
                        break;
                    case "DTVCAR":
                        break;
                    case "DTULTRIB":
                        break;
                    case "VLCOTRIB":
                        break;
                    case "VLIOFUTB":
                        break;
                    case "QTCOTDIA":
                        break;
                    case "DTAULTAN":
                        break;
                    case "VLCOTUAN":
                        break;
                    case "DTPXANI":
                        break;
                    case "SDOPREJ":
                        break;
                    case "INDACAPL":
                        break;
                    case "INDACRSG":
                        break;
                    case "MTBLQ":
                        break;
                    case "VLRNDAPL":
                        break;
                    case "RDIOFPAT":
                        break;
                    case "RDIRFPAT":
                        break;
                    case "VLIRCCAC":
                        break;
                    case "VRNDCCAC":
                        break;
                    default: 
                        break;
                }

                strTempBuffer.Clear();
            }
        }

        protected override void StartElement(string namespace1, string name, string name3, Hashtable attributes, bool hasInlineEnd)
        {
            strTempBuffer = new StringBuilder();

            if (name.Equals("SaldosCotaAberturaD0Bean"))
            {
                posicaoClienteInfo = new PosicaoClienteInfo();
            }
        }

    }
}
