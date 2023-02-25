CREATE DATABASE IF NOT EXISTS Programacion_Web;
use Programacion_Web;

/*Entidad 1: Person, contine la información general de las personas*/
CREATE TABLE IF NOT EXISTs Person(  
    ID int NOT NULL AUTO_INCREMENT,  
    FirstName varchar(100) not null,  
    LastName varchar (100) not null,  
	Phone varchar(20) not null,
    Birthdate date not null,
    Email varchar(30) not null,
    Genero bit not null,
    fec_transac datetime DEFAULT CURRENT_TIMESTAMP,
    primary key (ID)
);

/*ENTIDAD 2: ESTATUS, contine los distinos estatus que puede tener un usuario*/
CREATE TABLE IF NOT EXISTs Status(  
    ID int NOT NULL AUTO_INCREMENT,  
    Name varchar(30) not null,  
    Description varchar (150) not null,  
	vigente bit not null,
    fec_transac datetime DEFAULT CURRENT_TIMESTAMP,
    primary key (ID)
);

/*Entidad 3: User, contine la información del usuario de la persona*/
CREATE TABLE IF NOT EXISTs User(  
    ID int NOT NULL AUTO_INCREMENT,  
    Username varchar(30) not null,  
    Password varchar (30) not null,  
	Status_ID int not null,
    ID_Persona int not null,
    fec_transac datetime DEFAULT CURRENT_TIMESTAMP,
    primary key (ID)
);

/*Entidad 4: contacts, contiene a todos los contactos asociados al usuario*/
CREATE TABLE IF NOT EXISTS Contact_List (  
    ID int NOT NULL AUTO_INCREMENT,  
    primerUserId int not null,  
    segundoUserId int not null,   
	Status_ID int not null,
    fec_transac datetime DEFAULT CURRENT_TIMESTAMP,
    primary key (ID)
);

/*ENTIDAD 5: CHAT, contiene toda la información de los chats entre dos personas*/
CREATE TABLE IF NOT EXISTs Chat(  
    ID int NOT NULL AUTO_INCREMENT,  
    Lista_ID int not null,  
    mensaje varchar(16350) not null,
	archivos int not null,
    fec_transac datetime DEFAULT CURRENT_TIMESTAMP,
    primary key (ID)
);

/*ALTERS: PREFERIBLEMENTE REALIZAR LOS FOREING KEYS HASTA EL FINAL, TRAS HABER REALIZADO LA CREACIÓN DE LAS TABLAS*/
ALTER TABLE User ADD CONSTRAINT FK_PersonID FOREIGN KEY (ID_Persona) REFERENCES Person(ID);
ALTER TABLE User ADD CONSTRAINT FK_StatusID FOREIGN KEY (Status_ID) REFERENCES Status(ID);

ALTER TABLE Contact_List ADD CONSTRAINT FK_PPersonID FOREIGN KEY (primerUserId) REFERENCES Person(ID);
ALTER TABLE Contact_List ADD CONSTRAINT FK_SPersonID FOREIGN KEY (segundoUserId) REFERENCES Person(ID);

ALTER TABLE Chat ADD CONSTRAINT FK_ListaID FOREIGN KEY (Lista_ID) REFERENCES Contact_List(ID);

/*Inserción 1: A la tabla Status, para indicar cuales son los estatus que puede contener*/
INSERT Into Status (Name, Description, vigente) Values ('Activo','El usuario se encuentra activo para poder ingresar a la aplicación', 1);
INSERT Into Status (Name, Description, vigente) Values ('Eliminado','El usuario se encuentra eliminado de la aplicación. No podrá ingresar a la aplicación', 1);

/*Validacion: Validación del contenido*/
select * from Person;
select * from status;
select * from User;
select * from Contact_List;
select * from Chat;

/*Querys de apoyo:*/
/*
Drop Table if exists Chat;
Drop Table if exists Contact_List;
Drop Table if exists User;
Drop Table if exists status;
Drop Table if exists Person;

*/
