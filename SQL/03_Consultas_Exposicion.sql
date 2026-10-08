USE Bike_Store;
GO
-- Equivalente temático del paso 18: movimientos de cada producto.
SELECT p.product_name AS Producto,i.movement_date AS Fecha,
       i.movement_type AS Tipo,i.quantity AS Cantidad,u.username AS Usuario,i.notes AS Notas
FROM dbo.inventory i
JOIN dbo.products p ON p.product_id=i.product_id
JOIN dbo.users u ON u.user_id=i.user_id
ORDER BY i.movement_date DESC;
GO
-- Equivalente temático del paso 19: productos que necesitan reposición.
SELECT s.product_id,s.product_name AS Producto,s.stock_actual AS Stock,
       s.min_stock AS Minimo,s.price AS PrecioCordobas,
       v.supplier_name AS Proveedor,v.phone AS Telefono
FROM dbo.vw_inventory_summary s
LEFT JOIN dbo.product_suppliers ps ON ps.product_id=s.product_id
LEFT JOIN dbo.suppliers v ON v.supplier_id=ps.supplier_id AND v.is_active=1
WHERE s.stock_actual<=s.min_stock
ORDER BY s.stock_actual,s.product_name,v.supplier_name;
GO
-- Comprueba las tablas nuevas sin modificar datos.
SELECT name AS Tabla FROM sys.tables ORDER BY name;
