#pragma once

#include <iostream>
#include <string>
using namespace std;

class Person {
public:
    Person( string Name ) {
        _name = Name;
    };
    void SetName( string s ) {
        _name = s;
    };
    string GetName() const {
        return _name;
    };

    Person operator+( const Person& other );
    shared_ptr<Person> operator+( shared_ptr<Person> other );

    Person operator-( const Person& other );

    Person& operator+=( const Person& other );

    Person& operator-=( const Person& other );

    // prefix
    Person& operator++();

    // postfix
    // the postfix version takes a dummy int
    Person operator++(int);

    // prefix
    Person& operator--();
    
    // postfix
    // the postfix version takes a dummy int
    Person operator--(int);

private:
    string _name;

};
