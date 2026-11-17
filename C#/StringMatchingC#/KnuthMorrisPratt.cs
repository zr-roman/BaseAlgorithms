namespace StringMatching;

// according to Cormen
// Время предварительной обработки: Ɵ(m)
// Время сравнения: Ɵ(n)

public class KMP {

	public uint[] GetAllOccurences( string s, string pattern ) {
	
		var res = new List<uint>();
		if ( pattern.Length == 0 ) {
			for ( int i = 0; i <= s.Length; i++ ) {
				res.Add( (uint)i );
			}
			return res.ToArray();
		}

		var pi = ComputePrefixFunction( pattern );
		var q = 0;
		for ( int i = 0; i < s.Length; i++ ) {
			while ( q > 0 && pattern[ q ] != s[ i ] ) {
				q = pi[ q - 1 ];
			}
			if ( pattern[ q ] == s[ i ] ) {
				q++;
			}
			if ( q == pattern.Length ) {
				res.Add( (uint)( i - pattern.Length + 1 ) );
				q = pi[ q - 1 ];
			}
		}
		return res.ToArray();
	}

	private int[] ComputePrefixFunction( string pattern ) {
		
		int[] pi = new int[ pattern.Length ];
		var k = 0;
		for ( int i = 1; i < pattern.Length; i++ ) {
			while ( k > 0 && pattern[ k ] != pattern[ i ] ) {
				k = pi[ k - 1 ];
			}
			if ( pattern[ k ] == pattern[ i ] ) {
				k++;
			}
			pi[ i ] = k;
		}
		return pi;
	}
}
