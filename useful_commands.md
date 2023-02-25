# Useful commands

## Add migration

```
dotnet ef migrations add $MIGRATION_NAME -p .\src\MusicSchool.SchoolManagement.Infrastructure -o DataAccess\Migrations
```

## Update database schema

```
dotnet ef database update -p .\src\MusicSchool.SchoolManagement.Infrastructure --connection "server=localhost; database=school_management; user=root; password=password"
```

## Run "playground" console app

```
dotnet run --project .\src\MusicSchool.SchoolManagement.Playground
```
