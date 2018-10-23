# bb-test
### Задание для соискателя на junior C#

#### Стек технологий
* SQL;
* Entity Framework;
* ASP.MVC;
* WCF.

#### Задача
Разработать систему регистрации/авторизации/просмотра списка пользователей.
* Все пользователи должны храниться в БД серверного приложения (не Web).
* Вход пользователей осуществляется по паре логин+пароль.
* Взаимодействие web + backend server через WCF с использованием общей сборки ServiceContracts в которой будут описаны необходимые интерфейсы для wcf сервиса и типы данных которыми он (wcf сервис) оперирует.

#### Запуск
Перед запуском необходимо убедиться в правильности ConnectioString для Model1 со стороны backend2.

##### Быстрый старт:
* выполнить backend2.exe --run-service;
* запустить фронтенд.

##### Дополнительные аргументы командной строки для backend2:
* --help - this help
* --create-with-demo - создать базу данных и заполнить псевдослучайными данными (см. тело функции DemoInit())
* --drop-database - удалить существующую БД, если есть
* --create-database - создать БД с пустыми таблицами
* --run-service - создать БД с пустыми таблицами, если ее еще нет, и запустить WCF-сервис