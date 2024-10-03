# System Zarządzania Lotami

API do zarządzania lotami które pozwala przeglądać dodawać aktualizować i usuwać informacje o lotach.

## 🚀 Demo

[fms.norbertkaczmarek.pl](https://fms.norbertkaczmarek.pl)

[swagger](https://fms.norbertkaczmarek.pl/swagger)

## Lokalne środowisko

Po włączeniu obu projektów w VS2022, nawigujemy do `http://localhost:4200/` oraz `http://localhost/swagger`.

W celu włączenia bazy danych należy użyć `docker compose up` z konsoli z folderu z plikami źródłowymi lub zmienić ConnectionString.

## Obiekty

**User** 
| Parametr        | Typ      | Opis                              |
| :-------------- | :------- | :-------------------------------- |
| `Id`            | `Guid`   | Id obiektu                        |
| `Email`         | `string` | Email użytkownika                 |
| `FullName`      | `string` | Imię i nazwisko użytkownika       |
| `PasswordHash`  | `string` | Zabezpieczone hasło użytkownika   |

**Flight** 
| Parametr          | Typ              | Opis                              |
| :---------------- | :--------------- | :-------------------------------- |
| `Id`              | `Guid`            | Id obiektu                        |
| `NumerLotu`       | `int`            | Unikalny numer lotu               |
| `DataWylotu`      | `DateTimeOffset` | Data wylotu                       |
| `MiejsceWylotu`   | `string`         | Miejsce wylotu samolotu           |
| `MiejscePrzylotu` | `string`         | Miejsce przylotu samolotu         |
| `TypSamolotu`     | `PlaneType`      | Embraer / Boeing / Airbus         |
