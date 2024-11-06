#include <iostream>
#include <istream>
#include <vector>
#include <list>
#include <map>
#include <string>

using namespace std;

class Node
{
public:
    int a;
    char c;
    Node* left, * right;

    Node() { left = right = NULL; }

    Node(Node* L, Node* R)
    {
        left = L;
        right = R;
        a = L->a + R->a;
    }
};

struct MyCompare
{
    bool operator()(const Node* l, const Node* r) const {
        return l->a < r->a;
    }
};

vector<bool> code;
map<char, vector<bool>> table;

void BuildTable(Node* root)
{
    if (root->left != nullptr) {
        code.push_back(0);
        BuildTable(root->left);
    }

    if (root->right != nullptr) {
        code.push_back(1);
        BuildTable(root->right);
    }

    if (root->c) table[root->c] = code;

    if (!code.empty()) {
        code.pop_back();
    }
}

int main()
{
    //setlocale(LC_ALL, "Ru");
    string s;

    cout << "‚¢¥¤¨â¥ áâà®ªã: ";//Ââåäèòå ñòðîêó:
    getline(cin, s);

    map<char, int> m;

    for (int i = 0; i < s.length(); i++) {
        char c = s[i];
        m[c]++;
    }

    cout << "’ ¡«¨æ  ¢áâà¥ç ¥¬®áâ¨ á¨¬¢®«®¢ ¢ â¥ªáâ¥: \n";//Òàáëèöà âñòðå÷àåìîñòè ñèìâîëîâ â òåêñòå:

    list<Node*> t;
    map<char, int>::iterator i;


    for (i = m.begin(); i != m.end(); ++i)
    {
        cout << i->first << " : " << i->second << endl;
    }

    for (i = m.begin(); i != m.end(); ++i)
    {
        Node* p = new Node;
        p->c = i->first;
        p->a = i->second;
        t.push_back(p);
    }

    while (t.size() != 1)
    {
        t.sort(MyCompare());

        Node* SonL = t.front();
        t.pop_front();
        Node* SonR = t.front();
        t.pop_front();

        Node* parent = new Node(SonL, SonR);
        t.push_back(parent);
    }

    Node* root = t.front();
    BuildTable(root);

    cout << "\n‚ëå®¤­ ï ¯®á«¥¤®¢ â¥«ì­®áâì: \n";//Âûõîäíàÿ ïîñëåäîâàòåëüíîñòü:

    for (int i = 0; i < s.length(); i++) {
        char c = s[i];
        vector<bool> x = table[c];

        for (int j = 0; j < x.size(); j++) {
            cout << x[j];
        }
    }

    cout << "\n\n’ ¡«¨æ  á®®â¢¥âáâ¢¨ï á¨¬¢®«  ¨ ª®¤®¢®© ¯®á«¥¤®¢ â¥«ì­®áâ¨: \n";//Òàáëèöà ñîîòâåòñòâèÿ ñèìâîëà è êîäîâîé ïîñëåäîâàòåëüíîñòè:

    for (auto itr = table.begin(); itr != table.end(); ++itr)
    {
        cout << itr->first << " : ";

        for (bool bit : itr->second) {
            cout << bit;
        }

        cout << endl;
    }

    return 0;
}
