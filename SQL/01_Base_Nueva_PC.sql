-- BIKE STORE INVENTARIO: solo para una instalación nueva (base inexistente).
-- No elimina ninguna base; en una Bike_Store existente ejecutar únicamente SQL/02_Ampliar_Inventario.sql.
-- Usuario inicial del proyecto anterior: josue / 123456. Cambiar contraseña después del primer acceso.
USE master;
GO
IF DB_ID(N'Bike_Store') IS NULL EXEC(N'CREATE DATABASE Bike_Store');
GO
USE [Bike_Store];
GO

create table customers(
    customer_id int not null primary key,
    first_name varchar(255) not null,
    last_name varchar(255) not null,
    phone varchar(25) null,
    email varchar(255) not null,
    street varchar(255) null,
    city varchar(50) null,
    state varchar(25) null,
    create_date date not null
)
go

create table categories(
    category_id int not null identity(1,1) primary key,
    category_name varchar(255) not null
)
go

create table products(
    product_id int not null identity(1,1) primary key,
    product_name varchar(200) not null,
    model_year smallint not null,
    price decimal(10,2) not null,
    imagen image null,
    category_id int not null,
    create_date date not null,
    constraint FK_categoriesProducts foreign key (category_id) references categories(category_id)
)
go

create table users (
    user_id int primary key identity(1,1),
    username varchar(50) not null unique,
    password varchar(255) not null,
    full_name varchar(100) not null,
    email varchar(100) not null unique,
    role varchar(20) not null check (role IN ('admin', 'gerente', 'caja')),
    is_active bit default 1,
    created_at datetime default getdate(),
    last_login datetime null
)
go

create table orders (
    order_id int primary key identity(1,1),
    customer_id int not null,
    user_id int not null,
    order_date datetime default getdate(),
    constraint FK_orders_customers foreign key (customer_id) references customers(customer_id),
    constraint FK_orders_users foreign key (user_id) references users(user_id)
)
go

create table order_items (
    order_item_id int primary key identity,
    order_id int not null,
    product_id int not null,
    quantity int not null,
    price decimal(10,2) not null,
    discount decimal(10,2) default 0,
    foreign key (order_id) references orders(order_id),
    foreign key (product_id) references products(product_id)
)
go

create table inventory (
    inventory_id int identity(1,1) primary key,
    product_id int not null,
    quantity int not null check (quantity > 0),
    movement_type varchar(20) not null check (movement_type in ('ENTRADA','SALIDA')),
    user_id int not null,
    order_id int null,
    notes varchar(255) null,
    movement_date datetime not null default getdate(),
    constraint FK_inventory_products foreign key (product_id) references products(product_id),
    constraint FK_inventory_users foreign key (user_id) references users(user_id),
    constraint FK_inventory_orders foreign key (order_id) references orders(order_id)
)
GO

create proc spmostrar_customers
as
begin
    select 
        customer_id,
        first_name,
        last_name,
        phone,
        email,
        street,
        city,
        state,
        create_date
    from customers
    order by customer_id desc
end
go

create proc spinsertar_customers
    @customer_id int,
    @first_name varchar(255),
    @last_name varchar(255),
    @phone varchar(25),
    @email varchar(255),
    @street varchar(255),
    @city varchar(50),
    @state varchar(25)
as
begin
    insert into customers(
        customer_id,
        first_name,
        last_name,
        phone,
        email,
        street,
        city,
        state,
        create_date
    )
    values (
        @customer_id,
        @first_name,
        @last_name,
        @phone,
        @email,
        @street,
        @city,
        @state,
        getdate()
    )
end
go

create proc speditar_customers
    @customer_id int,
    @first_name varchar(255),
    @last_name varchar(255),
    @phone varchar(25),
    @email varchar(255),
    @street varchar(255),
    @city varchar(50),
    @state varchar(25)
as
begin
    update customers set 
        first_name = @first_name,
        last_name = @last_name,
        phone = @phone,
        email = @email,
        street = @street,
        city = @city,
        state = @state
    where customer_id = @customer_id
end
go

create proc speliminar_customers
    @customer_id int
as
begin
    delete from customers
    where customer_id = @customer_id
