#pragma once

#include <iostream>
#include <list>
#include <vector>
#include <memory>
#include <unordered_set>
#include <queue>
#include <map>

using namespace std;

class Person;

using Person_ptr = shared_ptr<Person>;

class Person {

public:

    unsigned int adj_id;
    string GUID;
    string Name;

    //for shortest paths: Bellman-Ford and Dijkstra
    int d; // верхняя граница веса, которым обладает кратчайший путь из истока s в эту вершину
    Person_ptr pi; // ссылка на предыдущий узел в кратчайшем пути

    // for PrimMST
    int key;

    Person( string name, string guid, int adj_id ) {
        Name = name; 
        GUID = guid;
        this->adj_id = adj_id;
    };

    Person( const Person& other ) {
        this->adj_id = other.adj_id;
        this->d = other.d;
        this->Name = other.Name;
        this->GUID = other.GUID;
        this->pi = other.pi;
    };

    void Print() {
        cout << adj_id << " " << Name << endl;
    };

    unsigned int GetAdjId() const {
        return adj_id;
    };

    inline bool operator<( const Person& a ) {
        if ( this->d < a.d ) {
            return true;
        }
        return false;
    };

    inline bool operator> (const Person& a) { return *this < a; }
    inline bool operator<=(const Person& a) { 
        return this->d <= a.d; 
    }
    inline bool operator>=(const Person& a) { return !(*this < a); }
    inline bool operator==(const Person& a) { return this->d == a.d; }
    inline bool operator!=(const Person& a) { return !(*this == a); }

};

template <typename T>
class MinHeap {
public:

    MinHeap( vector<T> init ) {
        size_t counter = 0;
        for ( auto item : init ) {
            _minheap.push_back( item );
            _map.insert({ item, counter });
            counter++;
        }
        BuildHeap();
    };

    void Decrease_key( const T& obj ) {

        if ( _minheap.size() == 1 ) {
            return;
        }

        auto search = _map.find( obj );
        auto i = search->second;

        while ( true ) {
            if ( i == 0 ) {
                break;
            }
            int ip = ( i - 1 ) / 2; // iparent
            if ( *(_minheap[ ip ].get()) <= *(_minheap[ i ].get()) ) {
                break;
            }
            SyncronizeMapAndHeap( ip, i );
            i = ip;
        }
    };

    size_t size() {
        return _minheap.size();
    };

    T& extract_min() {

        auto elm = _minheap.at( 0 );

        if ( _minheap.size() > 1 ) {
            SyncronizeMapAndHeap( 0 , _minheap.size() - 1 );
        }
        
        _map.erase( _minheap[ _minheap.size() - 1 ] );
        _minheap.pop_back();

        Heapify( 0 );

        return elm;
    };


private:
    
    vector<T> _minheap;

    map<T, size_t> _map;
    
    void BuildHeap() {
        for ( int i = _minheap.size()/2 - 1; i >= 0; i-- ) {
            Heapify( i );
        }
    };

    void Heapify( size_t i ) {
        
        auto iL = 2 * i + 1;
        auto iR = 2 * i + 2;
        size_t iSmallest;

        if ( iL < _minheap.size() && *(_minheap.at( iL ).get()) < *(_minheap.at( i ).get()) ) {
            iSmallest = iL;
        } else {
            iSmallest = i;
        }

        if ( iR < _minheap.size() && *(_minheap.at( iR ).get()) < *(_minheap.at( iSmallest ).get()) ) {
            iSmallest = iR;
        }

        if ( iSmallest == i ) {
            return;
        }
        
        SyncronizeMapAndHeap( i, iSmallest );
        Heapify( iSmallest );
    }

    void SyncronizeMapAndHeap( size_t i, size_t j ) {
        _map[ _minheap[ i ] ] = j;
        _map[ _minheap[ j  ] ] = i;
        swap( _minheap.at( i ), _minheap.at( j ) );
    };
};

void Relax( const Person_ptr& u, const Person_ptr& v, int& w ) {
    if ( v->d > u->d + w ) {
        v->d = u->d + w;
        v->pi = u;
    }
};

// not optimal implementation, just a sketch
template <typename T>
class disjoint_set {

public:

    void make_set( T obj ) {
        vector<T> t { obj };
        arr_of_sets.push_back( t );
    };

    size_t find_set( const T& obj ) {
        for ( size_t i = 0; i < arr_of_sets.size(); i++ ) {
            for ( size_t j = 0; j < arr_of_sets[ i ].size(); j++ ) {
                if ( arr_of_sets[ i ][ j ] == obj ) {
                    return i;
                }
            }
        }
    };

    void union_sets( T o1, T o2, int w ) {
        edges.push_back( tuple<T, T, int>( o1, o2, w ) );
        auto i1 = find_set( o1 );
        auto i2 = find_set( o2 );
        while( arr_of_sets[ i2 ].size() > 0 ) {
            arr_of_sets[ i1 ].push_back( arr_of_sets[ i2 ][ 0 ] );
            arr_of_sets[ i2 ].erase( arr_of_sets[ i2 ].begin() );
        }
    };

    vector<tuple<T, T, int>>& GetPairs() {
        return edges;
    };

private:
    vector<tuple<T, T, int>> edges;
    vector<vector<T>> arr_of_sets { };
};
