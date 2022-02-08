#pragma once
#include <vector>
#include <string>

using namespace std;

// Время предварительной обработки: 0
// Время сравнения: Ɵ((n - m + 1)m)

class BruteForce {

public:
	vector<unsigned int> GetAllOccurences( string s, string pattern ) {

		vector<unsigned int> res;

		for ( size_t i = 0; i < s.length() - pattern.length() + 1; i++ ) {
			
			bool found = true;

			for ( size_t j = 0; j < pattern.length(); j++ ) {

				if ( s[ i + j ] != pattern[ j ] ) {
					found = false;
					break;
				}

			}
			if ( found ) {
				res.push_back( i );
			}
		}
		return res;
	};
	
};