end
go

create proc spbuscar_customers
    @column_name varchar(100) = null,
    @search_value varchar(255) = null,
    @textobuscar varchar(255) = null
as
begin
    set nocount on;

    if @textobuscar is not null
    begin
        select *
        from customers
        where first_name like '%' + @textobuscar + '%'
           or last_name like '%' + @textobuscar + '%'
           or email like '%' + @textobuscar + '%'
        order by customer_id desc;
        return;
    end;

    if @column_name not in ('customer_id','first_name','last_name','phone','email','street','city','state')
    begin
        raiserror('Columna de búsqueda no válida', 16, 1);
        return;
    end;

    declare @sql nvarchar(max);
    declare @patron varchar(260);

    set @sql = N'select * from customers where ' + quotename(@column_name) +
               N' like @patron order by customer_id desc';

    set @patron = '%' + isnull(@search_value, '') + '%';

    exec sp_executesql
        @sql,
        N'@patron varchar(260)',
        @patron = @patron;
end
GO

create proc spinsertar_categories
    @category_id int output,
    @category_name varchar(255)
as
begin
    insert into categories(category_name)
    values (@category_name)
    set @category_id = SCOPE_IDENTITY()
end
go

create proc speditar_categories
    @category_id int,
    @category_name varchar(255)
as
begin
    update categories set 
        category_name = @category_name
    where category_id = @category_id
end
go

create proc speliminar_categories
    @category_id int
as
begin
    delete from categories
    where category_id = @category_id
end
go

create proc spmostrar_categories
as
begin
    select category_id, category_name 
    from categories
    order by category_id desc
end
go

create proc spbuscar_categories
    @textobuscar varchar(50)
as
begin
    select category_id, category_name 
    from categories
    where category_name like '%' + @textobuscar + '%'
    order by category_id desc
end
go

create proc spinsertar_products
    @product_id int output,
    @product_name varchar(200),
    @model_year smallint,
    @price decimal(10,2),
    @imagen image,
    @category_id int,
    @create_date date
as
begin
    insert into products(product_name, model_year, price, imagen, category_id, create_date)
    values (@product_name, @model_year, @price, @imagen, @category_id, @create_date)
    set @product_id = SCOPE_IDENTITY()
end
go

create proc speditar_products
    @product_id int,
    @product_name varchar(200),
    @model_year smallint,
    @price decimal(10,2),
    @imagen image,
    @category_id int,
    @create_date date
as
begin
    update products set 
        product_name = @product_name,
        model_year = @model_year,
        price = @price,
        imagen = @imagen,
        category_id = @category_id,
        create_date = @create_date
    where product_id = @product_id
end
go

create proc speliminar_products
    @product_id int
as
begin
    delete from products
    where product_id = @product_id
end
go

create proc spmostrar_products
as
begin
    select 
        p.product_id,
        p.product_name,
        p.model_year,
        p.price,
        p.imagen,
        p.category_id,
        p.create_date,
        c.category_name as Category
    from products p 
    inner join categories c on p.category_id = c.category_id
    order by p.product_id desc
end
go

create proc spbuscar_product_name
    @textbuscar varchar(50)
as
begin
    select 
        p.product_id,
        p.product_name,
        p.model_year,
        p.price,
        p.imagen,
        p.category_id,
        p.create_date,
        c.category_name as Category
    from products p 
    inner join categories c on p.category_id = c.category_id
where product_name like '%' + @textbuscar + '%'
order by p.product_id desc
end
go

insert into users (username, password, full_name, email, role)
values ('josue', '123456', 'Josue Claros Roca', 'josueclaros@gmail.com', 'admin')
go

create proc sp_login_user
    @username varchar(50),
    @password varchar(255)
as
begin
    select user_id, username, full_name, email, role, is_active
    from users 
    where username = @username AND password = @password and is_active = 1
end
go

create proc sp_create_user
    @username varchar(50),
    @password varchar(255),
    @full_name varchar(100),
    @email varchar(100),
    @role varchar(20),
    @user_id int output
as
begin
    insert into users (username, password, full_name, email, role)
    values (@username, @password, @full_name, @email, @role)
    set @user_id = scope_identity()
