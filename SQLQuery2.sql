-- 0) Aseg�rate de que est�s en master, no en la base que vas a borrar
USE master;
GO

-- 1) Poner en un solo usuario y cerrar conexiones activas
ALTER DATABASE tablDoItList
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- 2) Eliminar la base si ya existe
IF DB_ID('tablDoItList') IS NOT NULL
    DROP DATABASE tablDoItList;
GO

-- 3) Crear la base limpia
CREATE DATABASE tablDoItList;
GO

-- 4) Cambiar el contexto para trabajar dentro de ella
USE tablDoItList;
GO

-- 5) Definir esquema -------------------------------------------------------

-- 5.1) Tabla Users
CREATE TABLE Users (
    Id           BIGINT       IDENTITY(1,1) PRIMARY KEY,       -- PK auto-incremental
    Name         NVARCHAR(255)    NOT NULL,                     -- Nombre
    Email        NVARCHAR(255)    NOT NULL UNIQUE,              -- Correo �nico
    PasswordHash NVARCHAR(255)    NOT NULL,                     -- Hash de contrase�a
    CreatedAt    DATETIME2        NOT NULL 
                  DEFAULT SYSUTCDATETIME()                     -- Fecha creaci�n autom�tica
);
GO

-- 5.2) Tabla TaskItems
CREATE TABLE TaskItems (
    Id           BIGINT       IDENTITY(1,1) PRIMARY KEY,        -- PK auto-incremental
    Title        NVARCHAR(255) NOT NULL,                        -- T�tulo de la tarea
    Description  NVARCHAR(MAX) NULL,                            -- Descripci�n libre
    IsCompleted  BIT          NOT NULL 
                  DEFAULT(0),                                  -- 0=pendiente, 1=completada
    CreatedAt    DATETIME2    NOT NULL 
                  DEFAULT SYSUTCDATETIME(),                    -- Fecha creaci�n
    UpdatedAt    DATETIME2    NOT NULL 
                  DEFAULT SYSUTCDATETIME(),                    -- Fecha �ltima modificaci�n
    DueDate      DATE         NULL,                            -- Fecha l�mite (opcional)
    Priority     NVARCHAR(6)  NOT NULL,                        -- LOW/MEDIUM/HIGH
    UserId       BIGINT       NOT NULL,                        -- FK ? Users(Id)

    CONSTRAINT CK_TaskItems_Priority
      CHECK (Priority IN ('LOW','MEDIUM','HIGH')),              -- Valida valores

    CONSTRAINT FK_TaskItems_Users
      FOREIGN KEY(UserId) REFERENCES Users(Id)
      ON DELETE CASCADE                                         -- Borra tareas si borras user
);
GO

-- 6) �ndices para acelerar consultas frecuentes
CREATE INDEX IX_TaskItems_UserId      ON TaskItems(UserId);      -- Filtrar por usuario
CREATE INDEX IX_TaskItems_IsCompleted ON TaskItems(IsCompleted); -- Filtrar por estado
CREATE INDEX IX_TaskItems_DueDate     ON TaskItems(DueDate);     -- Filtrar por fecha l�mite
GO
