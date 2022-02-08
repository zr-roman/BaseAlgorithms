
#include <iostream>
#include <vector>
#include "utils.h"
#include "strategy.h"
#include "singleton.h"
#include "singleton.cpp"

using namespace std;

void MainProgram()
{
    /*///////////////////////////////////////////-------FLYWEIGHT--------////////////////////////////////////////*/
    auto a = make_unique<char>('a');
    auto b = make_unique<char>('b');
    char s[10] = "ababababa";

    char* word[9] = { a.get(), b.get(), a.get(), b.get(), a.get(), b.get(), a.get(), b.get(), a.get() };
    
    /*
    vector<char*> str;
    str.push_back( a );
    str.push_back(b);
    str.push_back(a);
    str.push_back(a);
    str.push_back(b);

    cout << *str.at(3)<< endl;*/
    
    string s1 = "sdfsdf";


    /*///////////////////////////////////////////-------STRATEGY--------////////////////////////////////////////*/
    cout << "///////////////////////////////////////////-------STRATEGY--------////////////////////////////////////////";

    vector<int> arr {6,5,3,2,8,0,12,5,34,8,6,3};
            
    auto worker1 = make_unique<WorkerOnCollection<int>>( make_unique<BubbleSort<int>>( Order::ASC ) );
    worker1->DoSomeWorkOnCollection( arr );

    auto worker2 = make_unique<WorkerOnCollection<int>>( make_unique<ShellSort<int>>( Order::DESC ) );
    worker2->DoSomeWorkOnCollection( arr );

    worker2->SetSortingAlgorithm( make_unique<BruteForce<int>>( Order::ASC ) );
    worker2->DoSomeWorkOnCollection( arr );

    arr.push_back(24);
    arr.insert(arr.cbegin() + 4, 58);
    arr.insert(arr.cbegin() , 19);

    auto worker3 = make_unique<WorkerOnCollection<int>>();
    worker3->DoSomeWorkOnCollection( arr );

    /*///////////////////////////////////////////--------SINGLETON------////////////////////////////////////////*/

    cout << endl << "///////////////////////////////////////////--------SINGLETON------////////////////////////////////////////" << endl;

    auto ptr = make_unique<BubbleSortAscService<int>>( );
    auto ptr1 = make_unique<SelectionSortDescService<int>>();

    auto service1 = SortingService<int>::GetInstance( "BubbleSortAscService" );
    if ( service1 ) {
        service1->Sort( arr );
    } else {
        cout << "Сервис сортировки BubbleSortAscService не найден";
    }
    
    for (auto& item : arr) {
        cout << item << " ";
    }
    
    auto service2 = SortingService<int>::GetInstance( "BubbleSortDescService" );
    if ( service2 ) {
        service2->Sort( arr );
    } else {
        cout << endl << endl << "Service BubbleSortDescService not found";
        cout << endl;
    }

    arr.push_back(45);
    arr.insert(arr.cbegin() + 10, 99);
    arr.insert(arr.cbegin(), 102);

    service2 = SortingService<int>::GetInstance("BubbleSortAscService");
    if ( service2 ) {
        service2->Sort(arr);
    }
    else {
        cout << "Service BubbleSortAscService not found";
    }
    
    for (auto& item : arr) {
        cout << item << " ";
    }
    cout << endl;
    auto service3 = SortingService<int>::GetInstance("SelectionSortDescService");
    if (service3) {
        service3->Sort(arr);
    }
    else {
        cout << "Service SelectionSortDescService not found";
    }
    
    for (auto& item : arr) {
        cout << item << " ";
    }
    cout << endl;
}

int main() {

    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);

    MainProgram();

    _CrtDumpMemoryLeaks();
}
