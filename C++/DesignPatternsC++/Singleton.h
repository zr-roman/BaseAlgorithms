#pragma once
#include <string>
#include <map>
#include <vector>
#include <iostream>
#include "utils.h"

using namespace std;

template <typename T>
class SortingService {

public:
	static SortingService<T>* GetInstance( string name );
	static void Register( string name, SortingService<T>* service );
	//
	virtual void Sort( vector<T>& arr ) = 0;
	//
protected:
	Order _order;
private:
	static SortingService<T>* _instance;
	static map<string, SortingService<T>*> _registry; // реестр одиночек
};

template <typename T>
class BubbleSortAscService : public SortingService<T> {
public:
	BubbleSortAscService();
	virtual void Sort( vector<T>& arr ) override;
};

template <typename T>
class SelectionSortDescService : public SortingService<T> {
public:
	SelectionSortDescService();
	virtual void Sort( vector<T>& arr ) override;
};

#pragma region Impl

template <typename T>
SelectionSortDescService<T>::SelectionSortDescService() {
	this->_order = Order::DESC;
	SortingService<T>::Register( "SelectionSortDescService", this );
}

template <typename T>
void SelectionSortDescService<T>::Sort( vector<T>& arr ) {
	
	cout << endl << "Singleton SelectionSortDescService is working... " << endl;

	auto o{ this->_order };
    auto comparison_succeeds = [o](T key, T comparant) -> bool& {
        auto res = ( o == Order::ASC && comparant < key) || ( o == Order::DESC && comparant > key );
        return res;
    };

    for ( size_t i = 0; i < arr.size(); i++ ) {
        T key = arr.at( i );
        auto index = i;
        for ( size_t j = i + 1; j < arr.size(); j++ ) {
            if ( comparison_succeeds( key, arr.at( j ) ) ) {
                key = arr.at( j );
                index = j;
            }
        }
        if ( index != i ) {
            swap( arr.at( index ), arr.at( i ) );
        }
    }
};

template <typename T>
void BubbleSortAscService<T>::Sort( vector<T>& arr ) {
		
	cout << endl << "Singleton BubbleSortAscService is working... " << endl;
		
	int work_area_size = arr.size();
	while ( work_area_size > 0 ) {
		for ( size_t i = 0; i + 1 < work_area_size; i++ ) {
			if ( ( this-> _order == Order::ASC && arr.at( i ) > arr.at( i + 1 ) ) || ( this->_order == Order::DESC && arr.at( i ) < arr.at( i + 1 ) ) ) {
				swap( arr.at( i ), arr.at( i + 1 ) );
			}
		}
		work_area_size--;
	}
};

template <typename T>
BubbleSortAscService<T>::BubbleSortAscService( ) {
	this->_order = Order::ASC;
	SortingService<T>::Register( "BubbleSortAscService", this );
};

template <typename T>
void SortingService<T>::Register( string name, SortingService<T>* service ) {
	if ( SortingService<T>::_registry.size() == 0 || ( SortingService<T>::_registry.size() != 0 && SortingService<T>::_registry.find( name ) == SortingService<T>::_registry.end() ) ) {
		auto pair = make_pair( name, service );
		SortingService<T>::_registry.insert( pair );
	}
};

template <typename T>SortingService<T>* SortingService<T>::_instance = nullptr;

template <typename T> map<string, SortingService<T>*> SortingService<T>::_registry = { {"", nullptr} };

template <typename T>
SortingService<T>* SortingService<T>::GetInstance(string name) {

	auto it = SortingService<T>::_registry.find( name );

	if ( it != SortingService<T>::_registry.end() ) {
		SortingService<T>::_instance = it->second;
	} else {
		SortingService<T>::_instance = nullptr;
	}
	
	return SortingService<T>::_instance;
};

//static BubbleSortAscService<int> theService1; // компилируется, но падает при вставке значений в map
//static SelectionSortDescService<int> theService2; // компилируется, но падает при вставке значений в map

#pragma endregion
