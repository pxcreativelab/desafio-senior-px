/* =====================================================================
   Hubie - .NET Senior Tech Test
   Script 02 - Initial data (seed)
   ---------------------------------------------------------------------
   Password for every sample user: 123456
   Stored hash = SHA-256(password) as lowercase hex:
       SHA256('123456') = 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
   (the same hash function lives in HubieTest.Business / SecurityHelper)
   ===================================================================== */

USE HubieTest;
GO

/* ----------------------------- USERS ----------------------------- */
SET IDENTITY_INSERT dbo.APP_USER ON;

INSERT INTO dbo.APP_USER (USER_ID, USER_NAME, USER_LOGIN, USER_PASSWORD, USER_EMAIL, USER_PROFILE, USER_ACTIVE)
VALUES
 (1, N'Mary Requester', N'requester', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'mary@customer.com', N'REQUESTER', 1),
 (2, N'John Agent',     N'agent',     N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'john@hubie.com',    N'AGENT',     1);

SET IDENTITY_INSERT dbo.APP_USER OFF;
GO

/* ---------------------------- CATEGORIES ---------------------------- */
SET IDENTITY_INSERT dbo.CATEGORY ON;

INSERT INTO dbo.CATEGORY (CATEGORY_ID, CATEGORY_NAME, CATEGORY_ACTIVE)
VALUES
 (1, N'Complaint',  1),
 (2, N'Question',   1),
 (3, N'Suggestion', 1);

SET IDENTITY_INSERT dbo.CATEGORY OFF;
GO

PRINT 'Seed applied. Logins: requester / agent  (password: 123456)';
GO
