create schema if not exists broadcast;

comment on schema broadcast is 'Схема для хранения спортивных трансляций.';

create table if not exists broadcast.football
(
    id serial primary key,
    home_team varchar(64) not null,
    guest_team varchar(64) not null,
    start_time timestamptz not null,
    score_of_home_team int,
    score_of_guest_team int,
    current_half int,
    extra_time int,
    total_extra_time int,
    status int not null default 0
);


comment on table broadcast.football is 'Таблица футбольных трансляций.';
comment on column broadcast.football.id is 'Айди футбольной трансляции.';
comment on column broadcast.football.home_team is 'Название команды, которая играет дома.';
comment on column broadcast.football.guest_team is 'Название команды, которая играет в гостях.';
comment on column broadcast.football.start_time is 'Время начала трансляции.';
comment on column broadcast.football.score_of_home_team is 'Количество забитых голов домашней команды.';
comment on column broadcast.football.score_of_guest_team is 'Количество забитых голов гостевой команды.';
comment on column broadcast.football.current_half is 'Текующий тайм в трансляции.';
comment on column broadcast.football.extra_time is 'Дополнительное время в тайме.';
comment on column broadcast.football.total_extra_time is 'Общее дополнительное время в матче.';
comment on column broadcast.football.status is 'Статус трансляции.';

create index if not exists index_football_start_time on broadcast.football(start_time);
comment on index broadcast.index_football_start_time is 'Индекс для поиска матчей по времени.';