#pragma once

#include<vector>
#include<memory>

using namespace std;

struct ChainOrder {
	vector<vector<size_t>> arr_m;
	vector<vector<size_t>> arr_s;
	ChainOrder( vector<vector<size_t>> arr_m, vector<vector<size_t>> arr_s ) {
		this->arr_m = arr_m;
		this->arr_s = arr_s;
	};
};

shared_ptr<ChainOrder> MatrixChainOrder( vector<size_t>& p ) {

	vector<vector<size_t>> arr_m{};
	vector<vector<size_t>> arr_s{};

	for ( size_t i = 0; i < p.size()-1; i++ ) {
		vector<size_t> t{};
		arr_m.push_back( t );
		arr_s.push_back(t);
		for ( size_t j = 0; j < p.size()-1; j++ ) {
			arr_m[ i ].push_back( 0 );
			arr_s[ i ].push_back( 0 );
		}
	}

	// проход по диагоналям слева направо начиная со средней диагонали и выше
	
	for ( size_t c = 0; c < arr_m.size(); c++ ) {

		for ( size_t i = 0, j = c; i < arr_m.size() && j < arr_m.size(); i++, j++ ) {
			
			size_t min = UINT_MAX;
			for ( size_t k = i; k + 1 <= j; k++ ) {
				
				auto tmp = arr_m[ i ][ k ] + 
						   arr_m[ k + 1 ][ j ] + 
						   p[ i ] * p[ k + 1 ] * p[ j + 1 ];

				if ( tmp < min ) {
					min = tmp;
					arr_s[ i ][ j ] = k;
				}
			}
			
			arr_m[ i ][ j ] = min == UINT_MAX ? 0 : min;
		}
	}
	
#pragma region for_test
	if (   !(arr_m[0][1] == 15'750 && arr_m[0][2]== 7'875 && arr_m[0][3] == 9'375 && arr_m[0][4] == 11'875 && arr_m[0][5] == 15'125 
		&& arr_m[1][2] == 2'625 && arr_m[1][3] == 4'375 && arr_m[1][4] == 7'125 && arr_m[1][5] == 10'500
		&& arr_m[2][3] == 750   && arr_m[2][4] == 2'500  && arr_m[2][5] == 5'375
		&& arr_m[3][4] == 1'000 && arr_m[3][5] == 3'500
		&& arr_m[4][5] == 5'000)
		) {
		throw runtime_error("must not hit into here!");
	}
#pragma endregion

	return make_shared<ChainOrder>( arr_m, arr_s );

};

void PrintMatrixParenthesis( vector<vector<size_t>>& s, size_t i, size_t j ) {
	if ( i == j ) { 
		cout << "A" << i;
	} else {
		cout << "(";
		PrintMatrixParenthesis( s, i, s[ i ][ j ] );
		PrintMatrixParenthesis( s, s[ i ][ j ] + 1, j );
		cout << ")";
	}
};
