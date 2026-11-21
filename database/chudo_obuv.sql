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


-- ============================================================
--  Тестовые данные «Чудо Обувь» (импорт из xlsx)
--  Выполнять после schema.sql на пустой базе.
-- ============================================================
PRAGMA foreign_keys = ON;
BEGIN TRANSACTION;

-- Роли
INSERT INTO Roles (RoleId, RoleName) VALUES (1, 'Администратор');
INSERT INTO Roles (RoleId, RoleName) VALUES (2, 'Менеджер');
INSERT INTO Roles (RoleId, RoleName) VALUES (3, 'Авторизованный пользователь');

-- Пользователи (20)
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (1, 'Иванов', 'Иван', 'Сергеевич', 'isivanov', 1);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (2, 'Петров', 'Пeтр', 'Алексеевич', 'papetrov', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (3, 'Сидорова', 'Анна', 'Дмитриевна', 'asidorova', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (4, 'Кузнецова', 'Екатерина', 'Андреевна', 'ekuznetsova', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (5, 'Смирнов', 'Дмитрий', 'Владимирович', 'dsmirnov', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (6, 'Попов', 'Алексей', 'Николаевич', 'apopov', 1);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (7, 'Васильева', 'Ольга', 'Игоревна', 'ovasileva', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (8, 'Соколов', 'Павел', 'Викторович', 'psokolov', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (9, 'Михайлова', 'Татьяна', 'Юрьевна', 'tmikhailova', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (10, 'Новикова', 'Наталья', 'Сергеевна', 'nnovikova', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (11, 'Фёдоров', 'Андрей', 'Максимович', 'afedorov', 1);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (12, 'Морозов', 'Сергей', 'Павлович', 'smorozov', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (13, 'Волкова', 'Юлия', 'Александровна', 'jvolkova', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (14, 'Алексеев', 'Артём', 'Евгеньевич', 'aalekseev', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (15, 'Лебедева', 'Ирина', 'Владимировна', 'ilebedeva', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (16, 'Егоров', 'Никита', 'Денисович', 'negorov', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (17, 'Павлова', 'Елена', 'Петровна', 'epavlova', 3);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (18, 'Козлов', 'Роман', 'Олегович', 'rkozlov', 1);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (19, 'Степанова', 'Виктория', 'Романовна', 'vstepanova', 2);
INSERT INTO Users (UserId, LastName, FirstName, MiddleName, Login, RoleId) VALUES (20, 'Николаев', 'Владимир', 'Иванович', 'vnikolaev', 3);

-- Категории
INSERT INTO Categories (CategoryId, CategoryName) VALUES (1, 'Детская обувь');
INSERT INTO Categories (CategoryId, CategoryName) VALUES (2, 'Женская обувь');
INSERT INTO Categories (CategoryId, CategoryName) VALUES (3, 'Мужская обувь');

-- Подкатегории
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (1, 1, 'Кроссовки');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (2, 1, 'Сапоги');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (3, 1, 'Босоножки');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (4, 1, 'Туфли');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (5, 2, 'Сапоги');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (6, 2, 'Босоножки');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (7, 2, 'Туфли');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (8, 2, 'Кроссовки');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (9, 3, 'Кроссовки');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (10, 3, 'Туфли');
INSERT INTO Subcategories (SubcategoryId, CategoryId, SubcategoryName) VALUES (11, 3, 'Ботинки');

-- Товары (31 модель)
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (1, 1, 'Кроссовки детские «Звёздочка» экокожа', 'Малыш-Спорт', 'Кроссовки детские «Звёздочка» экокожа. Производитель: ООО «Малыш-Спорт», г. Смоленск', 'Верх — экокожа (полиуретан на хлопковой основе), подкладка — хлопок 100%, подошва — ПВХ', 2190, 'IMG_KS_195217.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (2, 1, 'Кроссовки детские «Гонщик» экокожа', 'Топ-Топ', 'Кроссовки детские «Гонщик» экокожа. Производитель: Фабрика «Топ-Топ», г. Киров', 'Верх — экокожа (микрофибра), подкладка — текстиль сетчатый (полиэстер), подошва — термопластичная резина (ТЭП)', 2490, 'IMG_KS_195213.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (3, 1, 'Кроссовки детские «Радуга» экокожа перфорированная', 'Лапушка', 'Кроссовки детские «Радуга» экокожа перфорированная. Производитель: ИП Смирнова Л.В. (бренд «Лапушка»), г. Вологда', 'Верх — экокожа перфорированная, вставки — нейлон, стелька — ортопедическая с латексной подушкой, подошва — ЭВА', 1990, 'IMG_KS_195222.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (4, 2, 'Сапожки детские «Снежинка» зимние на меху', 'Тёплые ножки', 'Сапожки детские «Снежинка» зимние на меху. Производитель: ООО «Тёплые ножки», г. Новосибирск', 'Верх — экокожа морозостойкая, утеплитель — искусственный мех (акрил 70%, полиэстер 30%), подкладка — флис, подошва — термоэластопласт (ТЭП)', 3490, 'IMG_KB_185501.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (5, 2, 'Сапожки детские «Медвежонок» демисезонные непромокаемые', 'Северята', 'Сапожки детские «Медвежонок» демисезонные непромокаемые. Производитель: Компания «Северята», г. Архангельск', 'Верх — плащёвая ткань с водоотталкивающей пропиткой (полиэстер 100%), отделка — экокожа, подкладка — микрофибра, мембрана — AirTex, подошва — резина', 3190, 'IMG_KB_164816.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (6, 2, 'Сапожки детские «Принцесса» кожаные', 'Весна', 'Сапожки детские «Принцесса» кожаные. Производитель: Мастерская «Весна», г. Тула', 'Верх — натуральная кожа (КРС), подкладка — шерсть натуральная (70%), байка (30%), стелька — войлочная, подошва — полиуретан', 4290, 'IMG_KB_164811.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (7, 3, 'Сандалии детские «Пчёлка» ортопедические летние', 'Здоровый шаг', 'Сандалии детские «Пчёлка» ортопедические летние. Производитель: ООО «Здоровый шаг», г. Воронеж', 'Верх — нубук натуральный, подкладка — кожа растительного дубления, стелька — анатомическая с супинатором (латекс + пробка), подошва — каучук', 2790, 'IMG_KSd_195207.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (8, 3, 'Сандалии детские «Морячок» текстильные с фиксатором', 'Карапуз', 'Сандалии детские «Морячок» текстильные с фиксатором. Производитель: ИП Кузнецов А.Д. (бренд «Карапуз»), г. Краснодар', 'Верх — джинсовая ткань (хлопок 100%), окантовка — искусственная кожа, подкладка — хлопок, подошва — ЭВА лёгкая', 1690, 'IMG_KSd_185508.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (9, 4, 'Туфли детские «Бантик» лаковые праздничные', 'Маленький франт', 'Туфли детские «Бантик» лаковые праздничные. Производитель: Дом обуви «Маленький франт», г. Москва', 'Верх — искусственная лаковая кожа (ПВХ), носок усиленный — термопластичный полиуретан, подкладка — кожа искусственная дышащая, стелька — анатомическая с амортизацией, подошва — полиуретан', 2390, 'IMG_KSho_185512.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (10, 5, 'Сапоги зимние натуральная кожа', 'Барбари', 'Сапоги женские зимние, нескользящая, устойчивая подошва, натуральный мех. Производитель: Дом обуви «Барбари», г. Новороссийск', 'Верх — натуральная кожа, подкладка — евромех, натуральный мех, текстильный утеплитель, подошва — резина', 25000, 'IMG_WB_170905.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (11, 5, 'Сапоги осенние натуральная кожа', 'Барбари', 'Сапоги женские демисезонные из высококачественной натуральной кожи. Производитель: Дом обуви «Барбари», г. Новороссийск', 'Верх — натуральная кожа, подкладка — текстильный утеплитель, стелька — текстиль, подошва — ТПР', 15000, 'IMG_WB_170859.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (12, 5, 'Сапоги трубы демисезонные замшевые', 'Твой комфорт', 'Сапоги женские демисезонные замшевые. Производитель: ОАО «Твой комфорт», г. Ярославль', 'Верх — натуральная замша, утепленные, тракторная подошва, стелька — байка, подошва — резина', 17000, 'IMG_WB_170906.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (13, 6, 'Босоножки «Песчаный берег» коричневые', 'Стиль и комфорт', 'Коричневая кожаная модель с открытым носком, боковой пряжкой и закрытым каблуком. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург', 'Верх — 100% козья кожа, подкладка — 100% кожа; подошва: 100% кожа', 7500, 'IMG_WSd_174332.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (14, 6, 'Женские босоножки «Черный кофе» со скульптурным каблуком', 'Кожевенные традиции', 'Открытый носок и ремешок со сборной пряжкой, низкий каблук и резиновая подошва гарантируют комфорт. Производитель: ЗАО «Кожевенные традиции», г. Вятка', 'Верх — кожа собственного производств, подкладка — кожа; подошва: резина', 5600, 'IMG_WSd_185156.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (15, 6, 'Босоножки женские «Летний бриз»', 'Модный силуэт', 'Босоножки женские кожаные белые, регулируемый ремешок, закрытый каблук и кожаная подошва. Производитель: ООО «Модный силуэт», г. Кострома', 'Верх — натуральная кожа, подкладка — натуральная кожа, подошва — кожа', 7130, 'IMG_WSd_185551.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (16, 7, 'Демисезонные туфли натуральная кожа', 'Модный силуэт', 'Комфортные лаконичные лодочки, классический дизайна с современными акцентами. Производитель: ООО «Модный силуэт», г. Кострома', 'Верх — натуральная овечья кожа, подкладка и стелька — натуральная овечья кожа, подошва — резина', 12340, 'IMG_WSho_174409.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (17, 7, 'Туфли женские натуральная кожа с металлическими вставками', 'Кожевенные традиции', 'Стильные туфли для весеннего и летнего сезона. Производитель: ЗАО «Кожевенные традиции», г. Вятка', 'Верх — натуральная телячьей кожа, металлическая пряжка, подкладка и стелька— натуральная овечья кожа, подошва — резина', 7757, 'IMG_WSho_185202.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (18, 7, 'Туфли классические лодочки натуральная кожа', 'Стиль и комфорт', 'Модель из натуральной кожи, классический острый носок и изящная шпилька. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург', 'Верх — натуральная кожа, подкладка и стелька — натуральная кожа, подошва — тунит', 7550, 'IMG_WSho_185545.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (19, 8, 'Кроссовки белые с оранжевым и красным', 'Топ-Топ', 'Многослойный верх из кожи и текстиля. Производитель: Фабрика «Топ-Топ», г. Киров', 'Верх — 70% кожа, 30% текстиль, подкладка — текстиль, подошва — резина', 9450, 'IMG_WS_174110.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (20, 8, 'Кроссовки летние кожаные', 'Топ-Топ', 'Уверенность на каждом шагу. Материал обеспечивает долговечность и комфорт при носке. Производитель: Фабрика «Топ-Топ», г. Киров', 'Верх — 87% кожа, 11% текстиль, 2% синтетика; подкладка: 100% текстиль; подошва: резина, пластик', 8565, 'IMG_WS_185528.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (21, 8, 'Кроссовки кожаные', 'Стиль и комфорт', 'Кроссовки женские из натуральной кожи. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург', 'Верх — натуральная кожа, подкладка — кожа, подошва — полиуретан', 8765, 'IMG_WS_185535.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (22, 8, 'Кроссовки «Стиль и комфорт»', 'Стиль и комфорт', 'Кроссовки женские из натуральной кожи и синтетических материалов, сетчатые вставки. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург', 'Верх — 70% кожа, 20% полиамид, 10% полиуретан; подкладка — текстиль, подошва — ТПУ', 7795, 'IMG_WS_185538.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (23, 9, 'Кроссовки для бега «Драйв»', 'Азимут', 'Сочетание стиля, комфорта и современных технологий. Производитель: Фабрика «Азимут», г. Санкт-Петербург', 'Верх — сетка, подкладка — текстиль, подошва — резина', 6550, 'IMG_MS_185531.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (24, 9, 'Кроссовки «Азимут Бег»', 'Азимут', 'Универсальный выбор для повседневного использования. Производитель: Фабрика «Азимут», г. Санкт-Петербург', 'Верх — 6% кожа, 29% текстиль, 15% полимерные материалы; подкладка — 100% текстиль; подошва — полимерные материалы, резина', 7890, 'IMG_MS_185532.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (25, 9, 'Кроссовки', 'Топ-Топ', 'Кроссовки для мужчин, идеальное сочетание стиля и комфорта. Производитель: Фабрика «Топ-Топ», г. Киров', 'Верх — 56% кожа, 29% текстиль, 15% полимерные материалы; подкладка — 100% текстиль; подошва — полимерные материалы, резина', 9567, 'IMG_MS_185533.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (26, 10, 'Черные туфли в классическом стиле — база для деловых образов', 'Барбари', 'Модель выполнена из натуральной кожи, на резиновой подошве. Производитель: Дом обуви «Барбари», г. Новороссийск', 'Верх — натуральная кожа, подкладка — текстиль, стелька — натуральная кожа, подошва — резина', 24569, 'IMG_MSho_190514.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (27, 10, 'Туфли мужские светлые классика демисезонные', 'Берестов', 'Туфли на низком каблуке кожаные', 'Верх — натуральная кожа, подкладка — текстиль, стелька — натуральная кожа, подошва — ТЭП', 10155, 'IMG_MSho_190518.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (28, 10, 'Туфли из натуральной кожи', 'Берестов', 'Модель выполнена из натуральной кожи, на резиновой подошве, поддеживает классический и деловой стиль образов. Производитель: Дом обуви «Берестов», г. Москва', 'Верх — натуральная кожа, подкладка — натуральная кожа, стелька — натуральная кожа, подошва — резина', 25625, 'IMG_MSho_191612.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (29, 11, 'Ботинки мужские демисезонные', 'Барбари', 'Ботинки из натуральной кожи темно-коричневого и черного цвета на шнурках. Производитель: Дом обуви «Берестов», г. Москва', 'Верх — натуральная кожа, подкладка — текстиль, подошва — искусственный материал', 21500, 'IMG_MB_174349.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (30, 11, 'Ботинки черные из натуральной кожи', 'Кожевенные традиции', 'Ботинки представляют баланс традиций и современности. Выполнены из натуральной кожи насыщенного черного цвета. Производитель: ЗАО «Кожевенные традиции», г. Вятка', 'Верх — натуральная кожа, подкладка — ворсин, стелька — ворсин, подошва — тунит', 14567, 'IMG_MB_191200.png');
INSERT INTO Products (ProductId, SubcategoryId, ProductName, Manufacturer, Description, Composition, Price, ImageName) VALUES (31, 11, 'Ботинки зимние классические', 'Барбари', 'Выбор на каждый день — натуральная кожа, натуральный мех. Производитель: Дом обуви «Барбари», г. Новороссийск', 'Верх — натуральная кожа, подкладка — натуральный мех, стелька — натуральный мех, подошва — ТЭП', 15670, 'IMG_MB_191205.png');

-- Товарные позиции: модель + размер + остаток
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (1, 1, '20', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (2, 1, '21', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (3, 1, '22', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (4, 1, '23', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (5, 1, '24', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (6, 1, '25', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (7, 1, '26', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (8, 1, '27', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (9, 1, '28', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (10, 1, '29', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (11, 1, '30', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (12, 1, '31', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (13, 2, '33', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (14, 2, '34', 0);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (15, 3, '19', 8);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (16, 3, '20', 8);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (17, 3, '25', 7);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (18, 3, '26', 7);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (19, 4, '19', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (20, 4, '20', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (21, 4, '26', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (22, 4, '30', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (23, 4, '32', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (24, 5, '21', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (25, 5, '26', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (26, 6, '27', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (27, 6, '28', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (28, 6, '29', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (29, 7, '21', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (30, 7, '22', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (31, 7, '23', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (32, 7, '24', 0);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (33, 8, '27', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (34, 8, '28', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (35, 8, '29', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (36, 8, '30', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (37, 9, '18', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (38, 9, '19', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (39, 9, '20', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (40, 9, '21', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (41, 9, '22', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (42, 9, '23', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (43, 10, '36', 6);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (44, 10, '37', 6);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (45, 11, '39', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (46, 11, '40', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (47, 12, '37', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (48, 12, '39', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (49, 12, '40', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (50, 12, '41', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (51, 13, '36', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (52, 13, '37', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (53, 13, '38', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (54, 13, '39', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (55, 13, '40', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (56, 14, '35', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (57, 15, '37', 6);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (58, 15, '39', 6);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (59, 15, '40', 6);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (60, 15, '38', 5);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (61, 16, '36', 5);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (62, 16, '37', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (63, 16, '38', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (64, 16, '39', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (65, 16, '40', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (66, 16, '41', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (67, 17, '36', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (68, 17, '38', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (69, 17, '39', 0);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (70, 17, '40', 0);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (71, 18, '35', 5);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (72, 18, '39', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (73, 19, '35', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (74, 19, '36', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (75, 20, '37', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (76, 20, '37.5', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (77, 20, '38', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (78, 20, '38.5', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (79, 21, '35', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (80, 21, '36', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (81, 21, '37', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (82, 21, '38', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (83, 21, '39', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (84, 21, '40', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (85, 22, '35', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (86, 22, '38', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (87, 23, '41.5', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (88, 23, '42', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (89, 23, '42.5', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (90, 23, '43', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (91, 23, '44', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (92, 23, '45', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (93, 24, '35', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (94, 24, '36', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (95, 24, '36.5', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (96, 24, '37', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (97, 24, '37.5', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (98, 24, '38', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (99, 24, '38.5', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (100, 24, '39', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (101, 24, '39.5', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (102, 24, '40', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (103, 24, '41', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (104, 24, '42', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (105, 24, '43', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (106, 24, '44', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (107, 24, '45', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (108, 25, '41', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (109, 25, '42', 0);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (110, 25, '44', 0);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (111, 26, '40', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (112, 26, '41', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (113, 26, '42', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (114, 26, '43', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (115, 26, '44', 4);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (116, 26, '45', 3);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (117, 27, '42', 5);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (118, 27, '43', 5);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (119, 27, '44', 5);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (120, 28, '44', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (121, 29, '41', 2);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (122, 29, '42', 1);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (123, 30, '40', 7);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (124, 30, '42', 7);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (125, 30, '44', 6);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (126, 31, '42', 10);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (127, 31, '44', 10);
INSERT INTO ProductSizes (ProductSizeId, ProductId, Size, Quantity) VALUES (128, 31, '45', 10);

-- Заказы (10)
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (1, '2026-04-01', 'Сидорова Анна Дмитриевна', 3);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (2, '2026-04-02', 'Смирнов Дмитрий Владимирович', 5);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (3, '2026-04-03', 'Васильева Ольга Игоревна', 7);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (4, '2026-04-03', 'Михайлова Татьяна Юрьевна', 9);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (5, '2026-04-03', 'Морозов Сергей Павлович', 12);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (6, '2026-05-04', 'Алексеев Артём Евгеньевич', 14);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (7, '2026-05-05', 'Лебедева Ирина Владимировна', 15);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (8, '2026-05-07', 'Павлова Елена Петровна', 17);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (9, '2026-05-08', 'Николаев Владимир Иванович', 20);
INSERT INTO Orders (OrderId, OrderDate, CustomerName, UserId) VALUES (10, '2026-05-08', 'Павлова Елена Петровна', 17);

-- Позиции заказов
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (1, 1, 6, 1, 2190, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (2, 1, 31, 2, 2790, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (3, 2, 14, 1, 2490, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (4, 2, 21, 1, 3490, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (5, 2, 41, 1, 2390, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (6, 3, 16, 1, 1990, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (7, 3, 34, 2, 1690, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (8, 4, 27, 1, 4290, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (9, 4, 25, 1, 3190, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (10, 4, 40, 1, 2390, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (11, 5, 44, 1, 25000, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (12, 5, 122, 1, 21500, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (13, 6, 109, 2, 9567, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (14, 6, 118, 1, 10155, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (15, 6, 104, 1, 7890, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (16, 7, 60, 1, 7130, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (17, 7, 62, 1, 12340, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (18, 7, 48, 1, 17000, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (19, 7, 52, 1, 7500, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (20, 7, 82, 2, 8765, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (21, 8, 72, 1, 7550, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (22, 8, 86, 1, 7795, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (23, 8, 46, 1, 15000, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (24, 9, 104, 1, 7890, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (25, 9, 124, 1, 14567, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (26, 9, 127, 1, 15670, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (27, 9, 114, 1, 24569, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (28, 10, 40, 1, 2390, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (29, 10, 64, 1, 12340, 0);
INSERT INTO OrderItems (OrderItemId, OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent) VALUES (30, 10, 48, 1, 17000, 0);

COMMIT;
