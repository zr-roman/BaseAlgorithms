#pragma once

#include<vector>
#include<list>
#include <unordered_set>
#include "Header.h"
#include <unordered_map>

using namespace std;

/////////////////////////// Breadth-first search: find all vertices reacheable from a given vertex //////////////////////////

// Breadth-first search with adjacency lists
void BFS( const vector<list<Person_ptr>>& adj_lists, const Person_ptr& s ) {

    int i = 0; // counter for levels

    unordered_set<Person_ptr> frontier = { s };
    unordered_map<Person_ptr, int> level = { { s, i++ } };
    unordered_map<Person_ptr, Person_ptr> parent = { { s, nullptr } };
        
    while ( frontier.size() != 0 ) {

        cout << " Level " << ( i - 1 ) << ":" << endl;

        unordered_set<Person_ptr> next_frontier;

        for ( auto& u : frontier ) {
                        
            u.get()->Print();

            auto& lst = adj_lists.at(u.get()->GetAdjId());

            for ( auto const& v : lst ) {
                if ( level.count( v ) == 0 ) {
                    level[ v ] = i;
                    parent[ v ] = u;
                    next_frontier.insert( v );
                }
            }
        }
        frontier = next_frontier;
        i++;
    }

};

// Breadth-first search with adjacency matrix
void BFS( const vector<vector<bool>>& adj_matrix, const vector<Person_ptr>& vertices, const Person_ptr& s ) {

    int i = 0;

    vector<Person_ptr> frontier = { s };
    unordered_map<Person_ptr, int> level = { { s, i++ } };
    unordered_map<Person_ptr, Person_ptr> parent = { { s, nullptr } };

    while ( frontier.size() != 0 ) {

        cout << " Layer " << (i - 1) << ":" << endl;

        vector<Person_ptr> next_frontier;

        for ( auto& u : frontier ) {

            u.get()->Print();
            
            auto& arr = adj_matrix.at(u.get()->GetAdjId());

            for ( size_t j = 0; j < arr.size(); j++ ) {
                if ( arr.at( j ) == 0 ) {
                    continue;
                }
                auto& v = vertices.at( j );
                if ( level.count( v ) == 0 ) {
                    level[ v ] = i;
                    parent[ v ] = u;
                    next_frontier.push_back( v );
                }
            }
        }
        frontier = next_frontier;
        i++;
    }
};
