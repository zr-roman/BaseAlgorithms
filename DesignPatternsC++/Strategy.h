// Класс SortingAlgorithm - это абстратктный класс. В его конкретных подклассах определены различные стратегии (алгоритмы) сотртировки.
// Класс SortingAlgorithm с подклассами называется "Стратегией".
// Класс WorkerOnCollection производит какие-то действия с коллекцией посредстом метода DoSomeWorkOnCollection, 
// который в свою очередь вызывает сортировщика
// Также это называется Dependency Injection

#pragma once
#include <vector>
#include "utils.h"
using namespace std;

#pragma region Классы, составляющие Стратегию

#pragma region Strategy (abstract)

template <typename T>
class SortingAlgorithm {
public:
	virtual void Sort( vector<T>& arr ) = 0;
protected:
	SortingAlgorithm( const Order& order );
	const Order& _order;
};

#pragma endregion

#pragma region Классы ConcreteStrategy

template <typename T>
class BubbleSort : public SortingAlgorithm<T> {
public:
	BubbleSort( const Order& order );
	virtual void Sort( vector<T>& arr ) override;
};

template <typename T>
class BruteForce : public SortingAlgorithm<T> {
public:
	BruteForce( const Order& order );
	virtual void Sort( vector<T>& arr );
};

template <typename T>
class ShellSort : public SortingAlgorithm<T> {
public:
	ShellSort( const Order& order );
	virtual void Sort( vector<T>& arr ) override;
};
#pragma endregion

#pragma endregion 

#pragma region класс Context, хранит ссылку на объект класса Strategy

template <typename T>
class WorkerOnCollection {
public:	
	WorkerOnCollection( unique_ptr<SortingAlgorithm<T>> sortingAlgorithm );
	WorkerOnCollection();
	void DoSomeWorkOnCollection( vector<T>& arr );
	void SetSortingAlgorithm( unique_ptr<SortingAlgorithm<T>> sortingAlgorithm );

private:
	unique_ptr<SortingAlgorithm<T>> _sortingAlgorithm;
};

#pragma endregion

#pragma region Implementation

template <typename T>
ShellSort<T>::ShellSort( const Order& order ) : SortingAlgorithm<T>( order ) {

};

template <typename T>
void ShellSort<T>::Sort( vector<T>& arr ) {
		
	cout << " Sorting ShellSort " << ToString(this->_order) << ": " << endl;

	auto o{this->_order};

	auto comparison_succeeds = [&o](const T& key, const T& comparant) -> bool {
		return ( o == Order::ASC && key < comparant ) || ( o == Order::DESC && key > comparant );
	};

	vector<int> gaps{5,4,3,2,1};

	for (const auto& gap : gaps) {

		for (auto i = 0 + gap; i < arr.size(); i += gap) {
			int key = arr.at(i);
			auto j = i - gap;
			while (j >= 0 && comparison_succeeds(key, arr.at(j))) {
				arr.at(j + gap) = arr.at(j);
				j -= gap;
			}
			arr.at(j + gap) = key;
		}
	}
};

template <typename T>
BruteForce<T>::BruteForce( const Order& order ): SortingAlgorithm<T>( order ) {

};

template <typename T>
void BruteForce<T>::Sort( vector<T>& arr ) {

	cout << " Sorting BruteForce " << ToString(this->_order) << ": " << endl;

	auto o{ this->_order };
	auto comparison_succeeds = [o]( T &Ti, T &Tj ) -> bool {
		return ( o == Order::ASC && Ti > Tj ) || ( o == Order::DESC && Ti < Tj );
	};

	for ( size_t i = 0; i < arr.size(); i++ ) {
		for ( size_t j = i; j < arr.size(); j++ ) {
			if ( comparison_succeeds( arr.at( i ), arr.at( j ) ) ) {
				swap( arr.at( i ), arr.at( j ) );
			}
		}
	}
};

template <typename T>
BubbleSort<T>::BubbleSort( const Order& order ): SortingAlgorithm<T>( order ) {

};

template <typename T>
void BubbleSort<T>::Sort( vector<T>& arr ) {
		
	cout << " Sorting BubbleSort " << ToString(this->_order)<< ": "<< endl;
		
	int work_area_size = arr.size();
	while ( work_area_size > 0 ) {
		for ( size_t i = 0; i + 1 < work_area_size; i++ ) {
			if ( ( this->_order == Order::ASC && arr.at( i ) > arr.at( i + 1 ) ) || ( this->_order == Order::DESC && arr.at( i ) < arr.at( i + 1 ) ) ) {
				swap( arr.at( i ), arr.at( i + 1 ) );
			}
		}
		work_area_size--;
	}
};

template <typename T>
SortingAlgorithm<T>::SortingAlgorithm( const Order& order ) : _order( order ) {

};

template <typename T>
void WorkerOnCollection<T>::DoSomeWorkOnCollection( vector<T>& arr ) {
	
	cout << endl << "--------------Starting over---------" << endl;

	// do some work

	// print unsorted
	for ( auto& item : arr ) {
		cout << item << " ";
	}

	cout << endl;

	// sort
	if ( _sortingAlgorithm ) {
		_sortingAlgorithm->Sort( arr );
	} else {
		cout << "Sorter absent. Sorting was not done" << endl;
	}

	// print sorted (if done)
	for ( auto& item : arr ) {
		cout << item << " ";
	}

	// do some esle work

	cout << endl;
};

template <typename T>
WorkerOnCollection<T>::WorkerOnCollection( unique_ptr<SortingAlgorithm<T>> sortingAlgorithm ) {
	_sortingAlgorithm = move( sortingAlgorithm );
};

template <typename T>
WorkerOnCollection<T>::WorkerOnCollection() {

};

template <typename T>
void WorkerOnCollection<T>::SetSortingAlgorithm( unique_ptr<SortingAlgorithm<T>> sortingAlgorithm ) {
	_sortingAlgorithm = move(sortingAlgorithm);
};

#pragma endregion
