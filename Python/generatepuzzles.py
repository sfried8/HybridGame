from copy import deepcopy
from Board import *
from Shape import *
from colorama import init as colorInit, Back, Style
import time
from PuzzlesAndShapes import *

def boardString(board):
    return ["".join([["  ",". "][x]if x in [0, 1] else x for x in row]) for row in board]

def initShapes(shapeStringArray):
    shapes = [shapeStringToShape(shapestring) for shapestring in shapeStringArray]
    COLORS = [Back.RED,Back.GREEN,Back.YELLOW,Back.BLUE,Back.CYAN,Back.MAGENTA]
    for i in range(len(shapes)):
        shapes[i].symbol = COLORS[i]+"  "+Style.RESET_ALL
    return shapes

def findPotentialSpots(shapes, board):
    potentialspots = []
    for s in shapes:
        spots = board.findSpots(s)
        potentialspots.append([(spot, s.locationsRelativeTo(
            spot[1:], spot[0])) for spot in spots])
    return potentialspots

def validateSolutionsRecursive(spots, potentialspots, target,solutions):
    global iteration
    if len(potentialspots) == 0:
        if set(sum([x[1]for x in spots], [])) == target:
            solutions.append([x[0] for x in spots])
        return
    shape = potentialspots.pop()
    for s in shape:
        if any(x in y[1] for y in spots for x in s[1]):
            continue
        validateSolutionsRecursive(
            spots + [s], potentialspots[:], target,solutions)

def filterDuplicates(solutions,shapes):
    shapeLen = len(shapes) - 1
    duplicates = [(shapeLen-i,shapeLen-j) for i in range(shapeLen) for j in range(i+1,shapeLen+1) if str(shapes[i])==str(shapes[j])]
    result = []
    for sol in solutions:
        valid = True
        for d in duplicates:
            sol2 = sol[:]
            a,b = d
            sol2[a],sol2[b] = sol2[b],sol2[a]
            if tuple(sol2) in result:
                valid = False
                break
        if valid:
            result.append(tuple(sol))
    return result


def printSolutions(solutions,shapes,b):
    bss = []
    for sol in solutions:
        b.shapes = {}
        for i in range(len(sol)):
            b.addShape(shapes[~i], sol[i][1:], sol[i][0])
        bss.append(boardString(b.shapeGrid()))
    print (str(len(solutions))+" solutions found")
    if len(solutions)>0:
        for row in range(len(bss[0])):
            print(" ".join(bs[row] for bs in bss))


def main(shapeStringArray,board):
    start = time.time()
    colorInit()
    shapes = initShapes(shapeStringArray)
    potentialspots = findPotentialSpots(shapes,board)
    solutions = []
    validateSolutionsRecursive([], potentialspots, board.allOpenings,solutions)
    newsols = filterDuplicates(solutions,shapes)
    printSolutions(newsols,shapes,board)
    print(time.time()-start)


b = boardStringToBoard(
        # BoardsDict["key"]
        BoardsDict["arrow"]
)
# b = boardStringToBoard(
# )
shapestrings = [ShapesDict["jJn"],
                ShapesDict["jJn"],
                ShapesDict["tIL"],
                ShapesDict["brstairs"]
                ]

main(shapestrings,b)



