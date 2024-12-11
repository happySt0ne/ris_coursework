import random 
import numpy as np 
import os

n: int = int(input("Введите размерность задач: "))
z: int = int(input("Введите номер для файла с данными: "))

# directory = "./A"
# file_name = f"a{z}.csv"
# file_path = os.path.join(directory, file_name)

# for z in range(10):
with open(f'./A/a{z}.csv', 'w') as file:
    for i in range(n):
        for j in range(n):
            number: int = random.randint(0, 10)

            _ = file.write(f"{number} ")
        _ = file.write("\n")

with open(f'./B/b{z}.csv', 'w') as file:
    for i in range(n):
        number: int = random.randint(0, 10)

        _ = file.write(f"{number}\n")
