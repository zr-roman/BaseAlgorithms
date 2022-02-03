
using System;

public abstract class Hashing<TKey, TValue> {

    public abstract void Insert( TKey key, TValue value );
    public abstract void Delete( TKey key );
    public abstract Tuple<bool, TValue> Search( TKey key );

    protected Hashing( HashMethod hm = HashMethod.DIVISION ) {

        rnd = new Random();

        hash_method = hm;

        if ( hash_method == HashMethod.UNIVERSAL_HASHING ) {
            a = GenerateRandomUInt( 0, uint.MaxValue );
            b = GenerateRandomUInt( 1, uint.MaxValue );
        }
    }

    protected class Record_Base {
        public readonly TKey key;
        public readonly TValue value;
        public Record_Base( TKey key, TValue value ) {
            this.key = key;
            this.value = value;
        }
	}

    protected enum HashMethod {
        DIVISION,
        MULTIPLICATION,
        UNIVERSAL_HASHING
    };

    protected readonly HashMethod hash_method;

    protected uint m = 17; //700001; // table size

    protected uint a; // parameter for universal hashing
    protected uint b; // parameter for universal hashing

    private static readonly double A = ( Math.Sqrt( 5 ) - 1 ) / 2; // Knuth offered to use this value, golden ratio

    private readonly Random rnd;

    protected uint GetHash( TKey key, uint? a_par = null, uint? b_par = null, uint? m_par = null ) {
        return hash_method switch {
            HashMethod.DIVISION          => HashFuncDivision( key ),
            HashMethod.MULTIPLICATION    => HashFuncMultiplication( key ),
            HashMethod.UNIVERSAL_HASHING => HashFuncUniversal( key, a_par, b_par, m_par ),
            _ => throw new Exception( "Must not hit into here!" ),
        };
    }

    uint HashFuncDivision( TKey key ) {
        var number = GetNumericRepresentationOfKey( key );
        uint hash = number % m;
        return hash;
    }

    protected uint HashFuncMultiplication( TKey key ) {
        
        var number = GetNumericRepresentationOfKey( key );
        return (uint)( m * ( number * A % 1 ) ); // this is: m * ( ( key * A ) % 1 )
        // выражение ( key * A ) % 1 означает получение дробной части произведения ( key * A ), т.е. ( key * A ) - floor( key * A )
    }

    uint HashFuncUniversal( TKey key, uint? a_par = null, uint? b_par = null, uint? m_par = null ) {
        
        if ( !a_par.HasValue ) { a_par = a; }
        if ( !b_par.HasValue ) { b_par = b; }
        if ( !m_par.HasValue ) { m_par = m; }

        // key in range of 0 ... UINT_MAX
        long p = 4294967311; // the nearest following prime to UINT_MAX
        var number = GetNumericRepresentationOfKey( key );
        uint res = (uint)((uint) ( ( (long)a_par * number + (long)b_par ) % p) % m_par); // this is: ((a*k + b) mod p) mod m;
        return res;
    }
    
    protected uint GetNumericRepresentationOfKey( TKey key ) {
        var str = key.ToString();
        uint res = 1;
        for ( int i = 0; i < str.Length; i++ ) {
            if ( i % 2 == 0 ) {
                res = ( res + 1 ) * str[ i ];
                continue;
            }
            res = ( res + 1 ) * str[ i ] * str[ i ];
        }
        for ( int i = str.Length - 1; i >= 0; i-- ) {
            if ( i % 2 == 0 ) {
                res = ( res + 1 ) * str[ i ] * str[ i ];
                continue;
            }
            res = (res + 1) * str[ i ];
        }
        return res;
    }

    protected uint GenerateRandomUInt( uint start, uint end ) {
        
        uint thirtyBits = (uint)rnd.Next(0, 1 << 30 );
        uint twoBits = (uint)rnd.Next( 1 << 2 );
        uint fullRange = ( thirtyBits << 2 ) | twoBits;
        return (end - start) * fullRange + start;
    }
    
}
