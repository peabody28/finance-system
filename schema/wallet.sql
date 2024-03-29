CREATE TABLE user (
	Id uuid PRIMARY KEY,
	Name string NOT NULL
);
CREATE TABLE currency (
	Id uuid PRIMARY KEY,
	Code string NOT NULL
);

CREATE TABLE wallet (
	Id uuid NOT NULL PRIMARY KEY,
	UserFk uuid NOT NULL,
	CurrencyFk uuid NOT NULL,
	Number string NOT NULL,
	
	FOREIGN KEY (UserFk)
		REFERENCES user (Id)
			ON DELETE CASCADE
			ON UPDATE NO ACTION
			
	FOREIGN KEY (CurrencyFk)
		REFERENCES currency (Id)
			ON DELETE CASCADE
			ON UPDATE NO ACTION
);

INSERT OR IGNORE INTO currency (Id, Code) VALUES('7A4FCB37-6575-4B37-B3E8-E101C2D25F50', 'EUR');
INSERT OR IGNORE INTO currency (Id, Code) VALUES('099BD595-62F3-45E1-A60A-93CB2316FF64', 'USD');
INSERT OR IGNORE INTO currency (Id, Code) VALUES('80CC7A4D-C6B3-4078-8F2A-A3FA43C958B9', 'BYN');