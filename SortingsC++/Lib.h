#ifndef LIB_H
#define LIB_H

#include <vector>
#include <random>
#include <iostream>
#include<stdexcept>


using namespace std;

/*//////////////////////////// HEADERS ///////////////////////////////*/

class RandomizationLib {
    public:
        static int GenerateRandomNumber( const int& start, const int& end );
};

class SortingLib {

    // -------------------------------- Public declarations ------------------------------------------------
	public:

        enum class Order {
            ASC,
            DESC
        };

        enum class Pivot {
            FIRST,
            LAST,
            RANDOM
        };

        template<typename T>
        static void ShellSort( vector<T>* arr, const Order& order = Order::ASC );
                
        template<typename T>
        static void QuickSort( vector<T>* arr, const Order& order = Order::ASC, const Pivot& pivot = Pivot::RANDOM );
        
        template<typename T>
        static void HeapSort( vector<T>* arr, const Order& order = Order::ASC );
        
        template<typename T>
        static void MergeSort( vector<T>* arr, const T& positiveInfinity, const T& negativeInfinity, const Order& order = Order::ASC );
        
        template <typename T>
        static void BubbleSort( vector<T>* arr, const Order& order = Order::ASC );
        
        template<typename T>
        static void InsertionSort( vector<T>* arr, const Order& order = Order::ASC );
        
        template <typename T>
        static void SelectionSort( vector<T>* arr, const Order& order = Order::ASC );

        template <typename T>
        static void CombSort( vector<T>* arr, const Order& order = Order::ASC );

        template <typename T>
        static void BruteForceSort( vector<T>* arr, const Order& order = Order::ASC );
                
        static void RadixSort( vector<int>* arr, const Order& order = Order::ASC );
        
        static void CountingSort( vector<int>* arr, const Order& order = Order::ASC );
        
        static void BucketSort( vector<int>* arr, const Order& order = Order::ASC );
    
    // -------------------------------- Private declarations -----------------------------------------------
    private:

        template <class T>
        class SortInner;
                                
        template <class T>
        class QuickSortInner;

        template <class T>
        class HeapSortInner;

        template <class T>
        class MergeSortInner;

        template <class T>
        class BruteForceSortInner;

        template <class T>
        class SelectionSortInner;

        template <class T>
        class CombSortInner;

        template <class T>
        class BubbleSortInner;

        template <class T>
        class ShellSortInner;

        class LinearTimeSort;
        
        class CountingSortInner;

        class RadixSortInner;

        static vector<int> gaps;

        static double shrinkFactor;

        template <typename T>
        static T GetGreatestKeyValue( const vector<T>& arr );

        // перегрузка 1
        template <class T>
        static void DoPerformSort( unique_ptr<SortingLib::SortInner<T>> ptr );

        // перегрузка 2
        template <class T>
        static void DoPerformSort( unique_ptr<SortingLib::SortInner<T>, void (*)(SortingLib::SortInner<T>*)> ptr );
};

/*////////////////////////////// Class SortInner ///////////////////////////////*/

template <typename T>
class SortingLib::SortInner {
    
    public:
        virtual void Sort( const unsigned int& start, const unsigned int& end ) final;
        size_t GetArraySize() { return !arr ? 0 : arr->size(); };
        string GetName() const { return _name; };
        virtual ~SortInner() { };
    protected:
        SortInner( vector<T>* arr, const Order& order, string&& name = ""); //.ctor
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) = 0;
        vector<T>* arr;
        const Order order;
        bool already_sorted;

    private:
        const string _name;
};

/*////////////////////////////// Class QuickSortInner ///////////////////////////////*/

template <class T>
class SortingLib::QuickSortInner: public SortingLib::SortInner<T> {

    public:
        QuickSortInner( vector<T>* arr, const Order& order, const Pivot& pivot ); //.ctor
             
    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
        unsigned int PartitionFirst( const unsigned int& start, const unsigned int& end );
        unsigned int PartitionLast( const unsigned int& start, const unsigned int& end );
        unsigned int PartitionRandom( const unsigned int& start, const unsigned int& end ); // Las Vegas version of QuickSort
        bool comparison_succeeds( const size_t& j, const T& pivot );
        const Pivot pivot;
};

