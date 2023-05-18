
        --Есть таблицы: 
        --games  (id, name, base_id, date)
        --game_orders (id, game_id, order)

        --Таблица games имеет 3 уровня parent -> child
        --Пример: parent->child->child
        --Поле name на последних уровнях может быть не заполнено.
        --Требуется вывести список игр, отсортированных по полю order (по возрастанию), с учетом,
        --что поле name в ответе всегда будет заполнено.      
        --Считать что name, можно взять у родительских записей, но стараться доставать с более низкого уровня. 
        
        create table games  (
            id int primary KEY, 
            name varchar(20), 
            base_id int null FOREIGN KEY (base_id) REFERENCES games(id), 
            [date] varchar(20)
        );

        create table games_order (
            id int primary KEY, 
            game_id int FOREIGN key (game_id) REFERENCES games(id), 
            [order] int
        );

        GO

        insert into games values(1, "Base1_Game", null, "date1");
        insert into games values(2, null, 1, "date2");
        insert into games values(3, null, 2, "date3");

        insert into games_order values(1, 2, 1);
        insert into games_order values(2, 1, 2);
        insert into games_order values(3, 3, 3);

        GO

        WITH Rec --(id, base_id, name)
         AS (
           SELECT id, base_id, name, (case when name is null then '~' else cast(name as varchar(100)) end) as hierPath  FROM games where id = 1
           UNION ALL
           SELECT g.id, g.base_id, coalesce(g.name, rec.name) as name, cast(rec.hierPath + ' / ' + (case when g.name is null then '~' else g.name end) as varchar(100) )
            FROM Rec 
            inner join games as g on rec.id = g.base_id
           )
         SELECT [go].[Order], name, hierPath FROM Rec
        inner join games_order [go] on rec.id = [go].game_id
        Order by [go].[order]
   
        GO
