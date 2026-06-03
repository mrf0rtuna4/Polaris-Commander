# Polaris Commander

Современный файловый менеджер под Windows 10/11.

# Структура проекта

```text
PolarisCommander/
│
├── src/
│   ├── PolarisCommander.App/
│   ├── PolarisCommander.Core/
│   ├── PolarisCommander.Infrastructure/
│   ├── PolarisCommander.Protocols/
│   └── PolarisCommander.UI/
│
├── plugins/
├── themes/
└── PolarisCommander.sln
```

---

# Используемые технологии

* .NET
* WinUI 3
* MVVM Toolkit
* Microsoft.Extensions.Hosting
* Serilog
* FluentFTP
* SSH.NET

---

# Быстрый старт

## Создание solution

```bash
dotnet new sln -n PolarisCommander
```

---

## Добавление проектов

```bash
dotnet sln add .\src\**\*.csproj
```

---

## Установка пакетов

### App

```bash
dotnet add package CommunityToolkit.Mvvm

dotnet add package Microsoft.Extensions.DependencyInjection

dotnet add package Microsoft.Extensions.Hosting

dotnet add package Serilog.Extensions.Hosting
```

### Protocols

```bash
dotnet add package SSH.NET

dotnet add package FluentFTP
```

## Запуск


### Ручной dotnet
```bash
dotnet clean
dotnet build
dotnet run --project .\src\PolarisCommander.App\PolarisCommander.App.csproj
```

### Ручной powershell

```powershell
.\scripts\Start-PolarisCommander.ps1
```

Полезные параметры:

```powershell
.\scripts\Start-PolarisCommander.ps1 -SkipClean      # не выполнять clean перед build
.\scripts\Start-PolarisCommander.ps1 -NoLaunch       # только restore/build без запуска
.\scripts\Start-PolarisCommander.ps1 -Configuration Release
```

---

# TODO

* [x] Создать solution
* [x] Добавить проекты
* [x] Настроить references
* [x] Подключить MVVM
* [x] Подключить DI
* [x] Сделать MainWindow
* [x] Добавить sidebar
* [x] Добавить file list
* [x] Сделать LocalFileProvider
* [x] Подготовить FTP provider
* [x] Подготовить SFTP provider
* [x] Добавить базовую навигацию
* [x] Настроить themes
* [x] Добавить .gitignore
* [x] double click navigation
* [x] back history
* [x] forward history
* [x] breadcrumbs
* [x] drive picker
* [x] current folder refresh
* [ ] keyboard navigation
* [ ] path validation
* [ ] copy queue
* [ ] move queue
* [ ] delete queue
* [ ] progress tracking
* [ ] cancel operation
* [ ] retry operation
* [ ] overwrite dialog
* [ ] background workers
* [ ] operation logs
* [ ] connection profiles
* [ ] encrypted credentials
* [ ] reconnect support
* [ ] session restore
* [ ] connection testing
* [ ] timeout handling
* [ ] host fingerprint validation
* [ ] async remote navigation
* [ ] tabs support
* [ ] split panels
* [ ] independent navigation
* [ ] drag between panels
* [ ] tab restore
* [ ] tab history
* [ ] active panel tracking

---

Copyright (c) 2026 Mr_Fortuna