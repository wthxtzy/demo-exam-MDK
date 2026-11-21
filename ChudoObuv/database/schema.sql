-- ============================================================
--  БД «Чудо Обувь» — система оформления заказа обуви
--  СУБД: SQLite 3 (fallback instead of MS Access)
--  Схема приведена к третьей нормальной форме (3НФ).
-- ============================================================
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Roles (
    RoleId      INTEGER PRIMARY KEY,
    RoleName    TEXT    NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Users (
    UserId      INTEGER PRIMARY KEY,
    LastName    TEXT    NOT NULL,
    FirstName   TEXT    NOT NULL,
    MiddleName  TEXT    NOT NULL DEFAULT '',
    Login       TEXT    NOT NULL UNIQUE,
    RoleId      INTEGER NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId)
        REFERENCES Roles (RoleId) ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS Categories (
    CategoryId   INTEGER PRIMARY KEY,
    CategoryName TEXT    NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Subcategories (
    SubcategoryId   INTEGER PRIMARY KEY,
    CategoryId      INTEGER NOT NULL,
    SubcategoryName TEXT    NOT NULL,
    CONSTRAINT FK_Subcategories_Categories FOREIGN KEY (CategoryId)
        REFERENCES Categories (CategoryId) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT UQ_Subcategories UNIQUE (CategoryId, SubcategoryName)
);

CREATE TABLE IF NOT EXISTS Products (
    ProductId     INTEGER PRIMARY KEY,
    SubcategoryId INTEGER NOT NULL,
    ProductName   TEXT    NOT NULL,
    Manufacturer  TEXT    NOT NULL,
    Description   TEXT    NOT NULL DEFAULT '',
    Composition   TEXT    NOT NULL DEFAULT '',
    Price         REAL    NOT NULL CHECK (Price >= 0),
    ImageName     TEXT    NOT NULL DEFAULT '',
    CONSTRAINT FK_Products_Subcategories FOREIGN KEY (SubcategoryId)
        REFERENCES Subcategories (SubcategoryId) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT UQ_Products UNIQUE (ProductName, Manufacturer)
);

CREATE TABLE IF NOT EXISTS ProductSizes (
    ProductSizeId INTEGER PRIMARY KEY,
    ProductId     INTEGER NOT NULL,
    Size          TEXT    NOT NULL,
    Quantity      INTEGER NOT NULL DEFAULT 0 CHECK (Quantity >= 0),
    CONSTRAINT FK_ProductSizes_Products FOREIGN KEY (ProductId)
        REFERENCES Products (ProductId) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT UQ_ProductSizes UNIQUE (ProductId, Size)
);

CREATE TABLE IF NOT EXISTS Orders (
    OrderId      INTEGER PRIMARY KEY,
    OrderDate    TEXT    NOT NULL,              -- ISO: YYYY-MM-DD
    CustomerName TEXT    NOT NULL,
    UserId       INTEGER NULL,                  -- кто оформил (NULL — импорт)
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId)
        REFERENCES Users (UserId) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT CK_Orders_Date CHECK (OrderDate GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]')
);

CREATE TABLE IF NOT EXISTS OrderItems (
    OrderItemId    INTEGER PRIMARY KEY,
    OrderId        INTEGER NOT NULL,
    ProductSizeId  INTEGER NOT NULL,
    Quantity       INTEGER NOT NULL CHECK (Quantity > 0),
    UnitPrice      REAL    NOT NULL CHECK (UnitPrice >= 0),
    DiscountPercent REAL   NOT NULL DEFAULT 0 CHECK (DiscountPercent >= 0 AND DiscountPercent <= 100),
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId)
        REFERENCES Orders (OrderId) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_ProductSizes FOREIGN KEY (ProductSizeId)
        REFERENCES ProductSizes (ProductSizeId) ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_Users_RoleId ON Users (RoleId);
CREATE INDEX IF NOT EXISTS IX_Subcategories_CategoryId ON Subcategories (CategoryId);
CREATE INDEX IF NOT EXISTS IX_Products_SubcategoryId ON Products (SubcategoryId);
CREATE INDEX IF NOT EXISTS IX_ProductSizes_ProductId ON ProductSizes (ProductId);
CREATE INDEX IF NOT EXISTS IX_Orders_OrderDate ON Orders (OrderDate);
CREATE INDEX IF NOT EXISTS IX_OrderItems_OrderId ON OrderItems (OrderId);
CREATE INDEX IF NOT EXISTS IX_OrderItems_ProductSizeId ON OrderItems (ProductSizeId);
