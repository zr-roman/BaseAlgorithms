#pragma once

#include <cmath>
#include <cstdio>
#include <vector>
#include <iostream>
#include <algorithm>
#include <string>
#include <typeinfo>
#include <memory>
#include <array>
#include <tuple>

#include "HashingAbstract.h"

using namespace std;

// HASHING WITH CHAINING
template <typename TKey, typename TValue>
class DicChaining : public HashingAbstract<TKey, TValue> {

public:

    DicChaining() : HashingAbstract<TKey, TValue>() {
        m = k;
    };
    
    virtual void Insert( const TKey& key, const TValue& value ) override {

        auto res = Search( key );
        if ( res.first ) {
            return;
        }

        auto hash = HashFunc( key );

        auto rec = make_shared<Record>( key, value );
        
        if (arr.at( hash ) == nullptr) {
            arr.at( hash ) = rec;
        } else {
            auto p = arr.at( hash );
            shared_ptr<Record> x;
            while ( p != nullptr ) {
                x = p;
                p = p->next;
            }
            x->next = rec;
        }
    };

    virtual pair<bool, TValue> Search( const TKey& key ) override {

        auto hash = HashFunc( key );

        auto ptr = arr.at( hash );
        TValue v;
        auto p = make_pair( false, move( v ) );
        if ( ptr == nullptr ) {
            return p;
        }
        
        while ( ptr != nullptr) {
            if ( ptr->key == key ) {
                v = ptr->value;
                return make_pair( true, move( v ) );
            }
            ptr = ptr->next;
        }

        return p;
    };

    virtual void Delete( const TKey& key ) override {
        
        auto r = GetChainItem( key );

        auto prev = get<0>( r );
        auto item = get<1>( r );
        auto hash = get<2>( r );

        if ( item == nullptr ) {
            return;
        }

        ( prev == nullptr ? arr[ hash ] : item->next ) = item->next;
    };

private:

    using HashingAbstract<TKey, TValue>::m;
    using HashingAbstract<TKey, TValue>::HashFunc;

    struct Record : HashingAbstract<TKey, TValue>::Record_Base {
    public:
        shared_ptr<Record> next;
        Record( const TKey& key, const TValue& value ) : HashingAbstract<TKey, TValue>::Record_Base( key, value ) {
            next = nullptr;
        }
    };
    
    const static size_t k = 7; //700001; // table size
    array<shared_ptr<Record>, k> arr; // main table

    tuple<shared_ptr<Record>, shared_ptr<Record>, unsigned int> GetChainItem( const TKey& key ) {
        
        auto hash = HashFunc( key );

        auto not_found_res = make_tuple( nullptr, nullptr, 0 );

        if ( arr[ hash ] == nullptr ) {
            return not_found_res;
        }

        shared_ptr<Record> prev( nullptr );
        shared_ptr<Record> curr = arr[ hash ];

        bool found = false;

        while ( true ) {

            if ( curr->key == key ) {
                found = true;
                break;
            }

            prev = curr;
            curr = curr->next;

            if ( curr == nullptr ) {
                break;
            }
        }

        return found ? make_tuple( prev, curr, hash ) : not_found_res;
    };
};
