
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/13/2024 23:24:48
-- Generated from EDMX file: C:\Users\Arvind\source\repos\OnlineBikeStore\OnlineBikeStore\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OnlineBikeStore];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[sales].[FK__order_ite__order__4D94879B]', 'F') IS NOT NULL
    ALTER TABLE [sales].[order_items] DROP CONSTRAINT [FK__order_ite__order__4D94879B];
GO
IF OBJECT_ID(N'[sales].[FK__order_ite__produ__4E88ABD4]', 'F') IS NOT NULL
    ALTER TABLE [sales].[order_items] DROP CONSTRAINT [FK__order_ite__produ__4E88ABD4];
GO
IF OBJECT_ID(N'[sales].[FK__orders__customer__47DBAE45]', 'F') IS NOT NULL
    ALTER TABLE [sales].[orders] DROP CONSTRAINT [FK__orders__customer__47DBAE45];
GO
IF OBJECT_ID(N'[production].[FK__products__brand___3C69FB99]', 'F') IS NOT NULL
    ALTER TABLE [production].[products] DROP CONSTRAINT [FK__products__brand___3C69FB99];
GO
IF OBJECT_ID(N'[production].[FK__products__catego__3B75D760]', 'F') IS NOT NULL
    ALTER TABLE [production].[products] DROP CONSTRAINT [FK__products__catego__3B75D760];
GO
IF OBJECT_ID(N'[sales].[FK__staffs__manager___13F1F5EB]', 'F') IS NOT NULL
    ALTER TABLE [sales].[staffs] DROP CONSTRAINT [FK__staffs__manager___13F1F5EB];
GO
IF OBJECT_ID(N'[sales].[FK__staffs__manager___2334397B]', 'F') IS NOT NULL
    ALTER TABLE [sales].[staffs] DROP CONSTRAINT [FK__staffs__manager___2334397B];
GO
IF OBJECT_ID(N'[sales].[FK__staffs__manager___24285DB4]', 'F') IS NOT NULL
    ALTER TABLE [sales].[staffs] DROP CONSTRAINT [FK__staffs__manager___24285DB4];
GO
IF OBJECT_ID(N'[sales].[FK__staffs__manager___44FF419A]', 'F') IS NOT NULL
    ALTER TABLE [sales].[staffs] DROP CONSTRAINT [FK__staffs__manager___44FF419A];
GO
IF OBJECT_ID(N'[sales].[FK__staffs__store_id__440B1D61]', 'F') IS NOT NULL
    ALTER TABLE [sales].[staffs] DROP CONSTRAINT [FK__staffs__store_id__440B1D61];
GO
IF OBJECT_ID(N'[production].[FK__stocks__product___52593CB8]', 'F') IS NOT NULL
    ALTER TABLE [production].[stocks] DROP CONSTRAINT [FK__stocks__product___52593CB8];
GO
IF OBJECT_ID(N'[production].[FK__stocks__store_id__5165187F]', 'F') IS NOT NULL
    ALTER TABLE [production].[stocks] DROP CONSTRAINT [FK__stocks__store_id__5165187F];
