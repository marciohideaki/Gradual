CREATE PROCEDURE frases_upd_sp
@ds_frase varchar(200)
AS
INSERT INTO tb_frases
       (    ds_frase)
VALUES (    @ds_frase)
 