namespace StringMatching;
public enum KarpRabinHash : byte {
	TRIVIAL,
	STRONGER
}

public class KarpRabin {
	private const long Prime = 1_000_000_007;
	private const long HashBase = 257;

	private readonly KarpRabinHash hashType;

	public KarpRabin( KarpRabinHash h = KarpRabinHash.STRONGER ) {
		if ( h != KarpRabinHash.TRIVIAL && h != KarpRabinHash.STRONGER ) {
			throw new ArgumentOutOfRangeException( nameof( h ), h, "Unknown hash mode." );
		}

		hashType = h;
	}

	public uint[] GetAllOccurences( string s, string pattern ) {
		ArgumentNullException.ThrowIfNull( s );
		ArgumentNullException.ThrowIfNull( pattern );

		if ( pattern.Length == 0 ) {
			var boundaries = new uint[ s.Length + 1 ];
			for ( int i = 0; i < boundaries.Length; i++ ) {
				boundaries[ i ] = (uint)i;
			}
			return boundaries;
		}

		if ( pattern.Length > s.Length ) {
			return Array.Empty<uint>();
		}

		return hashType == KarpRabinHash.TRIVIAL
			? GetAllOccurencesTrivial( s, pattern )
			: GetAllOccurencesStrongerHash( s, pattern );
	}

	private static uint[] GetAllOccurencesTrivial( string s, string pattern ) {
		var matches = new List<uint>();
		long patternHash = 0;
		long windowHash = 0;

		for ( int i = 0; i < pattern.Length; i++ ) {
			patternHash += pattern[ i ];
			windowHash += s[ i ];
		}

		var lastStart = s.Length - pattern.Length;
		for ( int start = 0; start <= lastStart; start++ ) {
			if ( patternHash == windowHash && string.CompareOrdinal( s, start, pattern, 0, pattern.Length ) == 0 ) {
				matches.Add( (uint)start );
			}

			if ( start < lastStart ) {
				windowHash -= s[ start ];
				windowHash += s[ start + pattern.Length ];
			}
		}

		return matches.ToArray();
	}

	private static uint[] GetAllOccurencesStrongerHash( string s, string pattern ) {
		var matches = new List<uint>();
		long patternHash = 0;
		long windowHash = 0;
		long highestPower = 1;

		for ( int i = 1; i < pattern.Length; i++ ) {
			highestPower = highestPower * HashBase % Prime;
		}

		for ( int i = 0; i < pattern.Length; i++ ) {
			patternHash = ( patternHash * HashBase + pattern[ i ] ) % Prime;
			windowHash = ( windowHash * HashBase + s[ i ] ) % Prime;
		}

		var lastStart = s.Length - pattern.Length;
		for ( int start = 0; start <= lastStart; start++ ) {
			if ( patternHash == windowHash && string.CompareOrdinal( s, start, pattern, 0, pattern.Length ) == 0 ) {
				matches.Add( (uint)start );
			}

			if ( start < lastStart ) {
				var leadingValue = (long)s[ start ] * highestPower % Prime;
				windowHash = ( windowHash - leadingValue + Prime ) % Prime;
				windowHash = ( windowHash * HashBase + s[ start + pattern.Length ] ) % Prime;
			}
		}

		return matches.ToArray();
	}
}
