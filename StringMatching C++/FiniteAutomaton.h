#pragma once

#include <string>
#include <vector>
#include <string>
#include <unordered_set>
#include <unordered_map>

using namespace std;

// according to Cormen
// Время предварительной обработки: O(m|Σ|)
// Время сравнения: Ɵ(n)

class FiniteAutomaton {

public:

	vector<unsigned int> GetAllOccurences( string s, string pattern ) {
		res.clear();
		this->pattern = pattern;
		this->s = s;
		curr_state = 0;
		final_state = 0;
		curr_index = 0;
		ComputeTransitions();
		for ( unsigned int i = 0; i < s.length(); i++ ) {
			MoveNext();
		}
		return res;
	};

private:
	unordered_set<char> alphabet;
	string s;
	string pattern;
	unordered_map<string, int> transitions;
	unsigned int curr_state;
	unsigned int final_state;
	unsigned int curr_index;
	vector<unsigned int> res;

	void MoveNext() {
		
		auto key = to_string( curr_state ) + " -> " + s [curr_index ];
		
		if ( transitions.count( key ) == 0 ) {
			curr_state = 0;
		} else {
			curr_state = transitions[ key ];
			if ( curr_state == final_state ) {
				res.push_back( curr_index - pattern.length() + 1 );
			}
		}
		curr_index++;
	};

	void ComputeTransitions() {

		alphabet.clear();
		
		for ( size_t i = 0; i < pattern.length(); i++ ) {
			alphabet.insert( pattern[ i ] );
		}
		transitions.clear();
		auto m = pattern.length();
		for ( unsigned int i = 0; i <= m; i++ ) {
			for ( auto& a : alphabet ) {
				auto k = min<unsigned int>( m + 1, i + 2 );
				do {
					k--;
				} while ( !IsSuffix( pattern.substr( 0, k ), pattern.substr( 0, i ) + a ) );
				
				transitions[ to_string( i ) + " -> " + a ] = k; //sigma( q, a ) = k;
				if ( k > final_state ) {
					final_state = k;
				}
			}
		}
	};

	bool IsSuffix( string suffix, string whole_str ) {
		if ( suffix.length() == 0 ) {
			return true;
		}
		for ( int i = whole_str.length() - 1, j = suffix.length() - 1; i >= 0 && j >= 0; i--, j-- ) {
			if ( suffix[ j ] != whole_str[ i ] ) {
				return false;
			}
		}
		return true;
	};

// Example
/*
	e v s
0  1 0 0 e
1  1 2 0 v
2  1 0 3 s
3  4 0 0 e
4  5 2 0 e
5  1 6 0 v
6  5 6 3
*/

};
