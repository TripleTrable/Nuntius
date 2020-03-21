--Create db an users
CREATE DATABASE nuntius;
USE nuntius;
CREATE USER nuntiusserver;


-- Create tabels
CREATE TABLE users (
	id INT AUTO_INCREMENT PRIMARY KEY,
	alias VARCHAR(32) NOT NULL UNIQUE,
	pwd_md5 text NOT NULL,
	publicKey text NOT NULL
);

CREATE TABLE token (
	token VARCHAR(32) PRIMARY KEY,
	expire TIMESTAMP NOT NULL,
	userID INT UNIQUE NOT NULL,

	FOREIGN KEY (userID) REFERENCES users(id)
);

CREATE TABLE messages (
	id INT AUTO_INCREMENT PRIMARY KEY,
	from_user INT NOT NULL,
	to_user INT NOT NULL,
	send TIMESTAMP NOT NULL,
	content TEXT NOT NULL,
	unread BOOLEAN DEFAULT true NOT NULL,

	FOREIGN KEY (from_user) REFERENCES users(id),
	FOREIGN KEY (to_user)   REFERENCES users(id)
);


-- Grants
GRANT CONNECT ON DATABASE nuntius TO nuntiusserver;
GRANT SELECT ON users TO nuntiusServer;
GRANT INSERT ON users TO nuntiusServer;
GRANT UPDATE ON users TO nuntiusServer;

GRANT SELECT ON token TO nuntiusServer;
GRANT INSERT ON token TO nuntiusServer;
GRANT DELETE ON token TO nuntiusServer;

GRANT SELECT ON messages TO nuntiusServer;
GRANT INSERT ON messages TO nuntiusServer;
GRANT UPDATE ON messages TO nuntiusServer;