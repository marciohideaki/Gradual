set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_tb_ficha_perfil_ins]
( @id_ficha_perfil   INT OUTPUT
, @id_cliente        INT
, @ds_faixa_etaria   VARCHAR(100)
, @ds_ocupacao       VARCHAR(100)
, @ds_conhecimento   VARCHAR(100)
, @tp_investidor     VARCHAR(100)
, @tp_investimento   VARCHAR(100)
, @tp_instituicao    VARCHAR(100)
, @ds_renda_familiar VARCHAR(100))
AS
   INSERT INTO [dbo].[tb_ficha_perfil]
          (    [id_cliente]
          ,    [ds_faixa_etaria]
          ,    [ds_ocupacao]
          ,    [ds_conhecimento]
          ,    [tp_investidor]
          ,    [tp_investimento]
          ,    [tp_instituicao]
          ,    [ds_renda_familiar]
          ,    [dt_inclusao])
   VALUES (    @id_cliente
          ,    @ds_faixa_etaria
          ,    @ds_ocupacao
          ,    @ds_conhecimento
          ,    @tp_investidor
          ,    @tp_investimento
          ,    @tp_instituicao
          ,    @ds_renda_familiar
          ,    GETDATE())

   SELECT @id_ficha_perfil = SCOPE_IDENTITY()

