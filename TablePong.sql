-- Creación de base de datos
CREATE DATABASE TablePong;
GO

USE TablePong;

-- Tabla TIPO_PARTIDO
CREATE TABLE TIPO_PARTIDO (
    id INT PRIMARY KEY IDENTITY,
    sets INT NOT NULL,
    puntos INT NOT NULL
);

-- Tabla JUGADOR
CREATE TABLE JUGADOR (
    id INT PRIMARY KEY IDENTITY,
    nombre NVARCHAR(50) NOT NULL,
    apellido NVARCHAR(50) NOT NULL,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    fecha_creacion DATE NOT NULL DEFAULT GETDATE()
);

-- Tabla PARTIDO
CREATE TABLE PARTIDO (
    id INT PRIMARY KEY IDENTITY,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    tipo_partido_id INT NOT NULL,
    duracion INT NOT NULL,
    FOREIGN KEY (tipo_partido_id) REFERENCES TIPO_PARTIDO(id)
);

-- Tabla PARTIDO_JUGADOR
CREATE TABLE PARTIDO_JUGADOR (
    partido_id INT NOT NULL,
    jugador_id INT NOT NULL,
    puntos INT NOT NULL,
    PRIMARY KEY (partido_id, jugador_id),
    FOREIGN KEY (partido_id) REFERENCES PARTIDO(id),
    FOREIGN KEY (jugador_id) REFERENCES JUGADOR(id)
);

-- Tabla TORNEO
CREATE TABLE TORNEO (
    id INT PRIMARY KEY IDENTITY,
    nombre NVARCHAR(100) NOT NULL,
    fecha_inicio DATETIME NOT NULL,
    fecha_fin DATETIME NULL,
    tipo_torneo NVARCHAR(50) NOT NULL,  -- por ejemplo, 'round-robin' o 'eliminatorio'
    estado NVARCHAR(20) NOT NULL DEFAULT 'activo' -- 'activo', 'finalizado'
);

-- Tabla PARTIDO_TORNEO
CREATE TABLE PARTIDO_TORNEO (
    id INT PRIMARY KEY IDENTITY,
    torneo_id INT NOT NULL,
    partido_id INT NOT NULL,
    FOREIGN KEY (torneo_id) REFERENCES TORNEO(id),
    FOREIGN KEY (partido_id) REFERENCES PARTIDO(id)
);

-- Tabla PRODE
CREATE TABLE PRODE (
    id INT PRIMARY KEY IDENTITY,
    partido_id INT NOT NULL,
    jugador_id INT NOT NULL,
    apuesta NVARCHAR(50) NOT NULL,  -- Predicción de puntaje o resultado
    FOREIGN KEY (partido_id) REFERENCES PARTIDO(id),
    FOREIGN KEY (jugador_id) REFERENCES JUGADOR(id)
);

-- Tabla HEAD_TO_HEAD
CREATE TABLE HEAD_TO_HEAD (
    id INT PRIMARY KEY IDENTITY,
    jugador1_id INT NOT NULL,
    jugador2_id INT NOT NULL,
    victorias_jugador1 INT DEFAULT 0,
    victorias_jugador2 INT DEFAULT 0,
    empates INT DEFAULT 0,
    FOREIGN KEY (jugador1_id) REFERENCES JUGADOR(id),
    FOREIGN KEY (jugador2_id) REFERENCES JUGADOR(id),
    CONSTRAINT UC_Jugadores UNIQUE (jugador1_id, jugador2_id)
);
