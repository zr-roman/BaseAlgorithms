#pragma once

#include <vector>
#include <array>
#include <string>
#include <unordered_map>

using namespace std;

// по Макконнеллу

class BoyerMoore {
public:
	vector<unsigned int> GetAllOccurences( string s, string pattern ) {
		unordered_map<unsigned int, unsigned int> slide = GetSlide( pattern );
		vector<unsigned int> jump = GetJump( pattern );
		vector<unsigned int> res;
		auto textLoc = pattern.length() - 1;
		int patternLoc = pattern.length() - 1;
		while ( textLoc < s.length() && patternLoc >= 0 ) {
			if ( s[ textLoc ] == pattern[ patternLoc ] ) {
				textLoc--;
				patternLoc--;
			} else {
				textLoc += max( slide[ s[ textLoc ] ], jump[ patternLoc ] );
				patternLoc = pattern.length() - 1;
			}

			if ( patternLoc == -1 ) {
				res.push_back( textLoc + 1 );
				textLoc += max( slide[ s[ textLoc + 1 ] ], jump[ patternLoc + 1 ] );
				patternLoc = pattern.length() - 1;
			}
		}
		return res;
	};

private:

	unordered_map<unsigned int, unsigned int> GetSlide( string pattern ) {
		unordered_map<unsigned int, unsigned int> res;
		for ( size_t i = 0; i < pattern.length(); i++ ) {
			res[ pattern[ i ] ] = pattern.length() - 1 - i;
		}
		return res;
	};

	vector<unsigned int> GetJump( string pattern ) {

		vector<unsigned int> jump;
		for ( size_t i = 0; i < pattern.length(); i++ ) {
			jump.push_back( 2 * pattern.length() - 1 - i );
		}
		vector<unsigned int> link;
		link.resize( pattern.length() );

		int test = pattern.length() - 1;
		auto target = pattern.length();
		while ( test >= 0 ) {
			link[ test ] = target;
			while ( target <= pattern.length() - 1 && pattern[ test ] != pattern[ target ] ) {
				jump[ target ] = min<unsigned int>( jump[ target ], pattern.length() - 1 - test );
				target = link[ target ];
			}
			test--;
			target--;
		}

		for ( size_t i = 0; i <= target; i++ ) {
			jump[ i ] = min<unsigned int>( jump[ i ], pattern.length() + target - i );
		}

		auto temp = link[ target ];
		while ( target < pattern.length()  ) {
			while ( target <= temp ) {
				jump[ target ] = min<unsigned int>( jump[ target ], temp - target + pattern.length() );
				target--;
			}
			temp = link[ temp ];
		}
		return jump;
	};

};
