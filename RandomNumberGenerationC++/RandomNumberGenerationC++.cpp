
#include <iostream>
#include <windows.h>
#include<chrono>
#include<thread>
#include<map>
#include <set>
#include <unordered_set>

using namespace std;

double RandNumEx() {
    
    
    auto start = std::chrono::high_resolution_clock::now();
    
    double p = 25'173;
    double i = 13'849;
    double m = 65'536;
        
    double seed = 1;
    
    double res;

    do
    {
        int n = 31;
        seed = 1;
        double time;

        while (n > 0) {
            auto finish = chrono::high_resolution_clock::now();

            time = chrono::duration_cast<chrono::nanoseconds>(finish - start).count();
            if (time < 400)
            {
                time *= sqrt(time);
            }
            time = fmod(time / 100, 100);

            if (time < 1) {
                continue;
            }

            if (time < 10) {
                seed *= chrono::milliseconds(GetTickCount64()).count() & chrono::milliseconds(GetTickCount64()).count() * (ULONGLONG)time;
            }
            else if (time >= 10 && time < 20) {
                seed /*+*/ *= chrono::milliseconds(GetTickCount64()).count() | chrono::milliseconds(GetTickCount64()).count();
            }
            else if (time >= 20 && time < 30) {
                seed /= chrono::milliseconds(GetTickCount64()).count() ^ 65537 * (ULONGLONG)time;
            }
            else if (time >= 30 && time < 40) {
                seed /*+*/ *= chrono::milliseconds(GetTickCount64()).count() * chrono::milliseconds(GetTickCount64()).count() * (ULONGLONG)time;
            }
            else if (time >= 40 && time < 50) {
                seed *= chrono::milliseconds(GetTickCount64()).count() + chrono::milliseconds(GetTickCount64()).count() * (ULONGLONG)time;
            }
            else if (time >= 50 && time < 60) {
                start = std::chrono::high_resolution_clock::now();
                seed *= (chrono::milliseconds(GetTickCount64()).count() * chrono::milliseconds(GetTickCount64()).count() << 5);
            }
            else if (time >= 60 && time < 70) {
                seed /*+*/ *= chrono::milliseconds(GetTickCount64()).count() + chrono::milliseconds(GetTickCount64()).count();
            }
            else if (time >= 70 && time < 80) {
                seed /*-*/ *= chrono::milliseconds(GetTickCount64()).count() & (ULONGLONG)seed;
            }
            else if (time >= 80 && time < 90) {
                seed /*-*/ *= chrono::milliseconds(GetTickCount64()).count() | (ULONGLONG)seed;
            }
            else if (time >= 90 && time < 100) {
                seed /= chrono::milliseconds(GetTickCount64()).count() + chrono::milliseconds(GetTickCount64()).count();
            }

            n --;
        }

        seed = fmod(seed, 100'000);
    
        res = abs( (fmod((seed * p + i), m) ) / m );

        
        auto bb = (int)seed % (int)time;

        if (bb == 0) {
            res *= res;
        } else {
            res *= res * (double)bb;
        }

        while (res > 1)
        {
            res /= 10.0;
        }

        if (seed == 0.0 || seed == 1.0 || isnan(res))
        {
            continue;
        }
        return res;

    } while (true);
    return 0;
};

double RandNum() {
    
    double arr[5];
    double tmp;
    double e = 2.71828182845904523536028747135266249775724709369995;
    double pi = 3.14159265358979323846264338327950288419716939937510;

    while (true)
    {
        unordered_set<double> s = {};
        while (s.size() < 5)
        {
            s.insert(RandNumEx());
        }

        int n = 0;
        
        for (auto& r : s) {
            arr[n] = r;
            n++;
        }

        tmp = arr[0];
        arr[0] = arr[4];
        arr[4] = arr[1];
        arr[1] = arr[3];
        arr[3] = arr[2];
        arr[2] = tmp;

        double sum1 = arr[1] + arr[3];
        double sum2 = arr[0] + arr[4];

        auto res = arr[2] * sum1 * sum2;

        while (abs(res) > 1)
        {
            res /= 10.0;
        }
                
        if (arr[0] <= 0.5) {

            if (res < 0.01)
            {
                continue;
            }
            while (res < 0.1)
            {
                res *= 10.0;
            }
            if (res >= 0.25 && res < 0.5)
            {
                res *= e;
            }
            if (res >= 0.5 && res < 0.6)
            {
                res *= sqrt(2);
            }
        }
        if (res >= 0.1 && res < 0.2)
        {
            if (arr[0] <= 0.5)
            {
                res *= pi;
            } else if (arr[0] <= 0.85)
            {
                res *= e;
            }
        }

        while (abs(res) > 1)
        {
            res /= 10.0;
        }

        if (!isnan(res) && res != 0)
        {
            return abs(res);
        }
    }
    return 0;
};

int RandNumRange( int start, int end ) {
    return (end - start)* RandNum() + start;
};

int main()
{
    cout << chrono::milliseconds(GetTickCount64()).count() << endl << endl;

    set<double> set{};
    cout.precision(27);
    int dupCount = 0;
    int count0_ = 0;
    int count1_ = 0;
    int count2_ = 0;
    int count3_ = 0;
    int count4_ = 0;
    int count5_ = 0;
    int count6_ = 0;
    int count7_ = 0;
    int count8_ = 0;
    int count9_ = 0;
    int count10_ = 0;

    for (size_t i = 0; i < 1000; i++)
    {
        auto r = RandNum();
        if (set.count(r) != 0)
        {
            dupCount++;
            cout << "dup: ";
        }

        if ( r > 0 && r < 0.1) {
            count0_++;
        }else if (r >= 0.1 && r < 0.2) {
            count1_++;
        }
        else if (r >= 0.2 && r < 0.3) {
            count2_++;
        }
        else if (r >= 0.3 && r < 0.4) {
            count3_++;
        }
        else if (r >= 0.4 && r < 0.5) {
            count4_++;
        }
        else if (r >= 0.5 && r < 0.6) {
            count5_++;
        }
        else if (r >= 0.6 && r < 0.7) {
            count6_++;
        }
        else if (r >= 0.7 && r < 0.8) {
            count7_++;
        }
        else if (r >= 0.8 && r < 0.9) {
            count8_++;
        }
        else if (r >= 0.9 && r < 1.0) {
            count9_++;
        }
        else if (r >= 1) {
            count10_++;
        }

        set.insert(r);
        cout << r << endl;
    }

    cout << endl;
    cout << "Dup Count: " << dupCount << endl << endl;
    cout << "count0_: " << count0_ << endl;
    cout << "count1_: " << count1_ << endl;
    cout << "count2_: " << count2_ << endl;
    cout << "count3_: " << count3_ << endl;
    cout << "count4_: " << count4_ << endl;
    cout << "count5_: " << count5_ << endl;
    cout << "count6_: " << count6_ << endl;
    cout << "count7_: " << count7_ << endl;
    cout << "count8_: " << count8_ << endl;
    cout << "count9_: " << count9_ << endl << endl;

    cout << "Exceeds 1 : " << count10_ << endl << endl;

    map<int, int> qq = {};
    int counter = 0;
    while(qq.size() < 199)
    {
        //cout << RandNumRange(-100, 100) << endl;
        
        int i = RandNumRange(-100, 100);

        if (qq.count(i) != 0)
        {
            qq[i]++;
        }
        else
        {
            qq.insert({ i, 1 });
            
        }
        counter++;
    }

    for (auto& item: qq)
    {
        cout << item.first << " : " << item.second << endl;
    }

    cout << endl << endl;

    cout << "counter: " << counter << endl << endl;

    int n = 10;
    while (n > 0)
    {
                
        map<double, int> dic{};
        for (size_t i = 0; i < 10'000'000; i++)
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
        int dups = 0;
        for (auto& item : dic)
        {
            if ( item.second > maximum ) {
                maximum = item.second;
            }
            if (item.second > 1)
            {
                dups++;
                cout << item.first << " : " << item.second << endl;
            }
        }

        cout << endl << dups << endl;
        cout  << maximum << " " << endl << endl;
        n--;
    }
    
}
