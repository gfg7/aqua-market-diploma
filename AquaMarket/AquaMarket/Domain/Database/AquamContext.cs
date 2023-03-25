using AquaServer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#nullable disable

namespace AquaServer.Domain.Database
{
    public partial class AquamContext : DbContext
    {
        public AquamContext() { }

        public AquamContext(DbContextOptions<AquamContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<UserAccount> Accounts { get; set; }
        public virtual DbSet<ConfirmationToken> ConfirmationTokens { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PackageType> PackageTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Accessory> Accessory { get; set; }
        public virtual DbSet<AccessoryType> AccessoryTypes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<WaterProduct> WaterProducts { get; set; }
        public virtual DbSet<WaterType> WaterTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<ConfirmationToken>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email)
                  .HasMaxLength(50)
                  .HasColumnType("nvarchar(50)")
                  .IsFixedLength(true);

                entity.Property(e => e.Token)
                    .IsRequired();

                entity.HasIndex(x => x.Token).IsUnique();

                entity.HasOne(d => d.Account)
                   .WithOne(p => p.ConfirmationToken)
                   .HasForeignKey<ConfirmationToken>(k => k.Email)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_ConfirmationToken_Account");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .IsFixedLength(true);

                entity.HasIndex(x => x.Title).IsUnique();
            });
            modelBuilder.Entity<City>().HasData(new City[]
            {
                new()
                {
                    Id=1,
                    Title = "Уфа"
                },
                new()
                {
                    Id=2,
                    Title = "Салават"
                },
                new()
                {
                    Id=3,
                    Title = "Белебей"
                },
                new()
                {
                    Id=4,
                    Title = "Бирск"
                },
                new()
                {
                    Id=5,
                    Title = "Стерлитамак"
                }
            });
    

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email)
                  .HasMaxLength(50)
                  .HasColumnType("nvarchar(50)")
                  .IsFixedLength(true);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(12)
                    .IsFixedLength(true);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .IsRequired()
                    .IsFixedLength(true);

                entity.Property(e => e.IsConfirmed)
                    .HasDefaultValue(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .IsFixedLength(true);
            });
            modelBuilder.Entity<UserAccount>().HasData(new UserAccount[]
            {
                new()
                {
                    Email = "reyibawe@yandex.ru",
                    IsConfirmed = true,
                    Name = "Александр",
                    Patronymic = "Алексеевич",
                    Surname = "Кудрявцев",
                    Phone = "+79875897888"
                },
                new()
                {
                    Email = "dfsggfgfd@yandex.ru",
                    IsConfirmed = true,
                    Name = "Анна",
                    Surname = "Оршенко",
                    Phone = "+79873255869"
                },
                new()
                {
                    Email = "zxccxzcfd@yandex.ru",
                    IsConfirmed = true,
                    Name = "Александра",
                    Patronymic = "Владимировна",
                    Surname = "Викторова",
                    Phone = "+79325892365"
                }
            });


            modelBuilder.Entity<Content>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Count)
                .IsRequired()
                .HasDefaultValue(1);

                entity.HasOne(d => d.Accessory)
                    .WithMany(p => p.Contents)
                    .HasForeignKey(d => d.Article)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contents_Accessory");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Contents)
                    .HasForeignKey(d => d.OrderNum)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contents_Order");
            });


            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsFixedLength(true);

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .IsFixedLength(true);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .IsFixedLength(true);

                entity.Property(e => e.IsAccActive)
                .HasColumnType("bit")
                .HasDefaultValue(true);

                entity.HasOne(d => d.Account)
                   .WithOne(p => p.Employee)
                   .HasForeignKey<Employee>(k => k.Email)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Employee_Account");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employees_Position");
            });
            modelBuilder.Entity<Employee>().HasData(new Employee[]
            {
                new()
                {
                    Email = "reyibawe@yandex.ru",
                    IsAccActive =true,
                    PositionId = 1,
                    Hash = "SnoO+fc1rBDoejr4ov/tHQ==",
                    Salt = "TUoWa8Lk7rzy8x6q1L295A=="
                },
                new()
                {
                    Email = "dfsggfgfd@yandex.ru",
                    IsAccActive =true,
                    PositionId = 2,
                    Hash = "v2qPf51PNM5slqAd6I3hvg==",
                    Salt = "n+2eClJ/EjWw4FXquwIbiA=="
                },
                new()
                {
                    Email = "zxccxzcfd@yandex.ru",
                    IsAccActive =true,
                    PositionId = 3,
                    Hash = "Ndfxx6+JKYr9FsfirsVAMQ==",
                    Salt = "RH/n08v8YVfwHc+WxdYJMw=="
                }
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(50)")
                    .IsFixedLength(true);

                entity.Property(x => x.ClientId)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Orders_Client");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Orders_City");
            });

            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Comment)
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)")
                    .IsFixedLength(true);

                entity.Property(e => e.Date)
                    .IsRequired();

                entity.HasOne(d => d.OrderNumNavigation)
                    .WithMany(p => p.OrderStatusHistories)
                    .HasForeignKey(d => d.OrderNum)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderStatusHistories_Order");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OrderStatusHistories)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderStatusHistories_Status");
            });

            modelBuilder.Entity<PackageType>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.AddToCost)
                .HasColumnType("decimal(5, 2)")
                .IsRequired();

                entity.HasIndex(x => x.Title).IsUnique();
            });
            modelBuilder.Entity<PackageType>().HasData(new PackageType[]
            {
                new()
                {
                    Id=1,
                    Title="Пластик",
                    AddToCost = 15
                },
                new()
                {
                    Id=2,
                    Title="Стекло",
                    AddToCost = 50
                },
                new()
                {
                    Id=3,
                    Title="Тетрапак",
                    AddToCost = 35
                }
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasIndex(x => x.Title).IsUnique();
            });
            modelBuilder.Entity<Position>().HasData(new Position[]
            {
                 new()
                {
                    Id=1,
                    Title="Менеджер"
                },
                new()
                {
                    Id=2,
                    Title="Сотрудник пункта выдачи"
                },
                new()
                {
                    Id=3,
                    Title="Курьер"
                }
            });

            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.HasKey(e => e.Article);

                entity.Property(e => e.Article)
                    .HasMaxLength(15)
                    .HasColumnType("nvarchar(15)")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(5000)")
                    .IsFixedLength(true);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(300)")
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasIndex(x => x.Title).IsUnique();

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasOne(d => d.AccessoryType)
                    .WithMany(p => p.Accessories)
                    .HasForeignKey(d => d.AccessoryTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Accessories_AccessoryType");
            });
            modelBuilder.Entity<Accessory>().HasData(new Accessory[]
            {
                new()
                {
                    Article="VATTENV09WE",
                    AccessoryTypeId = 4,
                    Title = "VATTEN V09WE",
                    Price = 8625,
                    Description = "Электронное охлаждение и нагрев. Верхняя загрузка бутыли. Два крана для подачи воды. Производительность системы нагрева / охлаждения, л/ч: 6/0,6 Размеры (ВхШхГ, мм) 830х296х300. Гарантия: 12 месяцев Стандартный напольный электронный кулер для воды бюджетного уровня. Со шкафчиком. Для дома. Для офиса. Современный дизайн. Цвет корпуса белый, все панели изготовлены из качественного, устойчивого к ультрафиолету пластика. Два крана-рычага. Подается горячая и холодная вода."
                },
                new()
                {
                    Article="VATTEND27WF",
                    AccessoryTypeId = 4,
                    Title = "VATTEN D27WF",
                    Price = 4945,
                    Description = "Настольный кулер с нагревом и без охлаждения. Современный дизайн. Корпус изготовлен из высококачественного, устойчивого к ультрафиолету ABS nпластика белого цвета. Два классических для моделей этого класса крана с рычагом для нажима. Два LED индикатора (индикатор питания, индикатор нагрева)."
                },
                new()
                {
                    Article="VATTEN",
                    AccessoryTypeId = 4,
                    Title = "Подстаканник",
                    Price = 1183,
                    Description = "Незаменимый аксессуар для кулеров и пурифаеров установленных в офисе и общественных местах. Вмещает 70 пластиковых стаканчиков. Верхняя крышка защищает стаканчики от пыли и случайных попаданий воды. Удобно размещается с помощью магнита или шурупов."
                },
                new()
                {
                    Article="105",
                    AccessoryTypeId = 1,
                    Title = "Спортивная бутылка красная",
                    Price = 299,
                    Description = "Бутылка спортивная красного цвета с логотипом"
                },
                new()
                {
                    Article="1055",
                    AccessoryTypeId = 1,
                    Title = "Стакан прозрачный кристалл 0,2 литра 50 шт",
                    Price = 530 ,
                    Description = "Согласно ГОСТ Р 509620- 96 «Посуда и изделия хозяйственного назначения из пластмасс, общие технические условия» пункта 3.8 изделие должно сохранять внешний вид, окраску, не деформироваться и не растрескиваться при температуре не выше 70+/-5 градусов, поэтому настоятельно не рекомендуем использовать пластиковую посуду для разогревания в микроволновой печи и  на открытом огне."
                },
                new()
                {
                    Article="10355",
                    AccessoryTypeId = 1,
                    Title = "Ящик КолорСтайл с крышкой белый 5 л",
                    Price = 345  ,
                    Description = "Универсальный ящик «КолорСтайл» поможет рационально организовать пространство и поддерживать идеальный порядок в доме, сэкономит место."
                },
                new()
                {
                    Article="art45",
                    AccessoryTypeId = 3,
                    Title = "Стаканы 200 мл Крафт картон 37 шт",
                    Price = 740   ,
                    Description = "Стакан одноразовый двухслойный для горячих и холодных напитков. Изготовлен из плотного картона с внутренней ламинацией. Воздушный зазор между стенками обеспечивает устойчивую теплоизоляцию по всей поверхности стакана."
                },
                new()
                {
                    Article="art46",
                    AccessoryTypeId = 3,
                    Title = "Стаканы Taste Quality картон 0,45 литра 50 шт",
                    Price = 450    ,
                    Description = "Стаканчики изготовлены из высококачественного европейского сырья, бумага не пропускает влагу, швы полностью герметичны, жидкость не вытекает, долго держит тепло, отлично сохраняют форму."
                },
                new()
                {
                    Article="lig46",
                    AccessoryTypeId = 2,
                    Title = "Стаканы Taste ",
                    Price = 45,
                },
                new()
                {
                    Article="470",
                    AccessoryTypeId = 6,
                    Title = "Фильтрующий сменный модуль Аквафор B6",
                    Price = 470     ,
                    Description = "Модуль В6 (В100-6) очищает водопроводную воду от вредных примесей и уменьшает образование накипи в чайнике, мультиварке и кофемашине. В жесткой воде чай и кофе долго завариваются и получаются не ароматными, а бульоны и другие блюда − пресными, потому что из-за избытка минералов вода плохо “вбирает” вкус ингредиентов."
                },
                new()
                {
                    Article="870",
                    AccessoryTypeId = 6,
                    Title = "Комплект фильтрующих модулей Аквафор A5 2 шт",
                    Price = 870      ,
                    Description = "Картридж A5 создан специально для сложной водопроводной воды: ржавой, мутной, с большим количеством механических примесей. Даже в таких условиях A5 не забивается раньше времени, и продолжает очищать вашу воду от хлора и других растворенных примесей."
                },
                new()
                {
                    Article="d23A",
                    AccessoryTypeId = 6,
                    Title = "Фильтр кувшин Аквафор Лайн 2.8 литра",
                    Price = 695 ,
                    Description = "Мобильный и компактный, экономит место на маленькой кухне. Надёжно и необратимо удаляет из питьевой воды хлор, ржавчину, тяжелые металлы, пестициды и другие распространенные примеси."
                },
                new()
                {
                    Article="d2dk3A",
                    AccessoryTypeId = 5,
                    Title = "Помпа Smixx Maximum",
                    Price = 600 ,
                    Description = "Помпа механическая SMixx Maximum подходит ко стандартным бутылям емкостью 11 и 19 литров, эргономична, обладает оптимальным усилием нажатия и длительным сроком эксплуатации. Выполнена из пищевого пластика."
                },
                new()
                {
                    Article="hhh2",
                    AccessoryTypeId = 7,
                    Title = "Стаканчики прозрачные 100 шт",
                    Price = 215 ,
                    Description = "Стаканы пластиковые прозрачные объемом 200 мл."
                },
                new()
                {
                    Article="hhdh2",
                    AccessoryTypeId = 7,
                    Title = "Ложка чайная пласт 200 шт",
                    Price = 281 ,
                    Description = "Одноразовые чайные ложки изготовлены из полистерола белого цвета, выполнены в классической форме. Пластиковая ложка для чая, для десерта подходит для холодных и горячих (до+70С) продуктов питания. Одноразовые пластиковые приборы не вступают во взаимодействие с продуктами питания и не выделяют токсинов."
                }
            });

            modelBuilder.Entity<AccessoryType>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasIndex(x => x.Title).IsUnique();
            });
            modelBuilder.Entity<AccessoryType>().HasData(new AccessoryType[]
            {
                new()
                {
                    Id=1,
                    Title="Емкости из пластика"
                },
                new()
                {
                    Id=2,
                    Title="Емкости из стекла"
                },new()
                {
                    Id=3,
                    Title="Емкости из тетрапака"
                },
                new()
                {
                    Id=4,
                    Title="Кулеры и оборудование"
                },
                new()
                {
                    Id=5,
                    Title="Помпы и аксессуары для кулеров"
                },
                new()
                {
                    Id=6,
                    Title="Фильтры для питьевой воды"
                },
                new()
                {
                    Id=7,
                    Title="Одноразовая посуда"
                }
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasIndex(x => x.Title).IsUnique();
            });
            modelBuilder.Entity<Status>().HasData(new Status[]
            {
                new()
                {
                    Id=1,
                    Title = "В корзине"
                },
                new()
                {
                    Id=2,
                    Title = "Ожидается подтверждение оператора"//менеджер
                },
                new()
                {
                    Id=3,
                    Title = "На сборке"//склад
                },
                new()
                {
                    Id=4,
                    Title = "Готово к выдаче"//склад
                },
                new()
                {
                    Id=5,
                    Title = "Доставка"//курьер
                },
                new()
                {
                    Id=6,
                    Title = "Завершено"//нажимает клиент или склад в order detail
                },
                new()
                {
                    Id=7,
                    Title = "Отклонено оператором"
                },
                new()
                {
                    Id=8,
                    Title = "Отменено заказчиком"
                }
            });

            modelBuilder.Entity<WaterProduct>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Volume)
                .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Count);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.WaterProducts)
                    .HasForeignKey(d => d.OrderNum)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_WaterProducts_Order");

                entity.HasOne(d => d.PackageType)
                    .WithMany(p => p.WaterProducts)
                    .HasForeignKey(d => d.PackageTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_WaterProducts_PackageType");

                entity.HasOne(d => d.WaterType)
                    .WithMany(p => p.WaterProducts)
                    .HasForeignKey(d => d.WaterTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_WaterProducts_WaterType");
            });

            modelBuilder.Entity<WaterType>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.AddToCost)
                .HasColumnType("decimal(5, 2)")
                  .IsRequired();

                entity.HasIndex(x => x.Title).IsUnique();
            });
            modelBuilder.Entity<WaterType>().HasData(new WaterType[]
            {
                new()
                {
                    Id=1,
                    Title = "Негазированная",
                    AddToCost = 30
                },
                new()
                {
                    Id=2,
                    Title = "Минеральная",
                    AddToCost = 40
                },
                new()
                {
                    Id=3,
                    Title = "Газированная",
                    AddToCost = 35
                },
                new()
                {
                    Id=4,
                    Title = "Дистилированная",
                    AddToCost = 35
                }
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
