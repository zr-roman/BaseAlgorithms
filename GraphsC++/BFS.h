#pragma once

#include<vector>
#include<list>
#include <unordered_set>
#include "Header.h"

using namespace std;

/////////////////////////// Breadth-first search: find all vertices reacheable from a given vertex //////////////////////////

// Breadth-first search with adjacency lists
void BFS( const vector<list<Person_ptr>>& adj_lists, const Person_ptr& s ) {

    vector<Person_ptr> frontier = { s };
    unordered_set<Person_ptr> visited = { };

    size_t counter = 0;

    while ( frontier.size() != 0 ) {

        cout << " Layer " << counter << ":" << endl;
        counter++;

        vector<Person_ptr> next_frontier;

        for ( auto& item : frontier ) {
                        
            item.get()->Print();
            visited.insert( item );

            auto lst = adj_lists.at( item.get()->GetAdjId() );

            for ( auto const& p : lst ) {
                if ( visited.count( p ) ) {
                    continue;
                }
                next_frontier.push_back( p );
            }
        }
        frontier = next_frontier;
    }

};

// Breadth-first search with adjacency matrix
void BFS( const vector<vector<bool>>& adj_matrix, const vector<Person_ptr>& vertices, const Person_ptr& s ) {

    vector<Person_ptr> frontier = { s };
    unordered_set<Person_ptr> visited = { s };

    size_t counter = 0;

    while ( frontier.size() != 0 ) {

        cout << " Layer " << counter << ":" << endl;
        counter++;

        vector<Person_ptr> next_frontier;

        for ( auto& item : frontier ) {

            item.get()->Print();
            visited.insert( item );
            
            auto arr = adj_matrix.at( item.get()->GetAdjId() );

            for ( size_t j = 0; j < arr.size(); j++ ) {
                if ( arr.at( j ) == 0 ) {
                    continue;
                }
                if ( visited.count( vertices.at( j ) ) ) {
                    continue;
                }
                next_frontier.push_back( vertices.at( j ) );
                
            }
        }
        frontier = next_frontier;
    }
};
