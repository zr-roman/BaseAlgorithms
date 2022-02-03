using System;

/// <summary>
/// OPEN ADDRESSING 
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
/// <typeparam name="TValue">Тип значения</typeparam>
public class DicOpenAddr<TKey, TValue> : Hashing<TKey, TValue> where TKey: IComparable {
        
    public DicOpenAddr() {
        arr = new Record[ m ];
        probing_method = ProbingMethod.QUADRATIC_PROBING;
        count_elements = 0;
    }

    public override void Insert( TKey key, TValue value ) {

        if ( search( key ).Item1 ) { return; }

        void func() {
            curr_key = key;
            curr_value = value;
            TableDoubling_FullRehash_And_CallInsert();
        }

        if ( count_elements == arr.Length ) { func(); return; }

        var hash = GetHash( key );

        bool done = false;
        for ( uint i = 0; i < arr.Length; i++ ) {
            var hash_ex = DoProbe( key, hash, i );
            if ( arr[ hash_ex ] == null || arr[ hash_ex ].deleted ) {
                _insert( hash_ex, key, value, hash );
                done = true;
                break;
            }
        }

        if ( !done ) { func(); }
    }

    public override void Delete( TKey key ) {

        var search_res = search( key );

        if ( search_res.Item1 ) {
            _delete( search_res.Item2 );
        }
    }

    public override Tuple<bool, TValue> Search( TKey key ) {

        var search_res = search( key );

        if ( !search_res.Item1 ) {
            return new Tuple<bool, TValue>( false, default );
        }
        return new Tuple<bool, TValue>( true, arr[ search_res.Item2 ].value );
    }
    
    // constants for quadratic probing
    const double c1 = 1.5;
    const double c2 = 1.5;

    TKey curr_key;
    TValue curr_value;

    uint count_elements;

    private enum ProbingMethod {
        LINEAR_PROBING,
        QUADRATIC_PROBING,
        DOUBLE_HASHING
    };

    readonly ProbingMethod probing_method;

	class Record : Record_Base {
        public uint hash;
        public bool deleted;
        public Record( TKey key, TValue value, uint hash ) : base( key, value ) {
            deleted = false;
            this.hash = hash;
        }
	};

    Record[] arr;  // main table

    private void TableResizing_FullRehash( uint new_size ) {

        m = new_size;
        Record[] new_arr = new Record[ m ];

        for ( uint i = 0; i < arr.Length; i++ ) {
            
            if ( arr[ i ] == null || arr[ i ].deleted ) { continue; }

            var hash = GetHash( arr[ i ].key );
            for ( uint j = 0; true; j++ ) {
                var hash_ex = DoProbe( arr[ i ].key, hash, j );
                if ( new_arr[ hash_ex ] == null ) {
                    arr[ i ].hash = hash;
                    new_arr[ hash_ex ] = arr[ i ];
                    break;
                }
            }
        }

        arr = new_arr;
    }

    void TableDoubling_FullRehash_And_CallInsert( ) {
        long t = m;
        long tmp = t * 2;
        if ( tmp > uint.MaxValue ) {
            throw new Exception( "Limit exceeded!" );
        }
        TableResizing_FullRehash( m * 2 );
        Insert( curr_key, curr_value );
    }

    void TableHalfеning_FullRehash() {
        if ( count_elements > arr.Length / 4 ) {
            return;
        }
        TableResizing_FullRehash( m / 2 );
    }

    Tuple<bool, uint> search( TKey key ) {

        var hash = GetHash( key );

        var not_found = new Tuple<bool, uint>( false, 0 );

        for ( uint i = 0; i < arr.Length; i++ ) {

            var hash_ex = DoProbe( key, hash, i );
            
            if ( arr[ hash_ex ] == null ) {
                return not_found;
            }

            if ( arr[ hash_ex ].deleted ) {
                continue;
            }

            if ( arr[ hash_ex ].key.CompareTo( key ) == 0 ) {
                return new Tuple<bool, uint>( true, hash_ex );
            }
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
        return (uint)( hash + c1 * i + c2 * i * i ) % m;
    }

    /// <summary>
    /// Double Hashing 
    /// Offers one of the best methods available for open addressing because the permutations produced have many of the characteristics of randomly chosen permutations
    /// </summary>
    /// <returns></returns>
    uint DoubleHashing( TKey key, uint hash, uint i ) {
        return ( hash + i * HashFuncMultiplication( key ) ) % m;
    }

    uint DoProbe( TKey key, uint hash, uint i ) {

        return probing_method switch {
            ProbingMethod.DOUBLE_HASHING => DoubleHashing(key, hash, i),
            ProbingMethod.LINEAR_PROBING => LinearProbing(hash, i),
            ProbingMethod.QUADRATIC_PROBING => QuadraticProbing(hash, i),
            _ => throw new Exception("Must not hit into here!"),
        };
    }

    void _delete( uint index ) {
        arr[ index ].deleted = true;
        count_elements--;
        TableHalfеning_FullRehash();
    }

    void _insert( uint hash_ex, TKey key, TValue value, uint hash ) {
        arr[ hash_ex ] =  new Record( key, value, hash );
        count_elements++;
    }
    
}
