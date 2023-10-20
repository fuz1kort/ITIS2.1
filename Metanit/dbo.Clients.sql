CREATE TABLE [dbo].[Clients] (
    [Id]          INT         NULL,
    [Status]      INT         NULL,
    [Name]        NCHAR (255) NULL,
    [Age]         INT         NULL,
    [Contact]     NCHAR (255) NULL,
    [IsBlocked]   BIT         NULL,
    [IsAnonymous] BIT         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