end
go

create proc sp_get_all_users
as
begin
    select user_id, username, full_name, email, role, is_active, 
           convert(varchar, created_at, 103) as created_date,
           convert(varchar, last_login, 103) as last_login_date
    from users
    order by username
end
go

create proc sp_update_user
    @user_id int,
    @username varchar(50),
    @full_name varchar(100),
    @email varchar(100),
    @role varchar(20),
    @is_active bit
as
begin
    update users set
        username = @username,
        full_name = @full_name,
        email = @email,
        role = @role,
        is_active = @is_active
    where user_id = @user_id
end
go

create proc sp_delete_user
    @user_id int
as
begin
    delete from users where user_id = @user_id
end
go

create proc sp_search_users
    @search_text varchar(100)
as
begin
    select user_id, username, full_name, email, role, is_active, 
           convert(varchar, created_at, 103) as created_date
    from users
    where username LIKE '%' + @search_text + '%' OR 
          full_name LIKE '%' + @search_text + '%' OR
          email LIKE '%' + @search_text + '%'
    order by username
end
go

create proc sp_change_password
    @user_id int,
    @new_password varchar(255)
as
begin
    update users set password = @new_password 
    where user_id = @user_id
end
go

create proc sp_verify_user
    @user_id int,
    @email varchar(100),
    @username varchar(100)
as
begin
    set nocount on;

    declare @NormalizedEmail varchar(100) = lower(ltrim(rtrim(@email)));
    declare @NormalizedName varchar(100) = lower(ltrim(rtrim(@username)));

    select 
        user_id,
        username,
        full_name,
        email,
        role,
        is_active
    from users
    where user_id = @user_id
      and lower(ltrim(rtrim(email))) = @NormalizedEmail
      and lower(ltrim(rtrim(full_name))) like '%' + @NormalizedName + '%'
      and is_active = 1;
end
GO

create proc spinsertar_order_items
    @order_item_id int = null output,
    @order_id int,
    @product_id int,
    @quantity int,
    @price decimal(10,2),
    @discount decimal(10,2)
as
begin
    set nocount on;
    insert into order_items(order_id, product_id, quantity, price, discount)
    values(@order_id, @product_id, @quantity, @price, @discount);

    set @order_item_id = scope_identity();
end
GO

create proc speliminar_order
    @order_id int
as
begin
    set nocount off;
    begin try
        begin transaction;

        delete from inventory
        where order_id = @order_id;

        delete from order_items
        where order_id = @order_id;

        delete from orders
        where order_id = @order_id;

        commit transaction;
    end try
    begin catch
        if @@trancount > 0 rollback transaction;
        raiserror('No se pudo eliminar el pedido.', 16, 1);
    end catch
end
GO

create proc spmostrar_order
as
begin
    select 
        o.order_id,
        u.full_name as usuario,
        (c.first_name + ' ' + c.last_name) as Cliente,
        o.order_date,
        sum((oi.quantity * oi.price) - oi.discount) as Total
    from order_items oi 
    inner join orders o on oi.order_id = o.order_id
    left join customers c on o.customer_id = c.customer_id
    left join users u on o.user_id = u.user_id
    group by o.order_id, u.full_name, c.first_name, c.last_name, o.order_date
    order by o.order_id desc
end
go

create proc spbuscar_order_fecha
    @textobuscar1 varchar(50),
    @textobuscar2 varchar(50)
as
begin
    select 
        o.order_id,
        u.full_name as usuario,
        (c.first_name + ' ' + c.last_name) as Cliente,
        o.order_date,
        SUM((oi.quantity * oi.price) - oi.discount) as Total
    from order_items oi 
    inner join orders o on oi.order_id = o.order_id
    left join customers c on o.customer_id = c.customer_id
    left join users u on o.user_id = u.user_id
    group by o.order_id, u.full_name, c.first_name, c.last_name, o.order_date
    having o.order_date between @textobuscar1 and @textobuscar2 
    order by o.order_id desc 
end
go

create proc spmostrar_order_items
    @textobuscar varchar(50)
