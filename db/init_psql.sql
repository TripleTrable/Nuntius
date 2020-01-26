--Create db an users
CREATE DATABASE nuntius;
CREATE ROLE nuntiusserver WITH password 'h:gDxAHG]Td)(+O:R5xuVJ';
\c nuntius;


-- Create tabels
CREATE TABLE users (
	id SERIAL PRIMARY KEY,
	alias VARCHAR(32) NOT NULL UNIQUE,
	pwd_md5 text NOT NULL
);

CREATE TABLE token (
	token VARCHAR(32) PRIMARY KEY,
	expire TIMESTAMP NOT NULL,
	userID INT UNIQUE NOT NULL,

	FOREIGN KEY (userID) REFERENCES users(id)
);

CREATE TABLE messages (
	id SERIAL PRIMARY KEY,
	from_user INT NOT NULL,
	to_user INT NOT NULL,
	send TIMESTAMP NOT NULL,
	content TEXT NOT NULL,

	FOREIGN KEY (from_user) REFERENCES users(id),
	FOREIGN KEY (to_user)   REFERENCES users(id)
);


-- Grants
GRANT CONNECT ON DATABASE nuntius TO nuntiusserver ;
GRANT SELECT ON users TO nuntiusServer;
GRANT INSERT ON users TO nuntiusServer;
GRANT SELECT ON token TO nuntiusServer;
GRANT INSERT ON token TO nuntiusServer;
