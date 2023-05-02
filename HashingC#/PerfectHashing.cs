using System;

/// <summary>
/// PERFECT HASHING
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
/// <typeparam name="TValue">Тип значения</typeparam>
public class DicPerfect<TKey, TValue> : Hashing<TKey, TValue> where TKey: IComparable {

	public DicPerfect( int max_num_of_keys ) : base( HashMethod.UNIVERSAL_HASHING ) {
		m = (uint)(max_num_of_keys / 5);
		main_t = new Record_Base[ m ][];
		pars = new Pars[ m ];
	}

	public override void Insert( TKey key, TValue value ) {

		if ( Search( key ).Item1 ) { return; }

		var hash1 = GetHash( key );

		if ( main_t[ hash1 ] == null ) {
			pars[ hash1 ] = new Pars();
			pars[ hash1 ].a = pars[ hash1 ].b = 0;
			pars[ hash1 ].m = pars[ hash1 ].cnt_elms = 1;
			main_t[ hash1 ] = new Record_Base[ pars[ hash1 ].cnt_elms ];
		} else {
			FullRehash2ndLevel( hash1, pars[ hash1 ].cnt_elms + 1 );
		}

		uint hash2;
		int counter = 0;

		while ( true ) {
			
			hash2 = GetHash( key, pars[ hash1 ].a, pars[ hash1 ].b, pars[ hash1 ].m );
			
			if ( main_t[ hash1 ][ hash2 ] == null ) { 
				break; 
			}

			FullRehash2ndLevel( hash1, pars[ hash1 ].cnt_elms );
			
			// we do 100 tries before quitting
			// we do this because perfect hashing is used only for predefined and not changing finite reasonable number of keys
			// if the number of keys is varied or potentially infinite - then perfect hashing is not working and cannot be chosen for implementing
			if ( counter > 100 ) {
				throw new Exception( "Not wokring! Set lesser number of keys!" );
			}
			counter++;
		}

		main_t[ hash1 ][ hash2 ] = new Record_Base( key, value );
	}

	public override void Delete( TKey key ) {
		var res = search( key );
		if ( ! res.Item1 ) { return; }
		var hash1 = res.Item2;
		main_t[ hash1][ res.Item3 ] = null;
		FullRehash2ndLevel( hash1, pars[ hash1 ].cnt_elms - 1 );
	}

	public override Tuple<bool, TValue> Search( TKey key) {

		var r = search( key );
		
		if ( !r.Item1 ) {
			return new Tuple<bool, TValue>( false, default );
		}

		return new Tuple<bool, TValue>( true, main_t[ r.Item2 ][ r.Item3 ].value );
	}

	struct Pars {
		public uint m;
		public uint a;
		public uint b;
		public uint cnt_elms;
	}

    readonly Record_Base[][] main_t;
    readonly Pars[] pars;

	Tuple<bool, uint, uint> search( TKey key ) {
		
		var not_found_res = new Tuple<bool, uint, uint>( false, 0, 0);

		var hash1 = GetHash( key );

		if ( main_t[ hash1 ] == null ) {
			return not_found_res;
		}

		var hash2 = GetHash( key, pars[ hash1 ].a, pars[ hash1 ].b, pars[ hash1 ].m );
		
		if ( main_t[ hash1 ][ hash2 ] == null ) {
			return not_found_res;
		}

		if ( main_t[ hash1 ][ hash2 ].key.CompareTo( key ) == 0 ) {
			return new Tuple<bool, uint, uint>( true, hash1, hash2 );
		}

		return not_found_res;
		
	}

	void FullRehash2ndLevel( uint hash1, uint cnt_elms ) {

		if ( cnt_elms == 0 ) { main_t[ hash1 ] = null; return; }

		pars[ hash1 ].cnt_elms = cnt_elms;
		pars[ hash1 ].m = pars[ hash1 ].cnt_elms * pars[ hash1 ].cnt_elms;
		var tmp_vect = main_t[ hash1 ];

		while ( true ) {
								
			main_t[ hash1 ] = null;
			main_t[ hash1 ] = new Record_Base[ pars[ hash1 ].m ];
			pars[ hash1 ].a = GenerateRandomUInt( 0, uint.MaxValue - 1 );
			pars[ hash1 ].b = GenerateRandomUInt( 1, uint.MaxValue - 1 );
			
			bool done = true;
			for ( uint i = 0; i < tmp_vect.Length; i++ ) {

				if ( tmp_vect[ i ] == null ) { 
					continue; 
				}

				var hash2 = GetHash( tmp_vect[ i ].key, pars[ hash1 ].a, pars[ hash1 ].b,  pars[ hash1 ].m );

				if ( main_t[ hash1 ][ hash2 ] != null ) { 
					done = false; 
					break; 
				}

				main_t[ hash1 ][ hash2 ] = new Record_Base( tmp_vect[ i ].key, tmp_vect[ i ].value );
			}

			if ( done ) {
				break;
			}
		}
	}
}
