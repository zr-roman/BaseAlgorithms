#pragma once

#include<vector>
#include "Header.h"

using namespace std;

// Bellman-Ford ( works also when there are negative cycles ), used to detect negative cycles, O(V*E) time complexity

bool BellmanFord( const vector<vector<int>>& adj_matrix, const vector<Person_ptr>& vertices, const Person_ptr& s ) {
    
    /* N = 0; */

    // initialization
    for ( auto& v : vertices ) {
        v->d = INT_MAX;
        v->pi = nullptr;
    }
    s->d = 0;

    // Bellman-Ford
    for ( size_t i = 0; i < vertices.size() - 1 ; i++) {

        for ( size_t k = 0; k < adj_matrix.size(); k++ ) {

            for ( size_t j = 0; j < adj_matrix.at( k ).size(); j++ ) {

                if ( adj_matrix.at( k ).at( j ) == INT_MAX ) {
                    continue;
                }
                
                auto u = vertices.at( k );
                auto v = vertices.at( j );
                auto w = adj_matrix.at( k ).at( j );

                Relax( u, v, w );
            }
        }
    }

    for ( size_t k = 0; k < adj_matrix.size(); k++ ) {

        for ( size_t j = 0; j < adj_matrix.at( k ).size(); j++ ) {

            if ( adj_matrix.at( k ).at( j ) == INT_MAX ) {
                    continue;
                }

            auto u = vertices.at( k );
            auto v = vertices.at( j );
            auto w = adj_matrix.at( k ).at( j );

            if ( v->d > u->d + w ) {
                return false;
            }

        }
    }

    return true;

};
