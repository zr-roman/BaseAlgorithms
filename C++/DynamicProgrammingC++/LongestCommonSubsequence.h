#pragma once

#include<string>
#include<vector>
#include <stack>

using namespace std;

enum class Arrow {
	UP,
	LEFT,
	DIAGONAL,
	NONE
};

string LongestCommonSubsequence( string a, string b ) {

	vector<vector<size_t>> arr;
	vector<vector<Arrow>> t;
	
	for ( size_t i = 0; i <= a.size(); i++ ) {
		vector<size_t> k{ 0 };
		vector<Arrow> r{ Arrow::NONE };
		arr.push_back( k );
		t.push_back( r );
		for (size_t j = 0; j < b.size(); j++) {
			arr[ i ].push_back( 0 );
			t[ i ].push_back( Arrow::NONE );
		}
	}

	

	stack<char> res;

	for ( size_t i = 0; i <= a.size(); i++ ) {
		for ( size_t j = 0; j <= b.size(); j++ ) {

			if ( i == 0 || j == 0 ) { 
				continue;
			}

			if ( a[ i - 1 ] == b[ j - 1 ] ) {
				arr[ i ][ j ] = arr[ i - 1 ][ j - 1 ] + 1;
				t[ i ][ j ] = Arrow::DIAGONAL; 
			}
			else if ( arr[ i - 1 ][ j ] >= arr[ i ][ j - 1 ] ) {
				
				arr[ i ][ j ] = arr[ i - 1 ][ j ];
				t[ i ][ j ] = Arrow::UP;				
			} else {

				arr[ i ][ j ] = arr[ i ][ j - 1 ];
				t[ i ][ j ] = Arrow::LEFT;

			}
		}
	}

	// preparing the result
	size_t i = a.size();
	size_t j = b.size();
	while ( i > 0 && j > 0 ) {
		
		if ( t[ i ][ j ] == Arrow::DIAGONAL ) {
			res.push (a[ i - 1 ]);
			i--;
			j--;
			continue;
		}

		if ( t[ i ][ j ] == Arrow::UP ) {
			i--;
			continue;
		}

		if ( t[ i ][ j ] == Arrow::LEFT ) {
			j--;
		}
	}

	vector<char> tmp;
	while ( res.size() > 0 ) {
		tmp.push_back( res.top() );
		res.pop();
	}


	cout << "    ";
	for (size_t i = 0; i < b.size(); i++) {
		cout << b[i] << " ";
	}
	cout << endl;
	for ( size_t i = 0; i < arr.size(); i++ ) {
		if (i > 0) {
			cout << a[i-1] << " ";
		} else {
			cout << "  ";
		}
		
		for ( size_t j = 0; j < arr[ i ].size(); j++ ) {
			cout << arr[i][j] << " ";
		}
		cout << endl;
	}

	return string( tmp.begin(), tmp.end() );

};
