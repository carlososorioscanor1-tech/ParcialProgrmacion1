-- =============================================
-- PRUEBA PARCIAL: PROGRAMACIÓN I
-- Base de datos: ParcialProgramacion1
-- =============================================

-- 1. Crear la base de datos
IF DB_ID('ParcialProgramacion1') IS NULL
BEGIN
    CREATE DATABASE ParcialProgramacion1;
END;
GO

USE ParcialProgramacion1;
GO

-- 2. Crear la tabla Productos
IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL
    DROP TABLE dbo.Productos;
GO

CREATE TABLE dbo.Productos
(
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Categoria NVARCHAR(50) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL CHECK (Precio > 0),
    Existencias INT NOT NULL CHECK (Existencias >= 0)
);
GO

-- 3. Registrar 3 productos de ejemplo
INSERT INTO dbo.Productos (Nombre, Categoria, Existencias, Precio) VALUES
('Laptop HP 15', 'Tecnologia', 10, 750.00),
('Mouse Inalambrico', 'Accesorios', 50, 15.99),
('Teclado Mecanico', 'Accesorios', 25, 45.50);
GO

SELECT * FROM dbo.Productos;