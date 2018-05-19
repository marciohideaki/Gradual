CREATE PROCEDURE frases_sel_sp
                 @id_frase bigint
AS
    SELECT TB_FRASES.ID_FRASE
         , TB_FRASES.DS_FRASE
    FROM   TB_FRASES
    WHERE  TB_FRASES.ID_FRASE = @id_frase