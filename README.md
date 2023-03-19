# music-school-ddd

## How to run

1. Check prerequisites:

* [Docker](https://www.docker.com/)
* [.NET SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

2. Test the project

```
$ dotnet test
```

3. Run dependencies

```
$ docker-compose up -d
```

4. Set up databases

```
$ dotnet ef database update -p ./src/MusicSchool.SchoolManagement.Api
$ dotnet ef database update -p ./src/MusicSchool.Finance.Api
```

5. Run applications

```
$ dotnet run --project ./src/MusicSchool.SchoolManagement.Api
$ dotnet run --project ./src/MusicSchool.Finance.Api
```

## How to add database migrations

* In School Management context:

```
$ dotnet ef migrations add $MIGRATION_NAME -p ./src/MusicSchool.SchoolManagement.Api -o Migrations
```

* In Finance context:

```
$ dotnet ef migrations add $MIGRATION_NAME -p ./src/MusicSchool.Finance.Api -o Migrations
```
