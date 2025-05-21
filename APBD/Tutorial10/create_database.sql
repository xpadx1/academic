CREATE TABLE Device (
    Id NVARCHAR(100) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    IsEnabled BIT NOT NULL
);

CREATE TABLE Embedded (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  IpAddress NVARCHAR(100) NOT NULL,
  NetworkName NVARCHAR(100) NOT NULL,
  DeviceId NVARCHAR(100) NOT NULL,
  CONSTRAINT FK_Embedded_Device FOREIGN KEY (DeviceId) REFERENCES Device(Id) ON DELETE CASCADE
);

CREATE TABLE PersonalComputer (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  OperationSystem NVARCHAR(100) NOT NULL,
  DeviceId NVARCHAR(100) NOT NULL,
  CONSTRAINT FK_PersonalComputer_Device FOREIGN KEY (DeviceId) REFERENCES Device(Id) ON DELETE CASCADE
);

CREATE TABLE Smartwatch (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BatteryPercentage NVARCHAR(10) NOT NULL,
    DeviceId NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_Smartwatch_Device FOREIGN KEY (DeviceId) REFERENCES Device(Id) ON DELETE CASCADE
);

INSERT INTO Device (Id, Name, IsEnabled) VALUES
    ('SW-1', 'Apple Watch SE2', 1),
    ('P-1', 'LinuxPC', 0),
    ('P-2', 'ThinkPad T440', 0),
    ('ED-1', 'Pi3', 1),
    ('ED-2', 'Pi4', 1),
    ('ED-3', 'Pi4', 1),
    ('Capital33', 'BestPC_Ever', 0),
    ('16//16//16//16', 'Unknown Device', 0);

INSERT INTO Smartwatch (BatteryPercentage, DeviceId) VALUES
    ('27%', 'SW-1');

INSERT INTO PersonalComputer (OperationSystem, DeviceId) VALUES
    ('Linux Mint', 'P-1'),
    ('Unknown', 'P-2'),
    ('Unknown', 'Capital33'),
    ('Unknown', '16//16//16//16');

INSERT INTO Embedded (IpAddress, NetworkName, DeviceId) VALUES
    ('192.168.1.44', 'MD Ltd.Wifi-1', 'ED-1'),
    ('192.168.1.45', 'eduroam', 'ED-2'),
    ('whatisIP', 'MyWifiName', 'ED-3');

ALTER TABLE Device ADD RowVersion ROWVERSION;
