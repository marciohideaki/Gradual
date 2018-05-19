USE GradualFundosAdm
GO

ALTER TABLE dbo.tbFundoCadastro
ADD TxGestao DECIMAL(5,2) NULL
GO

ALTER TABLE dbo.tbFundoCadastro
ADD TxCustodia DECIMAL(5,2) NULL
GO

ALTER TABLE dbo.tbFundoCadastro
ADD TxConsultoria DECIMAL(5,2) NULL
GO
