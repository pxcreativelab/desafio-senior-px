/* =====================================================================
   Hubie - .NET Senior Tech Test
   Script 01 - Schema creation (SQL Server / LocalDB)
   ---------------------------------------------------------------------
   Convention: mirrors the real Hubie style (UPPERCASE columns, _ID / _NAME
   / _DT suffixes, denormalized name columns stored next to the FK).
   Run this script against an empty database named "HubieTest".
   ===================================================================== */

IF DB_ID('HubieTest') IS NULL
BEGIN
    PRINT 'Create the database first: CREATE DATABASE HubieTest; (or use LocalDB, see README)';
END
GO

USE HubieTest;
GO

/* --------------------------------------------------------------------- */
/* APP_USER - requesters and agents                                      */
/* (named APP_USER because USER is a reserved keyword in T-SQL)          */
/* --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.APP_USER', 'U') IS NOT NULL DROP TABLE dbo.APP_USER;
GO
CREATE TABLE dbo.APP_USER
(
    USER_ID         BIGINT          IDENTITY(1,1) NOT NULL,
    USER_NAME       NVARCHAR(150)   NOT NULL,
    USER_LOGIN      NVARCHAR(100)   NOT NULL,
    USER_PASSWORD   NVARCHAR(200)   NOT NULL,   -- SHA-256 (hex) of the password
    USER_EMAIL      NVARCHAR(150)   NULL,
    USER_PROFILE    NVARCHAR(20)    NOT NULL,   -- 'REQUESTER' | 'AGENT'
    USER_ACTIVE     BIT             NOT NULL CONSTRAINT DF_APP_USER_ACTIVE DEFAULT (1),
    CONSTRAINT PK_APP_USER PRIMARY KEY (USER_ID),
    CONSTRAINT UQ_APP_USER_LOGIN UNIQUE (USER_LOGIN)
);
GO

/* --------------------------------------------------------------------- */
/* CATEGORY - ticket categories (Complaint, Question, ...)               */
/* --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.CATEGORY', 'U') IS NOT NULL DROP TABLE dbo.CATEGORY;
GO
CREATE TABLE dbo.CATEGORY
(
    CATEGORY_ID     BIGINT          IDENTITY(1,1) NOT NULL,
    CATEGORY_NAME   NVARCHAR(100)   NOT NULL,
    CATEGORY_ACTIVE BIT             NOT NULL CONSTRAINT DF_CATEGORY_ACTIVE DEFAULT (1),
    CONSTRAINT PK_CATEGORY PRIMARY KEY (CATEGORY_ID)
);
GO

/* --------------------------------------------------------------------- */
/* TICKET - the ticket opened by the requester                           */
/* --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.TICKET', 'U') IS NOT NULL DROP TABLE dbo.TICKET;
GO
CREATE TABLE dbo.TICKET
(
    TICKET_ID           BIGINT          IDENTITY(1,1) NOT NULL,
    TICKET_TITLE        NVARCHAR(200)   NOT NULL,
    TICKET_DESCRIPTION  NVARCHAR(MAX)   NULL,
    TICKET_STATUS       NVARCHAR(20)    NOT NULL,   -- OPEN | IN_PROGRESS | ANSWERED | CLOSED
    CATEGORY_ID         BIGINT          NOT NULL,
    CATEGORY_NAME       NVARCHAR(100)   NULL,       -- denormalized (Hubie style)
    REQUESTER_ID        BIGINT          NOT NULL,
    REQUESTER_NAME      NVARCHAR(150)   NULL,
    AGENT_ID            BIGINT          NULL,
    AGENT_NAME          NVARCHAR(150)   NULL,
    TICKET_CREATED_DT   DATETIME        NOT NULL CONSTRAINT DF_TICKET_CREATED DEFAULT (GETDATE()),
    TICKET_UPDATED_DT   DATETIME        NULL,
    TICKET_CLOSED_DT    DATETIME        NULL,
    CONSTRAINT PK_TICKET PRIMARY KEY (TICKET_ID),
    CONSTRAINT FK_TICKET_CATEGORY FOREIGN KEY (CATEGORY_ID) REFERENCES dbo.CATEGORY (CATEGORY_ID),
    CONSTRAINT FK_TICKET_REQUESTER FOREIGN KEY (REQUESTER_ID) REFERENCES dbo.APP_USER (USER_ID),
    CONSTRAINT FK_TICKET_AGENT FOREIGN KEY (AGENT_ID) REFERENCES dbo.APP_USER (USER_ID)
);
GO
CREATE INDEX IX_TICKET_STATUS ON dbo.TICKET (TICKET_STATUS);
CREATE INDEX IX_TICKET_REQUESTER ON dbo.TICKET (REQUESTER_ID);
GO

/* --------------------------------------------------------------------- */
/* INTERACTION - message thread between requester and agent              */
/* --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.INTERACTION', 'U') IS NOT NULL DROP TABLE dbo.INTERACTION;
GO
CREATE TABLE dbo.INTERACTION
(
    INTERACTION_ID       BIGINT         IDENTITY(1,1) NOT NULL,
    TICKET_ID            BIGINT         NOT NULL,
    USER_ID              BIGINT         NOT NULL,
    USER_NAME            NVARCHAR(150)  NULL,
    USER_PROFILE         NVARCHAR(20)   NULL,        -- author profile: REQUESTER | AGENT
    INTERACTION_MESSAGE  NVARCHAR(MAX)  NOT NULL,
    INTERACTION_CREATED_DT DATETIME     NOT NULL CONSTRAINT DF_INTERACTION_CREATED DEFAULT (GETDATE()),
    CONSTRAINT PK_INTERACTION PRIMARY KEY (INTERACTION_ID),
    CONSTRAINT FK_INTERACTION_TICKET FOREIGN KEY (TICKET_ID) REFERENCES dbo.TICKET (TICKET_ID),
    CONSTRAINT FK_INTERACTION_USER FOREIGN KEY (USER_ID) REFERENCES dbo.APP_USER (USER_ID)
);
GO
CREATE INDEX IX_INTERACTION_TICKET ON dbo.INTERACTION (TICKET_ID);
GO

/* --------------------------------------------------------------------- */
/* ATTACHMENT - files attached to a ticket (or to an interaction)        */
/* --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.ATTACHMENT', 'U') IS NOT NULL DROP TABLE dbo.ATTACHMENT;
GO
CREATE TABLE dbo.ATTACHMENT
(
    ATTACHMENT_ID       BIGINT          IDENTITY(1,1) NOT NULL,
    TICKET_ID           BIGINT          NOT NULL,
    INTERACTION_ID      BIGINT          NULL,
    ATTACHMENT_NAME     NVARCHAR(255)   NOT NULL,       -- original file name
    ATTACHMENT_TYPE     NVARCHAR(100)   NULL,           -- content-type
    ATTACHMENT_SIZE     BIGINT          NULL,           -- bytes
    ATTACHMENT_PATH     NVARCHAR(500)   NOT NULL,       -- relative path saved on the server
    USER_ID             BIGINT          NOT NULL,
    ATTACHMENT_CREATED_DT DATETIME      NOT NULL CONSTRAINT DF_ATTACHMENT_CREATED DEFAULT (GETDATE()),
    CONSTRAINT PK_ATTACHMENT PRIMARY KEY (ATTACHMENT_ID),
    CONSTRAINT FK_ATTACHMENT_TICKET FOREIGN KEY (TICKET_ID) REFERENCES dbo.TICKET (TICKET_ID),
    CONSTRAINT FK_ATTACHMENT_INTERACTION FOREIGN KEY (INTERACTION_ID) REFERENCES dbo.INTERACTION (INTERACTION_ID),
    CONSTRAINT FK_ATTACHMENT_USER FOREIGN KEY (USER_ID) REFERENCES dbo.APP_USER (USER_ID)
);
GO
CREATE INDEX IX_ATTACHMENT_TICKET ON dbo.ATTACHMENT (TICKET_ID);
GO

PRINT 'HubieTest schema created successfully.';
GO
