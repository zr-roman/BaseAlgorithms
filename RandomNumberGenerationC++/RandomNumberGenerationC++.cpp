
#include <iostream>
#include <windows.h>
#include<chrono>
#include<thread>
#include<map>

using namespace std;

double RandNumEx() {
    
    auto start = std::chrono::high_resolution_clock::now();
    
    double p = 25'173;
    double i = 13'849;
    double m = 65'536;

    int n = 97;
    double seed = 1;

    while ( n > 0 ) {
        auto finish = chrono::high_resolution_clock::now();
    
        double time = chrono::duration_cast<chrono::nanoseconds>(finish - start).count() ;
        time = fmod(time/100, 100);        

        if (time < 1 ) {
            continue;
        }
        
        if ( time < 10 ) {
            seed *= chrono::milliseconds(GetTickCount64()).count();
        } else if( time >= 10 && time < 20 ) {
            seed += chrono::milliseconds(GetTickCount64()).count();
        } else if( time >= 20 && time < 30 ) {
            seed /= chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >= 30 && time < 40 ) {
            seed += chrono::milliseconds(GetTickCount64()).count() * chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >= 40 && time < 50 ) {
            seed *= chrono::milliseconds(GetTickCount64()).count() + chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >=50 && time < 60 ) {
            seed *= chrono::milliseconds(GetTickCount64()).count()* chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >=60 && time < 70 ) {
            seed += chrono::milliseconds(GetTickCount64()).count()+ chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >= 70 && time < 80 ) {
            seed -= chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >=80 && time < 90 ) {
            seed -= chrono::milliseconds(GetTickCount64()).count()- chrono::milliseconds(GetTickCount64()).count();
        } else if ( time >= 90 && time < 100 ) {
            seed /= chrono::milliseconds(GetTickCount64()).count()+ chrono::milliseconds(GetTickCount64()).count();
        }
    
        n--;
    }

    seed = fmod( seed, 100000 );
   
    auto res = abs( (fmod((seed * p + i), m) ) / m );
    return res;
};

double RandNum() {
    auto res = RandNumEx() + RandNumEx() * RandNumEx();
    return res > 1 ? res - 1 : res;
};

int RandNumRange( int start, int end ) {
    return (end - start)* RandNum() + start;
};

int main()
{
    cout << GetTickCount64() << endl;
    for (size_t i = 0; i < 10; i++)
    {
        cout << RandNum() << endl;
    }

    cout << endl;

    int n = 15;
    while (n > 0)
    {
        cout << RandNumRange(-100, 100) << endl;

        /*map<double, int> dic{};
        for (size_t i = 0; i < 1000; i++)
        {
            auto s = RandNum();
            if (dic.count(s) != 0)
            {
                dic[s]++;
                continue;
            }

            dic.insert({ s, 1 });
        }
        int maximum = 0;
        for (auto& item : dic)
        {
            if ( item.second > maximum ) {
                maximum = item.second;
            }
        }

        cout << maximum << " ";*/
        n--;
    }
    
}
