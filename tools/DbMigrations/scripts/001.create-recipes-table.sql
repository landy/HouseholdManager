create table recipes
(
    id          uuid primary key,
    title       varchar(255) not null,
    description text         not null,
    created_at  timestamp    not null default now(),
    updated_at  timestamp    not null default now()
)