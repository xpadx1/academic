CREATE PROCEDURE AddDevice
    @Id NVARCHAR(50),
    @Name NVARCHAR(100),
    @IsEnabled BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Device (Id, Name, IsEnabled)
    VALUES (@Id, @Name, @IsEnabled);
END
GO

CREATE PROCEDURE AddSmartwatch
    @Battery NVARCHAR(50),
    @DeviceId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Smartwatch (BatteryPercentage, DeviceId)
    VALUES (@Battery, @DeviceId);
END
GO

CREATE PROCEDURE AddEmbedded
    @Ip NVARCHAR(100),
    @Network NVARCHAR(100),
    @DeviceId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Embedded (IpAddress, NetworkName, DeviceId)
    VALUES (@Ip, @Network, @DeviceId);
END
GO

CREATE PROCEDURE AddPersonalComputer
    @OS NVARCHAR(100),
    @DeviceId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PersonalComputer (OperationSystem, DeviceId)
    VALUES (@OS, @DeviceId);
END
GO

CREATE PROCEDURE UpdateDevice
    @Id NVARCHAR(50),
    @Name NVARCHAR(100),
    @IsEnabled BIT,
    @OriginalRowVersion ROWVERSION
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Device
    SET Name = @Name, IsEnabled = @IsEnabled
    WHERE Id = @Id AND RowVersion = @OriginalRowVersion;

    IF @@ROWCOUNT = 0
        BEGIN
            THROW 50001, 'The record was modified by another user.', 1;
        END
END
GO

CREATE PROCEDURE DeleteDevice
@Id NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Device WHERE Id = @Id;
END
GO
