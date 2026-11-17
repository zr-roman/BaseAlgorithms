
// Время предварительной обработки: 0
// Время сравнения: Ɵ((n - m + 1)m)

using System.Collections.Generic;

public class BruteForce {

	public uint[] GetAllOccurences( string s, string pattern ) {

		var res = new List<uint>();

		for ( uint i = 0; i < s.Length - pattern.Length + 1; i++ ) {
			
			bool found = true;

			for ( uint j = 0; j < pattern.Length; j++ ) {

				if ( s[ (int)(i + j) ] != pattern[ (int)j ] ) {
					found = false;
					break;
				}

			}
			if ( found ) {
				res.Add( i );
			}
		}
		return res.ToArray();
	}
	
}