/*////////////////////////////// Class MergeSortInner ///////////////////////////////*/

template <class T>
class SortingLib::MergeSortInner : public SortingLib::SortInner<T> {

    public:
        MergeSortInner( vector<T>* arr, const T& positiveInfinity, const T& negativeInfinity, const Order& order ) : SortInner<T>( arr, order ), negativeInfinity( negativeInfinity ), positiveInfinity( positiveInfinity ) { };

    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
        void Merge( const size_t& start, const size_t& mid, const size_t& end );
        const T positiveInfinity;
        const T negativeInfinity;
};

/*////////////////////////////// Class HeapSortInner ///////////////////////////////*/

template <class T>
class SortingLib::HeapSortInner : public SortingLib::SortInner<T> {

    public:
        HeapSortInner( vector<T>* arr, const Order& order ) : SortInner<T>( arr, order ) { }; //.ctor

    private: 
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
        void BuildHeap();
        void Heapify( const size_t& i, const size_t& heapSize );
};

/*////////////////////////////// Class BruteForceSortInner ///////////////////////////////*/

template <typename T>
class SortingLib::BruteForceSortInner : public SortingLib::SortInner<T> {

    public:
        BruteForceSortInner( vector<T>* arr, const Order& order ) : SortInner<T>( arr, order, "BruteForce" ) { };

    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
};

/*////////////////////////////// Class SelectionSortInner ///////////////////////////////*/

template <typename T>
class SortingLib::SelectionSortInner : public SortingLib::SortInner<T> {

    public:
        SelectionSortInner( vector<T>* arr, const Order& order ) : SortInner<T>( arr, order ) { };

    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
};

/*////////////////////////////// Class ShellSortInner ///////////////////////////////*/

template <typename T>
class SortingLib::ShellSortInner : public SortingLib::SortInner<T> {

    public:
        ShellSortInner( vector<T>* arr, const Order& order, const vector<int>& gaps ) : SortingLib::SortInner<T>( arr, order ), gaps(gaps) { };

    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
        const vector<int> gaps;
};

/*////////////////////////////// Class CombSortInner ///////////////////////////////*/

template <typename T>
class SortingLib::CombSortInner : public SortingLib::SortInner<T> {

    public:
        CombSortInner( vector<T>* arr, const Order& order ) : SortInner<T>( arr, order ) { };

    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
};

/*////////////////////////////// Class BubbleSortInner ///////////////////////////////*/

template <typename T>
class SortingLib::BubbleSortInner : public SortingLib::SortInner<T> {

    public:
        BubbleSortInner( vector<T>* arr, const Order& order ) : SortInner<T>( arr, order ) { };

    private:
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
};

/*///////////////////////////////////////////////////// IMPLEMENTATION ////////////////////////////////////////////////////*/

template<typename T>
void SortingLib::InsertionSort(vector<T>* arr, const Order& order) {
    // InsertionSort - it is a ShellSort with the only one gap = 1
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::ShellSortInner<T>>( arr, order, [](){ vector<int> g{ 1 }; return g; }() ) ) );
}

template<typename T>
void SortingLib::ShellSort(vector<T>* arr, const Order& order) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::ShellSortInner<T>>( arr, order, SortingLib::gaps ) ) );
}

template<typename T>
void SortingLib::BubbleSort( vector<T>* arr, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::BubbleSortInner<T>>( arr, order ) ) );
}

template<typename T>
void SortingLib::CombSort( vector<T>* arr, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::CombSortInner<T>>( arr, order ) ) );
};

template<typename T>
void SortingLib::SelectionSort( vector<T>* arr, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::SelectionSortInner<T>>( arr, order ) ) );
};

template <typename T>
void delSort( SortingLib::SortInner<T>* sort_ptr ) {
    cout << "Deleting " << sort_ptr->GetName() << endl;
    delete sort_ptr;
}

template<typename T>
void SortingLib::DoPerformSort( unique_ptr<SortingLib::SortInner<T>, void (*)(SortingLib::SortInner<T>*)> ptr ) {
    ptr->Sort( 0, ptr->GetArraySize() - 1 );
}

