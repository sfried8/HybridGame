import numpy as np
from copy import deepcopy

m = np.array([[[0,1,0],
               [1,0,1],
               [0,0,0]],

              [[0,1,0],
               [0,0,0],
               [0,1,0]],

              [[0,0,0],
               [0,1,0],
               [0,0,0]]])

def faceFromGrid(npgrid):
    grid = npgrid.tolist()
    face = deepcopy(grid[0])
    for i in range(len(face)):
        for j in range(len(face[0])):
            if any(g[i][j] for g in grid):
                face[i][j]=1
    return face

def p(g):
    for r in g:
        print(" ".join([str(x) for x in r]))
    print("")
p (faceFromGrid(m))
p (faceFromGrid(np.rot90(m,1)))
p (faceFromGrid(np.rot90(m,1,(0,2))))