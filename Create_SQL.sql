SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_ID(N'ColegioDb') IS NULL
    BEGIN
        EXEC(N'CREATE DATABASE ColegioDb;');
    END
    ELSE
        PRINT 'La base de datos ColegioDb ya existe.';

GO

BEGIN TRY    
   
    BEGIN TRAN;

    IF DB_ID(N'ColegioDb') IS NULL
    THROW 50000, 'No se pudo crear la base de datos ColegioDb.', 1;

    IF OBJECT_ID(N'ColegioDb.dbo.Estudiante', N'U') IS NULL
    BEGIN
        CREATE TABLE ColegioDb.dbo.Estudiante
        (
            Id     INT IDENTITY(1,1) NOT NULL,
            Nombre NVARCHAR(100)     NOT NULL,
            CONSTRAINT PK_Estudiante PRIMARY KEY CLUSTERED (Id)
        );
    END
    ELSE
        PRINT 'Tabla Estudiante ya existe.';

    IF OBJECT_ID(N'ColegioDb.dbo.Profesor', N'U') IS NULL
    BEGIN
        CREATE TABLE ColegioDb.dbo.Profesor
        (
            Id     INT IDENTITY(1,1) NOT NULL,
            Nombre NVARCHAR(100)     NOT NULL,
            CONSTRAINT PK_Profesor PRIMARY KEY CLUSTERED (Id)
        );
    END
    ELSE
        PRINT 'Tabla Profesor ya existe.';

    IF OBJECT_ID(N'ColegioDb.dbo.Nota', N'U') IS NULL
    BEGIN
        CREATE TABLE ColegioDb.dbo.Nota
        (
            Id           INT IDENTITY(1,1) NOT NULL,
            Nombre       NVARCHAR(100)     NOT NULL,
            IdProfesor   INT               NOT NULL,
            IdEstudiante INT               NOT NULL,
            Valor        DECIMAL(4,2)      NOT NULL,

            CONSTRAINT PK_Nota PRIMARY KEY CLUSTERED (Id),
            CONSTRAINT CK_Nota_Valor CHECK (Valor >= 0 AND Valor <= 10)
        );
    END
    ELSE
        PRINT 'Tabla Nota ya existe.';

    
    IF NOT EXISTS (
        SELECT 1
        FROM ColegioDb.sys.foreign_keys fk
        WHERE fk.name = N'FK_Nota_Profesor'
          AND fk.parent_object_id = OBJECT_ID(N'ColegioDb.dbo.Nota')
    )
    BEGIN
        ALTER TABLE ColegioDb.dbo.Nota
        ADD CONSTRAINT FK_Nota_Profesor
            FOREIGN KEY (IdProfesor)
            REFERENCES ColegioDb.dbo.Profesor (Id)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

    END
    ELSE
        PRINT 'FK_Nota_Profesor ya existe.';

    IF NOT EXISTS (
        SELECT 1
        FROM ColegioDb.sys.foreign_keys fk
        WHERE fk.name = N'FK_Nota_Estudiante'
          AND fk.parent_object_id = OBJECT_ID(N'ColegioDb.dbo.Nota')
    )
    BEGIN
        ALTER TABLE ColegioDb.dbo.Nota
        ADD CONSTRAINT FK_Nota_Estudiante
            FOREIGN KEY (IdEstudiante)
            REFERENCES ColegioDb.dbo.Estudiante (Id)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

    END
    ELSE
        PRINT 'FK_Nota_Estudiante ya existe.';


    IF NOT EXISTS (
        SELECT 1
        FROM ColegioDb.sys.indexes i
        WHERE i.name = N'IX_Nota_IdEstudiante'
          AND i.object_id = OBJECT_ID(N'ColegioDb.dbo.Nota')
    )
    BEGIN
        CREATE INDEX IX_Nota_IdEstudiante ON ColegioDb.dbo.Nota(IdEstudiante);
    END
    ELSE
        PRINT 'Índice IX_Nota_IdEstudiante ya existe.';

    IF NOT EXISTS (
        SELECT 1
        FROM ColegioDb.sys.indexes i
        WHERE i.name = N'IX_Nota_IdProfesor'
          AND i.object_id = OBJECT_ID(N'ColegioDb.dbo.Nota')
    )
    BEGIN
        CREATE INDEX IX_Nota_IdProfesor ON ColegioDb.dbo.Nota(IdProfesor);

    END
    ELSE
        PRINT 'Índice IX_Nota_IdProfesor ya existe.';

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK;

    DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
    PRINT '❌ Error: ' + @Msg;
    THROW;
END CATCH;