template <typename T>
void SortingLib::BruteForceSort( vector<T>* arr, const Order& order ) {

    unique_ptr<SortingLib::SortInner<T>, void (*)(SortingLib::SortInner<T>*)> p( nullptr, delSort );
    p.reset( new SortingLib::BruteForceSortInner<T>( arr, order ));

    DoPerformSort( move( p ) );
};

template<typename T>
void SortingLib::DoPerformSort( unique_ptr<SortingLib::SortInner<T>> ptr ) {
    ptr->Sort( 0, ptr->GetArraySize() - 1 );
}

template<typename T>
void SortingLib::QuickSort( vector<T>* arr, const Order& order, const Pivot& pivot ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::QuickSortInner<T>>( arr, order, pivot ) ) );
}

template<typename T>
void SortingLib::MergeSort( vector<T>* arr, const T& positiveInfinity, const T& negativeInfinity, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::MergeSortInner<T>>( arr, positiveInfinity, negativeInfinity, order ) ) );
}

template<typename T>
void SortingLib::HeapSort( vector<T>* arr, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<T>> ( make_unique<SortingLib::HeapSortInner<T>>( arr, order ) ) );
}

template <typename T>
SortingLib::SortInner<T>::SortInner( vector<T>* arr, const SortingLib::Order& order, string&& name ) : order( order ), _name(move(name)) {
            
    if ( !arr || arr->size() == 0 ) {
        throw runtime_error("Error: null or emply array not allowed!");
    }
            
    already_sorted = true;

    if ( order == Order::ASC ) {
        for ( size_t i = 0; i + 1 < arr->size(); i++ ) {
            if ( arr->at( i ) > arr->at( i + 1 ) ) {
                already_sorted = false;
                break;
            }
        }
    } else  if ( order == Order::DESC ) {
        for ( size_t i = 0; i + 1 < arr->size(); i++ ) {
            if ( arr->at( i ) < arr->at( i + 1 ) ) {
                already_sorted = false;
                break;
            }
        }
    }

    this->arr = arr;
};

template <typename T>
void SortingLib::SortInner<T>::Sort( const unsigned int& start, const unsigned int& end ) {
    if ( already_sorted ) {
        return;
    }
    DoSort( start, end );
};

template <typename T>
SortingLib::QuickSortInner<T>::QuickSortInner( vector<T>* arr, const Order& order, const Pivot& pivot ) : SortingLib::SortInner<T>( arr, order ), pivot( pivot ) {
    // randomization: shuffle up the input
    for ( size_t i = 0; i < this->arr->size(); i++ ) {
        auto random_index = RandomizationLib::GenerateRandomNumber( 0, this->arr->size() - 1 );
        swap( this->arr->at( i ), this->arr->at( random_index ) );
    }
};

template <typename T>
void SortingLib::QuickSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {

    if ( start >= end ) {
        return;
    }

    unsigned int q;
    switch ( this->pivot ) {
        case Pivot::FIRST:
            q = this->PartitionFirst( start, end );
            break;
        case Pivot::LAST:
            q = this->PartitionLast( start, end );
            break;
        case Pivot::RANDOM:
            q = this->PartitionRandom( start, end );
            break;
        default:
            throw runtime_error( "Must not hit into here" );
    }

    if ( q != 0 ) { // unsigned int min value
        DoSort( start, q - 1 );
    }
        
    if ( q != 0xffffffff ) { // unsigned int max value
        DoSort( q + 1, end );
    }
};

template <typename T>
unsigned int SortingLib::QuickSortInner<T>::PartitionFirst( const unsigned int& start, const unsigned int& end ) {
    auto pivot = this->arr->at( start );
    auto i = start;
    for ( size_t j = start + 1; j <= end; j++ ) {
        if ( comparison_succeeds( j, pivot ) ) {
            i++;
            swap( this->arr->at( i ), this->arr->at( j ) );
        }
    }
    swap( this->arr->at( i ), this->arr->at( start ) );
    return i;
};

template <typename T>
unsigned int SortingLib::QuickSortInner<T>::PartitionLast( const unsigned int& start, const unsigned int& end ) {

    auto pivot = this->arr->at( end );
    auto i = start - 1;
    for ( size_t j = start; j < end; j++ ) {
        if ( comparison_succeeds( j, pivot ) ) {
            i++;
            swap( this->arr->at( i ), this->arr->at( j ) );
        }
    }
    swap( this->arr->at( i + 1 ), this->arr->at( end ) );
    return i + 1;
};

