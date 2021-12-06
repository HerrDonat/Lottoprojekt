CREATE TABLE [dbo].[Customer] (
    [Id]          INT  IDENTITY (1, 1) NOT NULL,
    [UserId]      INT  NOT NULL,
    [Datum]       DATE NULL,
    [number1]     INT  NULL,
    [number2]     INT  NULL,
    [number3]     INT  NULL,
    [number4]     INT  NULL,
    [number5]     INT  NULL,
    [number6]     INT  NULL,
    [numberSuper] INT  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);