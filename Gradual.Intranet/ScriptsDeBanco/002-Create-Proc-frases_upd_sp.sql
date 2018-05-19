CREATE PROCEDURE frases_upd_sp
                 @id_frase bigint
               , @ds_frase varchar(200)
AS
    UPDATE tb_frases
    SET    tb_frases.ds_frase = @ds_frase
    WHERE  tb_frases.id_frase = @id_frase
 