as
begin
    select 
        oi.product_id,
        p.product_name as Producto,
        oi.quantity,
        oi.price,
        oi.discount,
        ((oi.price * oi.quantity) - oi.discount) as Subtotal
    from order_items oi 
    left join products p on oi.product_id = p.product_id
    where oi.order_id = @textobuscar
    order by oi.order_item_id
end
go

create proc sp_rpt_obtener_cabecera_pedido
    @pedido_id int
as
begin
    select 
        o.order_id,
        c.first_name + ' ' + c.last_name as nombre_cliente,
        c.email,
        c.phone,
        c.street + ', ' + isnull(c.city, '') + ', ' + isnull(c.state, '') as direccion_completa,
        u.full_name as usuario,
        o.order_date as fecha_pedido,
        (select sum(quantity * price - discount) from order_items where order_id = o.order_id) as total
    from orders o
    inner join customers c on o.customer_id = c.customer_id
    inner join users u on o.user_id = u.user_id
    where o.order_id = @pedido_id
end
go

create proc sp_rpt_obtener_detalle_pedido
    @pedido_id int
as
begin
    select 
        oi.product_id,
        p.product_name as producto,
        oi.quantity as cantidad,
        oi.price as precio,
        oi.discount as descuento,
        ((oi.price * oi.quantity) - oi.discount) as subtotal
    from order_items oi 
    left join products p on oi.product_id = p.product_id
    where oi.order_id = @pedido_id
    order by oi.order_item_id
end
go

create proc sp_consultar_clientes_para_reporte
as
begin
    select 
        customer_id,
        first_name + ' ' + last_name as nombre_completo,
        email, 
        phone as telefono
    from customers
    order by last_name, first_name
end
go

create proc sp_buscar_clientes_para_reporte
    @criterio_busqueda varchar(100)
as
begin
    select 
        customer_id,
        first_name + ' ' + last_name as nombre_completo,
        email, 
        phone as telefono
    from customers
    where first_name like '%' + @criterio_busqueda + '%'
    or last_name like '%' + @criterio_busqueda + '%'
    or email like '%' + @criterio_busqueda + '%'
    order by last_name, first_name
end
go

create proc sp_consultar_productos_para_reporte
as
begin
    select  
        p.product_id, 
        p.product_name as producto, 
        p.price as precio, 
        p.model_year as año_modelo,
        c.category_name as categoria
    from products p
    inner join categories c on p.category_id = c.category_id
    order by product_name
end
go

create proc sp_buscar_productos_para_reporte
    @criterio_busqueda varchar(100)
as
begin
    select 
        p.product_id, 
        p.product_name as producto, 
        p.price as precio,
        p.model_year as año_modelo,
        c.category_name as categoria
    from products p
    inner join categories c on p.category_id = c.category_id
    where (p.product_name like '%' + @criterio_busqueda + '%'
    or cast(p.product_id as varchar(20)) like '%' + @criterio_busqueda + '%')
    order by product_name
end
go


create proc spbuscar_products
    @textobuscar varchar(50)
as
begin
    select 
        p.product_id,
        p.product_name,
        p.model_year,
        p.price,
        p.imagen,
        p.category_id,
        p.create_date,
        c.category_name as Category
    from products p
    inner join categories c on p.category_id = c.category_id
    where p.product_name like '%' + @textobuscar + '%'
    order by p.product_id desc;
end
go

create proc sp_filtrar_productos_por_fecha
    @fecha_inicio datetime,
    @fecha_fin datetime
as
begin
    select 
        p.product_id,
        p.product_name,
        p.model_year,
        p.price,
        p.imagen,
        p.category_id,
        p.create_date,
        c.category_name as Category
    from products p
    inner join categories c on p.category_id = c.category_id
    where p.create_date >= @fecha_inicio
      and p.create_date <= @fecha_fin
    order by p.create_date desc, p.product_id desc;
end
go

create proc spinsertar_orders
    @customer_id int,
    @user_id int,
    @order_id int output
as
begin
    insert into orders(customer_id, user_id, order_date)
    values (@customer_id, @user_id, getdate())
    set @order_id = scope_identity()
end
go

