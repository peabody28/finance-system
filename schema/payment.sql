CREATE TABLE balanceOperationType (
	Id uuid PRIMARY KEY,
	Code string NOT NULL
);

CREATE TABLE wallet (
	Id uuid NOT NULL PRIMARY KEY,
	Number string NOT NULL
);

CREATE TABLE payment (
Id uuid NOT NULL PRIMARY KEY,
WalletFk uuid NOT NULL,
BalanceOperationTypeFk uuid NOT NULL,
Amount decimal(10,2) NOT NULL,
Created string NOT NULL,
FOREIGN KEY (WalletFk)
REFERENCES wallet (Id)
ON DELETE CASCADE
ON UPDATE NO ACTION	
FOREIGN KEY (BalanceOperationTypeFk)
REFERENCES balanceOperationType (Id)
ON DELETE CASCADE
ON UPDATE NO ACTION
);

INSERT OR IGNORE INTO balanceOperationType (Id, Code) VALUES('425A1526-6DBC-4F00-B136-4F7B0E8F4A23', 'Credit');
INSERT OR IGNORE INTO balanceOperationType (Id, Code) VALUES('C2A332AF-C8C3-4BAB-823A-421209331EEB', 'Debit');

CREATE TABLE configuration (
	Id uuid PRIMARY KEY,
	Key string NOT NULL,
	Value string NOT NULL
);

INSERT OR IGNORE INTO configuration (Id, Key, Value) VALUES ('C19BE3C8-C138-4ACF-87DA-52718BCC54BF', 'WALLET_MS_ROUTE', 'http://wallet/wallet');