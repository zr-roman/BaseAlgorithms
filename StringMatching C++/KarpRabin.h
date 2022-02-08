#pragma once
#include<string>
#include<vector>

using namespace std;

// Idea: rolling hash
// Время предварительной обработки: Ɵ(m)
// Время сравнения: Ɵ((n - m + 1)m)

enum class KarpRabinHash {
	TRIVIAL,
	STRONGER
};

class KarpRabin {
public:

	KarpRabin( KarpRabinHash h = KarpRabinHash::STRONGER ) {
		hash_type = h;
	};
		
	vector<unsigned int> GetAllOccurences( string s, string pattern ) {
		switch ( hash_type ) {
			case KarpRabinHash::TRIVIAL:
				return GetAllOccurencesTrivial( s, pattern );
			case KarpRabinHash::STRONGER:
				return GetAllOccurencesStrongerHash( s, pattern );
			default:
				throw runtime_error("Must not get into here!");
		}
	};

private:

	string s;
	string pattern;
	vector<unsigned int> res;
	KarpRabinHash hash_type;
	unordered_set<char> alphabet;
	unsigned int prime = 701;

	vector<unsigned int> GetAllOccurencesTrivial( string s, string pattern ) {

		auto hash_patt = 0;
		auto rolling_hash = 0;
		for ( size_t i = 0; i < pattern.length(); i++ ) {
			hash_patt += pattern[i];
			rolling_hash += s[i];
		}
		
		for ( size_t i = pattern.length(); true; i++ ) {
			
			if ( hash_patt == rolling_hash ) {
				bool match = true;
				for ( size_t j = 0; j < pattern.length(); j++ ) {
					if ( pattern[ j ] != s[ j + i - pattern.length() ] ) {
						match = false;
						break;
					}
				}
				if ( match ) {
					res.push_back( i - pattern.length() );
				}
			}
			if ( i >= s.length() ) {
				break;
			}
			rolling_hash += s[ i ];
			rolling_hash -= s[ i - pattern.length() ];	
		}
		return res;
	};

	vector<unsigned int> GetAllOccurencesStrongerHash( string s, string pattern ) {

		auto base = alphabet.size();
		auto hash_patt = 0;
		auto rolling_hash = 0;
		
		for ( size_t i = 0; i < pattern.length(); i++ ) {
			hash_patt = (unsigned int)(hash_patt + pattern[ i ] * pow( base, pattern.length() - 1 - i ) ) % prime;
			rolling_hash = (unsigned int)(rolling_hash + s[ i ] * pow( base, pattern.length() - 1 - i ) ) % prime;
		}
		
		for ( size_t i = pattern.length(); true; i++ ) {
			
			if ( hash_patt == rolling_hash ) {
				bool match = true;
				for ( size_t j = 0; j < pattern.length(); j++ ) {
					if ( pattern[ j ] != s[ j + i - pattern.length() ] ) {
						match = false;
						break;
					}
				}
				if ( match ) {
					res.push_back( i - pattern.length() );
				}
			}
			if ( i >= s.length() ) {
				break;
			}
			
			rolling_hash = (unsigned int)(( rolling_hash - ( s[ i - pattern.length() ] * pow( base, pattern.length() - 1 ) ) ) * base + s[ i ]) % prime;
		}
		return res;
	};

};
