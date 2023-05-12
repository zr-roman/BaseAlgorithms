#pragma once

#include<vector>
#include "Header.h"

using namespace std;

// Dijkstra ( only when no negative cycles ), time: O(V^2) for array, O(E*logV) for binary minheap, O(V*logV + E) for Fibonacci heap
void Dijkstra( const vector<vector<int>>& adj_matrix, const vector<Person_ptr>& vertices, const Person_ptr& s ) {

    // initialization stage
    for ( auto& v : vertices ) {
        v->d = INT_MAX;
        v->pi = nullptr;
    }
    s->d = 0;

    // algo stage
    MinHeap<Person_ptr> Q { vertices };
    
    while ( Q.size() > 0 ) {

        auto u = Q.extract_min();

        for ( size_t i = 0; i < adj_matrix.at( u->GetAdjId() ).size(); i++ ) {
            
            auto w = adj_matrix.at( u->GetAdjId() ).at( i );
            if ( w == INT_MAX ) {
                continue;
            }
            auto v = vertices.at( i );

            // relax edge and decrease key
            if ( v->d > u->d + w ) {
                v->d = u->d + w;
                v->pi = u;
                Q.Decrease_key( v );
            }
        }
    }
};
