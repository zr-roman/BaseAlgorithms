#include "Lib.h"
#include <vector>
#include <algorithm>
#include <climits>
#include <iterator>

using namespace std;

vector<int> SortingLib::gaps = { 1750, 701, 301, 132, 57, 23, 10, 4, 1 }; //{ 5,4,3,2,1 };

double SortingLib::shrinkFactor = 1 / ( 1 - exp( -( (sqrt(5) + 1) / 2 ) ) ); // ≈ 1.2473309501039787

/// <summary>
/// Generates a random number
/// </summary>
/// <param name="start">: inclusive lower bound</param>
/// <param name="end">: inclusive upper bound</param>
/// <returns>A random number within a given range</returns>
int RandomizationLib::GenerateRandomNumber( int start, int end ) {
    thread_local std::mt19937 rng( std::random_device{}() ); // засевается один раз на поток
    std::uniform_int_distribution<int> dist( start, end );   // создавать дёшево, можно каждый раз
    auto res = dist( rng );
    return res;
};
// В данном случае возврат происходит by value, что означает, что int(4 байта в 32-разрядной системе) будет скопирован при возврате.
// Возвращаемый тип int& означает, что будет возвращена постоянная ссылка на int(4 байта в 32-разрядной системе).
// Как видно, в этом случае нет никакого преимущества в производительности при использовании return-by-reference вместо return-by-value.

class SortingLib::LinearTimeSort : public SortingLib::SortInner<int> {

    public:
        LinearTimeSort( vector<int>* arr, const SortingLib::Order& order ) : SortInner<int>( arr, order ) { };
        virtual void DoSort( const unsigned int& start, const unsigned int& end ) override;
        virtual void SpecificSort( vector<int>* arr, const int& radix_param = 0 ) = 0; // GoF template method
};

class SortingLib::CountingSortInner : public SortingLib::LinearTimeSort {

    public:
        CountingSortInner( vector<int>* arr, const SortingLib::Order& order ) : LinearTimeSort( arr, order ) { };
        virtual void SpecificSort( vector<int>* arr, const int& radix_param = 0 ) override; // GoF template method
};

class SortingLib::RadixSortInner : public SortingLib::LinearTimeSort {

    public: 
        RadixSortInner( vector<int>* arr, const Order& order );

    protected:
        virtual void SpecificSort( vector<int>* arr, const int& = 0 ) override; // GoF template method

    private:
        unique_ptr<SortingLib::CountingSortInner> _counting_sort;
};

void SortingLib::LinearTimeSort::DoSort( const unsigned int&, const unsigned int& ) {

    vector<int> negativeArray;
    vector<int> positiveArray;

    for ( size_t i = 0; i < arr->size(); i++ ) {
        if ( arr->at( i ) < 0 )
            negativeArray.push_back( ~arr->at( i ) ); // ~x == -x - 1: never overflows, even for INT_MIN
        else 
            positiveArray.push_back( arr->at( i ) );
    }
    
    SpecificSort( &negativeArray );
    SpecificSort( &positiveArray );
    
    int index = 0;
    if ( order == SortingLib::Order::ASC ) {
        for ( std::ptrdiff_t i = std::ssize( negativeArray ) - 1; i >= 0; i-- ) { // std::ssize — C++20
            arr->at( index ) = ~negativeArray.at( i );
            index++;
        }
        for ( size_t i = 0; i < positiveArray.size(); i++ ) {
            arr->at( index ) = positiveArray.at( i );
            index++;
        }
    } else {
        for ( std::ptrdiff_t i = std::ssize( positiveArray ) - 1; i >= 0; i-- ) { // std::ssize — C++20
            arr->at( index ) = positiveArray.at( i );
            index++;
        }
        for ( size_t i = 0; i < negativeArray.size(); i++ ) {
            arr->at( index ) = ~negativeArray.at( i );
            index++;
        }
    }
};

