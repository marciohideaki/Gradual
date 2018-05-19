set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rel_email_disparado_periodo_ins_sp]
    @id_email              int OUTPUT
  , @id_tipoemail          int
  , @ds_corpoemail         varchar(MAX)
  , @ds_emaildestinatario  varchar(200)
  , @dt_envioemail         datetime
  , @ds_assuntoemail	   varchar(255)
AS

INSERT INTO [dbo].[tb_emaildisparado]
       (    [dbo].[tb_emaildisparado].[id_tipoemail]
       ,    [dbo].[tb_emaildisparado].[ds_corpoemail]
       ,    [dbo].[tb_emaildisparado].[ds_emaildestinatario]
       ,    [dbo].[tb_emaildisparado].[dt_envioemail]
	   ,	[dbo].[tb_emaildisparado].[ds_assuntoemail])
VALUES (    @id_tipoemail
       ,    @ds_corpoemail
       ,    @ds_emaildestinatario
       ,    @dt_envioemail
	   ,	@ds_assuntoemail)

SELECT @id_email = SCOPE_IDENTITY()
