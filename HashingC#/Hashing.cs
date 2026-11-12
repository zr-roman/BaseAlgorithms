namespace HashTables;

public abstract class Hashing<TKey, TValue> {

    public abstract void Insert( TKey key, TValue value );
    public abstract void Delete( TKey key );
    public abstract bool Search( TKey key, out TValue value );

    protected Hashing( HashMethod hm = HashMethod.DIVISION ) {

        rnd = new Random();

        hash_method = hm;

        if ( hash_method == HashMethod.UNIVERSAL_HASHING ) {
            a = GenerateRandomUInt( 1, uint.MaxValue );
            b = GenerateRandomUInt( 0, uint.MaxValue );
        }
    }

    protected class Record_Base(TKey key, TValue value)
    {
        public readonly TKey key = key;
        public readonly TValue value = value;
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
            _ => throw new InvalidOperationException( "Must not hit into here!" ),
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

        ulong a_val = a_par ?? a;
        ulong b_val = b_par ?? b;
        ulong m_val = m_par ?? m;

        // key in range of 0 ... UINT_MAX
        const ulong p = 4294967311; // the nearest following prime to UINT_MAX
        ulong number = GetNumericRepresentationOfKey( key );
        uint res = (uint)( ( a_val * number + b_val ) % p % m_val); // this is: ((a*k + b) mod p) mod m;
        return res;
    }
    
    protected uint GetNumericRepresentationOfKey( TKey key ) {
        return unchecked((uint)EqualityComparer<TKey>.Default.GetHashCode(key));
    }

    protected uint GenerateRandomUInt( uint start, uint end ) {
        return (uint)rnd.NextInt64(start, (long)end + 1);  // [start, end], обе границы включены
    }
}