void SortingLib::CountingSortInner::SpecificSort( vector<int>* arr, const int& radix_param ) {

    auto func = [radix_param]( int key ) {
        return radix_param != 0 ? key / radix_param % 10 : key;
    };

    if ( !arr || arr->size() == 0 ) {
        return;
    }

    // вычисление максимального значения ключа в случае counting sort 
    // или установка максимального значения = 9 в случае если radix sort
    auto greatest = 9;
    
    if ( radix_param == 0 ) {
        greatest = arr->at( 0 );
    
        for ( size_t i = 0; i < arr->size(); i++ ) {
            if ( arr->at( i ) < 0 ) {
                throw runtime_error("Negative array keys are not allowed!");
            }
            if ( arr->at( i ) > greatest ) {
                greatest = arr->at( i );
            }
        }
    }
    
    // заполнение counting массива
    vector<size_t> arrayB( greatest + 1 );
    for ( size_t i = 0; i < arr->size(); i++ ) {
        arrayB.at( func( arr->at( i ) ) )++;
    }

    // инкрементирование counting массива 
    for ( size_t i = 0; i + 1 < arrayB.size(); i++ ) {
        arrayB.at( i + 1 ) += arrayB.at( i );
    }

    vector<int> arrayRes( arr->size() );

    // заполнение результирующего массива
    // для сохранения свойства stable цикл идет в обратном порядке
    for ( long long i = static_cast<long long>( arr->size() ) - 1; i >= 0 ; i-- ) {
        auto ind = func( arr->at( i ) );
        arrayB.at( ind )--;
        auto resInd = arrayB.at( ind );
        arrayRes.at( resInd ) = arr->at( i );
    }

    // переброска результирующего массива в исходный массив
    for ( size_t i = 0; i < arr->size(); i++ ) {
        arr->at( i ) = arrayRes.at( i );
    }

}

 void SortingLib::RadixSortInner::SpecificSort( vector<int>* arr, const int& /*radix_param*/ ) {

    if ( !arr || arr->size() == 0 ) {
        return;
    }

    auto greatest = GetGreatestKeyValue( *arr );
    int num_of_digits = greatest <= 9 && greatest >= 0 ? 1 : (int)log10( (double)greatest ) + 1;

    auto raise_of_ten = 1;

    for (int i = 0; i < num_of_digits; i++) {
        _counting_sort->SpecificSort( arr, raise_of_ten );
        if ( i + 1 < num_of_digits ) {
            raise_of_ten *= 10; // 10^10 does not fit into int, so stop at the last digit
        }
    }
};

 SortingLib::RadixSortInner::RadixSortInner( vector<int>* arr, const SortingLib::Order& order ) : SortingLib::LinearTimeSort( arr, order ) {
    vector<int> a( 1 );
    _counting_sort = make_unique<SortingLib::CountingSortInner>( &a, order );
};

void SortingLib::RadixSort( vector<int>* arr, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<int>> ( make_unique<RadixSortInner> ( arr, order ) ) );
}

void SortingLib::CountingSort( vector<int>* arr, const Order& order ) {
    SortingLib::DoPerformSort( unique_ptr<SortInner<int>> ( make_unique<CountingSortInner>( arr, order ) ) ) ;
}

 void SortingLib::BucketSort(vector<int>* arr, const Order& order ) {

    // Время работы в среднем случае равно Ɵ(n) + n * O( 2 - 1/n ) = Ɵ(n)
    // Bucket sort assumes that the input is generated by a random process that distributes elements uniformly and independently
    // over the interval [ 0, 1 ).
    // Bucket sort divides the interval into n equal-sized subintervals, or buckets, and then distributes the n input numbers into the buckets.
    // Since the inputs are uniformly and independently distributed over [0,1), we do not expect many numbers to fall into each bucket.
    // To produce the output, we simply sort the numbers in each bucket and then go through the buckets in order, listing elements in each.

    if ( !arr ) {
        throw invalid_argument( "Error: null array is not allowed!" );
    }

    const size_t n = arr->size();

    if ( n < 2 )
        return;

    const auto [ min_it, max_it ] = minmax_element( arr->begin(), arr->end() );
    const long long min = *min_it;
    const long long range = static_cast<long long>( *max_it ) - min + 1;

    vector<vector<int>> buckets( n );
    for ( int x : *arr ) {
        auto index = static_cast<size_t>( ( x - min ) * static_cast<long long>( n ) / range ); // 0 .. n-1
        buckets[ index ].push_back( x );
    }

    size_t pos = 0;
    for ( auto& bucket : buckets ) {
        // insertion sort inside a bucket: buckets are expected to be small
        for ( size_t i = 1; i < bucket.size(); i++ ) {
            int key = bucket[ i ];
            size_t j = i;
            while ( j > 0 && bucket[ j - 1 ] > key ) {
                bucket[ j ] = bucket[ j - 1 ];
                j--;
            }
            bucket[ j ] = key;
        }
        for ( int x : bucket ) {
            arr->at( pos++ ) = x;
        }
    }

    if ( order == Order::DESC ) {
        reverse( arr->begin(), arr->end() );
    }
}
