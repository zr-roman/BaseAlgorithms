#include "Lib.h"
#include <vector>

using namespace std;

vector<int> SortingLib::gaps = { 1750, 701, 301, 132, 57, 23, 10, 4, 1 }; //{ 5,4,3,2,1 };

double SortingLib::shrinkFactor = 1 / ( 1 - exp( -( (sqrt(5) + 1) / 2 ) ) ); // ≈ 1.2473309501039787

/// <summary>
/// Generates a random number
/// </summary>
/// <param name="start">: inclusive lower bound</param>
/// <param name="end">: inclusive upper bound</param>
/// <returns>A random number within a given range</returns>
int RandomizationLib::GenerateRandomNumber( const int& start, const int& end ) {
    random_device dev;
    mt19937 rng( dev() );
    uniform_int_distribution<int> dist( start, end );
    auto res = dist( rng );
    return res;
};
//В данном случае возврат происходит by value, что означает, что int(4 байта в 32-разрядной системе) будет скопирован при возврате.
//Возвращаемый тип int& означает, что будет возвращена постоянная ссылка на int(4 байта в 32-разрядной системе).
//Как видно, в этом случае нет никакого преимущества в производительности при использовании return-by-reference вместо return-by-value.

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
        virtual void SpecificSort( vector<int>* arr, const int& radix_param = 0 ) override; // GoF template method

    private:
        unique_ptr<SortingLib::CountingSortInner> _counting_sort;
};

void SortingLib::LinearTimeSort::DoSort( const unsigned int& start, const unsigned int& end ) {

    vector<int> negativeArray;
    vector<int> positiveArray;

    for ( size_t i = 0; i < arr->size(); i++ ) {
        if ( arr->at( i ) < 0 ) {
            negativeArray.push_back( -arr->at( i ) );
            continue;
        }
        positiveArray.push_back( arr->at( i ) );
    }
    
    SpecificSort( &negativeArray );
    SpecificSort( &positiveArray );
    
    int index = 0;
    if ( order == SortingLib::Order::ASC ) {
        for ( int i = negativeArray.size() - 1; i >= 0; i-- ) {
            arr->at( index ) = -negativeArray.at( i );
            index++;
        }
        for ( size_t i = 0; i < positiveArray.size(); i++ ) {
            arr->at( index ) = positiveArray.at( i );
            index++;
        }
    } else {
        for ( int i = positiveArray.size() - 1; i >= 0; i-- ) {
            arr->at( index ) = positiveArray.at( i );
            index++;
        }
        for ( size_t i = 0; i < negativeArray.size(); i++ ) {
            arr->at( index ) = -negativeArray.at( i );
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
    for ( int i = arr->size() - 1; i >= 0 ; i-- ) {
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

 void SortingLib::RadixSortInner::SpecificSort( vector<int>* arr, const int& radix_param ) {

    if ( !arr || arr->size() == 0 ) {
        return;
    }

    auto greatest = GetGreatestKeyValue( *arr );
    int num_of_digits = greatest <= 9 && greatest >= 0 ? 1 : (int)log10( (double)greatest ) + 1;

    auto raise_of_ten = 1;

    for (size_t i = 0; i < num_of_digits; i++) {
        _counting_sort->SpecificSort( arr, raise_of_ten );
        raise_of_ten *= 10;
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

}
