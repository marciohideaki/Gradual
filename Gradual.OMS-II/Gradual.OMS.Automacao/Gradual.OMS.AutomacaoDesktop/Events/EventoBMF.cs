using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.AutomacaoDesktop.Events
{
    public class EventoBMF
    {
        public string Sequencia { get; set; }
        public string Tipo { get; set; }
        public string Instrumento { get; set; }
        public string CodigoInstrumento { get; set; }
        public string Mensagem { get; set; }
    
	    public EventoBMF(
			    string sequencia, 
			    string tipo, 
			    string instrumento,
			    string codigoInstrumento, 
			    string mensagem) 
	    {
		    this.Sequencia = sequencia;
		    this.Tipo = tipo;
		    this.Instrumento = instrumento;
		    this.CodigoInstrumento = codigoInstrumento;
		    this.Mensagem = mensagem;
	    }


	    public const int SEQNUM_INI = 0;
	    public const int SEQNUM_FIM = 15;
	    public const int TYPE_INI = 15;
	    public const int TYPE_FIM = 16;
	    public const int SYMBOL_INI = 16;
	    public const int SYMBOL_FIM = 36;
	    public const int SECURITYID_INI = 36;
	    public const int SECURITYID_FIM = 56;

	    public const int BODY_INI = 56;

	    /* Formato da mensagem de MarketData */
	    public const int MDUPDATEACTION_INI = 0;
	    public const int MDUPDATEACTION_FIM = 1;
	    public const int MDENTRYID_INI = 1;
	    public const int MDENTRYID_FIM = 11;
	    public const int MDENTRYDATE_INI = 11;
	    public const int MDENTRYDATE_FIM = 19;
	    public const int MDENTRYTIME_INI = 19;
	    public const int MDENTRYTIME_FIM = 27;
	    public const int MDENTRYPOSITIONNO_INI = 27;
	    public const int MDENTRYPOSITIONNO_FIM = 33;
	    public const int MDENTRYPX_INI = 33;
	    public const int MDENTRYPX_FIM = 48;
	    public const int MDENTRYSIZE_INI = 48;
	    public const int MDENTRYSIZE_FIM = 63;
	    public const int NUMBEROFORDERS_INI = 63;
	    public const int NUMBEROFORDERS_FIM = 78;
	    public const int ORDERID_INI = 78;
	    public const int ORDERID_FIM = 128;
	    public const int MDENTRYBUYER_INI = 128;
	    public const int MDENTRYBUYER_FIM = 138;
	    public const int MDENTRYSELLER_INI = 138;
	    public const int MDENTRYSELLER_FIM = 148;
	    public const int TICKDIRECTION_INI = 148;
	    public const int TICKDIRECTION_FIM = 149;
	    public const int NETCHGPREVDAY_INI = 149;
	    public const int NETCHGPREVDAY_FIM = 160;
	    public const int UNIQUETRADEID_INI = 160;
	    public const int UNIQUETRADEID_FIM = 180;
	    public const int OPENCLOSESETTLFLAG_INI = 180;
	    public const int OPENCLOSESETTLFLAG_FIM = 181;
	    public const int TRADINGSESSIONSUBID_INI = 181;
	    public const int TRADINGSESSIONSUBID_FIM = 183;
	    public const int SECURITYTRADINGSTATUS_INI = 183;
	    public const int SECURITYTRADINGSTATUS_FIM = 186;
	    public const int NOREFERENTIALPRICES_INI = 186;
	    public const int NOREFERENTIALPRICES_FIM = 189;

	    /* mensagem de MarketData Incremental - formato das ocorrencias de NOREFERENTIALPRICES */
	    public const int REFERENTIALPRICETYPE_INI = 0;
	    public const int REFERENTIALPRICETYPE_FIM = 1;
	    public const int REFERENTIALPX_INI = 1;
	    public const int REFERENTIALPX_FIM = 16;
	    public const int TRANSACTTIME_INI = 16;
	    public const int TRANSACTTIME_FIM = 36;

	    /* Formato da mensagem de Security */
	    public const int SECURITYIDSOURCE_INI = 0;
	    public const int SECURITYIDSOURCE_FIM = 1;
	    public const int PRODUCT_INI = 1;
	    public const int PRODUCT_FIM = 4;
	    public const int CFICODE_INI = 4;
	    public const int CFICODE_FIM = 10;
	    public const int SECURITYTYPE_INI = 10;
	    public const int SECURITYTYPE_FIM = 42;
	    public const int SECURITYSUBTYPE_INI = 42;
	    public const int SECURITYSUBTYPE_FIM = 74;
	    public const int MATURITYMONTHYEAR_INI = 74;
	    public const int MATURITYMONTHYEAR_FIM = 80;
	    public const int MATURITYDATE_INI = 80;
	    public const int MATURITYDATE_FIM = 88;
	    public const int ISSUEDATE_INI = 88;
	    public const int ISSUEDATE_FIM = 96;
	    public const int COUNTRYOFISSUE_INI = 96;
	    public const int COUNTRYOFISSUE_FIM = 98;
	    public const int CONTRACTMULTIPLIER_INI = 98;
	    public const int CONTRACTMULTIPLIER_FIM = 113;
	    public const int SECURITYEXCHANGE_INI = 113;
	    public const int SECURITYEXCHANGE_FIM = 123;
	    public const int SECURITYDESC_INI = 123;
	    public const int SECURITYDESC_FIM = 223;
	    public const int CONTRACTSETTLMONTH_INI = 223;
	    public const int CONTRACTSETTLMONTH_FIM = 229;
	    public const int DATEDDATE_INI = 229;
	    public const int DATEDDATE_FIM = 237;
	    public const int CURRENCY_INI = 237;
	    public const int CURRENCY_FIM = 247;
	    public const int ROUNDLOT_INI = 247;
	    public const int ROUNDLOT_FIM = 262;
	    public const int MINTRADEVOL_INI = 262;
	    public const int MINTRADEVOL_FIM = 277;
	    public const int ASSET_INI = 277;
	    public const int ASSET_FIM = 287;
	    public const int STRIKEPRICE_INI = 287;
	    public const int STRIKEPRICE_FIM = 302;
	    public const int SECURITYGROUP_INI = 302;
	    public const int SECURITYGROUP_FIM = 317;

    }
}
