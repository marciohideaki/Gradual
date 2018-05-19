
/****** Object:  StoredProcedure [dbo].[pep_ins_sp]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pep_ins_sp]
	  @ds_documento varchar(20),
      @ds_identificacao varchar(15),
	  @ds_nome varchar(200),
      @ds_linha varchar(2000),
	  @id_pep int output
AS
/*
DESCRIÇÃO:
	Insere registro na tabela tb_pep.
CRIAÇÃO:
	Desenvolvedor: Luciano De Maria 
	Data: 23/06/2010
*/
SET NOCOUNT ON
INSERT [dbo].[tb_pep]
     ( 
       [ds_documento]
     , [ds_identificacao]
     , [ds_nome]
     , [dt_importacao]
     , [ds_linha]
     )
VALUES
     (
       @ds_documento
     , @ds_identificacao
     , @ds_nome
     , getdate()
     , @ds_linha
     )
-- Recupera o valor IDENTITY da linha inserida
SELECT @id_pep = @@identity