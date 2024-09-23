# System Zarządzania Lotami

API do zarządzania lotami które pozwala przeglądać dodawać aktualizować i usuwać informacje o lotach.

## 🚀 Demo

[fms.norbertkaczmarek.pl](https://fms.norbertkaczmarek.pl)

[swagger](https://fms.norbertkaczmarek.pl/swagger)

## Lokalne środowisko

W celu włączenia bazy danych należy użyć `docker compose up` z konsoli z folderu z plikami źródłowymi lub zmienić ConnectionString.

Solution -> Build Solution

Project -> Configure Startup Projects -> Multiple startup projects -> FMS.API oraz FMS.UI ustaw na Start

Po włączeniu obu projektów w VS2022, nawigujemy do `http://localhost:4200/` oraz `http://localhost/swagger`.

## Obiekty

**User** 
| Parametr        | Typ      | Opis                              |
| :-------------- | :------- | :-------------------------------- |
| `Id`            | `int`    | Id obiektu                        |
| `Email`         | `string` | Email użytkownika                 |
| `FullName`      | `string` | Imię i nazwisko użytkownika       |
| `PasswordHash`  | `string` | Zabezpieczone hasło użytkownika   |

**Flight** 
| Parametr          | Typ              | Opis                              |
| :---------------- | :--------------- | :-------------------------------- |
| `Id`              | `int`            | Id obiektu                        |
| `NumerLotu`       | `int`            | Unikalny numer lotu               |
| `DataWylotu`      | `DateTimeOffset` | Data wylotu                       |
| `MiejsceWylotu`   | `string`         | Miejsce wylotu samolotu           |
| `MiejscePrzylotu` | `string`         | Miejsce przylotu samolotu         |
| `TypSamolotu`     | `PlaneType`      | Embraer / Boeing / Airbus         |