create proc sp_obtener_estadisticas_dashboard
as
begin
    declare @totalClientes int = (select count(*) from customers)
    declare @totalProductos int = (select count(*) from products)
    declare @ventasMes decimal(10,2) = (
        select isnull(sum((oi.quantity * oi.price) - oi.discount), 0)
        from order_items oi
        inner join orders o on oi.order_id = o.order_id
        where month(o.order_date) = month(getdate()) 
        and year(o.order_date) = year(getdate())
    )
    declare @pedidosRecientes int = (
        select count(*) from orders 
        where datediff(day, order_date, getdate()) <= 7
    )
    declare @adminName varchar(100), @adminEmail varchar(100)
    select top 1 @adminName = full_name, @adminEmail = email 
    from users where role = 'admin'
    select 
        @totalClientes as total_clientes,
        @totalProductos as total_productos,
        @ventasMes as ventas_mes,
        @pedidosRecientes as pedidos_recientes,
        @adminName as admin_nombre,
        @adminEmail as admin_email
end
go

create proc sp_registrar_movimiento_inventario
    @product_id int,
    @quantity int,
    @movement_type varchar(20),
    @user_id int,
    @order_id int = null,
    @notes varchar(255) = null
as
begin
    set nocount on;

    if @quantity <= 0
    begin
        raiserror('La cantidad debe ser mayor a cero.', 16, 1);
        return;
    end;

    if @movement_type not in ('ENTRADA','SALIDA')
    begin
        raiserror('Tipo de movimiento no válido.', 16, 1);
        return;
    end;

    if @movement_type = 'SALIDA'
    begin
        declare @stock_actual int;
        select @stock_actual = isnull(sum(
            case when movement_type = 'ENTRADA' then quantity
                 when movement_type = 'SALIDA' then -quantity
                 else 0 end), 0)
        from inventory
        where product_id = @product_id;

        if @quantity > @stock_actual
        begin
            raiserror('Stock insuficiente para realizar la salida.', 16, 1);
            return;
        end;
    end;

    insert into inventory (product_id, quantity, movement_type, user_id, order_id, notes, movement_date)
    values (@product_id, @quantity, @movement_type, @user_id, @order_id, @notes, getdate());
end
GO

create proc sp_actualizar_stock_calculado
    @product_id int
as
begin
    set nocount on;
    select isnull(sum(
        case when movement_type = 'ENTRADA' then quantity
             when movement_type = 'SALIDA' then -quantity
             else 0 end), 0) as stock_actual
    from inventory
    where product_id = @product_id;
end
GO

create proc sp_obtener_stock
    @product_id int
as
begin
    select isnull(sum(
        case when movement_type = 'ENTRADA' then quantity 
             when movement_type = 'SALIDA' then -quantity 
             else 0 end), 0) as stock_actual
    from inventory
    where product_id = @product_id;
end
go

create procedure sp_reducir_stock
    @product_id int,
    @cantidad int
as
begin
    declare @user_id int = (select top 1 user_id from users where is_active = 1 order by user_id);
    exec sp_registrar_movimiento_inventario
        @product_id = @product_id,
        @quantity = @cantidad,
        @movement_type = 'SALIDA',
        @user_id = @user_id,
        @order_id = null,
        @notes = 'Salida manual mediante sp_reducir_stock';
end
GO

create procedure sp_aumentar_stock
    @product_id int,
    @cantidad int
as
begin
    declare @user_id int = (select top 1 user_id from users where is_active = 1 order by user_id);
    exec sp_registrar_movimiento_inventario
        @product_id = @product_id,
        @quantity = @cantidad,
        @movement_type = 'ENTRADA',
        @user_id = @user_id,
        @order_id = null,
        @notes = 'Entrada manual mediante sp_aumentar_stock';
end
GO

create procedure speliminar_order_items
    @order_id int
as
begin
    delete from order_items where order_id = @order_id
end
go

-- COMPROBACION FINAL
SELECT DB_NAME() AS BaseActual;
SELECT user_id, username, full_name, role, is_active FROM users;
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'inventory'
ORDER BY ORDINAL_POSITION;
SELECT COUNT(*) AS CantidadProcedimientos
FROM sys.procedures;
GO
