#pragma once

#include<vector>
#include "Header.h"

/////////////////////////// Depth-first search: find all vertices reacheable from a given vertex//////////////////////////

// Depth-first search with adjacency lists
void DFS( const vector<list<Person_ptr>>& adj_lists, const Person_ptr& s, shared_ptr<unordered_set<Person_ptr>> visited = nullptr ) {

    if ( visited == nullptr ) {
        visited = make_shared<unordered_set<Person_ptr>>();
    }
  
    s->Print();
    visited->insert( s );
    
    for ( auto& item : adj_lists.at( s->GetAdjId() ) ) {

        if ( visited->count( item ) ) {
            continue;
        }

        DFS( adj_lists, item, visited );
    }
    
};

// Depth-first search with adjacency matrix
void DFS( const vector<vector<bool>>& adj_matrix, const vector<Person_ptr>& vertices, const Person_ptr& s, shared_ptr<unordered_set<Person_ptr>> visited = nullptr ) {

    if ( visited == nullptr ) {
        visited = make_shared<unordered_set<Person_ptr>>();
    }

    s->Print();
    visited->insert( s );

    auto& vect = adj_matrix.at(s->GetAdjId());

    for ( auto i = 0; i < vect.size(); i++ ) {

        if ( vect.at( i ) == 0 ) {
            continue;
        }

        auto& item = vertices.at(i);

        if ( visited->count( item ) ) {
            continue;
        }
        
        DFS( adj_matrix, vertices, item, visited );
    }

};
