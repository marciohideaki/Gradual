using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Gradual.Integracao.Itau.EdiFundos.Lib.Registros;
using log4net;
using Gradual.Integracao.Itau.EdiFundos.Lib.Dados;
using System.Collections;
using System.Globalization;

namespace Gradual.Integracao.Itau.EdiFundos
{
    public class ParserArquivosFundos
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Trata e carrega arquivos gerados pelos sistemas de Fundos e Carteiras e Passivo de Fundos
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        public bool Parse(string arquivo)
        {
            bool bRet = false;
            string filename = Path.GetFileName(arquivo).ToUpperInvariant();

            logger.Info("Processando arquivo [" + filename + "]");

            switch (filename)
            {
                case "ARQAPD0.TXT":
                    bRet = _processa_ItauFJ_29_AplicacoesEmAbertoCotas(arquivo);
                    break;
                case "ARQATV.TXT":
                case "ARQCOT.TXT":
                case "ARQCTR.TXT":
                case "ARQOSM.TXT":
                case "ARQOTI.TXT":
                case "ARQPPC.TXT":
                case "ARQSDAD0.TXT":
                case "ARQSDOME.TXT":
                    bRet = _processa_ItauFJ_62_SaldosCotasAberturaFechamentoMensal(arquivo);
                    break;
                case "CADASI.TXT":
                case "CADFUN.TXT":
                    bRet = _processa_ItauFJ_04_CadastroFundos(arquivo);
                    break;
                case "CADCOTCP.TXT":
                    bRet = _processa_ItauFJ_60_CadastroCompletoCotistas(arquivo);
                    break;
                case "CADGES.TXT":
                case "EMSER.TXT":
                    bRet = _processa_ItauFJ_05_OperacoesEmSER(arquivo);
                    break;
                case "EMSERSIM.TXT":
                case "RECTRIB.TXT":
                case "RECTRIBD.TXT":
                    return true;
                case "IMPORTACADCOT.CSV":
                    bRet = _processa_ItauFJ_ImportacaoCadastroCotistaTioPonso(arquivo);
                    break;
                case "RELACAOCOTISTASFUNDO.CSV":
                    bRet = _processa_ItauFJ_ImportacaoCadastroCotistaTioPonso1(arquivo);
                    break;
                case "IMPORTACADCOTAUGUSTO.CSV":
                    bRet = _processa_ItauFJ_ImportacaoCadastroCotistaTioPonso2(arquivo);
                    break;
                case "DOWNLOADARQUIVOS.XML":
                    bRet = _processa_XML_DownloadArquivos(arquivo);
                    break;
                default:
                    return _tratarArquivoFC(arquivo);
            }

            return bRet;
        }


        /// <summary>
        /// Trata arquivos gerados pelo sistema de fundos e carteiras
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _tratarArquivoFC(string arquivo)
        {
            bool bRet = false;
            string filename = Path.GetFileName(arquivo).ToUpperInvariant();

            if (filename.IndexOf("COTASPATRIMONIO") >= 0)
            {
                bRet = _processa_ItauFC_CotasPatrimonio(arquivo);
            }

            if (filename.IndexOf("CARTEIRASDOGESTOR") >= 0)
            {
                bRet = _processa_ItauFC_CarteirasGestor(arquivo);
            }

            return bRet;
        }


