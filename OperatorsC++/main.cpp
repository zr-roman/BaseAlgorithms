
#include <iostream>
#include <string>
#include "Header.h"
#include <memory>
using namespace std;



int main() {

    Person A ("Ivanov");
    Person B ("Petrov");
    shared_ptr<Person> D = make_shared<Person>("Sidorov");
    shared_ptr<Person> E = make_shared<Person>("Nikolaev");
    shared_ptr<Person> F = A + E;

    Person C = A + B + *D;

    cout << C.GetName() << endl;

    A += B;

    cout << A.GetName() << endl;

    cout << (++A).GetName() << endl;

    cout << (--*D).GetName() << endl;

    

}
