# Useful commands

## Add migration

### School Management context

```
dotnet ef migrations add $MIGRATION_NAME -p .\src\MusicSchool.SchoolManagement.Api -o Design\Migrations
```

### Finance context

```
dotnet ef migrations add $MIGRATION_NAME -p .\src\MusicSchool.Finance.Api -o Design\Migrations
```

## Update database schema

### School Management context

```
dotnet ef database update -p .\src\MusicSchool.SchoolManagement.Api
```

### Finance context

```
dotnet ef database update -p .\src\MusicSchool.Finance.Api
```

## Run API

### School Management context

```
dotnet run --project .\src\MusicSchool.SchoolManagement.Api
```

### Finance context

```
dotnet run --project .\src\MusicSchool.Finance.Api
```