GO
IF OBJECT_ID(N'[dbo].[FK_feedback_products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[feedback] DROP CONSTRAINT [FK_feedback_products];
GO
IF OBJECT_ID(N'[dbo].[FK_feedback_users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[feedback] DROP CONSTRAINT [FK_feedback_users];
GO
IF OBJECT_ID(N'[dbo].[FK_query_products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[query] DROP CONSTRAINT [FK_query_products];
GO
IF OBJECT_ID(N'[dbo].[FK_query_users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[query] DROP CONSTRAINT [FK_query_users];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_1_customers:uid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[userCart] DROP CONSTRAINT [FK_Table_1_customers:uid];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_1_products:pid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[userCart] DROP CONSTRAINT [FK_Table_1_products:pid];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRole_customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[user_role] DROP CONSTRAINT [FK_UserRole_customers];
GO
IF OBJECT_ID(N'[dbo].[FK_wishlist_products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[wishlist] DROP CONSTRAINT [FK_wishlist_products];
GO
IF OBJECT_ID(N'[dbo].[FK_wishlist_users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[wishlist] DROP CONSTRAINT [FK_wishlist_users];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[feedback]', 'U') IS NOT NULL
    DROP TABLE [dbo].[feedback];
GO
IF OBJECT_ID(N'[dbo].[query]', 'U') IS NOT NULL
    DROP TABLE [dbo].[query];
GO
IF OBJECT_ID(N'[dbo].[user_role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[user_role];
GO
IF OBJECT_ID(N'[dbo].[userCart]', 'U') IS NOT NULL
    DROP TABLE [dbo].[userCart];
GO
IF OBJECT_ID(N'[dbo].[users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[users];
GO
IF OBJECT_ID(N'[dbo].[wishlist]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wishlist];
GO
IF OBJECT_ID(N'[production].[brands]', 'U') IS NOT NULL
    DROP TABLE [production].[brands];
GO
IF OBJECT_ID(N'[production].[categories]', 'U') IS NOT NULL
    DROP TABLE [production].[categories];
GO
IF OBJECT_ID(N'[production].[products]', 'U') IS NOT NULL
    DROP TABLE [production].[products];
GO
IF OBJECT_ID(N'[production].[stocks]', 'U') IS NOT NULL
    DROP TABLE [production].[stocks];
GO
IF OBJECT_ID(N'[sales].[order_items]', 'U') IS NOT NULL
    DROP TABLE [sales].[order_items];
GO
IF OBJECT_ID(N'[sales].[orders]', 'U') IS NOT NULL
    DROP TABLE [sales].[orders];
GO
IF OBJECT_ID(N'[sales].[staffs]', 'U') IS NOT NULL
    DROP TABLE [sales].[staffs];
GO
IF OBJECT_ID(N'[sales].[stores]', 'U') IS NOT NULL
    DROP TABLE [sales].[stores];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'feedbacks'
CREATE TABLE [dbo].[feedbacks] (
    [feedback_id] int  NOT NULL,
    [customer_id] int  NOT NULL,
    [product_id] int  NOT NULL,
    [date] datetime  NOT NULL,
    [image_url] varchar(500)  NULL,
    [feedback_text] varchar(200)  NULL,
    [ratingValue] int  NOT NULL
);
GO

-- Creating table 'queries'
CREATE TABLE [dbo].[queries] (
    [qus_id] int IDENTITY(1,1) NOT NULL,
    [qus_text] nvarchar(255)  NOT NULL,
    [c_id] int  NOT NULL,
    [p_id] int  NOT NULL,
    [date] datetime  NOT NULL,
    [ans_text] nvarchar(255)  NULL
);
GO

-- Creating table 'user_role'
CREATE TABLE [dbo].[user_role] (
    [role_id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [role] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'userCarts'
CREATE TABLE [dbo].[userCarts] (
    [cart_id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [product_id] int  NOT NULL
);
GO

-- Creating table 'users'
CREATE TABLE [dbo].[users] (
    [user_id] int  NOT NULL,
    [first_name] varchar(255)  NOT NULL,
    [last_name] varchar(255)  NOT NULL,
    [phone] varchar(25)  NULL,
    [email] varchar(255)  NOT NULL,
    [street] varchar(255)  NULL,
    [city] varchar(50)  NULL,
    [state] varchar(25)  NULL,
    [zip_code] varchar(10)  NULL,
    [password] varchar(25)  NOT NULL
);
GO

-- Creating table 'wishlists'
CREATE TABLE [dbo].[wishlists] (
    [id] int IDENTITY(1,1) NOT NULL,
    [p_id] int  NOT NULL,
    [u_id] int  NOT NULL,
    [p_name] varchar(255)  NULL,
    [p_price] decimal(10,0)  NULL,
    [url] nvarchar(1000)  NULL
);
GO

-- Creating table 'brands'
CREATE TABLE [dbo].[brands] (
    [brand_id] int IDENTITY(1,1) NOT NULL,
    [brand_name] varchar(255)  NOT NULL,
    [brand_image] varchar(255)  NULL
);
GO

-- Creating table 'categories'
CREATE TABLE [dbo].[categories] (
    [category_id] int IDENTITY(1,1) NOT NULL,
    [category_name] varchar(255)  NOT NULL,
    [category_image] varchar(255)  NULL
);
GO

-- Creating table 'products'
CREATE TABLE [dbo].[products] (
    [product_id] int IDENTITY(1,1) NOT NULL,
    [product_name] varchar(255)  NOT NULL,
    [brand_id] int  NOT NULL,
    [category_id] int  NOT NULL,
    [model_year] smallint  NOT NULL,
    [list_price] decimal(10,2)  NOT NULL,
    [description] nvarchar(1000)  NULL,
    [url] nvarchar(1000)  NULL,
    [product_type] varchar(20)  NOT NULL
);
GO

-- Creating table 'stocks'
CREATE TABLE [dbo].[stocks] (
    [store_id] int  NOT NULL,
    [product_id] int  NOT NULL,
    [quantity] int  NULL
);
GO

-- Creating table 'order_items'
CREATE TABLE [dbo].[order_items] (
    [order_id] int  NOT NULL,
    [item_id] int  NOT NULL,
    [product_id] int  NOT NULL,
    [quantity] int  NOT NULL,
    [list_price] decimal(10,2)  NOT NULL,
    [discount] decimal(4,2)  NOT NULL
);
GO

-- Creating table 'orders'
CREATE TABLE [dbo].[orders] (
    [order_id] int IDENTITY(1,1) NOT NULL,
    [customer_id] int  NULL,
    [order_status] tinyint  NOT NULL,
    [order_date] datetime  NOT NULL,
    [required_date] datetime  NOT NULL,
    [shipped_date] datetime  NULL,
    [store_id] int  NOT NULL,
    [staff_id] int  NOT NULL
);
GO

-- Creating table 'staffs'
CREATE TABLE [dbo].[staffs] (
    [staff_id] int IDENTITY(1,1) NOT NULL,
    [first_name] varchar(50)  NOT NULL,
    [last_name] varchar(50)  NOT NULL,
    [email] varchar(255)  NOT NULL,
    [phone] varchar(25)  NULL,
    [active] tinyint  NOT NULL,
    [store_id] int  NOT NULL,
    [manager_id] int  NULL
);
GO

-- Creating table 'stores'
CREATE TABLE [dbo].[stores] (
    [store_id] int IDENTITY(1,1) NOT NULL,
    [store_name] varchar(255)  NOT NULL,
    [phone] varchar(25)  NULL,
    [email] varchar(255)  NULL,
    [street] varchar(255)  NULL,
    [city] varchar(255)  NULL,
    [state] varchar(10)  NULL,
    [zip_code] varchar(8)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [feedback_id] in table 'feedbacks'
ALTER TABLE [dbo].[feedbacks]
ADD CONSTRAINT [PK_feedbacks]
    PRIMARY KEY CLUSTERED ([feedback_id] ASC);
GO

-- Creating primary key on [qus_id] in table 'queries'
ALTER TABLE [dbo].[queries]
ADD CONSTRAINT [PK_queries]
    PRIMARY KEY CLUSTERED ([qus_id] ASC);
GO

-- Creating primary key on [role_id] in table 'user_role'
ALTER TABLE [dbo].[user_role]
ADD CONSTRAINT [PK_user_role]
    PRIMARY KEY CLUSTERED ([role_id] ASC);
GO

-- Creating primary key on [cart_id] in table 'userCarts'
ALTER TABLE [dbo].[userCarts]
ADD CONSTRAINT [PK_userCarts]
    PRIMARY KEY CLUSTERED ([cart_id] ASC);
GO

-- Creating primary key on [user_id] in table 'users'
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [PK_users]
    PRIMARY KEY CLUSTERED ([user_id] ASC);
GO

-- Creating primary key on [id] in table 'wishlists'
ALTER TABLE [dbo].[wishlists]
ADD CONSTRAINT [PK_wishlists]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [brand_id] in table 'brands'
ALTER TABLE [dbo].[brands]
ADD CONSTRAINT [PK_brands]
    PRIMARY KEY CLUSTERED ([brand_id] ASC);
GO

-- Creating primary key on [category_id] in table 'categories'
ALTER TABLE [dbo].[categories]
ADD CONSTRAINT [PK_categories]
    PRIMARY KEY CLUSTERED ([category_id] ASC);
GO

-- Creating primary key on [product_id] in table 'products'
ALTER TABLE [dbo].[products]
ADD CONSTRAINT [PK_products]
    PRIMARY KEY CLUSTERED ([product_id] ASC);
GO

-- Creating primary key on [store_id], [product_id] in table 'stocks'
ALTER TABLE [dbo].[stocks]
ADD CONSTRAINT [PK_stocks]
    PRIMARY KEY CLUSTERED ([store_id], [product_id] ASC);
GO

-- Creating primary key on [order_id], [item_id] in table 'order_items'
ALTER TABLE [dbo].[order_items]
ADD CONSTRAINT [PK_order_items]
    PRIMARY KEY CLUSTERED ([order_id], [item_id] ASC);
GO

-- Creating primary key on [order_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [PK_orders]
    PRIMARY KEY CLUSTERED ([order_id] ASC);
GO

-- Creating primary key on [staff_id] in table 'staffs'
ALTER TABLE [dbo].[staffs]
ADD CONSTRAINT [PK_staffs]
    PRIMARY KEY CLUSTERED ([staff_id] ASC);
GO

-- Creating primary key on [store_id] in table 'stores'
ALTER TABLE [dbo].[stores]
ADD CONSTRAINT [PK_stores]
    PRIMARY KEY CLUSTERED ([store_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [product_id] in table 'feedbacks'
ALTER TABLE [dbo].[feedbacks]
ADD CONSTRAINT [FK_feedback_products]
    FOREIGN KEY ([product_id])
    REFERENCES [dbo].[products]
        ([product_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_feedback_products'
CREATE INDEX [IX_FK_feedback_products]
ON [dbo].[feedbacks]
    ([product_id]);
GO

-- Creating foreign key on [customer_id] in table 'feedbacks'
ALTER TABLE [dbo].[feedbacks]
ADD CONSTRAINT [FK_feedback_users]
    FOREIGN KEY ([customer_id])
    REFERENCES [dbo].[users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_feedback_users'
CREATE INDEX [IX_FK_feedback_users]
ON [dbo].[feedbacks]
    ([customer_id]);
GO

-- Creating foreign key on [p_id] in table 'queries'
ALTER TABLE [dbo].[queries]
ADD CONSTRAINT [FK_query_products]
    FOREIGN KEY ([p_id])
    REFERENCES [dbo].[products]
        ([product_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_query_products'
CREATE INDEX [IX_FK_query_products]
ON [dbo].[queries]
    ([p_id]);
GO

-- Creating foreign key on [qus_id] in table 'queries'
ALTER TABLE [dbo].[queries]
ADD CONSTRAINT [FK_query_users]
    FOREIGN KEY ([qus_id])
    REFERENCES [dbo].[users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [user_id] in table 'user_role'
ALTER TABLE [dbo].[user_role]
ADD CONSTRAINT [FK_UserRole_customers]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole_customers'
CREATE INDEX [IX_FK_UserRole_customers]
ON [dbo].[user_role]
    ([user_id]);
GO

-- Creating foreign key on [user_id] in table 'userCarts'
ALTER TABLE [dbo].[userCarts]
ADD CONSTRAINT [FK_Table_1_customers_uid]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_1_customers_uid'
CREATE INDEX [IX_FK_Table_1_customers_uid]
ON [dbo].[userCarts]
    ([user_id]);
GO

-- Creating foreign key on [product_id] in table 'userCarts'
ALTER TABLE [dbo].[userCarts]
ADD CONSTRAINT [FK_Table_1_products_pid]
    FOREIGN KEY ([product_id])
    REFERENCES [dbo].[products]
        ([product_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_1_products_pid'
CREATE INDEX [IX_FK_Table_1_products_pid]
ON [dbo].[userCarts]
    ([product_id]);
GO

-- Creating foreign key on [customer_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK__orders__customer__47DBAE45]
    FOREIGN KEY ([customer_id])
    REFERENCES [dbo].[users]
        ([user_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__orders__customer__47DBAE45'
CREATE INDEX [IX_FK__orders__customer__47DBAE45]
ON [dbo].[orders]
    ([customer_id]);
GO

-- Creating foreign key on [u_id] in table 'wishlists'
ALTER TABLE [dbo].[wishlists]
ADD CONSTRAINT [FK_wishlist_users]
    FOREIGN KEY ([u_id])
    REFERENCES [dbo].[users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wishlist_users'
CREATE INDEX [IX_FK_wishlist_users]
ON [dbo].[wishlists]
    ([u_id]);
GO

-- Creating foreign key on [p_id] in table 'wishlists'
ALTER TABLE [dbo].[wishlists]
ADD CONSTRAINT [FK_wishlist_products]
    FOREIGN KEY ([p_id])
    REFERENCES [dbo].[products]
        ([product_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wishlist_products'
CREATE INDEX [IX_FK_wishlist_products]
ON [dbo].[wishlists]
    ([p_id]);
GO

-- Creating foreign key on [brand_id] in table 'products'
ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK__products__brand___3C69FB99]
    FOREIGN KEY ([brand_id])
    REFERENCES [dbo].[brands]
        ([brand_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__products__brand___3C69FB99'
CREATE INDEX [IX_FK__products__brand___3C69FB99]
ON [dbo].[products]
    ([brand_id]);
GO

-- Creating foreign key on [category_id] in table 'products'
ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK__products__catego__3B75D760]
    FOREIGN KEY ([category_id])
    REFERENCES [dbo].[categories]
        ([category_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__products__catego__3B75D760'
CREATE INDEX [IX_FK__products__catego__3B75D760]
ON [dbo].[products]
    ([category_id]);
GO

-- Creating foreign key on [product_id] in table 'order_items'
ALTER TABLE [dbo].[order_items]
ADD CONSTRAINT [FK__order_ite__produ__4E88ABD4]
    FOREIGN KEY ([product_id])
    REFERENCES [dbo].[products]
        ([product_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__order_ite__produ__4E88ABD4'
CREATE INDEX [IX_FK__order_ite__produ__4E88ABD4]
ON [dbo].[order_items]
    ([product_id]);
GO

-- Creating foreign key on [product_id] in table 'stocks'
ALTER TABLE [dbo].[stocks]
ADD CONSTRAINT [FK__stocks__product___52593CB8]
    FOREIGN KEY ([product_id])
    REFERENCES [dbo].[products]
        ([product_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__stocks__product___52593CB8'
CREATE INDEX [IX_FK__stocks__product___52593CB8]
ON [dbo].[stocks]
    ([product_id]);
GO

-- Creating foreign key on [store_id] in table 'stocks'
ALTER TABLE [dbo].[stocks]
ADD CONSTRAINT [FK__stocks__store_id__5165187F]
    FOREIGN KEY ([store_id])
    REFERENCES [dbo].[stores]
        ([store_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [order_id] in table 'order_items'
ALTER TABLE [dbo].[order_items]
ADD CONSTRAINT [FK__order_ite__order__4D94879B]
    FOREIGN KEY ([order_id])
    REFERENCES [dbo].[orders]
        ([order_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [staff_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK__orders__staff_id__12FDD1B2]
    FOREIGN KEY ([staff_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__orders__staff_id__12FDD1B2'
CREATE INDEX [IX_FK__orders__staff_id__12FDD1B2]
ON [dbo].[orders]
    ([staff_id]);
GO

-- Creating foreign key on [staff_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK__orders__staff_id__214BF109]
    FOREIGN KEY ([staff_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__orders__staff_id__214BF109'
CREATE INDEX [IX_FK__orders__staff_id__214BF109]
ON [dbo].[orders]
    ([staff_id]);
GO

-- Creating foreign key on [staff_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK__orders__staff_id__22401542]
    FOREIGN KEY ([staff_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__orders__staff_id__22401542'
CREATE INDEX [IX_FK__orders__staff_id__22401542]
ON [dbo].[orders]
    ([staff_id]);
GO

-- Creating foreign key on [staff_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK__orders__staff_id__49C3F6B7]
    FOREIGN KEY ([staff_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__orders__staff_id__49C3F6B7'
CREATE INDEX [IX_FK__orders__staff_id__49C3F6B7]
ON [dbo].[orders]
    ([staff_id]);
GO

-- Creating foreign key on [store_id] in table 'orders'
ALTER TABLE [dbo].[orders]
ADD CONSTRAINT [FK__orders__store_id__48CFD27E]
    FOREIGN KEY ([store_id])
    REFERENCES [dbo].[stores]
        ([store_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__orders__store_id__48CFD27E'
CREATE INDEX [IX_FK__orders__store_id__48CFD27E]
ON [dbo].[orders]
    ([store_id]);
GO

-- Creating foreign key on [manager_id] in table 'staffs'
ALTER TABLE [dbo].[staffs]
ADD CONSTRAINT [FK__staffs__manager___13F1F5EB]
    FOREIGN KEY ([manager_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__staffs__manager___13F1F5EB'
CREATE INDEX [IX_FK__staffs__manager___13F1F5EB]
ON [dbo].[staffs]
    ([manager_id]);
GO

-- Creating foreign key on [manager_id] in table 'staffs'
ALTER TABLE [dbo].[staffs]
ADD CONSTRAINT [FK__staffs__manager___2334397B]
    FOREIGN KEY ([manager_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__staffs__manager___2334397B'
CREATE INDEX [IX_FK__staffs__manager___2334397B]
ON [dbo].[staffs]
    ([manager_id]);
GO

-- Creating foreign key on [manager_id] in table 'staffs'
ALTER TABLE [dbo].[staffs]
ADD CONSTRAINT [FK__staffs__manager___24285DB4]
    FOREIGN KEY ([manager_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__staffs__manager___24285DB4'
CREATE INDEX [IX_FK__staffs__manager___24285DB4]
ON [dbo].[staffs]
    ([manager_id]);
GO

-- Creating foreign key on [manager_id] in table 'staffs'
ALTER TABLE [dbo].[staffs]
ADD CONSTRAINT [FK__staffs__manager___44FF419A]
    FOREIGN KEY ([manager_id])
    REFERENCES [dbo].[staffs]
        ([staff_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__staffs__manager___44FF419A'
CREATE INDEX [IX_FK__staffs__manager___44FF419A]
ON [dbo].[staffs]
    ([manager_id]);
GO

-- Creating foreign key on [store_id] in table 'staffs'
ALTER TABLE [dbo].[staffs]
ADD CONSTRAINT [FK__staffs__store_id__440B1D61]
    FOREIGN KEY ([store_id])
    REFERENCES [dbo].[stores]
        ([store_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__staffs__store_id__440B1D61'
CREATE INDEX [IX_FK__staffs__store_id__440B1D61]
ON [dbo].[staffs]
    ([store_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------