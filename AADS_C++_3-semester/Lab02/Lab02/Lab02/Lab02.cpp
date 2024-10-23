
#include <iostream>
#include <queue>
#include <vector>
using namespace std;

bool CheckArr[10];//для отслеживания посещ. вершин DF

//Матрица смежности для dfs
bool adjacencyMatrix[10][10] =
{
    {0, 1, 0, 0, 1, 0, 0, 0, 0, 0},
    {1, 0, 0, 0, 0, 0, 1, 1, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
    {0, 0, 0, 0, 0, 1, 0, 0, 1, 0},
    {1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
    {0, 0, 0, 1, 1, 0, 0, 0, 1, 0},
    {0, 1, 0, 0, 0, 0, 0, 1, 0, 0},
    {0, 1, 1, 0, 0, 0, 1, 0, 0, 0},
    {0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
};
//Список ребер 
vector<pair<int, int>> edges = {
    {1, 2}, {1, 5}, {2, 7}, {2, 8}, {3, 8},
    {4, 6}, {4, 9}, {5, 6}, {6, 9}, {7, 8},
    {9, 10}
};

// список смежности, для получения соседей
vector<vector<int>> adjacencyList(10);

//создание списка смежности
void createAdjacencyList() {
    for (const auto& edge : edges) {
        int from = edge.first - 1;
        int to = edge.second - 1;
        adjacencyList[from].push_back(to);
        adjacencyList[to].push_back(from);
    }
}


void bfs(int start) {
    queue<int> q;
    bool used[10] = { false };
    used[start] = true;
    q.push(start);
    while (!q.empty()) {
        int u = q.front();
        q.pop();
        printf("%d ", u + 1);

        for (int neighbor : adjacencyList[u]) {
            if (!used[neighbor]) {
                used[neighbor] = true;
                q.push(neighbor);
            }
        }
    }
}

void dfs(int v) {
    printf("%d ", v + 1);
    CheckArr[v] = true;//с 0
    for (int i = 0; i < 10; i++)
        if (adjacencyMatrix[v][i] == 1 && !CheckArr[i])//есть ребро и вершина не посещен
            dfs(i);
}

int main() {
    setlocale(LC_ALL, "ru");
    createAdjacencyList();

    cout << "Поиск в ширину: ";
    bfs(0);

    cout << "\nПоиск в глубину: ";
    fill(CheckArr, CheckArr + 10, false);//массив заполняется значениями false
    dfs(0);

    return 0;
}