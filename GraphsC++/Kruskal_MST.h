#pragma once
#include <vector>
#include "Header.h"

using namespace std;

struct Edge {
	Person_ptr u;
	Person_ptr v;
	size_t w;
	Edge( Person_ptr u, Person_ptr v, size_t w ) {
		this->u = u;
		this->v = v;
		this->w = w;
	};

	bool operator>( const Edge& other ) {
		return this->w > other.w;
	};
	bool operator<( const Edge& other ) {
		return this->w < other.w;
	};
	bool operator>=( const Edge& other ) {
		return this->w >= other.w;
	};
	bool operator<=( const Edge& other ) {
		return this->w <= other.w;
	};
};

// For Krusral algorithm it is used such a data structure as "Disjoint Set" to reach better productivity

vector<Edge> Make_arr_of_edges( vector<vector<size_t>>& adj_m, vector<Person_ptr>& vertices ) {
	
	vector<Edge> res;
	size_t c = 0;

	while ( c < vertices.size() ) {
		for ( size_t i = 0, j = c; i < vertices.size() && j < vertices.size(); i++, j++ ) {
			if ( i == j ) {
				continue;
			}
			if ( adj_m[ i ][ j ] > 0 ) {
				res.push_back( Edge( vertices[ i ], vertices[ j ], adj_m[ i ][ j ] ) );
			}
		}
		c++;
	}

	return res;
};

void InsertEdgeIntoAdj( vector<vector<size_t>>& adj_m, const Edge& e ) {
	adj_m[ e.u->GetAdjId() ][ e.v->GetAdjId() ] = e.w;
	adj_m[ e.v->GetAdjId() ][ e.u->GetAdjId() ] = e.w;
};

vector<vector<size_t>> KruskalMST( vector<vector<size_t>>& adj_m, vector<Person_ptr>& vertices ) {

	disjoint_set<Person_ptr> A;
	for ( auto& v : vertices ) {
		A.make_set( v );
	}

	vector<vector<size_t>> res;

	for ( size_t i = 0; i < adj_m.size(); i++ ) {
		vector<size_t> k{ };
		res.push_back( k );
		for ( size_t j = 0; j < adj_m[ i ].size(); j++ ) {
			res[ i ].push_back( 0 );
		}
	}

	vector<Edge> arr_edges = Make_arr_of_edges( adj_m, vertices );
	sort( arr_edges.begin(), arr_edges.end() );

	for ( size_t i = 0; i < arr_edges.size(); i++ ) {
		if ( A.find_set( arr_edges[ i ].u ) == A.find_set( arr_edges[ i ].v ) ) {
			continue;
		}
		A.union_sets( arr_edges[ i ].u, arr_edges[ i ].v, arr_edges[ i ].w );
		InsertEdgeIntoAdj( res, arr_edges[ i ] );
	}

	auto r = A.GetPairs();

	cout << endl;
	int counter = 0;
	for ( auto& v : r ) {
		cout << get<0>( v )->Name << " " << get<1>( v )->Name << get<2>( v ) << endl;
		counter += get<2>( v );
	}
	cout << counter << endl;
	
	return res;

};
