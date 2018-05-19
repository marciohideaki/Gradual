USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoGrupoEtapa')
BEGIN
	CREATE TABLE TbFundoFluxoGrupoEtapa
	(
		IdFundoFluxoGrupoEtapa INT PRIMARY KEY,
		IdFundoFluxoGrupo INT FOREIGN KEY REFERENCES TbFundoFluxoGrupo(IdFundoFluxoGrupo) NOT NULL,
		DsFundoFluxoGrupoEtapa VARCHAR(100) NOT NULL,
	)
	;

	CREATE NONCLUSTERED INDEX IX_TbFundoFluxoGrupoEtapa_IdFundoFluxoGrupo ON TbFundoFluxoGrupoEtapa(IdFundoFluxoGrupo)

	--CARGA INICIAL
	INSERT INTO TbFundoFluxoGrupoEtapa
	SELECT		 1, 1, 'Elabora��o e revis�o pela gestora'
	UNION SELECT 2, 1, 'Revis�o da administradora'
	UNION SELECT 3, 1, 'Revis�o do custodiante'
	UNION SELECT 4, 1, 'Registro dos documentos'

	UNION SELECT 5, 2, 'Elabora��o pelo advogado contratado'
	UNION SELECT 6, 2, 'Revis�o pela gestora'

	UNION SELECT 7, 3, 'Elabora��o do DBE pela administradora'
	UNION SELECT 8, 3, 'Protocolo na receita federal'

	UNION SELECT 9, 4, 'Elabora��o e revis�o pela gestora'
	UNION SELECT 10, 4, 'Revis�o da administradora'
	UNION SELECT 11, 4, 'Protocolo na CVM'
	UNION SELECT 12, 4, 'Emiss�o do oficio de registro de funcionamento pela CVM'
	
	UNION SELECT 13, 5, 'Solicitar para a administradora a contrata��o de agencia de rating'
	UNION SELECT 14, 5, 'Envio pela administra��o da documenta��o para a agencia de rating'

	UNION SELECT 15, 6, 'Envio de documenta��o, pela administradora, ao custodiante'
	UNION SELECT 16, 6, 'Assinatura das partes no contrato'

	UNION SELECT 17, 7, 'Envio de formul�rio para a consultoria'
	UNION SELECT 18, 7, 'Envio de formulario preenchido pela consultoria ao custodiante'

	UNION SELECT 19, 8, 'Defini��o pelo cliente do banco e agencia'
	UNION SELECT 20, 8, 'Elabora��o da documenta��o cadastral pela administradora'
	UNION SELECT 21, 8, 'Disponibiliza��o do documento cadastral para o banco'

	UNION SELECT 22, 9, 'Envio da documenta��o , ao banco, ela gestora ou administradora'
	UNION SELECT 23, 9, 'Solicita��o da administradora ao custodiante do acesso ao internet banking'

	UNION SELECT 24, 10, 'Elabora��o do contrato to termo de cess�o pela consultoria'

	UNION SELECT 25, 11, 'Elabora��o do contrato de consultoria pela administradora'
	UNION SELECT 26, 11, 'Envio, pela administradora, da minuta do contrato para a consultoria'
	UNION SELECT 27, 11, 'Finaliza��o do contrato e assinatura'

	UNION SELECT 28, 12, 'Envio de formulario de cadastramento para a consultoria'
	UNION SELECT 29, 12, 'Receber via original do formulario da consultoria'
	UNION SELECT 30, 12, 'Preencher o formulario de cadastramento com os dados do gestor'
	UNION SELECT 31, 12, 'Encaminhar os formularios originais para a administradora'
	UNION SELECT 32, 12, 'Envio dos formularios, pela administradora, para o custodiante'

	UNION SELECT 33, 13, 'Solicita��o da documenta��o para a consultoria'
	UNION SELECT 34, 13, 'Encaminhamento da documenta��o recebida da consultoria para a administradora'

	UNION SELECT 35, 14, 'Solicita��o para a administradora para a abertura da conta'
	UNION SELECT 36, 14, 'A administradora solicita ao custodiante a abertura da conta'
	UNION SELECT 37, 14, 'O custodiante encaminhar o contrato para a assinatura da administradora'

	UNION SELECT 38, 15, 'Solicita��o para a administradora para a abertura da conta'
	UNION SELECT 39, 15, 'A administradora solicita ao custodiante a abertura da conta'
	UNION SELECT 40, 15, 'O custodiante encaminhar o contrato para a assinatura da administradora'
	UNION SELECT 41, 15, 'A administradora e o custodiante providenciam a documenta��o para encaminhar a CETIP'

	UNION SELECT 42, 16, 'A administradora solicita a BMF a cria��o do c�digo ISIN'
END
GO
