#pragma once

#include<vector>
#include "Header.h"
using namespace std;

void PrimMST( vector<vector<size_t>>& adj_m, vector<Person_ptr>& vertices, Person_ptr& r ) {

	for ( auto& v : vertices ) {
		v->d = INT_MAX;
		v->pi = nullptr;
	}
	r->d = 0;

	MinHeap<Person_ptr> Q { vertices };
	unordered_set<Person_ptr> extracted{ };
	while ( Q.size() > 0 ) {
		auto u = Q.extract_min();
		extracted.insert( u );

		auto i = u->GetAdjId();
		for ( size_t j = 0; j < adj_m[ i ].size(); j++ ) {
			auto w = adj_m[ i ][ j ];
			if ( w != 0 ) {
				if ( !extracted.count( vertices[ j ] ) && w < vertices[ j ]->d ) {
					vertices[ j ]->pi = u;
					vertices[ j ]->d = w;
					Q.Decrease_key( vertices[ j ] );
				}
			}
		}

	}
};
