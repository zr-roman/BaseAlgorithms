#pragma once

#include<vector>

using namespace std;

int func( int m, int n, vector<vector<int>> arr ) {

	// 1. Из-за границ таблицы невозможно попасть никаким образом ни в какую клетку
	if ( m < 1 || n < 1 ) {
		return 0;
	}

	// 2. В любую клетку области (n, 1) и (1, m) можно попасть только одним-единственным способом
	if ( m == 1 || n == 1 ) {
		return 1;
	}

	if ( arr.size() == 0 ) {
		for ( size_t i = 0; i <= m; i++ ) {
			vector<int> v;
			arr.emplace_back(  v );
			for ( size_t j = 0; j <= n; j++ ) {
				arr[i].emplace_back( 0 );
			}
		}
	}

	// memoization
	if ( arr[ m ][ n ] != 0 ) {
		return arr[ m ][ n ];
	}

	// 3. Во всех остальных случаях значение для клетки получается суммированием соседей (снизу и слева)
	arr[m][n] = func(n - 1, m, arr) + func(n, m - 1, arr);
	return arr[ m ][ n ];
}
