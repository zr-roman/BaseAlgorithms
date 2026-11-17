#pragma once

#include "HashingAbstract.h"
#include <vector>
#include <memory>

using namespace std;

// PERFECT HASHING
template <typename TKey, typename TValue>
class DicPerfect : public HashingAbstract<TKey, TValue> {
public:

	using HashMethod = HashingAbstract<TKey, TValue>::HashMethod;

	DicPerfect( int max_num_of_keys ) : HashingAbstract<TKey, TValue>( HashMethod::UNIVERSAL_HASHING ) {
		m = max_num_of_keys / 5;
		main_t.resize( m );
		pars.resize( m );
	};

	virtual void Insert( const TKey& key, const TValue& value ) override {

		if ( Search( key ).first ) {
			return;
		}

		auto hash1 = HashFunc( key );

		if ( main_t[ hash1 ].size() == 0 ) {
			pars[ hash1 ] = move( make_unique<Pars>() );
			pars[ hash1 ]->a = pars[ hash1 ]->b = 0;
			pars[ hash1 ]->m = pars[ hash1 ]->cnt_elms = 1;
			main_t[ hash1 ].resize( pars[ hash1 ]->cnt_elms );
		} else {
			FullRehash2ndLevel( hash1, pars[ hash1 ]->cnt_elms + 1 );
		}

		unsigned int hash2;
		int counter = 0;

		while ( true ) {
			
			hash2 = HashFunc( key, move( make_unique<unsigned int>( pars[ hash1 ]->a ) ), move( make_unique<unsigned int>( pars[ hash1 ]->b ) ), move( make_unique<unsigned int>( pars[ hash1 ]->m ) ) );
			
			if ( main_t[ hash1 ][ hash2 ] == nullptr ) { break; }

			FullRehash2ndLevel( hash1, pars[ hash1 ]->cnt_elms );
			
			// we do 100 tries before quitting
			// we do this because perfect hashing is used only when we work with finite and relatively small (at least reasonable) number of keys
			// if the number of keys is potentially infinite or unreasonably large, the perfect hashin is not gonna work
			if ( counter > 100 ) {
				throw runtime_error("Not wokring! Set lesser number of keys!");
			}
			counter++;
		}

		main_t[ hash1 ][ hash2 ] = make_unique<Record_Base>( key, value );

	};

	virtual void Delete( const TKey& key ) override {
		auto res = _search( key );
		if ( !get<0>( res ) ) {
			return;
		}
		auto hash1 = get<1>( res );
		main_t[ hash1 ][ get<2>( res ) ] = nullptr;
		FullRehash2ndLevel( hash1, pars[ hash1 ]->cnt_elms - 1 );
	};

	virtual pair<bool, TValue>Search( const TKey& key) override {

		auto r = _search( key );
		
		if ( !get<0>( r ) ) {
			TValue v;
			return make_pair( false, move( v ) );
		}

		return make_pair( true, main_t[ get<1>( r ) ][ get<2>( r ) ]->value );
	};

private:
	using HashingAbstract<TKey, TValue>::m;
	using HashingAbstract<TKey, TValue>::Record_Base;
	using HashingAbstract<TKey, TValue>::HashFunc;	

	struct Pars {
		unsigned int m;
		unsigned int a;
		unsigned int b;
		unsigned int cnt_elms;
	};

	vector<vector<unique_ptr<Record_Base>>> main_t;
	vector<unique_ptr<Pars>> pars;

	tuple<bool, unsigned int, unsigned int> _search( const TKey& key ) {
		
		auto not_found_res = make_tuple( false, 0, 0);

		auto hash1 = HashFunc( key );

		if ( main_t[ hash1 ].size() == 0 ) {
			return not_found_res;
		}

		decltype(auto) hash2 = HashFunc( key, move( make_unique<unsigned int>( pars[ hash1 ]->a ) ), move( make_unique<unsigned int>( pars[ hash1 ]->b ) ), move( make_unique<unsigned int>( pars[ hash1 ]->m  ) ) );
		
		if ( main_t[ hash1 ][ hash2 ] == nullptr ) {
			return not_found_res;
		}

		if ( main_t[ hash1 ][ hash2 ]->key == key ) {
			return make_tuple( true, hash1, hash2 );
		}

		return not_found_res;
		
	};

	auto FullRehash2ndLevel( unsigned int hash1, unsigned int cnt_elms ) -> void {

		if ( cnt_elms == 0 ) { main_t[ hash1 ].clear(); return; }

		pars[ hash1 ]->cnt_elms = cnt_elms;
		pars[ hash1 ]->m = pars[ hash1 ]->cnt_elms * pars[ hash1 ]->cnt_elms;
		auto tmp_vect = move( main_t[ hash1 ] );

		while ( true ) {
								
			main_t[ hash1 ].clear();
			main_t[ hash1 ].resize( pars[ hash1 ]->m );
			pars[ hash1 ]->a = HashingAbstract<TKey, TValue>::GenerateRandomNumber( 0, UINT32_MAX - 1 );
			pars[ hash1 ]->b = HashingAbstract<TKey, TValue>::GenerateRandomNumber( 1, UINT32_MAX - 1 );
			
			bool done = true;
			for ( size_t i = 0; i < tmp_vect.size(); i++ ) {

				if ( tmp_vect[ i ] == nullptr ) { continue; }

				auto hash2 = HashFunc( tmp_vect[ i ]->key, move( make_unique<unsigned int>( pars[ hash1 ]->a ) ), move( make_unique<unsigned int>( pars[ hash1 ]->b ) ), move( make_unique<unsigned int>( pars[ hash1 ]->m ) ) );

				if ( main_t[ hash1 ][ hash2 ] != nullptr ) { done = false; break; }

				main_t[ hash1 ][ hash2 ] = make_unique<Record_Base>( tmp_vect[ i ]->key, tmp_vect[ i ]->value );
			}

			if ( done ) {
				break;
			}
		}
	};
};
