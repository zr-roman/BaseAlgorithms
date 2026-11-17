#pragma once
#include <vector>
#include <string>

using namespace std;

// according to Cormen
// Время предварительной обработки: Ɵ(m)
// Время сравнения: Ɵ(n)

class KMP {
public:
	vector<unsigned int> GetAllOccurences( string s, string pattern ) {
	
		vector<unsigned int> res;

		auto m = pattern.length() - 1;
		auto pi = ComputePrefixFunction( pattern );
		auto q = 0;
		for ( size_t i = 0; i < s.length(); i++ ) {
			while ( q > 0 && pattern[ q + 1 ] != s[ i ] ) {
				q = pi[ q ];
			}
			if ( pattern[ q + 1 ] == s[ i ] ) {
				q++;
			}
			if ( q == m ) {
				res.push_back( i - m );
				q = pi[ q ];
			}
		}
		return res;
	}

private:

	vector<int> ComputePrefixFunction( string pattern ) {
		vector<int> pi;
		pi.resize( pattern.length() );
		pi[0] =-1;
		/* for ababaca
		pi[1] =-1;
		pi[2] = 0;
		pi[3] = 1;
		pi[4] = 2;
		pi[5] =-1;
		pi[6] = 0;
		*/
		auto k = -1;
		for (size_t q = 1; q < pattern.length(); q++) {
			while ( k > -1 && pattern[ k + 1 ] != pattern[ q ] ) {
				k = pi[ k ];
			}
			if ( pattern[ k + 1 ] == pattern[ q  ] ) {
				k++;
			}
			pi[ q ] = k ;
		}
		return pi;
	};

};
