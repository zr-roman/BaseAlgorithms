#pragma once

#include <string>
#include <utility>
#include <random>
#include <cmath>
#include <cstdlib>

using namespace std;

template <typename TKey, typename TValue>
class HashingAbstract {
public: 
    
    // perfect forwarding
    /*template<typename TKey, typename TValue>
    void Insert( TKey&& key, TValue&& value ) {
        _insert( forward<TKey>( key ), forward<TValue>( value ) );
    };*/

    virtual void Insert( const TKey& key, const TValue& value ) = 0;
	virtual void Delete( const TKey& ) = 0;
	virtual pair<bool, TValue> Search( const TKey& ) = 0;

protected:
    
    enum class HashMethod {
        DIVISION,
        MULTIPLICATION,
        UNIVERSAL_HASHING
    };

    HashingAbstract( HashMethod hm = HashMethod::DIVISION ) {

        hash_method = hm;

        if (hash_method == HashMethod::UNIVERSAL_HASHING) {
            a = GenerateRandomNumber(0, _UI32_MAX - 1);
            b = GenerateRandomNumber(1, _UI32_MAX - 1);
        }
    };

    struct Record_Base {
        TKey key;
        TValue value;
        Record_Base( const TKey& key, const TValue& value ) {
            this->key = key;
            this->value = value;
        }
	};

    HashMethod hash_method;

    unsigned int m = 7; //700001; // table size

    unsigned int a; // parameter for universal hashing
    unsigned int b; // parameter for universal hashing
    
    unsigned int HashFunc( const TKey& key, unique_ptr<unsigned int> a_par = nullptr, unique_ptr<unsigned int> b_par = nullptr, unique_ptr<unsigned int> m_par = nullptr ) {
        switch ( hash_method ) {
            case HashMethod::DIVISION:
                return HashFuncDivision( key );
            case HashMethod::MULTIPLICATION:
                return HashFuncMultiplication( key );
            case HashMethod::UNIVERSAL_HASHING:
                return HashFuncUniversal( key, move( a_par ), move( b_par ), move( m_par ) );
            default:
                throw runtime_error( "Must not hit into here!" );
        }
    };

    unsigned int HashFuncDivision( const TKey& key ) {
        auto number = GetNumericRepresentationOfKey( key );
        unsigned int hash = number % m;
        return hash;
    };

    unsigned int HashFuncMultiplication( const TKey& key ) {
        auto const A = ( sqrt( 5 ) - 1 )/2; // Knuth offered to use this value
        auto number = GetNumericRepresentationOfKey( key );
        return m * fmod( number * A, 1 ); // this is: m * ( ( key * A ) % 1 )
        // выражение ( key * A ) % 1 означает получение дробной части произведения ( key * A )
    };

    unsigned int HashFuncUniversal(const TKey& key, unique_ptr<unsigned int> a_par = nullptr, unique_ptr<unsigned int> b_par = nullptr, unique_ptr<unsigned int> m_par = nullptr ) {
        
        if ( a_par == nullptr ) { a_par.reset( new unsigned int( a ) ); }
        if ( b_par == nullptr ) { b_par.reset( new unsigned int( b ) ); }
        if ( m_par == nullptr ) { m_par.reset( new unsigned int( m ) ); }

        // key in range of 0 ... UINT_MAX
        long long p = 4294967311; // the nearest following prime to UINT_MAX
        auto number = GetNumericRepresentationOfKey( key );
        unsigned int res =(unsigned int) ( ( (long long)(*(a_par.get())) * number + (long long)(*(b_par.get())) ) % p) % (*(m_par.get())) ; // this is: ((a*k + b) mod p) mod m;
        return res;
    };
    
    unsigned int GetNumericRepresentationOfKey( const TKey& key ) {
        return _getNumericRepresentationOfKey( key, is_integral<typename remove_reference<TKey>::type>(), is_same<TKey, string>() ); // tag dispatch
    };

    // if integral type is passed
    unsigned int _getNumericRepresentationOfKey( const TKey& key, true_type, false_type ) { // tag dispatch
        return _getNumericRepresentationOfKey( to_string( key ) );
    };

    // if string is passed
    unsigned int _getNumericRepresentationOfKey( const TKey& key, false_type, true_type ) { // tag dispatch
        return _getNumericRepresentationOfKey( key );
    };

    unsigned int _getNumericRepresentationOfKey( const string& str ) {
        unsigned int res = 1;
        for (int i = 0; i < str.length(); i++) {
            if (i % 2 == 0) {
                res = ( res + 1 ) * (unsigned int)str[i] ;
                continue;
            }
             res = ( res + 1 ) * (unsigned int)str[i] * (unsigned int)str[i];
        }
        for (int i = str.length() - 1; i >= 0; i--) {
            if ( i%2 == 0 ) {
                res = ( res + 1 ) * (unsigned int)str[i] * (unsigned int)str[i];    
                continue;
            }
            res = (res + 1) * (unsigned int)str[i];
        }
        return res;
    };

    // if non-integral type is passed
    unsigned int _getNumericRepresentationOfKey( const TKey& key, false_type, false_type ) { // tag dispatch
        throw runtime_error("Key must be integral type or string!");
    };

    /// <summary>
    /// Generates a random number
    /// </summary>
    /// <param name="start">: inclusive lower bound</param>
    /// <param name="end">: inclusive upper bound</param>
    /// <returns>A random number within a given range</returns>
    int GenerateRandomNumber( const unsigned int& start, const unsigned int& end ) {
        random_device dev;
        mt19937 rng( dev() );
        uniform_int_distribution<unsigned int> dist( start, end );
        auto res = dist( rng );
        return res;
    };

};

