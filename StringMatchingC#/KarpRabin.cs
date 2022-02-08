
// Idea: rolling hash
// Время предварительной обработки: Ɵ(m)
// Время сравнения: Ɵ((n - m + 1)m)

using System;
using System.Collections.Generic;

public enum KarpRabinHash : byte {
	TRIVIAL,
	STRONGER
};

public class KarpRabin {

	public KarpRabin( KarpRabinHash h = KarpRabinHash.STRONGER ) {
		hash_type = h;
	}
	
	public uint[] GetAllOccurences( string s, string pattern ) {
		this.s = s;
		this.pattern = pattern;
		res = new List<uint>();
		alphabet = new HashSet<char>();
        return hash_type switch {
            KarpRabinHash.TRIVIAL => GetAllOccurencesTrivial(),
            KarpRabinHash.STRONGER => GetAllOccurencesStrongerHash(),
            _ => throw new Exception( "Must not get into here!" ),
        };
    }

	private string s;
	private string pattern;
	private List<uint> res;
	private KarpRabinHash hash_type;
	private HashSet<char> alphabet;
	private uint prime = 701;

	private uint[] GetAllOccurencesTrivial( ) {

		var hash_patt = 0;
		var rolling_hash = 0;
		for ( int i = 0; i < pattern.Length; i++ ) {
			hash_patt += pattern[ i ];
			rolling_hash += s[ i ];
		}
		
		for ( int i = pattern.Length; true; i++ ) {
			
			if ( hash_patt == rolling_hash ) {
				bool match = true;
				for ( int j = 0; j < pattern.Length; j++ ) {
					if ( pattern[ j ] != s[ (j + i - pattern.Length) ] ) {
						match = false;
						break;
					}
				}
				if ( match ) {
					res.Add( (uint)(i - pattern.Length) );
				}
			}
			if ( i >= s.Length ) {
				break;
			}
			rolling_hash += s[ i ];
			rolling_hash -= s[ (i - pattern.Length) ];
		}
		return res.ToArray();
	}

	private uint[] GetAllOccurencesStrongerHash( ) {

		var _base = alphabet.Count;
		var hash_patt = 0;
		var rolling_hash = 0;
		
		for ( int i = 0; i < pattern.Length; i++ ) {
			hash_patt =    (int)((hash_patt + pattern[ i ] * Math.Pow( _base, pattern.Length - 1 - i ) ) % prime);
			rolling_hash = (int)((rolling_hash + s[ i ]    * Math.Pow( _base, pattern.Length - 1 - i ) ) % prime);
		}
		
		for ( int i = pattern.Length; true; i++ ) {
			
			if ( hash_patt == rolling_hash ) {
				bool match = true;
				for ( int j = 0; j < pattern.Length; j++ ) {
					if ( pattern[ j ] != s[ j + i - pattern.Length ] ) {
						match = false;
						break;
					}
				}
				if ( match ) {
					res.Add( (uint)(i - pattern.Length) );
				}
			}
			if ( i >= s.Length ) {
				break;
			}
			
			rolling_hash = (int)((( rolling_hash - ( s[ i - pattern.Length ] * Math.Pow( _base, pattern.Length - 1 ) ) ) * _base + s[ i ]) % prime);
		}
		return res.ToArray();
	}

}
