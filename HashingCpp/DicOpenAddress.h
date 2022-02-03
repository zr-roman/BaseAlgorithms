#pragma once

#include <vector>
#include <memory>
#include <string>
#include <typeinfo>
#include "HashingAbstract.h"

using namespace std;

// OPEN ADDRESSING
template <typename TKey, typename TValue>
class DicOpenAddr : public HashingAbstract<TKey, TValue> {
public:
        
    DicOpenAddr() {
        arr.resize( m );
        probing_method = ProbingMethod::QUADRATIC_PROBING;
        count_elements = 0;
    };

    virtual void Insert( const TKey& key, const TValue& value ) override {

        if ( _search( key ).first ) { return; }

        auto func( [&]() {
            curr_key_ptr = move( make_unique<TKey>( key ) );
            curr_value_ptr = move( make_unique<TValue>( value ) );
            TableDoubling_FullRehash_And_CallInsert();
        });

        if ( count_elements == arr.size() ) { func(); return; }

        auto hash = HashFunc( key );

        bool done( false );
        for ( size_t i = 0; i < arr.size(); i++ ) {
            auto hash_ex = DoProbe( key, hash, i );
            if ( arr[ hash_ex ] == nullptr || arr[ hash_ex ]->deleted ) {
                _insert( hash_ex, key, value, hash );
                done = true;
                break;
            }
        }

        if ( !done ) { func(); }
    };    

    virtual void Delete( const TKey& key ) override {

        auto search_res = _search( key );

        if ( search_res.first ) {
            _delete( search_res.second );
        }
    };

    virtual pair<bool, TValue> Search( const TKey& key ) override {

        auto search_res = _search( key );

        if ( !search_res.first ) {
            return make_pair( false, move( [](){ TValue v; return v; }() ) );
        }
        return make_pair( true, arr[ search_res.second ]->value );
    };

private:

    using HashingAbstract<TKey, TValue>::m;
    using HashingAbstract<TKey, TValue>::HashFunc;
    using HashingAbstract<TKey, TValue>::HashFuncMultiplication;
    
    // constants for quadratic probing
    const double c1 = 1.5;
    const double c2 = 1.5;

    unique_ptr<TKey> curr_key_ptr;
    unique_ptr<TValue> curr_value_ptr;

    size_t count_elements;

    enum class ProbingMethod {
        LINEAR_PROBING,
        QUADRATIC_PROBING,
        DOUBLE_HASHING
    };

    ProbingMethod probing_method;

	struct Record : HashingAbstract<TKey, TValue>::Record_Base {
        size_t hash;
        bool deleted;
        Record( const TKey& key, const TValue& value, const size_t& hash ) : HashingAbstract<TKey, TValue>::Record_Base( key, value ) {
            this->deleted = false;
            this->hash = hash;
        }
	};

    vector<unique_ptr<Record>> arr;  // main table

    void TableResizing_FullRehash( const size_t new_size ) {

        m = new_size;
        vector<unique_ptr<Record>> new_arr;
        new_arr.resize( m );

        for ( size_t i = 0; i < arr.size(); i++ ) {
            
            if ( arr[ i ] == nullptr || arr[ i ]->deleted ) { continue; }

            auto hash = HashFunc( arr[ i ]->key );
            for ( size_t j = 0; true; j++ ) {
                auto hash_ex = DoProbe( arr[ i ]->key, hash, j );
                if ( new_arr[ hash_ex ] == nullptr ) {
                    arr[ i ]->hash = hash;
                    new_arr[ hash_ex ] = move( arr[ i ] );
                    break;
                }
            }
        }

        arr.clear();
        arr.resize( m );
        arr = move( new_arr );
    };

    void TableDoubling_FullRehash_And_CallInsert( ) {
        long long t = m;
        long long tmp = t * 2;
        if ( tmp > UINT_MAX ) {
            throw runtime_error("Limit exceeded!");
        }
        TableResizing_FullRehash( m * 2 );
        Insert( move( *(curr_key_ptr.get()) ), move( *(curr_value_ptr.get()) ) );
    };

    void TableHalfеning_FullRehash() {
        if ( count_elements > arr.size() / 4 ) {
            return;
        }
        TableResizing_FullRehash( m / 2 );
    };

    pair<bool, size_t> _search( const TKey& key ) {

        auto hash = HashFunc( key );

        auto not_found = make_pair( false, 0 );

        for ( size_t i = 0; i < arr.size(); i++ ) {

            auto hash_ex = DoProbe( key, hash, i );
            
            if ( arr[ hash_ex ] == nullptr ) {
                return not_found;
            }

            if ( arr[ hash_ex ]->deleted ) {
                continue;
            }

            if ( arr[ hash_ex ]->key == key ) {
                return make_pair( true, hash_ex );
            }
        }

        return not_found;
    };

    /// <summary>
    /// Linear Probing
    /// Suffers from problem known as "primary clustering"
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    size_t LinearProbing( size_t hash, size_t i ) {
        return ( hash + i ) % m;
    };

    /// <summary>
    /// Quadratic Probing 
    /// Suffers from problem known as "secondary clustering"
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    size_t QuadraticProbing( size_t hash, size_t i ) {
        return (int)( hash + c1 * i + c2 * i * i ) % m;
    };

    /// <summary>
    /// Double Hashing 
    /// Offers one of the best methods available for open addressing because the permutations produced have many of the characteristics of randomly chosen permutations
    /// </summary>
    /// <returns></returns>
    size_t DoubleHashing( const TKey& key, size_t hash, size_t i ) {
        return ( hash + i * HashFuncMultiplication( key ) ) % m;
    }

    size_t DoProbe( const TKey& key, size_t hash, size_t i ) {

        switch ( probing_method ) {

            case ProbingMethod::DOUBLE_HASHING:
                return DoubleHashing( key, hash, i );

            case ProbingMethod::LINEAR_PROBING:
                return LinearProbing( hash, i );

            case ProbingMethod::QUADRATIC_PROBING:
                return QuadraticProbing( hash, i );

            default:
                throw runtime_error( "Must not hit into here!");
        }
    };

    void _delete( size_t index ) {
        arr[ index ]->deleted = true;
        count_elements--;
        TableHalfеning_FullRehash();
    }

    void _insert( size_t hash_ex, const TKey& key, const TValue& value, size_t hash ) {
        arr[ hash_ex ] = move( make_unique<Record>( key, value, hash ) );
        count_elements++;
    }
    
};
