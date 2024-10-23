#include <iostream>
#include <algorithm>
#include <numeric> 

using namespace std;

struct Edge {
    int start;
    int end;
    int weight;
};

class Graph {
private:
    static const int V = 8;
    int graphMatrix[V][V] = {
        {INT_MAX, 2, INT_MAX, 8, 2, INT_MAX, INT_MAX, INT_MAX},
        {2, INT_MAX, 3, 10, 5, INT_MAX, INT_MAX, INT_MAX},
        {INT_MAX, 3, INT_MAX, INT_MAX, 12, INT_MAX, INT_MAX, 7},
        {8, 10, INT_MAX, INT_MAX, 14, 3, 1, INT_MAX},
        {2, 5, 12, 14, INT_MAX, 11, 4, 8},
        {INT_MAX, INT_MAX, INT_MAX, 3, 11, INT_MAX, 6, INT_MAX},
        {INT_MAX, INT_MAX, INT_MAX, 1, 4, 6, INT_MAX, 9},
        {INT_MAX, INT_MAX, 7, INT_MAX, 8, INT_MAX, 9, INT_MAX}
    };

    Edge edgeList[16] = {
        {1, 2, 2}, {1, 4, 8}, {7, 5, 4}, {1, 5, 2}, {2, 3, 3},
        {2, 5, 5}, {2, 4, 10}, {3, 5, 12}, {3, 8, 7}, {4, 5, 14},
        {4, 6, 3}, {4, 7, 1}, {5, 8, 8}, {6, 7, 6}, {7, 8, 9},
        {5, 6, 11}
    };

    int edgeCount = 16;

    void sortEdges() {
        for (int i = 0; i < edgeCount - 1; i++) {
            for (int j = 0; j < edgeCount - i - 1; j++) {
                if (edgeList[j].weight > edgeList[j + 1].weight) {
                    swap(edgeList[j], edgeList[j + 1]);
                }
            }
        }
    }

    void unionComponents(int connectedVert[], int oldVert, int newVert) {
        for (int j = 0; j < V; j++) {
            if (connectedVert[j] == oldVert)
                connectedVert[j] = newVert;
        }
    }

public:
    void Prims();
    void Kruskals();
};

void Graph::Prims() {
    bool visited[V] = { false };
    int startVertex;

    cout << "Введите номер начальной вершины (1-" << V << "): ";
    cin >> startVertex;

    if (startVertex < 1 || startVertex > V) {
        cout << "Некорректный номер вершины. Используется вершина 1." << endl;
        startVertex = 1;
    }
      
    visited[startVertex - 1] = true;
    int currentEdge = 0;

    while (currentEdge < V - 1) {
        int min = INT_MAX, x = -1, y = -1;

        for (int i = 0; i < V; i++) {
            if (visited[i]) {
                for (int j = 0; j < V; j++) {
                    if (!visited[j] && graphMatrix[i][j] < min) {
                        min = graphMatrix[i][j];
                        x = i;
                        y = j;
                    }
                }
            }
        }

        if (x != -1 && y != -1) {
            cout << "V" << x + 1 << " - V" << y + 1 << " : " << graphMatrix[x][y] << endl;
            visited[y] = true;
            currentEdge++;
        }
    }
}

void Graph::Kruskals() {
    int connectedVert[V];
    iota(connectedVert, connectedVert + V, 0);

    sortEdges();

    for (int i = 0; i < edgeCount; i++) {
        Edge e = edgeList[i];
        if (connectedVert[e.start] != connectedVert[e.end]) {
            cout << "V" << e.start << " - V" << e.end << " : " << e.weight << endl;
            unionComponents(connectedVert, connectedVert[e.start], connectedVert[e.end]);
        }
    }
}

int main() {
    setlocale(LC_ALL, "Rus");
    Graph graph;
    cout << "Список рёбер остовного дерева и их вес (алгоритм Прима)" << endl;
    graph.Prims();
    cout << "\nСписок рёбер остовного дерева и их вес (алгоритм Краскала)" << endl;
    graph.Kruskals();
    return 0;
}