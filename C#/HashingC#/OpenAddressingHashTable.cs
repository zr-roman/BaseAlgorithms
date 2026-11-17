namespace HashTables;

public enum ProbingMethod
{
    LinearProbing,
    QuadraticProbing,
    DoubleHashing
}

/// <summary>
/// OPEN ADDRESSING 
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
/// <typeparam name="TValue">Тип значения</typeparam>
public class OpenAddressingHashTable<TKey, TValue> : HashTable<TKey, TValue> {

    public OpenAddressingHashTable( ProbingMethod probingMethod = ProbingMethod.QuadraticProbing )
            : base( HashMethod.UniversalHashing ) {
        m = 16;
        arr = new Record[ m ];
        this.probing_method = probingMethod;
        count_elements = 0;
    }

    public override void Insert( TKey key, TValue value ) {

        ArgumentNullException.ThrowIfNull(key);

        if ( search( key ).Found )
            throw new ArgumentException( "Key duplicating is not allowed!" );

        if ( count_elements + total_deleted >= arr.Length / 2 ) {
            if ( count_elements >= arr.Length / 4)
                TableDoubling_FullRehash();      // живых ключей много - удвоить таблицу
            else
                TableResizing_FullRehash(m); // место заняли удалённые записи - пересобрать в том же размере
        }

        // если проба не нашла свободной ячейки - удвоить таблицу и попробовать снова
        while ( !TryPlace( key, value ) )
            TableDoubling_FullRehash();
    }

    public override void Delete( TKey key ) {

        var search_res = search( key );

        if ( search_res.Found )
            _delete( search_res.Index );
    }

    public override bool Search( TKey key, out TValue value ) {

        var search_res = search( key );

        value = search_res.Item1 ? arr[ search_res.Item2 ].value : default;

        return search_res.Item1;
    }

    // constants for quadratic probing
    // h(k, i) = ( h(k) + c1*i + c2*i*i ) mod m,  где c1 = C1 / DEN, c2 = C2 / DEN
    const ulong C1 = 3;   // c1 = 3/2
    const ulong C2 = 3;   // c2 = 3/2
    const ulong DEN = 2;

    uint count_elements; // всего элементов со значениями

    uint total_deleted; // всего элементов помеченных как удаленные

    readonly ProbingMethod probing_method;

	class Record( TKey key, TValue value ) : Record_Base( key, value ) {
        public bool deleted = false;
    };

    Record[] arr; // main table

    // Поставить новый ключ в первую свободную или удалённую ячейку его последовательности проб
    private bool TryPlace( TKey key, TValue value ) {

        var hash = GetHash( key );

        for ( uint i = 0; i < arr.Length; i++ ) {
            var hash_ex = DoProbe( key, hash, i );
            if ( arr[ hash_ex ] == null || arr[ hash_ex ].deleted ) {
                if ( arr[ hash_ex ] != null )
                    total_deleted--; // удалённая запись заменяется живой

                _insert( hash_ex, key, value );
                return true;
            }
        }

        return false;
    }

    private void TableDoubling_FullRehash() {
        if ( (ulong)m * 2 > uint.MaxValue )
            throw new InvalidOperationException( "Limit exceeded!" );

        TableResizing_FullRehash( m * 2 );
    }

    private void TableResizing_FullRehash( uint new_size ) {

        m = new_size;
        Record[] new_arr = new Record[ m ];

        for ( uint i = 0; i < arr.Length; i++ ) {
            
            if ( arr[ i ] is null || arr[ i ].deleted )
                continue;

            var hash = GetHash( arr[ i ].key );
            bool placed = false;
            for ( uint j = 0; j < m; j++ ) {
                var hash_ex = DoProbe( arr[ i ].key, hash, j );
                if ( new_arr[ hash_ex ] is null ) {
                    new_arr[ hash_ex ] = arr[ i ];
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                m = (uint)arr.Length;                     // вернуть прежний размер: старая таблица ещё цела
                throw new InvalidOperationException("Rehash failed: the probe sequence found no free slot");
            }
        }

        total_deleted = 0;
        arr = new_arr;
    }

    void TableHalfеning_FullRehash() {
        if ( count_elements > arr.Length / 8 )
            return;

        TableResizing_FullRehash( Math.Max( 1, m / 2 ) );
    }

    (bool Found, uint Index) search( TKey key ) {

        var hash = GetHash( key );

        (bool Found, uint Index) not_found = ( false, 0 );

        for ( uint i = 0; i < arr.Length; i++ ) {

            var hash_ex = DoProbe( key, hash, i );
            
            if ( arr[ hash_ex ] == null )
                return not_found;

            if ( arr[ hash_ex ].deleted )
                continue;

            if ( EqualityComparer<TKey>.Default.Equals(arr[ hash_ex ].key,  key ) )
                return ( true, hash_ex );
        }

        return not_found;
    }

    /// <summary>
    /// Linear Probing
    /// Suffers from problem known as "primary clustering"
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    uint LinearProbing( uint hash, uint i ) {
        return ( hash + i ) % m;
    }

    /// <summary>
    /// Quadratic Probing 
    /// Suffers from problem known as "secondary clustering"
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    uint QuadraticProbing( uint hash, uint i ) {
         return (uint)( ( hash + ( C1 * i + C2 * i * i ) / DEN ) % m );
    }

    /// <summary>
    /// Double Hashing 
    /// Offers one of the best methods available for open addressing because the permutations produced have many of the characteristics of randomly chosen permutations
    /// </summary>
    /// <returns></returns>
    uint DoubleHashing( TKey key, uint hash, uint i ) {
        uint step = HashFuncMultiplication( key ) | 1; // нечётный шаг взаимно прост с m = 2^k
        return ( hash + i * step ) % m;
    }

    uint DoProbe( TKey key, uint hash, uint i ) {

        return probing_method switch {
            ProbingMethod.LinearProbing => LinearProbing(hash, i),
            ProbingMethod.QuadraticProbing => QuadraticProbing(hash, i),
            ProbingMethod.DoubleHashing => DoubleHashing(key, hash, i),
            _ => throw new InvalidOperationException("Must not hit into here!"),
        };
    }

    void _delete( uint index ) {
        arr[ index ].deleted = true;
        total_deleted++;
        count_elements--;
        TableHalfеning_FullRehash();
    }

    void _insert( uint hash_ex, TKey key, TValue value ) {
        arr[ hash_ex ] =  new Record( key, value );
        count_elements++;
    }
}