// Las Vegas version of QuickSort
template <typename T>
unsigned int SortingLib::QuickSortInner<T>::PartitionRandom( const unsigned int& start, const unsigned int& end ) {
    auto randomIndex = RandomizationLib::GenerateRandomNumber( start, end );
    swap( this->arr->at( end ), this->arr->at( randomIndex ) );
    return PartitionLast( start, end );
};

template <typename T>
bool SortingLib::QuickSortInner<T>::comparison_succeeds( const size_t& j, const T& pivot ) {
    return ( this->order == Order::ASC && this->arr->at( j ) < pivot ) || ( this->order == Order::DESC && this->arr->at( j ) > pivot );
}

template <typename T>
void SortingLib::MergeSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {
            
    if ( start >= end ) {
        return;
    }

    auto mid = ( start + end ) / 2;
            
    DoSort( start, mid );
    if ( mid != 0xffffffff) { // max value for unsigned int
        DoSort( mid + 1, end );
    }
            
    Merge( start, mid, end );
};

template <typename T>
void SortingLib::MergeSortInner<T>::Merge( const size_t& start, const size_t& mid, const size_t& end ) {
            
    auto n1 = mid - start + 1;
    auto n2 = end - mid;

    vector<T> arrayB( n1 + 1 );
    vector<T> arrayC( n2 + 1 );
                        
    auto bi=0, ci = 0;
            
    for ( size_t i = start; i <= mid; i++ ) {
        arrayB.at( bi ) = this->arr->at( i );
        bi++;
    }
    arrayB[ arrayB.size() - 1 ] = this->order == Order::ASC ? positiveInfinity : negativeInfinity;

    for ( size_t i = mid + 1; i <= end; i++ ) {
        arrayC.at( ci ) = this->arr->at( i );
        ci++;
    }
    arrayC[ arrayC.size() - 1 ] = this->order == Order::ASC ? positiveInfinity : negativeInfinity;

    bi = ci = 0;
    for ( size_t i = start; i <= end; i++ ) {
        if ( ( this->order == Order::ASC && arrayB.at( bi ) < arrayC.at( ci ) ) || ( this->order == Order::DESC && arrayB.at( bi ) > arrayC.at( ci ) ) ) {
            this->arr->at( i ) = arrayB.at( bi );
            bi++;
        } else {
            this->arr->at( i ) = arrayC.at( ci );
            ci++;
        }
    }
};

template <typename T>
void SortingLib::HeapSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {
    BuildHeap();
    size_t heapSize = this->arr->size();
    while( heapSize > 1 ) {
        swap( this->arr->at( 0 ), this->arr->at( heapSize - 1 ) );
        heapSize--;
        Heapify( 0, heapSize );
    }
};

template <typename T>
void SortingLib::HeapSortInner<T>::BuildHeap() {
    for ( int i = this->arr->size() / 2 - 1; i >= 0; i-- ) {
        Heapify( i, this->arr->size() );
    }
};

template <typename T>
void SortingLib::HeapSortInner<T>::Heapify( const size_t& i, const size_t& heapSize ) {

    auto indexLeftChild = i * 2 + 1;
    auto indexRightChild = i * 2 + 2;

    size_t indexBiggestOrSmallestElm;

    if ( indexLeftChild < heapSize && ( ( this->order == Order::ASC && this->arr->at( indexLeftChild ) > this->arr->at( i ) || ( this->order == Order::DESC && this->arr->at( indexLeftChild ) < this->arr->at( i ) ) ) ) ) {
        indexBiggestOrSmallestElm = indexLeftChild;
    } else {
        indexBiggestOrSmallestElm = i;
    }

    if ( indexRightChild < heapSize && ( ( this->order == Order::ASC && this->arr->at( indexRightChild ) > this->arr->at( indexBiggestOrSmallestElm ) || ( this->order == Order::DESC && this->arr->at( indexRightChild ) < this->arr->at( indexBiggestOrSmallestElm ) ) ) ) ) {
        indexBiggestOrSmallestElm = indexRightChild;
    }

    if ( indexBiggestOrSmallestElm == i ) {
        return;
    }
    swap( this->arr->at( i ), this->arr->at( indexBiggestOrSmallestElm ) );
    Heapify( indexBiggestOrSmallestElm, heapSize );
};

