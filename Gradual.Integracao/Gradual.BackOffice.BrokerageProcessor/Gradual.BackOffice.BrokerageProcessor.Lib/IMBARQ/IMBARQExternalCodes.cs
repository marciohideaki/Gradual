using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.IMBARQ
{
    public sealed class IMBARQ_Side
    {
        private readonly String name;
        private readonly String value;

        private static readonly Dictionary<string, IMBARQ_Side> instance = new Dictionary<string, IMBARQ_Side>();

        public static readonly IMBARQ_Side COMPRA = new IMBARQ_Side("COMPRA", "1");
        public static readonly IMBARQ_Side VENDA = new IMBARQ_Side("VENDA", "2");
        public static readonly IMBARQ_Side DOADOR = new IMBARQ_Side("DOADOR", "3");
        public static readonly IMBARQ_Side TOMADOR = new IMBARQ_Side("TOMADOR", "4");
        public static readonly IMBARQ_Side TITULAR_COMPRADOR = new IMBARQ_Side("TITULAR_COMPRADOR", "T");
        public static readonly IMBARQ_Side LANCADOR_VENDEDOR = new IMBARQ_Side("LANCADOR_VENDEDOR", "L");
        public static readonly IMBARQ_Side COMPRA2 = new IMBARQ_Side("COMPRA", "C");
        public static readonly IMBARQ_Side VENDA3 = new IMBARQ_Side("VENDA", "V");
        public static readonly IMBARQ_Side EMPTY = new IMBARQ_Side(" ", " ");

        private IMBARQ_Side(string name, string value)
        {
            this.name = name;
            this.value = value;
            instance[value] = this;
        }

        public override string ToString()
        {
            return name;
        }

        public static explicit operator IMBARQ_Side(string str)
        {
            IMBARQ_Side result;
            if (instance.TryGetValue(str, out result))
                return result;
            else
                throw new InvalidCastException();
        }
    }

    public enum IMBARQ_SettlementType
    {
        MULTILATERAL=0,
        BRUTO=1,
        SEM_TRANSFERENÇAS_FINANCEIRAS_VIA_CAMARA=2,
        ENTRE_AS_PARTES=3,
        LIQUIDACAO_ANTECIPADA=4,
        LIQUIDACAO_POR_DIFERENCA_ESPECIAL=5,
        LIQUIDACAO_ANTECIPADA_POR_DIFERENCA=6,
        LIQUIDACAO_POR_DECURSO_DE_PRAZO=7
    }

    public enum IMBARQ_ContractType
    {
        OFERTA_DOADORA_REGULAR=1,
        OFERTA_TOMADORA_REGULAR=2,
        DIRETO_REGULAR=3,
        MANDATORIO=4,
        NAO_PADRONIZADO=5,
        RENOVACAO=6,
        EVENTOS_CORPORATIVOS=7,
        EMPRESTIMO_DE_ATIVOS_POR_FALHA=8
    }

    public enum IMBARQ_MarketType
    {
        DISPONIVEL = 1,
        FUTURO = 2,
        OPCOES_SOBRE_DISPONIVEL = 3,
        OPCOES_SOBRE_FUTURO = 4,
        TERMO = 5,
        ETF_PRIMARIO = 8,
        VISTA = 10,
        EXERCICIO_DE_OPCAO_CALL = 12,
        EXERCICIO_DE_OPCAO_PUT = 13,
        LEILAO = 17,
        FRACIONARIO = 20,
        TERMO_DE_ACOES = 30,
        OPC = 70,
        OPV = 80,
        SWAP = 81,
        OPCAO_FLEXIVEL_PUT = 82,
        OPCAO_FLEXIVEL_CALL = 83,
        FORWARD = 84,
        INDICES = 85,
        CURVA = 86,
        SUPERFICIE = 87,
        EMPRESTIMO_DE_ATIVOS = 91
    }

    public sealed class IMBARQ_CollateralIndicator
    {
        private readonly String name;
        private readonly String value;

        private static readonly Dictionary<string, IMBARQ_CollateralIndicator> instance = new Dictionary<string, IMBARQ_CollateralIndicator>();

        public static readonly IMBARQ_CollateralIndicator COM_GARANTIAS = new IMBARQ_CollateralIndicator("COM_GARANTIAS", "C");
        public static readonly IMBARQ_CollateralIndicator SEM_GARANTIAS = new IMBARQ_CollateralIndicator("SEM_GARANTIAS", "S");

        private IMBARQ_CollateralIndicator(string name, string value)
        {
            this.name = name;
            this.value = value;
            instance[value] = this;
        }

        public override string ToString()
        {
            return name;
        }

        public static explicit operator IMBARQ_CollateralIndicator(string str)
        {
            IMBARQ_CollateralIndicator result;
            if (instance.TryGetValue(str, out result))
                return result;
            else
                throw new InvalidCastException();
        }
    }


    public enum IMBARQ_SubAccountCode
    {
        CART_UNITS=2105,
        ADMINISTRACAO_DE_BDRS=2127,
        BLOQUEIO_ATIVOS_E_RECURSOS_FINANCEIROS=2198,
        RESERVA_DE_ACOES_PARA_PARTICIPACAO_EM_DISSIDENCIA=5305,
        BLOQUEIO_DE_POSICAO_DE_CLIENTE_POR_SOLICITACAO_DO_AGENTE=5901,
        CARTEIRA_LIVRE=21016,
        CONTA_TRANSITORIA_PARA_INTE_RESGATE_DE_COTA_DE_FDO_INDICE=21024,
        ATIVOS_EM_PROCESSO_DE_DEPOSITOS=21032,
        BONUS_DE_SUBSCRICAO_DO_BANCO_DO_BRASIL=21040,
        CAUCAO_PARA_FINANCIAMENTO_DE_CONTA_MARGEM=21059,
        ACOES_FUNGIVEIS=21067,
        ADMINISTRACAO_DE_CERTIFICADOS=21075,
        CONTROLE_INTERNO_CBLC=21083,
        ADMCERTIFICADOS_DE_DESDOBRAMENTO_CLC=21091,
        CERTIFICADOS_DE_DESDOBRAMENTO_CBLC_SUB_JUDICE=21105,
        CONTOLE_DE_QUEBRAS_CALCULOS_DE_EVENTOS_EM_ATIVOS=21156,
        ADMATIVOS_ISENTOS_DE_TAXA_DE_CUSTODIA=21164,
        OFERTA_FUNDO_DE_INDICE=21172,
        ACOES_FUNGIVEIS__SUBJUDICE_=21180,
        LASTRO_NO_EMISSOR=21199,
        PROCESSOS_EM_EXIGENCIA=21245,
        OPERACOES_ESPECIAIS=21407,
        OFERTA_PUBLICA_COMPA_A_VISTA_SEM_DESCONTO=21504,
        INTERFACE_COM_OUTRAS_BOLSAS_MES_PAR____ARBITRAGEM=21601,
        INTERFACE_COM_OUTRAS_BOLSAS_MES_IMPAR____ARBITRAGEM=21610,
        INTERFACE_COM_OUTRAS_BOLSAS_MES_IMPAR_=21954,
        INTERFACE_COM_OUTRAS_BOLSAS__MES_PAR_=21962,
        BLOQUEIO_ACORDO_DE_ACIONISTA=21970,
        BLOQUEIO_DE_ATIVOS_E_RECURSOS_FINANCEIROS=21989,
        BLOQUEIOS_DE_ATIVOS=21997,
        COBERTURA_DO_BTC=22012,
        BLOQUEIO_DE_VENDA=22020,
        CARTEIRA_SELECIONADA_DE_ACOES__CESTA_DE_ACOES=22080,
        GARANTIAS_DE_DERIVATIVOS_OU_BTC=23019,
        BLOQUEIO_POR_SOLICITACAO_DE_RETIRADA=23035,
        INTEGRALIZACAO_E_RESGATE_DE_COTAS_DO_FUNDO_DE_INDICE=23043,
        CONTROLE_DE_PROCESSOS_DE_CUSTODIA=23051,
        GARANTIAS=23906,
        ACOES_FUNGIVEIS__PLANO_COLLOR_=24015,
        RESERVA_DE_ACOES_PARA_DIREITO_DE_RETIRADA=24023,
        ACOES_FUNGIVEIS_PLANO_COLLOR_=24031,
        ADMIN_DE_BDRS=24040,
        OPERACOES_DE_CUSTODIA=24058,
        ACOES_FUNGIVEIS2_PLANO_COLLOR_=24066,
        CONVERSAO_DE_ATIVOS=24074,
        VENDA_ACOES_INVESTIDOR_MINORITARIO=24082,
        COBERTURA_DE_VENDA_A_VISTA=24090,
        COBERTURA_FUTURO=25011,
        CANCELAMENTO_DE_RECOMPRA=25020,
        COBERTURA_PARA_TERMO_ESPECIAL=25038,
        REPACTUACAO_RENDA_FIXA=25054,
        COBERTURA_PARA_TERMO=26018,
        COBERTURA_OPCOES=27014,
        PROTECAO_DO_INVESTIMENTO_COM_PARTICIPACAO=27057,
        EMPRESTIMO_DE_ATIVOS=28010,
        BLOQUEIO_POR_SOLICITACAO_DO_AGENTE_DE_CUSTODIA=29017,
        ADMINISTRACAO_DE_ATIVOS_NAO_COTADOS_SIMPLES_GUARDA=31011,
        ATIVOS_EM_PROCESSO_DE_RETIRADA=31032,
        ATIVOS_EM_PROCESSO_DE_RETIRADA2=31038,
        ADMIN_DE_CERTIFICADOS=31070,
        PRIVATIZACAO_DE_EMPRESAS_NAO_COTADAS=37010,
        ACOES_NOMINATIVAS_INFUNGIVEIS=41017,
        ACOES_NOMINATIVAS2_INFUNGIVEIS=42803,
        CARTEIRA_LIVRE_MERCADO_DE_BALCAO_ORGANIZADO=51012,
        ATIVOS_EM_PROC_DE_DEPOSITOS=51039,
        CAUCAO_PARA_FINAN_DE_CONTA_MARGEM=51055,
        ADMDE_CERTIFICADOS_MERCADO_DE_BALCAO_ORGANIZADO=51071,
        CONTROLE_INTERNO_CBLC_MERCADO_DE_BALCAO_ORGANIZADO=51080,
        CONTQUEBRAS_CALCEVEM_ATIVOS_MERCDE_BALCAO_ORGANIZADO=51152,
        BLOQUEIO_ACORDO_DE_ACIONISTAS_MERCBALCAO_ORGANIZADO=51977,
        BLOQATIVOS_E_RECURSOS_FINANC_MERCBALCAO_ORGANIZADO=51985,
        BLOQUEIOS_DE_ATIVOS_MERCADO_DE_BALCAO_ORGANIZADO=51993,
        COBERTURA_DO_BTC_MERCADO_DE_BALCAO_ORGANIZADO=52019,
        BLOQUEIO_DE_VENDA_MERCADO_DE_BALCAO_ORGANIZADO=52027,
        GARANTIAS_DE_DERIVAT_OU_BTC_MERCDE_BALCAO_ORGANIZADO=53015,
        BLOQPARA_OPERACESPECIAIS_MERCDE_BALCAO_ORGANIZADO=53031,
        RESDE_ACOES_DIRDE_RET_MERCDE_BALCAO_ORGANIZADO=53040,
        CONTPROCESSO_DE_CUSTODIA_MERCDE_BALCAO_ORGANIZADO=53058,
        GARANTIAS_DE_DERIVATIVOS_OU_MERC_DE_BALCAO=53902,
        RESERV_ACOES_DIRDE_RET_MERCDE_BALCAO_ORGANIZADO=54020,
        CONVERSAO_DE_ATIVOS_MERCADO_DE_BALCAO_ORGANIZADO=54070,
        COBDE_VENDA_A_VISTA_MERCDE_BALCAO_ORGANIZADO=54097,
        COBERTURA_FUTURO_MERCADO_DE_BALCAO_ORGANIZADO=55018,
        CANCELDE_RECOMPRA_MERCADO_DE_BALCAO_ORGANIZADO=55026,
        COBPARA_TERMO_MERCADO_DE_BALCAO_ORGANIZADO=56014,
        COBERTURA_OPCOES_MERCADO_DE_BALCAO_ORGANIZADO=57010,
        EMPREST_DE_ATIVOS=58017,
        BLOQSOLICAGENTE_DE_CUST_MERCDE_BALCAO_ORGANIZADO=59013,
        ADMINVESTIDORES_PESSOAS_FISICAS_NAO_RESIDENTES=61018,
        PETROBRAS_BNDES_OFERTA_PUBLICA_DE_VENDA_DE_ACOES=71013,
        PROCESSO_PRIVATIZACAO___OFERTA_FUNCIONARIOS=71021,
        OFERTA_PUBLICA_FUNCIONARIOS=71030,
        DISTRIB_PUBL_PRIMARIA_ACOES_ON_E_PN_NET_SA=71048,
        PERMUTA_DE_ATIVOS=71056,
        BLOQUEIO_INSTRUCAO_CVM_400=71064,
        BLOQUEIO_INSTRUCAO_CVM_400___MERCADO_DE_BALCAO_ORGANIZADO=71072,
        VALE_DO_RIO_DOCE_OFERTA_PUBLICA_DE_VENDA_DE_ACOES_BNDES=72010,
        OFERTA_PUBLICA_DE_VENDA_DE_ACOES=72028,
        PETROBRAS_COBERTURA_OU_GARANTIA=73016,
        VALE_OFERTA_PUBLICA_DE_VENDA_DE_ACOES_DEPOSITO_DE_GARANTIAS=73024,
        SABESP_OFERTA_PUB_DE_VENDA_DE_ACOES_DEPOSITO_GARANTIAS=73032,
        OFPUBLICA_DE_VENDA_DE_ACOES_DEPOSITO_DE_GARANTIAS=73040
    }

    public enum IMBARQ_PayingInstitution
    {
        BOVESPA = 1,
        SOC_EMISSORA_BANCO = 2
    }

    public sealed class IMBARQ_Launch
    {
        private readonly String name;
        private readonly String value;

        private static readonly Dictionary<string, IMBARQ_Launch> instance = new Dictionary<string, IMBARQ_Launch>();

        public static readonly IMBARQ_Launch CREDITO = new IMBARQ_Launch("CREDITO", "C");
        public static readonly IMBARQ_Launch DEBITO = new IMBARQ_Launch("DEBITO", "D");
        public static readonly IMBARQ_Launch NONE = new IMBARQ_Launch(" ", " ");

        private IMBARQ_Launch(string name, string value)
        {
            this.name = name;
            this.value = value;
            instance[value] = this;
        }

        public override string ToString()
        {
            return name;
        }

        public static explicit operator IMBARQ_Launch(string str)
        {
            IMBARQ_Launch result;
            if (instance.TryGetValue(str, out result))
                return result;
            else
                throw new InvalidCastException();
        }
    }

    public enum IMBARQ_CollateralType
    {
        CARTA_DE_FIANCA=1,
        TIT_PRIVADO=2,
        ACAO=3,
        TIT_PUBLICO=4,
        VALOR_EM_ESPECIE=8,
        TIT_TESOURO=9,
        MOEDA_ESTRANGEIRA=10,
        CEDULA_DE_PRODUTO_RURAL=12,
        FUNDO_DE_INVESTIMENTO=13,
        OURO=14
    }

    public enum IMBARQ_CollateralCoverageType
    {
        TODOS=0,
        GARANTIAS_DE_OPERACOES=1,
        LIMITE_OPERACIONAL=2,
        FUNDO_DE_LIQUIDACAO_RISCO_RESIDUAL=9,
        FUNDO_DE_LIQUIDACAO=11,
        FUNDO_DE_LIQUIDACAO_CAMBIO_A_VISTA=12,
        GARANTIA_MINIMA_NAO_OPERACIONAL=31,
        AMPLIACAO_DE_LIMITE_DE_EMISSAO=40,
        LEILAO=41,
        DEPOSITO=42
    }


    public sealed class IMBARQ_OptionType
    {
        private readonly String name;
        private readonly String value;

        private static readonly Dictionary<string, IMBARQ_OptionType> instance = new Dictionary<string, IMBARQ_OptionType>();

        public static readonly IMBARQ_OptionType CALL = new IMBARQ_OptionType("CALL", "C");
        public static readonly IMBARQ_OptionType PUT = new IMBARQ_OptionType("PUT", "P");
        public static readonly IMBARQ_OptionType NONE = new IMBARQ_OptionType(" ", " ");

        private IMBARQ_OptionType(string name, string value)
        {
            this.name = name;
            this.value = value;
            instance[value] = this;
        }

        public override string ToString()
        {
            return name;
        }

        public static explicit operator IMBARQ_OptionType(string str)
        {
            IMBARQ_OptionType result;
            if (instance.TryGetValue(str, out result))
                return result;
            else
                throw new InvalidCastException();
        }
    }

}
