from copy import deepcopy

def boardStringToBoard(boardString):
    rows = [row.strip() for row in boardString.split("\n")]
    grid = [[int(x) for x in row]for row in rows]
    return Board(grid)
class Board():
    def __init__(self, puzzle):
        self.puzzle = puzzle
        self.width = len(puzzle[0])
        self.height = len(puzzle)
        self.shapes = {}
        self.shapeSpotCache = {}
        self.gridWithShapes = None
        self.allOpenings = set([(i, j) for i in range(self.height)
                                for j in range(self.width) if self.puzzle[i][j] == 1])

    def __str__(self):
        return [" ".join([".."[x]if x in [0, 1] else x for x in row]) for row in self.puzzle]

    def shapeFits(self, shape, location, rotation=0):
        if (location, rotation) in self.shapes:
            return False
        sg = self.puzzle
        if any(x[0] >= self.height or x[1] >= self.height or sg[x[0]][x[1]] != 1 for x in shape.locationsRelativeTo(location, rotation)):
            return False
        return True

    def addShape(self, shape, location, rotation=0):
        # if self.shapeFits(shape, location, rotation):
        self.shapes[(location, rotation)] = shape
        self.gridWithShapes = None

    def shapeGrid(self):
        # if self.gridWithShapes:
            # return self.gridWithShapes
        res = deepcopy(self.puzzle)
        for location, rotation in self.shapes.keys():
            for shapeLocation in self.shapes[(location, rotation)].locationsRelativeTo(location, rotation):
                res[shapeLocation[0]][shapeLocation[1]
                                      ] = self.shapes[(location, rotation)].symbol
        self.gridWithShapes = res
        return res

    def findSpots(self, shape):
        if str(shape) in self.shapeSpotCache:
            return self.shapeSpotCache[str(shape)]
        spots = []
        for r in range(len(shape.faces)):
            for i in range(self.height):
                for j in range(self.width):
                    if self.shapeFits(shape, (i, j), r):
                        spots.append((r, i, j))
        self.shapeSpotCache[str(shape)] = spots
        return spots
