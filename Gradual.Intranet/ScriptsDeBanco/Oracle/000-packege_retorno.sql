CREATE OR REPLACE PACKAGE GLOBALPKG
AS
	TYPE Retorno IS REF CURSOR;
	TRANCOUNT INTEGER := 0;
	IDENTITY INTEGER;
END;
/