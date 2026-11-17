namespace HashTables;

/// <summary>
/// PERFECT HASHING
/// Двухуровневое хеширование для заранее известного числа ключей: поиск за O(1) в худшем случае.
/// </summary>
/// <remarks>
/// Ограничения:
/// 1. Число ключей не может превышать заданное в конструкторе.
/// 2. У всех ключей должны быть разные хеш-коды (GetHashCode). Таблица различает ключи по числу,
///    полученному из хеш-кода, и два ключа с одинаковым числом на втором уровне развести невозможно.
///    Для int, short, char и подобных типов это выполняется всегда. Для long, string, Guid
///    совпадения возможны, например у long 1 и 1L &lt;&lt; 32.
/// </remarks>
/// <typeparam name="TKey">Тип ключа</typeparam>
/// <typeparam name="TValue">Тип значения</typeparam>
public class PerfectHashTable<TKey, TValue> : HashTable<TKey, TValue> {

	private readonly int max_num_of_keys = 0;
    private const int AvgKeysPerSlot = 5;  // в среднем ключей на ячейку первого уровня

    public PerfectHashTable( int max_num_of_keys ) : base( HashMethod.UniversalHashing ) {
        ArgumentOutOfRangeException.ThrowIfNegative(max_num_of_keys);

        m = (uint)Math.Max(1, max_num_of_keys / AvgKeysPerSlot);

        main_t = new Record_Base[ m ][ ];
		pars = new Pars[ m ];
		this.max_num_of_keys = max_num_of_keys;
	}

	public override void Insert( TKey key, TValue value ) {

        ArgumentNullException.ThrowIfNull(key);

        if ( search( key ).Found )
            throw new ArgumentException("Key duplicating is not allowed!");

        if (total_count >= max_num_of_keys)
            throw new InvalidOperationException($"The table was created for {max_num_of_keys} keys and is full");

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
			
			if ( main_t[ hash1 ][ hash2 ] == null )
				break;

			FullRehash2ndLevel( hash1, pars[ hash1 ].cnt_elms );

            // Второй уровень перестраивается со случайными a и b, пока новый ключ не попадёт в свободную ячейку.
            // Для ключей с разными числами одна попытка неудачна с вероятностью не больше 1/2,
            // поэтому 100 неудач подряд означают, что у нового ключа то же число, что и у ключа,
            // который уже лежит в этой ячейке: их хеш-коды совпали.
            if ( counter > 100 ) {
                // ключ не вставлен: вернуть ячейку к прежнему числу элементов
                FullRehash2ndLevel(hash1, pars[hash1].cnt_elms - 1);

                throw new ArgumentException($"Key '{key}' has the same hash code as a key that is already in the table. " +
					"Perfect hashing requires keys with distinct hash codes.",
					nameof(key));
			}
			counter++;
		}

		main_t[ hash1 ][ hash2 ] = new Record_Base( key, value );
		total_count++;

    }

	public override void Delete( TKey key ) {
		var res = search( key );
		if ( ! res.Found ) { return; }
		var hash1 = res.Index1;
		main_t[ hash1 ][ res.Index2 ] = null;
		FullRehash2ndLevel( hash1, pars[ hash1 ].cnt_elms - 1 );
        total_count--;
    }

	public override bool Search( TKey key, out TValue value ) {

		var r = search( key );

		value = r.Found ? main_t[ r.Index1 ][ r.Index2 ].value : default;

		return r.Found;
	}

	struct Pars {
		public uint m;
		public uint a;
		public uint b;
		public uint cnt_elms;
	}

    readonly Record_Base[][] main_t;
    readonly Pars[] pars;

	int total_count = 0;

	private (bool Found, uint Index1, uint Index2) search( TKey key ) {

        (bool Found, uint Index1, uint Index2) not_found_res = ( false, 0, 0 );

		var hash1 = GetHash( key );

		if ( main_t[ hash1 ] == null )
			return not_found_res;

		var hash2 = GetHash( key, pars[ hash1 ].a, pars[ hash1 ].b, pars[ hash1 ].m );
		
		if ( main_t[ hash1 ][ hash2 ] == null )
			return not_found_res;

		if ( EqualityComparer<TKey>.Default.Equals( main_t[ hash1 ][ hash2 ].key, key ) )
			return ( true, hash1, hash2 );

		return not_found_res;
		
	}

	void FullRehash2ndLevel( uint hash1, uint cnt_elms ) {

		if ( cnt_elms == 0 ) {
			main_t[ hash1 ] = null;
			return; 
		}

		pars[ hash1 ].cnt_elms = cnt_elms;
		pars[ hash1 ].m = pars[ hash1 ].cnt_elms * pars[ hash1 ].cnt_elms;
		var tmp_vect = main_t[ hash1 ];

		while ( true ) {
								
			main_t[ hash1 ] = null;
			main_t[ hash1 ] = new Record_Base[ pars[ hash1 ].m ];
			pars[ hash1 ].a = GenerateRandomUInt( 1, uint.MaxValue - 1 );
			pars[ hash1 ].b = GenerateRandomUInt( 0, uint.MaxValue - 1 );
			
			bool done = true;
			for ( uint i = 0; i < tmp_vect.Length; i++ ) {

				if ( tmp_vect[ i ] == null )
					continue; 

				var hash2 = GetHash( tmp_vect[ i ].key, pars[ hash1 ].a, pars[ hash1 ].b, pars[ hash1 ].m );

				if ( main_t[ hash1 ][ hash2 ] != null ) { 
					done = false; 
					break; 
				}

				main_t[ hash1 ][ hash2 ] = new Record_Base( tmp_vect[ i ].key, tmp_vect[ i ].value );
			}

			if ( done )
				break;
		}
	}
}
