CREATE TABLE [dbo].[tblBankAccounts]
(
    [BankAccountId] INT NOT NULL PRIMARY KEY,
    [CorrelationId] NVARCHAR(200) NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [Address] NVARCHAR(200) NOT NULL,
    [OpeningBalance] MONEY NOT NULL
)