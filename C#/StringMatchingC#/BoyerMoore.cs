
// по Макконнеллу

using System;
using System.Collections.Generic;

public class BoyerMoore {
	public int[] GetAllOccurences( string s, string pattern ) {
		Dictionary<int, int> slide = GetSlide( pattern );
		int[] jump = GetJump( pattern );
		var res = new List<int>();
		var textLoc = pattern.Length - 1;
		var patternLoc = pattern.Length - 1;
		while ( textLoc < s.Length && patternLoc >= 0 ) {
			if ( s[ textLoc ] == pattern[ patternLoc ] ) {
				textLoc--;
				patternLoc--;
			} else {
				textLoc += Math.Max( slide.ContainsKey( s[ textLoc ]) ? slide[ s[ textLoc ] ] : 0, jump[ patternLoc ] );
				patternLoc = pattern.Length - 1;
			}

			if ( patternLoc == -1 ) {
				res.Add( textLoc + 1 );
				textLoc += Math.Max( slide[ s[ textLoc + 1 ] ], jump[ patternLoc + 1 ] );
				patternLoc = pattern.Length - 1;
			}
		}
		return res.ToArray();
	}

	private Dictionary<int, int> GetSlide( string pattern ) {
		var res = new Dictionary<int, int>();
		for ( int i = 0; i < pattern.Length; i++ ) {
			res[ pattern[ i ] ] = pattern.Length - 1 - i;
		}
		return res;
	}

	private int[] GetJump( string pattern ) {

		var jump = new List<int>();
		for ( int i = 0; i < pattern.Length; i++ ) {
			jump.Add( 2 * pattern.Length - 1 - i );
		}

		var link = new int[ pattern.Length ];

		var test = pattern.Length - 1;
		var target = pattern.Length;
		while ( test >= 0 ) {
			link[ test ] = target;
			while ( target <= pattern.Length - 1 && pattern[ test ] != pattern[ target ] ) {
				jump[ target ] = Math.Min( jump[ target ], pattern.Length - 1 - test );
				target = link[ target ];
			}
			test--;
			target--;
		}

		for ( int i = 0; i <= target; i++ ) {
			jump[ i ] = Math.Min( jump[ i ], pattern.Length + target - i );
		}

		var temp = link[ target ];
		while ( target < pattern.Length && target >= 0 ) {
			while ( target <= temp && target >= 0 ) {
				jump[ target ] = Math.Min( jump[ target ], temp - target + pattern.Length );
				target--;
			}
			temp = link[ temp ];
		}
		return jump.ToArray();
	}

}
