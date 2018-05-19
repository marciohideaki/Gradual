CREATE PROCEDURE frases_del_sp
                 @id_frase bigint
AS
    DELETE
    FROM   TB_FRASES
    WHERE  TB_FRASES.ID_FRASE = @id_frase
 