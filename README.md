# CombCircuitsGenerator
Программное обеспечение для генерации комбинационных схем на основе заданных параметров.

## Оглавление
<a name="content"></a> 
1. [Основные параметры разработки](#programmParameters)
2. [Используемые Packages](#packages)
3. [Список генераторов](#generators)

## Основные параметры разработки
<a name="programmParameters"></a> 
Для корректной работы необходимы:
- Microsoft Visual Studio 17.1.1
- .NETFramework,Version=v4.8

[&#8593; Оглавление](#content)

## Используемые Packages
<a name="packages"></a> 
Для корректной работы необходимы следующие пакеты:
- [ConsoleTables] - вывод таблиц в консоль. Версия: 2.4.2.
- [Newtonsoft] - взаимодействие с файлами типа JSON. Версия: 13.0.1.
- [NotificationWindow] - библиотека для отображения всплывающих окон. Версия: 1.1.38.

Все перечисленные пакеты можно установиться через "Упарвелние пакетами NuGet" в Microsoft Visual Studio.

[&#8593; Оглавление](#content)

## Список генераторов
<a name="generators"></a> 
В данный раздел будут добавляться описания генераторов, реализованных в программе:
- [X] Основные методы и классы для реализации генераторов
    - [X] Перевод таблицы истинности в логическое выражение
        - [X] В канонические нормальные формы
    - [X] Реализация парсинга логического выражения
    - [X] Реализация графа для храния комбинационной схемы
    - Генерация структурного Verilog на основе графа
    - [X] Реализация структуры с оснвоными логическими элементами, их обозначениями и уровня иерархии
    - [X] Реализации структуры для хранения таблицы истинности
    - [X] Реализации базового класса для генерации датасета комбинационной схемы
        - [X] Генерация с использованием генератора на основе случайной таблицы истинности
    - [X] Реализация класса для хранения комбинационной схемы
- [X] Реализация генераторов комбинационных схем
    - [X] На основе таблиц истинности
    - [X] Методом поуровнего случайного соединения элементов между собой
    - [X] Со случайным соединением вершин
        - [X] Реализация вспомогательных функций
        - [X] Основна программная реализация
    - [X] Генетический алгоритм 
        - [X] Реализация отбора родителей
          - [X] Реализация класса параметров отбора родителей
          - [X] Реализация класса с 5 типами отбора родителей
        - [X] Реализация скрещивания 
          - [X] Реализация класса параметров скрещивания
          - [X] Реализация класса с 5 типами скрещивания
        - [X] Реализация мутаций 
          - [X] Реализация класса параметров мутации
          - [X] Реализация класса с 6 типами мутаций
          - [X] Реализация основной класса для выбора типа и непосредственно мутации
        - [X] Реализация отбора новой популяции 
          - [X] Реализация класса параметров отбора новой популяции
          - [X] Реализация класса с базовым типом отбора новой популяции
    - [X] Механизм добавления и оценки параметров сторонних схем 
- [X] Реализация алгоритмов оценки надежности комбинационных схем
    - [X] Реализация вычисления полинома
    - [X] Подключение сторонней программы Nadezhda

[&#8593; Оглавление](#content)


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [ConsoleTables]: <https://github.com/khalidabuhakmeh/ConsoleTables>
   [Newtonsoft]: <https://www.newtonsoft.com/json>
   [NotificationWindow]: <https://github.com/Tulpep/Notification-Popup-Window>
