
#include <iostream>
#include <thread>
#include <mutex>
#include <vector>
#include <random>

using namespace std;

class A {
    public:
        int i;

        A& operator++() {
            i++;
            return *this;
        };

        A& operator+(int c) {
            i += c;
            return *this;
        };

};

//atomic<size_t> i = 0;
size_t i = 0;

mutex m;

/// <summary>
/// Generates a random number
/// </summary>
/// <param name="start">: inclusive lower bound</param>
/// <param name="end">: inclusive upper bound</param>
/// <returns>A random number within a given range</returns>
int GenerateRandomNumber(const int& start, const int& end) {
    random_device dev;
    mt19937 rng( dev() );
    uniform_int_distribution<int> dist( start, end );
    auto res = dist( rng );
    return res;
};

atomic<A> a;
//A a;

void Add() {
    
    this_thread::sleep_for(std::chrono::milliseconds(GenerateRandomNumber(1,50)));

    /*
    //scoped_lock lock(m);    
    //a = a + 1;    
    lock_guard<mutex> lg(m);

    auto k = a.load().i;
    A b;
    b.i = ++k;
    a.exchange(b);*/
    i++;
};

int main()
{
    vector<thread> threads;

    for ( size_t i = 0; i < 100; i++ ) {
        
        threads.push_back(thread(Add));
    }
    
    for ( auto& th : threads ) {
        th.join();
    }

    cout << i << endl;
    
}
