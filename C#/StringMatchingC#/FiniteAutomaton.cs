
// according to Cormen
// Время предварительной обработки: O(m|Σ|)
// Время сравнения: Ɵ(n)

using System;
using System.Collections.Generic;

public class FiniteAutomaton {

	public uint[] GetAllOccurences( string s, string pattern ) {
		res = new List<uint>();
		this.pattern = pattern;
		this.s = s;
		curr_state = final_state = curr_index = 0;
		ComputeTransitions();		
		for ( uint i = 0; i < s.Length; i++ ) {
			MoveNext();
		}
		return res.ToArray();
	}

	private HashSet<char> alphabet;
	private string s;
	private string pattern;
	private Dictionary<string, int> transitions;
	private uint curr_state;
	private uint final_state;
	private uint curr_index;
	private List<uint> res;

	private void MoveNext(  ) {
		var key = curr_state + " -> " + s[ (int)curr_index ];
        if ( transitions.ContainsKey( key ) ) {
			curr_state = (uint)transitions[ key ];
			if ( curr_state == final_state ) {
				res.Add( (uint)( curr_index - pattern.Length + 1 ) );
			}
        } else {
			curr_state = 0;
        }
				
		curr_index++;
	}

	private void ComputeTransitions() {
		alphabet = new HashSet<char>();
		for ( int i = 0; i < pattern.Length; i++ ) {
			alphabet.Add( pattern[ i ] );
		}
		transitions = new Dictionary<string, int>();
		var m = pattern.Length;
		for ( int q = 0; q <= m; q++ ) {
			foreach ( var a in alphabet ) {
				var k = Math.Min( m + 1, q + 2 );
				do {
					k--;
				} while ( !IsSuffix( pattern.Substring( 0, k ), pattern.Substring( 0, q ) + a ) );
				
				transitions[ q + " -> " + a ] = k; //sigma( q, a ) = k;
				if ( k > final_state ) {
					final_state = (uint)k;
				}
			}
		}
	}

	private bool IsSuffix( string suffix, string whole_str ) {
		if ( suffix.Length == 0 ) {
			return true;
		}
		for ( int i = whole_str.Length - 1, j = suffix.Length - 1; i >= 0 && j >= 0; i--, j-- ) {
			if ( suffix[ j ] != whole_str[ i ] ) {
				return false;
			}
		}
		return true;
	}

// Example
/*
	e v s
0  1 0 0 e
1  1 2 0 v
2  1 0 3 s
3  4 0 0 e
4  5 2 0 e
5  1 6 0 v
6  5 6 3
*/

}