template <typename T>
T SortingLib::GetGreatestKeyValue( const vector<T>& arr ) {

    if ( arr.size() == 0 ) {
        throw runtime_error("Empty or null array is not allowed!");
    }

    // вычисление максимального значения ключа
    T greatest = arr.at( 0 );

    for ( auto it = arr.cbegin(); it < arr.cend(); it++ ) { // итератор
        if ( *it < 0 ) {
            throw runtime_error("Negative array keys are not allowed");
        }
        if ( *it > greatest ) {
            greatest = *it;
        }
    }

    return greatest;
};

template<typename T>
void SortingLib::ShellSortInner<T>::DoSort(const unsigned int& start, const unsigned int& end) {

    auto o{ this->order };
    auto comparison_succeeds = [&o](const T& key, const T& comparant) -> bool {
        return (o == Order::ASC && key < comparant) || (o == Order::DESC && key > comparant);
    };

    for (const auto& gap : this->gaps) {

        for (auto i = 0 + gap; i < this->arr->size(); i++) {
            auto key = this->arr->at(i);
            auto j = i - gap;
            while (j >= 0 && comparison_succeeds(key, this->arr->at(j))) {
                this->arr->at(j + gap) = this->arr->at(j);
                j -= gap;
            }
            this->arr->at(j + gap) = key;
        }
    }
};

template<typename T>
void SortingLib::BubbleSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {
    int work_area_size = this->arr->size();
    while ( work_area_size > 1 ) {
        for ( size_t i = 0; i + 1 < work_area_size; i++ ) {
            if ( ( this->order == Order::ASC && this->arr->at( i ) > this->arr->at( i + 1 ) ) || ( this->order == Order::DESC && this->arr->at( i ) < this->arr->at( i + 1 ) ) ) {
                swap( this->arr->at( i ), this->arr->at( i + 1 ) );
            }
        }
        work_area_size--;
    }
};

template<typename T>
void SortingLib::CombSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {

    int step = this->arr->size() - 1;
    
    while ( step >= 1 ) {
        
        bool swap_occured = false;
        
        for ( size_t i = 0; i + step < this->arr->size(); i++ ) {
            if ( ( this->order == Order::ASC && this->arr->at( i ) > this->arr->at( i + step ) || ( this->order == Order::DESC && this->arr->at( i ) < this->arr->at( i + step ) ) ) ) {
                swap( this->arr->at( i ), this->arr->at( i + step ) );
                swap_occured = true;
            }
        }
        if ( step == 1 && swap_occured ) {
            continue;
        }
        
        step /= SortingLib::shrinkFactor;
    }
};

template<typename T>
void SortingLib::SelectionSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {

    auto o{ this->order };
    auto comparison_succeeds = [o](T key, T comparant) -> bool& {
        auto res = ( o == Order::ASC && comparant < key) || ( o == Order::DESC && comparant > key );
        return res;
    };

    for ( size_t i = 0; i < this->arr->size(); i++ ) {
        auto iMin = i;
        for ( size_t j = i + 1; j < this->arr->size(); j++ ) {
            if ( comparison_succeeds( this->arr->at( iMin ), this->arr->at( j ) ) ) {
                iMin = j;
            }
        }
        if ( iMin != i ) {
            swap( this->arr->at( i ), this->arr->at( iMin ) );
        }
    }
};

template<typename T>
void SortingLib::BruteForceSortInner<T>::DoSort( const unsigned int& start, const unsigned int& end ) {

    auto o{ this->order };
    auto comparison_succeeds = [o]( T &Ti, T &Tj ) -> bool {
        return ( o == Order::ASC && Ti > Tj ) || ( o == Order::DESC && Ti < Tj );
    };

    for ( size_t i = 0; i < this->arr->size(); i++ ) {
        for ( size_t j = i; j < this->arr->size(); j++ ) {
            if ( comparison_succeeds( this->arr->at( i ), this->arr->at( j ) ) ) {
                swap( this->arr->at( i ), this->arr->at( j ) );
            }
        }
    }
};

#endif