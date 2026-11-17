
// according to Cormen
// Время предварительной обработки: Ɵ(m)
// Время сравнения: Ɵ(n)

using System.Collections.Generic;

public class KMP {

	public uint[] GetAllOccurences( string s, string pattern ) {
	
		var res = new List<uint>();

		var m = pattern.Length - 1;
		var pi = ComputePrefixFunction( pattern );
		var q = 0;
		for ( int i = 0; i < s.Length; i++ ) {
			while ( q > 0 && pattern[ q + 1 ] != s[ i ] ) {
				q = pi[ q ];
			}
			if ( pattern[ q + 1 ] == s[ i ] ) {
				q++;
			}
			if ( q == m ) {
				res.Add( (uint)( i - m ) );
				q = pi[ q ];
			}
		}
		return res.ToArray();
	}

	private int[] ComputePrefixFunction( string pattern ) {
		
		int[] pi = new int[ pattern.Length ];
		pi[ 0 ]  =-1;
		
		/* for ababaca
		pi[1] =-1;
		pi[2] = 0;
		pi[3] = 1;
		pi[4] = 2;
		pi[5] =-1;
		pi[6] = 0;
		*/

		var k = -1;
		for ( int i = 1; i < pattern.Length; i++ ) {
			while ( k > -1 && pattern[ k + 1 ] != pattern[ i ] ) {
				k = pi[ k ];
			}
			if ( pattern[ k + 1 ] == pattern[ i  ] ) {
				k++;
			}
			pi[ i ] = k ;
		}
		return pi;
	}
}
