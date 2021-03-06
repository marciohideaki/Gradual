set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE [dbo].[prc_tb_ficha_perfil_ins]
( @id_cliente int
, @ds_faixa_etaria   varchar(100)
, @ds_ocupacao       varchar(100)
, @ds_conhecimento   varchar(100)
, @tp_investidor     varchar(100)
, @tp_investimento   varchar(100)
, @tp_instituicao    varchar(100)
, @ds_renda_familiar varchar(100))
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

   SELECT SCOPE_IDENTITY()
