USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoStatus')
BEGIN
	CREATE TABLE TbFundoFluxoStatus
	(
		IdFundoFluxoStatus INT PRIMARY KEY,
		DsFundoFluxoStatus VARCHAR(100)
	)
	;

	INSERT INTO TbFundoFluxoStatus
	SELECT 1, 'Conclu�do'
	UNION SELECT 2, 'N�o se Aplica'
	UNION SELECT 3, 'A Definir'
	UNION SELECT 4, 'Em An�lise'
	;
END
GO