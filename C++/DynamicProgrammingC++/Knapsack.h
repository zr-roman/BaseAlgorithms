#pragma once

#include<vector>

using namespace std;

struct Item {
	size_t value;
	size_t weight;
};

vector<Item> Knapsack( vector<Item>& items, size_t total_weight ) {

	vector<vector<size_t>> m;
	for ( size_t i = 0; i <= items.size(); i++ ) {
		vector<size_t> tmp { 0 };
		m.push_back( tmp );
		for ( size_t j = 0; j < total_weight; j++ ) {
			m[ i ].push_back( 0 );
		}
	}

	// Если j меньше веса предмета, тогда берется значение из ячейки выше.
	// Если j равно весу предмета (или больше), тогда берется большее из двух значений:
	// 1) значение из ячейки выше,
	// 2) сумма value предмета и значения, которое получается так: 
	//    подняться на одну ячейку выше и из j вычесть вес предмета
	for ( size_t i = 1; i < m.size(); i++ ) {
		for ( size_t j = 1; j < m[ i ].size(); j++ ) {
			if ( j < items[ i-1 ].weight ) {
				m[ i ][ j ] = m[ i - 1 ][ j ];
			} else {
				auto c1 = m[ i - 1 ][ j ];
				auto c2 = items[ i-1 ].value + m[ i - 1 ][ j - items[ i-1 ].weight ];
				m[ i ][ j ] = c1 > c2 ? c1 : c2;
			}
		}
	}

	vector<Item> res;

	size_t i = m.size() - 1;
	size_t j = total_weight;
	while ( i > 0 && j > 0 ) {
		if ( m[ i ][ j ] != m[ i - 1 ][ j ] ) {
			res.push_back( items[ i - 1 ] );
			j -= items[i - 1].weight;
		}
		i--;
	}
	return res;
		
};
