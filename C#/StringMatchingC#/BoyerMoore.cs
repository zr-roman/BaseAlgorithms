namespace StringMatching;

// Boyer–Moore string search using the bad-character and good-suffix rules.
public class BoyerMoore {
	public int[] GetAllOccurences( string s, string pattern ) {
		ArgumentNullException.ThrowIfNull( s );
		ArgumentNullException.ThrowIfNull( pattern );

		if ( pattern.Length == 0 ) {
			var boundaries = new int[ s.Length + 1 ];
			for ( int i = 0; i < boundaries.Length; i++ ) {
				boundaries[ i ] = i;
			}
			return boundaries;
		}

		if ( pattern.Length > s.Length ) {
			return Array.Empty<int>();
		}

		var lastOccurrence = BuildLastOccurrenceTable( pattern );
		var goodSuffixShift = BuildGoodSuffixShiftTable( pattern );
		var matches = new List<int>();
		var start = 0;

		while ( start <= s.Length - pattern.Length ) {
			var patternIndex = pattern.Length - 1;
			while ( patternIndex >= 0 && pattern[ patternIndex ] == s[ start + patternIndex ] ) {
				patternIndex--;
			}

			if ( patternIndex < 0 ) {
				matches.Add( start );
				start += goodSuffixShift[ 0 ];
				continue;
			}

			var mismatchedCharacter = s[ start + patternIndex ];
			var lastIndex = lastOccurrence.TryGetValue( mismatchedCharacter, out var index ) ? index : -1;
			var badCharacterShift = Math.Max( 1, patternIndex - lastIndex );
			start += Math.Max( badCharacterShift, goodSuffixShift[ patternIndex + 1 ] );
		}

		return matches.ToArray();
	}

	private static Dictionary<char, int> BuildLastOccurrenceTable( string pattern ) {
		var lastOccurrence = new Dictionary<char, int>();
		for ( int i = 0; i < pattern.Length; i++ ) {
			lastOccurrence[ pattern[ i ] ] = i;
		}
		return lastOccurrence;
	}

	private static int[] BuildGoodSuffixShiftTable( string pattern ) {
		var length = pattern.Length;
		var shift = new int[ length + 1 ];
		var borderPosition = new int[ length + 1 ];

		var i = length;
		var j = length + 1;
		borderPosition[ i ] = j;

		while ( i > 0 ) {
			while ( j <= length && pattern[ i - 1 ] != pattern[ j - 1 ] ) {
				if ( shift[ j ] == 0 ) {
					shift[ j ] = j - i;
				}
				j = borderPosition[ j ];
			}

			i--;
			j--;
			borderPosition[ i ] = j;
		}

		j = borderPosition[ 0 ];
		for ( i = 0; i <= length; i++ ) {
			if ( shift[ i ] == 0 ) {
				shift[ i ] = j;
			}
			if ( i == j ) {
				j = borderPosition[ j ];
			}
		}

		return shift;
	}
}
