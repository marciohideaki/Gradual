-- Descri��o:       Realiza a inclus�o dos dados de situa��o financeira patrimonial do cliente
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 11/05/2010
-- Motivo                   : Inclus�o dos parametros
							-- @vl_capitalsocial			 numeric(17, 2)
							-- @vl_patrimonioliquido         numeric(17, 2)
							-- @dt_capitalsocial             datetime
							-- @dt_patrimonioliquido         datetime
CREATE PROCEDURE cliente_situacaofinanceirapatrimonial_ins_sp
                 @id_cliente                   bigint
               , @vl_totalbensimoveis          numeric(17, 2)
               , @vl_totalbensmoveis           numeric(17, 2)
               , @vl_totalaplicacaofinanceira  numeric(17, 2)
               , @vl_totalsalarioprolabore     numeric(17, 2)
               , @vl_totaloutrosrendimentos    numeric(17, 2)
               , @vl_capitalsocial			   numeric(17, 2)
               , @vl_patrimonioliquido         numeric(17, 2)
               , @dt_capitalsocial             datetime
               , @dt_patrimonioliquido         datetime
               , @ds_outrosrendimentos         varchar(2000)
               , @dt_dataatualizacao           datetime
			   , @id_sfp                       INT OUTPUT
AS
    INSERT INTO [dbo].[tb_cliente_situacaofinanceirapatrimonial]
	       (    [id_cliente]                      
		   ,    [vl_totalbensimoveis]        
		   ,    [vl_totalbensmoveis]         
		   ,	[vl_totalaplicacaofinanceira]
		   ,    [vl_totalsalarioprolabore]   
		   ,    [vl_totaloutrosrendimentos]  
		   ,    [ds_outrosrendimentos]       
		   ,    [dt_dataatualizacao]
		   ,	[vl_capitalsocial]
           ,	[vl_patrimonioliquido]
           ,	[dt_capitalsocial]         
           ,	[dt_patrimonioliquido]
		   )
    VALUES (    @id_cliente
		   ,    @vl_totalbensimoveis
		   ,    @vl_totalbensmoveis
		   ,    @vl_totalaplicacaofinanceira
		   ,    @vl_totalsalarioprolabore
		   ,    @vl_totaloutrosrendimentos
		   ,    @ds_outrosrendimentos
		   ,    @dt_dataatualizacao
		   ,	@vl_capitalsocial			   
           ,	@vl_patrimonioliquido     
           ,	@dt_capitalsocial         
           ,	@dt_patrimonioliquido     
           )

     SELECT @id_sfp = SCOPE_IDENTITY()
