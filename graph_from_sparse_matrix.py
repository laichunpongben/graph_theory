# Create graph with adjacency matrix in COO

import numpy as np
import scipy.sparse
import networkx as nx
import matplotlib.pyplot as plt

dataset_file = 'coo.csv'
dataset = np.loadtxt(open(dataset_file,'rb'),delimiter=',')

row  = np.array(dataset[:,0])
col  = np.array(dataset[:,1])
data = np.array(dataset[:,2])
size = max(row+col)+1
A = scipy.sparse.coo_matrix((data,(row,col)), shape=(size,size))
G = nx.from_scipy_sparse_matrix(A)

pos=nx.random_layout(G)
neighbor_count = [len(G.neighbors(n))/1000 for n in pos]
colors = '#FFFFFF'
out = 'graph_random_layout'
nx.draw(G,pos,node_size=neighbor_count,node_color=colors,linewidths=0.01,
    edge_color='#BB0000',width=0.01,edge_cmap=plt.cm.Blues,with_labels=False)
fig.savefig('graph.png', dpi=1000)
