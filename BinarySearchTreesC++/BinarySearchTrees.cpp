#include <iostream>
#include <vector>
#include <memory>
#include "RedBlackTree.h"

using namespace std;

int main()
{
    auto tree = make_unique<RedBlackTree<int, string>>();

    vector<int> arr; // = { 80, 29, 90, 6, 72, 88, 93, 5, 67, 85, 66, 68 };

    for (int i = 0; i < 30; i++) {
        auto r = GenerateRandomNumber(0, 100);
        arr.push_back( r );
        auto key = r;
        tree->RBInsert( pair<int, string>(key, "Ivanov" + i) );
    }

    cout << tree->GetRoot()->key << endl;
    cout << endl;
    tree->InOrderTraverse();
    cout << endl;

    int n = 13;
    while ( n > 0 )
    {
        n--;

        auto nodeToDelete = tree->GetTreeMinimum();

        if ( nodeToDelete == nullptr ) {
            break;
        }
        auto r = nodeToDelete->key; //arr.at( GenerateRandomNumber(0, 9) );

        cout << endl;
        cout << "Delete: " << r << endl;
        cout << endl;

        tree->RBDelete(r);

        //  Simple paths
        auto arr = tree->GetAllLeaves();
        cout << endl;
        cout << endl;
        for (auto i = 0; i < arr.size(); i++) {
            int x = RedBlackTree<int, string>::CountSimplePath(arr.at(i));
            cout << x << endl;
        }

        if ( tree->GetRoot() != nullptr )
        {
            cout << tree->GetRoot()->key << endl;
            cout << endl;
            tree->InOrderTraverse();
            cout << endl;
            cout << tree->GetTreeMinimum()->key << endl;
        }
    }

    tree->RBInsert(pair<int, string>(123, "Ivanov" + 123));
    cout << tree->GetRoot()->key << endl;
    cout << endl;
    tree->InOrderTraverse();
    cout << endl;
    cout << tree->GetTreeMinimum()->key << endl;
        
}
