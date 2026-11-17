#include "Header.h"

Person Person::operator+( const Person& other ) {
	shared_ptr<Person> tmp = make_shared<Person>( this->GetName(  ) + "+" + other.GetName() );
	return *(tmp.get());
};

shared_ptr<Person> Person::operator+( shared_ptr<Person> other ) {
	return make_shared<Person>( this->GetName() + "+" + other->GetName() );
};

Person& Person::operator+=( const Person& other ) {
	
	this->SetName( this->GetName() + "+" + other.GetName() );
	return *this;
};

// prefix
Person& Person::operator++() {
	this->SetName( this->GetName() + "+" + this->GetName() );
	return *this;
};

// postfix
// the postfix version takes a dummy int
Person Person::operator++(int) {
	auto tmp = *this;
	operator++();
	return tmp;
};

// prefix
Person& Person::operator--() {
	this->SetName( this->GetName().substr(0, this->GetName().size() - 1) );
	return *this;
};

// postfix
// the postfix version takes a dummy int
Person Person::operator--(int) {
	auto tmp = *this;
	operator--();
	return tmp;
};

Person Person::operator-( const Person& other ) {
	shared_ptr<Person> tmp = make_shared<Person>( this->GetName() + "-" + other.GetName() );
	return *(tmp.get());
};

Person& Person::operator-=( const Person& other ) {
	this->SetName( this->GetName() + "-" + other.GetName() );
	return *this;
};
