from copy import deepcopy
import numpy as np


def deepequals(a, b):
    if len(a) != len(b):
        return False
    for i in range(len(a)):
        if list(a[i]) != list(b[i]):
            return False
    return True

def shapeStringToPlane(s):
    rows = [x.strip() for x in s.strip().split("\n")]
    return [[int(c) for c in r]for r in rows]
    

def shapeStringToShape(s):
    return Shape([shapeStringToPlane(plane) for plane in s.split("-")])

def faceFromGrid(grid):
    face = deepcopy(grid[0])
    for i in range(len(face)):
        for j in range(len(face[0])):
            if any(g[i][j] for g in grid):
                face[i][j]=1
    return face



class Shape():
    def __init__(self, grid):
        g3d = np.array(grid)
        self.faces = []
        grids = [g3d.tolist(),np.rot90(g3d,1).tolist(),np.rot90(g3d,1,(0,2))]
        for g in grids:
            face = faceFromGrid(g)
            for r in range(4):
                if not any([deepequals(x, face) for x in self.faces]):
                    self.faces.append(face)
                if not any([deepequals(x, face[::-1]) for x in self.faces]):
                    self.faces.append(face[::-1])
                face = [*zip(*face[::-1])]
    
    def __str__(self):
        return "||".join("|".join(["".join([str(x) for x in row]) for row in f])for f in self.faces)

    def locationsRelativeTo(self, otherLocation, rotation=0):
        return [(i+otherLocation[0], j+otherLocation[1]) for i in range(len(self.faces[rotation])) for j in range(len(self.faces[rotation][i])) if self.faces[rotation][i][j] == 1]
