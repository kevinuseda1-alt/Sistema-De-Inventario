-- Ejecutar en Bike_Store existente, después de realizar un respaldo.
-- Conserva usuarios, productos, ventas, inventario y sus identificadores.
USE [Bike_Store];
GO
IF COL_LENGTH(N'dbo.users', N'password_hash') IS NULL
    ALTER TABLE dbo.users ADD password_hash VARBINARY(64) NULL;
GO
IF COL_LENGTH(N'dbo.users', N'password_salt') IS NULL
    ALTER TABLE dbo.users ADD password_salt VARBINARY(32) NULL;
GO
-- La cuenta de demostración se convierte si aún conserva la clave original.
-- Otras cuentas se convierten al primer login válido.
UPDATE dbo.users SET password_hash=0x3bbc3c6c809894c29cdbc5c8ec4ccd2b0361c7c5c1c0f03f3c827fcca5c855bdc368d2dfa0da96aa79670c86270b7b57ae82acd3d6a4606d489140671f4c007a,
    password_salt=0x4fdaadc424d8dcec31b98b9b2eb781be87c052c054af6f9758cabf3fe09f7eb6, password='MIGRADO-'+CONVERT(VARCHAR(36),NEWID())
WHERE username='josue' AND password='123456' AND password_hash IS NULL;
GO
IF OBJECT_ID(N'dbo.suppliers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.suppliers (
        supplier_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        supplier_name NVARCHAR(120) NOT NULL,
        contact_name NVARCHAR(120) NULL,
        phone NVARCHAR(30) NULL,
        email NVARCHAR(150) NULL,
        address NVARCHAR(250) NULL,
        is_active BIT NOT NULL CONSTRAINT DF_suppliers_active DEFAULT(1),
        created_at DATETIME2 NOT NULL CONSTRAINT DF_suppliers_created DEFAULT(SYSDATETIME())
    );
END;
GO
IF OBJECT_ID(N'dbo.product_suppliers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.product_suppliers (
        product_id INT NOT NULL,
        supplier_id INT NOT NULL,
        CONSTRAINT PK_product_suppliers PRIMARY KEY(product_id,supplier_id),
        CONSTRAINT FK_ps_product FOREIGN KEY(product_id) REFERENCES dbo.products(product_id),
        CONSTRAINT FK_ps_supplier FOREIGN KEY(supplier_id) REFERENCES dbo.suppliers(supplier_id)
    );
END;
GO
IF COL_LENGTH(N'dbo.products', N'min_stock') IS NULL
    ALTER TABLE dbo.products ADD min_stock INT NOT NULL CONSTRAINT DF_products_min_stock DEFAULT(5);
GO
IF COL_LENGTH(N'dbo.products', N'is_active') IS NULL
    ALTER TABLE dbo.products ADD is_active BIT NOT NULL CONSTRAINT DF_products_is_active DEFAULT(1);
GO
CREATE OR ALTER VIEW dbo.vw_inventory_summary AS
SELECT p.product_id,p.product_name,p.price,p.min_stock,
       ISNULL(SUM(CASE WHEN i.movement_type='ENTRADA' THEN i.quantity
                       WHEN i.movement_type='SALIDA' THEN -i.quantity ELSE 0 END),0) AS stock_actual
FROM dbo.products p LEFT JOIN dbo.inventory i ON i.product_id=p.product_id
WHERE p.is_active=1
GROUP BY p.product_id,p.product_name,p.price,p.min_stock;
GO
CREATE OR ALTER PROCEDURE dbo.speliminar_products @product_id INT
AS
BEGIN
    UPDATE dbo.products SET is_active=0 WHERE product_id=@product_id AND is_active=1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.spmostrar_products
AS
BEGIN
    SELECT p.product_id,p.product_name,p.model_year,p.price,p.imagen,p.category_id,
           p.create_date,c.category_name AS Category
    FROM dbo.products p INNER JOIN dbo.categories c ON c.category_id=p.category_id
    WHERE p.is_active=1 ORDER BY p.product_id DESC;
END;
GO
-- Una entrada/salida y su verificación de saldo se ejecutan bajo el mismo bloqueo.
CREATE OR ALTER PROCEDURE dbo.sp_registrar_movimiento_inventario
    @product_id INT,@quantity INT,@movement_type VARCHAR(20),@user_id INT,
    @order_id INT=NULL,@notes VARCHAR(255)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @quantity<=0 OR @quantity IS NULL THROW 51001,N'Cantidad inválida.',1;
    IF @movement_type NOT IN ('ENTRADA','SALIDA') OR @movement_type IS NULL
        THROW 51002,N'Tipo de movimiento inválido.',1;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS(SELECT 1 FROM dbo.products WITH (UPDLOCK,HOLDLOCK) WHERE product_id=@product_id)
            THROW 51003,N'Producto inexistente.',1;
        IF @movement_type='SALIDA'
        BEGIN
            DECLARE @stock INT;
            SELECT @stock=ISNULL(SUM(CASE WHEN movement_type='ENTRADA' THEN quantity ELSE -quantity END),0)
            FROM dbo.inventory WITH (UPDLOCK,HOLDLOCK) WHERE product_id=@product_id;
            IF @stock<@quantity THROW 51004,N'Stock insuficiente.',1;
        END;
        INSERT dbo.inventory(product_id,quantity,movement_type,user_id,order_id,notes,movement_date)
        VALUES(@product_id,@quantity,@movement_type,@user_id,@order_id,@notes,GETDATE());
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
SELECT name AS Tabla FROM sys.tables ORDER BY name;
