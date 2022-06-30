# ENG

## Investigation of the reliability of combinational circuits
This project was created to study the reliability of combinational circuits using various methods, including using machine learning methods.

### Contents
<a name="content_eng"></a> 
1. [Program for generating combinational circuits](#programm_eng)
2. [Dataset of combinational circuits](#dataset_eng)
3. [Dataset usage examples](#examples_eng)


### Program for generating combinational circuits
<a name="programm_eng"></a> 
The first part of this repository is a program for generating combinational circuits in Verilog format and calculating their main parameters.
The source code of the program can be found [here](Generator).

[&#8593; Contents](#content_eng)


### Dataset of combinational circuits
<a name="dataset_eng"></a> 
Using the developed program, a dataset of combinational circuits was generated. The dataset is presented in CSV format.
The dataset and its description are located [here](Dataset).

[&#8593; Contents](#content_eng)


### Dataset usage examples
<a name="examples_eng"></a> 
To determine the suitability of the developed methods and combinational circuits generated as a result of their work, for use in machine learning, two algorithms were developed, which allow predicting the reliability of combinational circuits based on training on the dataset generated. The XGBoost algorithm. was chosen as the first idea for implementing a method for assessing the reliability of combinational circuits using machine learning. This algorithm is a machine learning algorithm based on a decision tree and using the gradient boosting framework. It has gained a lot of popularity as the algorithm chosen by many winning teams in many machine learning competitions. As the second method for calculating reliability, a regression neural network was chosen, which analyzes the parameters of the circuit and calculates its reliability.
Examples of using the dataset are [here](Prediction).

[&#8593; Contents](#content_eng)

---

# RUS

## Исследование надежности комбинационных схем
Данный проект создан с целью исследования надежности комбиниацонных схем различными методами, в том числе с использованием методов машинного обучения.

### Оглавление
<a name="content"></a> 
1. [Программа генерации комбинационных схем](#programm)
2. [Датасет комбинационных схем](#dataset)
3. [Примеры использования датасета](#examples)


### Программа генерации комбинационных схем
<a name="programm"></a> 
Первой частью данного репозитория является программа для генерации комбинационных схем в формате Verilog и рассчет их основных параметров.
Исходный код программы можно найти [здесь](Generator).

[&#8593; Оглавление](#content)


### Датасет комбинационных схем
<a name="dataset"></a> 
С использованием разработанной программы был сгенерирован датасет комбинационных схем. Датасет представлен в формате CSV.
Датасет и его описание расположены [здесь](Dataset).

[&#8593; Оглавление](#content)


### Примеры использования датасета
<a name="examples"></a> 
Для определения пригодности предложенных методов и комбинационных схем, сгенерированных в результате их работы, для использования в машинном обучении были разработаны два алгоритма, позволяющие предсказать надежность комбинационных схем на основе обучения на сгенерированном датасете. В качестве первой идеи реализации метода оценки надежности комбинационных схем с использованием машинного обучения был выбран алгоритм XGBoost. Данный алгоритм является алгоритмом машинного обучения, основанным на дереве поиска решений и использующий фреймворк градиентного бустинга. Он приобрел большую популярность как алгоритм, который выбирали многие команды-победители во многих соревнования по машинному обучению. В качестве второго способа вычисления надежности была выбрана регрессионная нейронная сеть, которая производит анализ параметров схемы и вычисляет ее надежность.
Примеры использования датасеты находятся [здесь](Prediction).

[&#8593; Оглавление](#content)