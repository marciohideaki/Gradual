CREATE PROCEDURE prc_tb_ficha_perfil_upd
( @id_ficha_perfil   int
, @id_cliente        int
, @ds_faixa_etaria   varchar(100)
, @ds_ocupacao       varchar(100)
, @ds_conhecimento   varchar(100)
, @tp_investidor     varchar(100)
, @tp_investimento   varchar(100)
, @tp_instituicao    varchar(100)
, @ds_renda_familiar varchar(100))
AS
   UPDATE [dbo].[tb_ficha_perfil]
   SET    [id_cliente]        = @id_cliente
   ,      [ds_faixa_etaria]   = @ds_faixa_etaria
   ,      [ds_ocupacao]       = @ds_ocupacao
   ,      [ds_conhecimento]   = @ds_conhecimento
   ,      [tp_investidor]     = @tp_investidor
   ,      [tp_investimento]   = @tp_investimento
   ,      [tp_instituicao]    = @tp_instituicao
   ,      [ds_renda_familiar] = @ds_renda_familiar
   WHERE  [id_ficha_perfil]   = @id_ficha_perfil