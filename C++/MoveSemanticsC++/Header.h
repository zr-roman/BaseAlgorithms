#pragma once

#include <iostream>
#include <string>
#include <vector>

using namespace std;

// Rule of Three
// The rule of three (also known as the Law of The Big Three or The Big Three) is a rule of thumb in C++ (prior to C++11) 
// that claims that if a class defines any of the following then it should probably explicitly define all three:

// 1. destructor
// 2. copy constructor
// 3. copy assignment operator

// Rule of Five

// With the advent of C++11 the rule of three can be broadened to the rule of five (also known as the rule of big 5) 
// as C++11 implements move semantics, allowing destination objects to grab(or steal) data from temporary objects.

// 1. destructor
// 2. copy constructor
// 3. copy assignment operator
// 4. move constructor
// 5. move assignment operator
 
class Person {
public:

	// default constructor
	Person() {
        _name = "<default>";
        _father_ptr = nullptr;
    };

	// parameterized constructor
	Person( string name, Person* father ) {
		_name = name;
        _father_ptr = father;
	};

	// destructor
	~Person() {
		cout << "Destroy " << _name << endl;
	};

    // Copy assignment operator
    Person& operator=( const Person& other ) {
        // Guard self assignment
        if ( this == &other ) {
            return *this;
        }

        // shallow copy
		/*
		this->_name = other._name;
		this->_father_ptr = other._father_ptr; 
		*/

		// deep copy
		this->_name = other._name;

		auto p1 = other._father_ptr;
		auto p2 = this;
		
		while ( p1 != nullptr ) {
			p2->_father_ptr = new Person(p1->_name, nullptr);
            pointers_registry.push_back( p2->_father_ptr );
			p1 = p1->_father_ptr;
			p2 = p2->_father_ptr;
		}
        return *this;
    };

	// Copy constructor
	Person( const Person& other ) {
		
		// shallow copy
		/*
		this->_name = other._name;
		this->_father_ptr = other._father_ptr; 
		*/

		// deep copy
		this->_name = other._name;

		auto p1 = other._father_ptr;
		auto p2 = this;
		
		while ( p1 != nullptr ) {
            pointers_registry.push_back( new Person( p1->_name, nullptr ) );
            p2->_father_ptr = pointers_registry.at( pointers_registry.size() - 1 );
			p1 = p1->_father_ptr;
			p2 = p2->_father_ptr;
		}
	};

    // Move assignment operator (with &&)
    Person& operator=( Person&& p ) noexcept {
        
        // Guard self assignment
        if ( this == &p ) {
            return *this;
        }

        this->_father_ptr = p._father_ptr;
        this->_name = p._name;

        p._father_ptr = nullptr;
        p._name.clear();
        
        return *this;
    };

    // Move constructor (with &&)
    Person( Person&& p ) noexcept {
        *this = move( p ); // вызовется Move assignment operator
    };

    // std::forward предназначен исключительно для написания шаблонных методов, способных принимать на вход как lvalue, так и rvalue, 
    // в зависимости от того, что передал пользователь, и передавать соответствующий тип далее без изменений. 
    // Техника получила название perfect forwarding.
    friend Person operator+(Person&& a, const Person& b) {
        Person t( forward<Person>( a ) );
        t._name = a._name + b._name;
        return t;
    };

	void ChangeName(string name) {
		this->_name = name;
	};
    
    static void ClearRegistry() {
        for (size_t i = 0; i < pointers_registry.size(); i++) {
            if ( pointers_registry[ i ] ) {
                delete pointers_registry[ i ];
            }
        }
        pointers_registry.clear();
        pointers_registry.resize(0);
        pointers_registry.shrink_to_fit();
    };

private:
	string _name;
	Person* _father_ptr;
    static vector<Person*> pointers_registry;
};

template <typename T>
class my_vector {

public:

    my_vector() {
        arr.clear();
    };

    ~my_vector() {
        arr.clear();
    };

    // Copy Constructor
    my_vector(const my_vector<T>& v) {
        // arr = v; compile error
        arr.resize( v.GetSize() );
        for ( size_t i = 0; i < arr.size(); i++ ) {
            arr[i] = v[i];
        }
    };

    // Move Constructor
    my_vector( my_vector&& other ) {
        this->arr = move( other.arr );
    };

    // Constructor through list initializer
    my_vector( initializer_list<T> l ) : arr(l) {

    };

    // read-only, no assignment is possible
    const T& operator[]( unsigned int i ) const {
        return arr.at( i );
    };

    // assignment is possible, not read-only
    T& operator[]( unsigned int i ) {
        if ( arr.size() < i + 1 ) {
            arr.resize(i + 1);
        }
        return arr.at(i);
    };

    // Move assignment operator
    my_vector& operator=( my_vector&& other ) { // note the rvalue reference
        
        // Guard self assignment
        if ( this == &other ) {
            return *this; 
        }

        arr = other.arr;
        other.arr.clear();
        other.arr.resize(0);
        other.arr.shrink_to_fit();
        
        return *this;
    };
    
    unsigned int GetSize() const {
        return arr.size();
    };

private:
    vector<T> arr;
};
