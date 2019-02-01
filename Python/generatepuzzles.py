from copy import deepcopy
from Board import Board
from Shape import *
from colorama import init as colorInit, Back, Style

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
        potentialspots.append([(spot, s.locationsRelativeTo(
            spot[1:], spot[0])) for spot in board.findSpots(s)])
    return potentialspots

def validateSolutionsRecursive(spots, potentialspots, target,solutions):
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
    duplicates = [(len(shapes)-i-1,len(shapes)-j-1) for i in range(len(shapes)-1) for j in range(i+1,len(shapes)) if str(shapes[i])==str(shapes[j])]
    result = []
    for sol in solutions:
        valid = True
        for d in duplicates:
            sol2 = sol[:]
            sol2[d[0]],sol2[d[1]] = sol2[d[1]],sol2[d[0]]
            if tuple(sol2) in result:
                valid = False
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
    for row in range(len(bss[0])):
        print(" ".join(bs[row] for bs in bss))
    # for bs in bss:
    #     for bsl in bs:
    #         print (bsl)

def main(shapeStringArray,board):
    colorInit()
    shapes = initShapes(shapeStringArray)
    potentialspots = findPotentialSpots(shapes,board)
    solutions = []
    validateSolutionsRecursive([], potentialspots, board.allOpenings,solutions)
    newsols = filterDuplicates(solutions,shapes)
    printSolutions(newsols,shapes,board)


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

shapestrings = ["""
                000
                010
                110
                -
                000
                000
                100
                -
                100
                010
                110
                """,
                """
                000
                010
                110
                -
                000
                000
                100
                -
                100
                010
                110
                """,
                """
                000
                010
                111
                -
                000
                000
                101
                -
                000
                000
                111
                """]

main(shapestrings,b)