        #region DownloadArquivos.XML
        /// <summary>
        /// Nota: por enquanto, essa rotina trata somente o XML com saldo de cotistas
        /// o webservice do Itau desce o arquivo sempre com o mesmo nome para todos
        /// os servicos oferecidos.
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_XML_DownloadArquivos(string arquivo)
        {
            Hashtable hstPosicaoCot = new Hashtable();
            Hashtable hstFundos = new Hashtable();
            Hashtable hstCadCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCadCotistas.ContainsKey(info.IdCotistaItau) == false)
                        hstCadCotistas.Add(info.IdCotistaItau, info);
                }

                List<PosicaoClienteInfo> posicaocotistas = db.ObterPosicaoCotistas();
                foreach (PosicaoClienteInfo info in posicaocotistas)
                {
                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;
                    if (hstPosicaoCot.ContainsKey(key) == false)
                        hstPosicaoCot.Add(key, info);
                }

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    string key = info.Gestor + info.CodFundo;
                    if (hstFundos.ContainsKey(key) == false)
                        hstFundos.Add(key, info);
                }

                StreamReader str = new StreamReader(arquivo);

                string xml = str.ReadToEnd();

                str.Close();

                xml = xml.Replace("/ArrayList id=\"1\"","/ArrayList");

                ParserXMLDownloadArquivos pser = new ParserXMLDownloadArquivos();

                pser.Parse(xml);


                foreach (PosicaoClienteInfo info in pser.ListaPosicaoCliente)
                {
                    // considerar apenas os registros de posicao consolidada
                    if (info.IDTipoReg.Equals("21") == false)
                        continue;

                    string idCotistaItau = info.Agencia + info.Conta + info.DigitoConta;

                    if (hstCadCotistas.ContainsKey(idCotistaItau))
                    {
                        CotistaInfo cotista = hstCadCotistas[idCotistaItau] as CotistaInfo;
                        info.CPFCNPJ = cotista.CPFCNPJ;
                    }

                    string keyFundos = info.BancoFundo + info.CodFundo;
                    if (hstFundos.ContainsKey(keyFundos))
                    {
                        CadastroFundoInfo fundoInfo = hstFundos[keyFundos] as CadastroFundoInfo;
                        info.IDFundo = fundoInfo.IDFundo;
                    }

                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;

                    if (hstPosicaoCot.ContainsKey(key))
                    {
                        PosicaoClienteInfo posicaoInfo = hstPosicaoCot[key] as PosicaoClienteInfo;
                        posicaoInfo.DataProcessamento = info.DataProcessamento;
                        posicaoInfo.DataReferencia = info.DataReferencia;
                        posicaoInfo.QtdeCotas = info.QtdeCotas;
                        posicaoInfo.ValorBruto = info.ValorBruto;
                        posicaoInfo.ValorCota = info.ValorCota;
                        posicaoInfo.ValorIR = info.ValorIR;
                        posicaoInfo.ValorIOF = info.ValorIOF;
                        posicaoInfo.ValorLiquido = info.ValorLiquido;

                        db.AtualizarPosicaoCotista(posicaoInfo);
                    }
                    else
                    {
                        db.InserirPosicaoCotista(info);
                    }
                }

                return true;
            }
            catch(Exception ex )
            {
                logger.Error("_processa_XML_DownloadArquivos(): " + ex.Message, ex);
            }
            finally
            {
                //if (br != null)
                //    br.Close();

                //if (fs != null)
                //    fs.Close();
            }

            return false;
        }
        #endregion //DownloadArquivos.XML


        #region ItauFJ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_03_Cotacoes(string arquivo)
        {
            FileStream fs = null;
            BinaryReader br = null;
            Hashtable hstFundos = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                logger.Info("Inicio processamento arquivo 03_Cotacoes");

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    if ( hstFundos.ContainsKey(info.CodFundo) == false )
                        hstFundos.Add(info.CodFundo, info);
                }

                fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                long total = fs.Length;
                long lidos = 0;

                long tamstrut = Marshal.SizeOf(typeof(ItauFJ_03_Cotacoes));
                while (lidos < total)
                {
                    ItauFJ_03_Cotacoes strut = FromBinaryReaderBlock<ItauFJ_03_Cotacoes>(br);

                    // Pular CR+LF
                    byte[] eol = br.ReadBytes(2);
                    lidos += tamstrut + 2;

                    // Converter e processar
                    string idFundo = strut.CDFDO.ByteArrayToString().Trim();
                    DateTime dtProcessamento = DateTime.ParseExact(strut.DTAPROCE.ByteArrayToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    Decimal valorCota = strut.VLCOTAP.ByteArrayToDecimal(7);

                    // TODO: atualizacao se encaixa aqui ?
                    if (hstFundos.ContainsKey(idFundo))
                    {
                        CadastroFundoInfo info = hstFundos[idFundo] as CadastroFundoInfo;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_03_Cotacoes(): " + ex.Message, ex);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fs != null)
                    fs.Close();
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_04_CadastroFundos(string arquivo)
        {
            FileStream fs = null;
            BinaryReader br = null;
            Hashtable hstFundos = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                logger.Info("Inicio processamento arquivo 04_CadastroFundos");

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    if (hstFundos.ContainsKey(info.CNPJ) == false)
                        hstFundos.Add(info.CNPJ, info);
                }

                fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                long total = fs.Length;
                long lidos = 0;

                long tamstrut = Marshal.SizeOf(typeof(ItauFJ_04_CadastroFundos));
                while (lidos < total)
                {
                    ItauFJ_04_CadastroFundos strut = FromBinaryReaderBlock<ItauFJ_04_CadastroFundos>(br);

                    // Pular CR+LF
                    byte[] eol = br.ReadBytes(2);
                    lidos += tamstrut + 2;

                    // Converter e processar
                    CadastroFundoInfo info = new CadastroFundoInfo();

                    info.Agencia = strut.AGENCIA.ByteArrayToString();
                    info.CNPJ = strut.IDCGCCPF.ByteArrayToString();
                    info.CodFundo = strut.CDFDO.ByteArrayToString();
                    info.Conta = strut.CDCTA.ByteArrayToString();
                    info.IDDistribuidor = 1;
                    info.DataAtualizacao = Utils.ByteArrayToDateTime(strut.DTULTAAS, strut.HORALTEN, "yyyyMMddHHmmss");
                    info.DigitoConta = strut.DAC10.ByteArrayToString();
                    info.Gestor = strut.CDBANC.ByteArrayToString();
                    info.NomeFantasia = strut.NMFANFMT.ByteArrayToString();
                    info.RazaoSocial = strut.NMFMT.ByteArrayToString();

                    // TODO: atualizacao se encaixa aqui ?
                    if (hstFundos.ContainsKey(info.CNPJ) == false)
                    {
                        db.InserirCadastroFundo(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_04_CadastroFundos(): " + ex.Message, ex);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fs != null)
                    fs.Close();
            }

            return false;
        }

        /// <summary>
        /// _processa_ItauFJ_05_OperacoesEmSER 
        /// Efetua a carga do arquivos de OPERAÇÕES EM SER DO PASSIVO 
        /// Objetivo: Mostrar a relaçao das aplicações em aberto dos fundos por Cotista								
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_05_OperacoesEmSER(string arquivo)
        {
            FileStream fs = null;
            BinaryReader br = null;
            Hashtable hstCotistas = new Hashtable();
            Hashtable hstFundos = new Hashtable();
            Hashtable hstCadCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                logger.Info("Inicio processamento arquivo 05_OperacoesEmSER");

                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCadCotistas.ContainsKey(info.IdCotistaItau) == false)
                        hstCadCotistas.Add(info.IdCotistaItau, info);
                }

                List<PosicaoClienteInfo> posicaoCot = db.ObterPosicaoCotistas();
                foreach (PosicaoClienteInfo info in posicaoCot)
                {
                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;
                    if (hstCotistas.ContainsKey(key) == false)
                        hstCotistas.Add(key, info);
                }

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    string key = info.Gestor + info.CodFundo;
                    if (hstFundos.ContainsKey(key) == false)
                        hstFundos.Add(key, info);
                }


                fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                long total = fs.Length;
                long lidos = 0;

                long tamstrut = Marshal.SizeOf(typeof(ItauFJ_05_OperacoesEmSER));
                while (lidos < total)
                {
                    ItauFJ_05_OperacoesEmSER strut = FromBinaryReaderBlock<ItauFJ_05_OperacoesEmSER>(br);

                    // Pular CR+LF
                    byte[] eol = br.ReadBytes(2);
                    lidos += tamstrut + 2;

                    // Converter e processar
                    PosicaoClienteInfo info = new PosicaoClienteInfo();

                    info.Agencia = strut.AGENCIA.ByteArrayToString();
                    info.BancoCli = strut.CDBANCLI.ByteArrayToString();
                    info.Conta = strut.CDCTA.ByteArrayToString();
                    info.DataProcessamento = DateTime.ParseExact(strut.DTAPROCE.ByteArrayToString() + "000000", "yyyyMMddHHmmss",CultureInfo.InvariantCulture);
                    info.DataReferencia = DateTime.ParseExact(strut.DTACOTA.ByteArrayToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    info.DigitoConta = strut.DAC10.ByteArrayToString();

                    string idCotistaItau = info.Agencia + info.Conta + info.DigitoConta;

                    if (hstCadCotistas.ContainsKey(idCotistaItau))
                    {
                        CotistaInfo cotista = hstCadCotistas[idCotistaItau] as CotistaInfo;
                        info.CPFCNPJ = cotista.CPFCNPJ;
                    }


                    string keyFundos = strut.CDBANC.ByteArrayToString() + strut.CDFDO.ByteArrayToString();
                    if ( hstFundos.ContainsKey(keyFundos) )
                    {
                        CadastroFundoInfo fundoInfo = hstFundos[keyFundos] as CadastroFundoInfo;
                        info.IDFundo = fundoInfo.IDFundo;
                    }
                    info.QtdeCotas = strut.QTCOTPAT.ByteArrayToDecimal(5);
                    info.ValorBruto = strut.VLPATBRU.ByteArrayToDecimal(2);
                    info.ValorCota = strut.VLRCOT.ByteArrayToDecimal(7);
                    info.ValorIR = strut.VLIRRF.ByteArrayToDecimal(2);
                    info.ValorIOF = strut.VLPERADI.ByteArrayToDecimal(2);
                    info.ValorLiquido = strut.VLPATLIQ.ByteArrayToDecimal(2);

                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;

                    if ( hstCotistas.ContainsKey(key) )
                    {
                        PosicaoClienteInfo posicaoInfo = hstCotistas[key] as PosicaoClienteInfo;
                        posicaoInfo.DataProcessamento = info.DataProcessamento;
                        posicaoInfo.DataReferencia = info.DataReferencia;
                        posicaoInfo.QtdeCotas = info.QtdeCotas;
                        posicaoInfo.ValorBruto = info.ValorCota;
                        posicaoInfo.ValorCota = info.ValorCota;
                        posicaoInfo.ValorIR = info.ValorIR;
                        posicaoInfo.ValorIOF = info.ValorIOF;
                        posicaoInfo.ValorLiquido = info.ValorLiquido;

                        db.AtualizarPosicaoCotista(posicaoInfo);
                    }
                    else
                    {
                        db.InserirPosicaoCotista(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_05_OperacoesEmSER(): " + ex.Message, ex);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fs != null)
                    fs.Close();
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_29_AplicacoesEmAbertoCotas(string arquivo)
        {
            FileStream fs = null;
            BinaryReader br = null;
            Hashtable hstPosicaoCot = new Hashtable();
            Hashtable hstFundos = new Hashtable();
            Hashtable hstCadCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                logger.Info("Inicio processamento arquivo 29 - AplicacoesEmAbertoCotas");

                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCadCotistas.ContainsKey(info.IdCotistaItau) == false)
                        hstCadCotistas.Add(info.IdCotistaItau, info);
                }

                List<PosicaoClienteInfo> posicaocotistas = db.ObterPosicaoCotistas();
                foreach (PosicaoClienteInfo info in posicaocotistas)
                {
                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;
                    if (hstPosicaoCot.ContainsKey(key) == false)
                        hstPosicaoCot.Add(key, info);
                }

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    string key = info.Gestor + info.CodFundo;
                    if (hstFundos.ContainsKey(key) == false)
                        hstFundos.Add(key, info);
                }

                fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                long total = fs.Length;
                long lidos = 0;

                long tamstrut = Marshal.SizeOf(typeof(ItauFJ_29_AplicacoesEmAbertoCotas));
                while (lidos < total)
                {
                    ItauFJ_29_AplicacoesEmAbertoCotas strut = FromBinaryReaderBlock<ItauFJ_29_AplicacoesEmAbertoCotas>(br);
                    byte[] eol = br.ReadBytes(2);
                    lidos += tamstrut + 2;

                    // Converter e processar
                    PosicaoClienteInfo info = new PosicaoClienteInfo();

                    info.Agencia = strut.AGENCIA.ByteArrayToString();
                    info.BancoCli = strut.CDBANCLI.ByteArrayToString();
                    info.Conta = strut.CDCTA.ByteArrayToString();
                    info.DataProcessamento = DateTime.ParseExact(strut.DTAPROCE.ByteArrayToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    info.DataReferencia = DateTime.ParseExact(strut.DTACOTA.ByteArrayToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    info.DigitoConta = strut.DAC10.ByteArrayToString();

                    string idCotistaItau = info.Agencia + info.Conta + info.DigitoConta;

                    if (hstCadCotistas.ContainsKey(idCotistaItau))
                    {
                        CotistaInfo cotista = hstCadCotistas[idCotistaItau] as CotistaInfo;
                        info.CPFCNPJ = cotista.CPFCNPJ;
                    }

                    string keyFundos = strut.CDBANC.ByteArrayToString() + strut.CDFDO.ByteArrayToString();
                    if (hstFundos.ContainsKey(keyFundos))
                    {
                        CadastroFundoInfo fundoInfo = hstFundos[keyFundos] as CadastroFundoInfo;
                        info.IDFundo = fundoInfo.IDFundo;
                    }
                    info.QtdeCotas = strut.QTCOTPAT.ByteArrayToDecimal(5);
                    info.ValorBruto = strut.VLPATBRU.ByteArrayToDecimal(2);
                    info.ValorCota = strut.VLRCOT.ByteArrayToDecimal(7);
                    info.ValorIR = strut.VLIRRF.ByteArrayToDecimal(2);
                    info.ValorIOF = strut.VLRIOF.ByteArrayToDecimal(2);
                    info.ValorLiquido = strut.VLPATLIQ.ByteArrayToDecimal(2);

                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;

                    if (hstPosicaoCot.ContainsKey(key))
                    {
                        PosicaoClienteInfo posicaoInfo = hstPosicaoCot[key] as PosicaoClienteInfo;
                        posicaoInfo.DataProcessamento = info.DataProcessamento;
                        posicaoInfo.DataReferencia = info.DataReferencia;
                        posicaoInfo.QtdeCotas = info.QtdeCotas;
                        posicaoInfo.ValorBruto = info.ValorBruto;
                        posicaoInfo.ValorCota = info.ValorCota;
                        posicaoInfo.ValorIR = info.ValorIR;
                        posicaoInfo.ValorIOF = info.ValorIOF;
                        posicaoInfo.ValorLiquido = info.ValorLiquido;

                        hstPosicaoCot[key] = posicaoInfo;
                        db.AtualizarPosicaoCotista(posicaoInfo);
                    }
                    else
                    {
                        hstPosicaoCot.Add(key, info);
                        db.InserirPosicaoCotista(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_29_AplicacoesEmAbertoCotas(): " + ex.Message, ex);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fs != null)
                    fs.Close();
            }

            return false;
        }

        private bool _processa_ItauFJ_60_CadastroCompletoCotistas(string arquivo)
        {
            FileStream fs = null;
            BinaryReader br = null;
            bool bRet = false;
            CultureInfo ciBR = CultureInfo.GetCultureInfo("pt-BR");
            Hashtable hstCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCotistas.ContainsKey(info.IdCotistaItau) == false)
                        hstCotistas.Add(info.IdCotistaItau, info);
                }

                fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                long total = fs.Length;
                long lidos = 0;

                long tamstrut = Marshal.SizeOf(typeof(ItauFJ_60_CadastroCotistasCompletoMovimento));

                while (lidos < total)
                {
                    ItauFJ_60_CadastroCotistasCompletoMovimento strut = FromBinaryReaderBlock<ItauFJ_60_CadastroCotistasCompletoMovimento>(br);
                    byte[] eol = br.ReadBytes(2);

                    lidos += tamstrut + 2;
                    try
                    {

                        CotistaInfo cotista = new CotistaInfo();

                        cotista.BancoCli = strut.CDBANCLI.ByteArrayToString();
                        cotista.Agencia = strut.AGENCIA.ByteArrayToString();
                        cotista.Conta = strut.CDCTA.ByteArrayToString();
                        cotista.DigitoConta = strut.DAC10.ByteArrayToString();
                        cotista.Nome = strut.NMCLI.ByteArrayToString();
                        cotista.SubConta = "201";
                        cotista.CPFCNPJ = strut.IDCGCCPF.ByteArrayToString().Trim();
                        cotista.IdCotistaItau = cotista.Agencia + cotista.Conta + cotista.DigitoConta;

                        cotista.StatusAtivo = "S";
                        cotista.DataImportacao = DateTime.Now;

                        // TODO: puxar o registro do fundo/casar via cnpj
                        if (hstCotistas.ContainsKey(cotista.IdCotistaItau) == false)
                        {
                            logger.Info("Inserindo CPF/CNPJ [" + cotista.CPFCNPJ + "] [" + cotista.Conta + "] [" + cotista.Nome + "]");
                            db.InserirCotista(cotista);
                            hstCotistas.Add(cotista.IdCotistaItau, cotista);
                        }
                        else
                        {
                            logger.Info("Descartando CPF/CNPJ [" + cotista.CPFCNPJ + "] [" + cotista.Conta + "] [" + cotista.Nome + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        //logger.Error("Ignorando CNPJ/CPF [" + linha + "]");
                        logger.Error("Erro: " + ex.Message, ex);
                    }
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_60_CadastroCompletoCotistas(): " + ex.Message, ex);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fs != null)
                    fs.Close();
            }

            return bRet;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_62_SaldosCotasAberturaFechamentoMensal(string arquivo)
        {
            FileStream fs = null;
            BinaryReader br = null;
            Hashtable hstPosicaoCot = new Hashtable();
            Hashtable hstFundos = new Hashtable();
            Hashtable hstCadCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCadCotistas.ContainsKey(info.IdCotistaItau) == false)
                        hstCadCotistas.Add(info.IdCotistaItau, info);
                }

                // ATP 2013-05-03
                // Limpar a tabela de posicoes para remover cotistas com resgate total
                db.LimparTabelaPosicaoCotista();

                List<PosicaoClienteInfo> posicaocotistas = db.ObterPosicaoCotistas();
                foreach (PosicaoClienteInfo info in posicaocotistas)
                {
                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;
                    if (hstPosicaoCot.ContainsKey(key) == false)
                        hstPosicaoCot.Add(key, info);
                }

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    string key = info.Gestor + info.CodFundo;
                    if (hstFundos.ContainsKey(key) == false)
                        hstFundos.Add(key, info);
                }

                fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                long total = fs.Length;
                long lidos = 0;

                long tamstrut = Marshal.SizeOf(typeof(ItauFJ_62_SaldosCotasAberturaFechamentoMensal));
                while (lidos < total)
                {
                    ItauFJ_62_SaldosCotasAberturaFechamentoMensal strut = FromBinaryReaderBlock<ItauFJ_62_SaldosCotasAberturaFechamentoMensal>(br);
                    byte[] eol = br.ReadBytes(2);
                    lidos += tamstrut + 2;

                    // considerar apenas os registros de posicao consolidada
                    if (strut.IDTIPREG.ByteArrayToString().Equals("21") == false)
                        continue;

                    // Converter e processar
                    PosicaoClienteInfo info = new PosicaoClienteInfo();

                    info.Agencia = strut.AGENCIA.ByteArrayToString();
                    info.BancoCli = strut.CDBANCLI.ByteArrayToString();
                    info.Conta = strut.CDCTA.ByteArrayToString();
                    info.DataProcessamento = DateTime.ParseExact(strut.DTAPROCE.ByteArrayToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    info.DataReferencia = DateTime.ParseExact(strut.DTACOTA.ByteArrayToString() + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    info.DigitoConta = strut.DAC10.ByteArrayToString();

                    string idCotistaItau = info.Agencia + info.Conta + info.DigitoConta;

                    if ( hstCadCotistas.ContainsKey(idCotistaItau) )
                    {
                        CotistaInfo cotista = hstCadCotistas[idCotistaItau] as CotistaInfo;
                        info.CPFCNPJ = cotista.CPFCNPJ;
                    }

                    string keyFundos = strut.CDBANC.ByteArrayToString() + strut.CDFDO.ByteArrayToString();
                    if (hstFundos.ContainsKey(keyFundos))
                    {
                        CadastroFundoInfo fundoInfo = hstFundos[keyFundos] as CadastroFundoInfo;
                        info.IDFundo = fundoInfo.IDFundo;
                    }
                    info.QtdeCotas = strut.QTCOTPAT.ByteArrayToDecimal(5);
                    info.ValorBruto = strut.VLPATBRU.ByteArrayToDecimal(2);
                    info.ValorCota = strut.VLRCOT.ByteArrayToDecimal(7);
                    info.ValorIR = strut.VLIRRF.ByteArrayToDecimal(2);
                    info.ValorIOF = strut.VLRIOF.ByteArrayToDecimal(2);
                    info.ValorLiquido = strut.VLPATLIQ.ByteArrayToDecimal(2);

                    string key = info.IDFundo + info.BancoCli + info.Agencia + info.Conta + info.DigitoConta;

                    if (hstPosicaoCot.ContainsKey(key))
                    {
                        PosicaoClienteInfo posicaoInfo = hstPosicaoCot[key] as PosicaoClienteInfo;
                        posicaoInfo.DataProcessamento = info.DataProcessamento;
                        posicaoInfo.DataReferencia = info.DataReferencia;
                        posicaoInfo.QtdeCotas = info.QtdeCotas;
                        posicaoInfo.ValorBruto = info.ValorBruto;
                        posicaoInfo.ValorCota = info.ValorCota;
                        posicaoInfo.ValorIR = info.ValorIR;
                        posicaoInfo.ValorIOF = info.ValorIOF;
                        posicaoInfo.ValorLiquido = info.ValorLiquido;

                        db.AtualizarPosicaoCotista(posicaoInfo);
                    }
                    else
                    {
                        db.InserirPosicaoCotista(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_62_SaldosCotasAberturaFechamentoMensal(): " + ex.Message, ex);
            }
            finally
            {
                if ( br != null )
                    br.Close();

                if (fs != null)
                    fs.Close();
            }

            return false;
        }


        /// <summary>
        /// Quebra galho do TioPonso para importacao do cadastro de cotistas
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_ImportacaoCadastroCotistaTioPonso(string arquivo)
        {
            bool bRet = false;
            CultureInfo ciBR = CultureInfo.GetCultureInfo("pt-BR");
            StreamReader str = null;
            Hashtable hstCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                str = new StreamReader(arquivo);
                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCotistas.ContainsKey(info.CPFCNPJ) == false)
                        hstCotistas.Add(info.CPFCNPJ, info);
                }

                while (str.EndOfStream == false)
                {
                    string linha = str.ReadLine();

                    if (linha.Length == 0) continue;

                    try
                    {

                        string inicio = linha.Substring(0, 3);

                        if (inicio.Equals("Id;") || inicio.Equals("99;"))
                            continue;

                        CotistaInfo cotista = new CotistaInfo();

                        // Layout:
                        // Id;Quote Holder;Fund;Subconta_EN;Operador_EN;CPF/CNPJ
                        // Onde:
                        // Id: id do cotista no Itau (ag+cc+dig)
                        // Quote Holder: nome do cotista
                        // Fund: fundo
                        // Subconta_EN: Subconta
                        // Operador_EN: operador Gradual associado ao cadastro
                        // CPF/CNPJ: cpf/cnpj do cotista

                        string[] arrFields = linha.Split(';');

                        cotista.IdCotistaItau = arrFields[0].Trim();
                        cotista.Nome = arrFields[1].Trim();
                        cotista.SubConta = arrFields[3].Trim();
                        cotista.CPFCNPJ = arrFields[5].Trim();

                        cotista.BancoCli = "990686";
                        cotista.Agencia = cotista.IdCotistaItau.Substring(0, 4);
                        cotista.Conta = cotista.IdCotistaItau.Substring(4, 9);
                        cotista.DigitoConta = cotista.IdCotistaItau.Substring(13, 1);
                        cotista.StatusAtivo = "S";
                        cotista.DataImportacao = DateTime.Now;

                        // TODO: puxar o registro do fundo/casar via cnpj
                        if (hstCotistas.ContainsKey(cotista.CPFCNPJ) == false)
                        {
                            logger.Info("Inserindo CPF/CNPJ [" + cotista.CPFCNPJ + "]");
                            db.InserirCotista(cotista);
                            hstCotistas.Add(cotista.CPFCNPJ, cotista);
                        }
                        else
                        {
                            logger.Info("Descartando CPF/CNPJ [" + cotista.CPFCNPJ + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ignorando linha [" + linha + "]");
                        logger.Error("Erro: " + ex.Message, ex);
                    }
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_ImportacaoCadastroCotistaTioPonso(): " + ex.Message, ex);
            }
            finally
            {
                if (str != null)
                    str.Close();
            }

            return bRet;

        }

        /// <summary>
        /// Quebra galho do TioPonso para importacao do cadastro de cotistas
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_ImportacaoCadastroCotistaTioPonso1(string arquivo)
        {
            bool bRet = false;
            CultureInfo ciBR = CultureInfo.GetCultureInfo("pt-BR");
            StreamReader str = null;
            Hashtable hstCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                str = new StreamReader(arquivo);
                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCotistas.ContainsKey(info.CPFCNPJ) == false)
                        hstCotistas.Add(info.CPFCNPJ, info);
                }

                while (str.EndOfStream == false)
                {
                    string linha = str.ReadLine();

                    if (linha.Length == 0) continue;

                    try
                    {

                        string inicio = linha.Substring(0, 3);

                        if (inicio.Equals("Reg;") )
                            continue;

                        CotistaInfo cotista = new CotistaInfo();

                        // Layout:
                        // Id;Quote Holder;Fund;Subconta_EN;Operador_EN;CPF/CNPJ
                        // Onde:
                        // Id: id do cotista no Itau (ag+cc+dig)
                        // Quote Holder: nome do cotista
                        // Fund: fundo
                        // Subconta_EN: Subconta
                        // Operador_EN: operador Gradual associado ao cadastro
                        // CPF/CNPJ: cpf/cnpj do cotista

                        string[] arrFields = linha.Split(';');

                        string regnum = arrFields[0].Trim();

                        string[] saco = arrFields[1].Split('-');
                        cotista.IdCotistaItau = saco[0].Trim();
                        cotista.Nome = saco[1].Trim();
                        cotista.CPFCNPJ = arrFields[2].Replace("-",String.Empty).Replace("/",String.Empty).PadLeft(15,'0');

                        cotista.SubConta = arrFields[4].Trim();

                        cotista.BancoCli = "990686";
                        cotista.Agencia = cotista.IdCotistaItau.Substring(0, 4);
                        cotista.Conta = cotista.IdCotistaItau.Substring(4, 9);
                        cotista.DigitoConta = cotista.IdCotistaItau.Substring(13, 1);
                        cotista.StatusAtivo = "S";
                        cotista.DataImportacao = DateTime.Now;

                        // TODO: puxar o registro do fundo/casar via cnpj
                        if (hstCotistas.ContainsKey(cotista.CPFCNPJ) == false)
                        {
                            logger.Info("Inserindo CPF/CNPJ [" + cotista.CPFCNPJ + "]");
                            db.InserirCotista(cotista);
                            hstCotistas.Add(cotista.CPFCNPJ, cotista);
                        }
                        else
                        {
                            logger.Info("Descartando CPF/CNPJ [" + cotista.CPFCNPJ + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ignorando linha [" + linha + "]");
                        logger.Error("Erro: " + ex.Message, ex);
                    }
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_ImportacaoCadastroCotistaTioPonso1(): " + ex.Message, ex);
            }
            finally
            {
                if (str != null)
                    str.Close();
            }

            return bRet;

        }

        /// <summary>
        /// Quebra galho do TioPonso para importacao do cadastro de cotistas
        /// Planilha fornecida pelo Augusto
        /// </summary>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private bool _processa_ItauFJ_ImportacaoCadastroCotistaTioPonso2(string arquivo)
        {
            bool bRet = false;
            CultureInfo ciBR = CultureInfo.GetCultureInfo("pt-BR");
            StreamReader str = null;
            Hashtable hstCotistas = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {
                str = new StreamReader(arquivo);
                List<CotistaInfo> cotistas = db.ObterCadastroCotistas();
                foreach (CotistaInfo info in cotistas)
                {
                    if (hstCotistas.ContainsKey(info.IdCotistaItau) == false)
                        hstCotistas.Add(info.IdCotistaItau, info);
                }

                while (str.EndOfStream == false)
                {
                    string linha = str.ReadLine();

                    if (linha.Length == 0) continue;

                    try
                    {

                        string inicio = linha.Substring(0, 9);

                        if (inicio.Equals("CDBANCLI;") )
                            continue;

                        CotistaInfo cotista = new CotistaInfo();

                        // Layout:
                        // Id;Quote Holder;Fund;Subconta_EN;Operador_EN;CPF/CNPJ
                        // Onde:
                        // Id: id do cotista no Itau (ag+cc+dig)
                        // Quote Holder: nome do cotista
                        // Fund: fundo
                        // Subconta_EN: Subconta
                        // Operador_EN: operador Gradual associado ao cadastro
                        // CPF/CNPJ: cpf/cnpj do cotista

                        string[] arrFields = linha.Split(';');

                        cotista.BancoCli = arrFields[0].Trim();
                        cotista.Agencia = arrFields[1].Trim();
                        cotista.Conta = arrFields[2].Trim();
                        cotista.DigitoConta = arrFields[3].Trim();
                        cotista.Nome = arrFields[4].Trim();
                        cotista.SubConta = "201";
                        cotista.CPFCNPJ = arrFields[7].Trim();
                        cotista.IdCotistaItau = cotista.Agencia + cotista.Conta + cotista.DigitoConta;

                        cotista.StatusAtivo = "S";
                        cotista.DataImportacao = DateTime.Now;

                        // TODO: puxar o registro do fundo/casar via cnpj
                        if (hstCotistas.ContainsKey(cotista.IdCotistaItau) == false)
                        {
                            logger.Info("Inserindo CPF/CNPJ [" + cotista.CPFCNPJ + "] [" + cotista.Conta + "] [" + cotista.Nome + "]");
                            db.InserirCotista(cotista);
                            hstCotistas.Add(cotista.IdCotistaItau, cotista);
                        }
                        else
                        {
                            logger.Info("Descartando CPF/CNPJ [" + cotista.CPFCNPJ + "] [" + cotista.Conta + "] [" + cotista.Nome + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ignorando linha [" + linha + "]");
                        logger.Error("Erro: " + ex.Message, ex);
                    }
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFJ_ImportacaoCadastroCotistaTioPonso2(): " + ex.Message, ex);
            }
            finally
            {
                if (str != null)
                    str.Close();
            }

            return bRet;

        }


        #endregion // ItauFJ

        #region ItauFC
        private bool _processa_ItauFC_CarteirasGestor(string arquivo)
        {
            bool bRet = false;
            CultureInfo ciBR = CultureInfo.GetCultureInfo("pt-BR");
            StreamReader str = null;
            Hashtable hstFundos = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();


            try
            {
                str = new StreamReader(arquivo);
                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    if (hstFundos.ContainsKey(info.CNPJ) == false)
                        hstFundos.Add(info.CNPJ, info);
                }

                while (str.EndOfStream == false)
                {
                    string linha = str.ReadLine();

                    if (linha.Length == 0) continue;

                    try
                    {

                        string inicio = linha.Substring(0, 3);

                        if (inicio.Equals("COD") || inicio.Equals("99;"))
                            continue;

                        ItauFC_CarteirasGestor carteira = new ItauFC_CarteirasGestor();

                        string[] arrFields = linha.Split(';');

                        carteira.CodigoItau = arrFields[0];
                        carteira.CodigoCliente = arrFields[1];
                        carteira.Nome = arrFields[2];
                        carteira.Cnpj = arrFields[3].Trim().PadLeft(15,'0');
                        carteira.CodCetip = arrFields[4];
                        carteira.CodCSelic = arrFields[5];
                        carteira.NomeRes = arrFields[6];
                        carteira.CodIndex = arrFields[7];
                        carteira.MnemonicoCetip = arrFields[8];

                        // TODO: puxar o registro do fundo/casar via cnpj
                        if (hstFundos.ContainsKey(carteira.Cnpj))
                        {
                            CadastroFundoInfo info = hstFundos[carteira.Cnpj] as CadastroFundoInfo;

                            info.CodFundoFC = carteira.CodigoItau;

                            db.AtualizarCadastroFundo(info);
                        }
                        else
                        {
                            logger.Error("Fundo nao encontrado: [" + carteira.Cnpj + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ignorando linha [" + linha + "]");
                        logger.Error("Erro: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFC_CotasPatrimonio(): " + ex.Message, ex);
            }
            finally
            {
                if (str != null)
                    str.Close();
            }

            return bRet;
        }


        private bool _processa_ItauFC_CotasPatrimonio(string arquivo)
        {
            bool bRet = false;
            CultureInfo ciBR = CultureInfo.GetCultureInfo("pt-BR");
            StreamReader str = null;
            Hashtable hstFundos = new Hashtable();
            PersistenciaDB db = new PersistenciaDB();

            try
            {

                List<CadastroFundoInfo> fundos = db.ObterListaFundos();
                foreach (CadastroFundoInfo info in fundos)
                {
                    if (hstFundos.ContainsKey(info.CodFundoFC) == false)
                        hstFundos.Add(info.CodFundoFC, info);
                }

                str = new StreamReader(arquivo);

                while (str.EndOfStream == false)
                {
                    string linha = str.ReadLine();

                    string inicio = linha.Substring(0, 3);

                    if (inicio.Equals("DAT") || inicio.Equals("99;"))
                        continue;

                    ItauFC_CotasPatrimonio cota = new ItauFC_CotasPatrimonio();

                    string[] arrFields = linha.Split(';');

                    cota.Data = DateTime.ParseExact(arrFields[0] + " 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    cota.CodFundo = arrFields[1].Trim();
                    cota.ValorCota = Convert.ToDecimal(arrFields[2], ciBR);
                    cota.PatrimonioLiquido = Convert.ToDecimal(arrFields[3], ciBR);

                    // TODO: puxar o registro do fundo/casa
                    if ( hstFundos.ContainsKey(cota.CodFundo) )
                    {
                        CadastroFundoInfo info = hstFundos[cota.CodFundo] as CadastroFundoInfo;

                        info.PatrimonioLiquido = cota.PatrimonioLiquido;
                        info.NumeroCotas = cota.PatrimonioLiquido / cota.ValorCota;
                        info.DataAtualizacao = cota.Data;

                        db.AtualizarCadastroFundo(info);
                    }
                    else
                    {
                        logger.Error("Fundo nao encontrado: [" + cota.CodFundo + "]");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("_processa_ItauFC_CotasPatrimonio(): " + ex.Message, ex);
            }
            finally
            {
                if (str != null)
                    str.Close();
            }


            return bRet;
        }

        #endregion //ItauFC

        #region BlockReaders

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        private T FromBinaryReaderBlock<T>(BinaryReader br)
        {
            byte[] buff = br.ReadBytes(Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            T s = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return s;
        }



        #endregion //BlockReaders
    }
}
