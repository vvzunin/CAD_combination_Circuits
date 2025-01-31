# ENG

## Dataset of combinational circuits
This folder contains a dataset of combinational circuits.

The file [dataset.csv](dataset.csv) contains the generated dataset of combinational circuits combined into a single file with the main parameters and an assessment of their reliability.

The file [dataset_CCGRTT.csv](dataset_CCGRTT.csv) contains a part of the dataset generated by the method of randomly generating truth tables and converting them into PDNF and PCNF forms. More details in the description of [generator program](../Generator).

The file [dataset_CCGRCG.csv](dataset_CCGRCG.csv) contains a part of the dataset generated by the method of level-by-level random connection of elements to each other. More details in the description of [generator program](../Generator).

The file [dataset_CCGRVC.csv](dataset_CCGRVC.csv) contains a part of the dataset generated by the random vertex join method. More details in the description of [generator program](../Generator).

The file [dataset_CCGGA.csv](dataset_CCGGA.csv) contains a part of the dataset generated using the genetic algorithm. More details in the description of [generator program](../Generator).

The file [dataset_ISCAS85.csv](dataset_ISCAS85.csv) contains a dataset of combinational circuits based on the ISCAS85 benchmark.


Each combinational circuit in the dataset is represented by the following parameters:
* *numInputs* - number of circuit inputs;
* *numOutputs* - number of circuit outputs;
* *maxLevel* - length of the critical path;
* enumeration of logical elements by types (const, and, nand, or, nor, not, xor, xnor, buf);
* the number of edges by type of logical elements (from-to, for example, const-nand);
* *reliability_percent* - combinational circuit reliability, expressed as a percentage.

The first line of each dataset file lists the names of the parameters separated by commas.
Each subsequent line corresponds to a separate combinational circuit. The order of the schema parameters is indicated on the first line. The parameters themselves are separated by commas.

---

# RUS

## Датасет комбинационных схем
В данной папке представлен датасет комбинационных схем.

В файле [dataset.csv](dataset.csv) представлен объединенный в единый файл сгенерированный датасет комбинационных схем с основными параметрами и оценкой их надежности.

В файле [dataset_CCGRTT.csv](dataset_CCGRTT.csv) представлена часть датасета, сгенерированная методом случайной генерации таблиц истинности, их перевода в формы СДНФ и СКНФ. Подробнее в описании [программы-генератора](../Generator).

В файле [dataset_CCGRCG.csv](dataset_CCGRCG.csv) представлена часть датасета, сгенерированная методом поуровнего случайного соединения элементов между собой. Подробнее в описании [программы-генератора](../Generator).

В файле [dataset_CCGRVC.csv](dataset_CCGRVC.csv) представлена часть датасета, сгенерированная методом со случайным соединением вершин. Подробнее в описании [программы-генератора](../Generator).

В файле [dataset_CCGGA.csv](dataset_CCGGA.csv) представлена часть датасета, сгенерированная с использованием генетического алгоритма. Подробнее в описании [программы-генератора](../Generator).

В файле [dataset_ISCAS85.csv](dataset_ISCAS85.csv) представлена датасет комбинационных схем на основе бенчмарка ISCAS85.


Каждая комбинационная схема в датасете представлена следующими параметрами:
* *numInputs* - количество входов схемы;
* *numOutputs* - количество выходов схемы;
* *maxLevel* - длина критического пути;
* перечисление логических элементов по типам (const, and, nand, or, nor, not, xor, xnor, buf);
* количество ребер по типам логических элементов (откуда-куда, например, const-nand);
* *reliability_percent* - надежность комбинационной схемы, выраженная в процентах.

В первой строке каждого файла с датасетом перечислены названия пераметров, разделенных запятой.
Каждая последующая строка соответствует отдельной комбинационной схеме. Порядок параметров схемы обозначен в первой строке. Сами параметры разделены запятыми.