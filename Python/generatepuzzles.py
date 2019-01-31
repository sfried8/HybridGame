#!/usr/local/bin/python
# -*- coding: utf-8 -*-
from copy import deepcopy
from Board import Board
from colorama import init, Back, Style

def deepequals(a, b):
    if len(a) != len(b):
        return False
    for i in range(len(a)):
        if list(a[i]) != list(b[i]):
            return False
    return True


class Shape():
    def __init__(self, grid):
        self.grid = grid
        self.faces = []
        face = deepcopy(self.grid)
        for r in range(4):
            if not any([deepequals(x, face) for x in self.faces]):
                self.faces.append(face)
                self.faces.append(face[::-1])
            face = [*zip(*face[::-1])]

    def __str__(self):
        return "|".join(["".join([str(x) for x in row]) for row in self.grid])

    def locationsRelativeTo(self, otherLocation, rotation=0):
        return [(i+otherLocation[0], j+otherLocation[1]) for i in range(len(self.faces[rotation])) for j in range(len(self.faces[rotation][i])) if self.faces[rotation][i][j] == 1]


def boardString(board):
    return ["".join([["  ",". "][x]if x in [0, 1] else x for x in row]) for row in board]


b = Board([
    [0, 0, 0, 0, 0, 0, 0, 0],
    [0, 0, 0, 1, 1, 1, 0, 0],
    [0, 0, 0, 1, 0, 1, 0, 0],
    [0, 0, 0, 1, 1, 1, 0, 0],
    [0, 0, 0, 0, 1, 0, 0, 0],
    [0, 0, 0, 0, 1, 1, 0, 0],
    [0, 0, 0, 0, 1, 0, 0, 0],
    [0, 0, 0, 0, 1, 1, 0, 0]]
)

shapestring = """
                111
                101
                100
                -
                111
                001
                -
                11
                01
                10
                """



def shapeStringToArray(s):
    result = []
    for shape in s.split("-"):
        rows = [x.strip() for x in shape.strip().split("\n")]
        g = [[int(c) for c in r]for r in rows]
        result.append(Shape(g))
    return result


shapes = shapeStringToArray(shapestring)
init()
for x in zip(shapes, range(len(shapes))):
    
    x[0].symbol = [Back.RED,Back.GREEN,Back.YELLOW,Back.BLUE][x[1]]+"  "+Style.RESET_ALL

potentialspots = []
for s in shapes:
    potentialspots.append([(spot, s.locationsRelativeTo(
        spot[1:], spot[0])) for spot in b.findSpots(s)])


solutions = []


def validateSolutionsRecursive(spots, potentialspots, target):
    if len(potentialspots) == 0:
        if set(sum([x[1]for x in spots], [])) == target:
            solutions.append([x[0] for x in spots])
        return
    shape = potentialspots.pop()
    for s in shape:
        if any(x in y[1] for y in spots for x in s[1]):
            continue
        validateSolutionsRecursive(
            spots + [s], potentialspots[:], target)


validateSolutionsRecursive([], potentialspots, b.allOpenings)


bss = []
for sol in solutions:
    b.shapes = {}
    for i in range(len(sol)):
        b.addShape(shapes[~i], sol[i][1:], sol[i][0])
    bss.append(boardString(b.shapeGrid()))

print (str(len(solutions))+" solutions found")

print("\n".join(['\n'.join(bs) for bs in bss]))

