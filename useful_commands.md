# Useful commands

## Add migration

```
dotnet ef migrations add $MIGRATION_NAME -p .\src\MusicSchool.SchoolManagement.Api -o Design\Migrations
```

## Update database schema

```
dotnet ef database update -p .\src\MusicSchool.SchoolManagement.Api
```

## Run API

```
dotnet run --project .\src\MusicSchool.SchoolManagement.Api
```
