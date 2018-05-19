CREATE PROCEDURE [dbo].[paises_blacklist_ins_sp]
	  @cd_pais varchar(3)
AS
/*
DESCRIÇÃO:
	Insere registro na tabela tb_paises_blacklist.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
INSERT [dbo].[tb_paises_blacklist]
     ( 
       [cd_pais]
     )
VALUES
     (
       @cd_pais
     )
-- Recupera o valor IDENTITY da linha inserida
SELECT SCOPE_IDENTITY()