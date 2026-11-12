namespace HashTables;

/// <summary>
/// HASHING WITH CHAINING
/// </summary>
/// <typeparam name="Key">Тип ключа</typeparam>
/// <typeparam name="Value">Тип значения</typeparam>
public class ChainingHashTable<TKey, TValue> : Hashing<TKey, TValue> {

    public ChainingHashTable() : base( HashMethod.UNIVERSAL_HASHING ) {
        arr = new Record[ m ];
    }

    public override void Insert( TKey key, TValue value ) {

        // проверить на дублирование ключа
        if ( CheckKeyExists( key ) )
            throw new ArgumentException("Key duplicating is not allowed!");

        if (total_elements >= arr.Length)
            TableResizing_FullRehash( (uint)arr.Length * 2 );

        // вычислить хеш
        var hash = GetHash( key );

        var new_rec = new Record( key, value, null );

        // если ячейка пустая - вставить в нее
        if ( arr[ hash ] == null ) {
            arr[ hash ] = new_rec;
        }
        else
        {
            // если ячейка заполнена - вставить в chain
            var lastChainItem = GetLastChainItem(arr[hash]);
            lastChainItem.Next = new_rec;
        }

        total_elements++;
    }

    // Поиск по ключу
    public override Tuple<bool, TValue> Search( TKey key ) {
        var item = GetChainItem( key ).Curr;
        return item == null ? new Tuple<bool, TValue>( false, default ) : new Tuple<bool, TValue>( true, item.value );
    }

    // Удаление по ключу
    public override void Delete( TKey key ) {
            
        var r = GetChainItem( key );

        var prev = r.Prev;
        var item = r.Curr;
        var hash = r.Hash;

        if ( item == null )
            return;

        if ( prev != null ) {
            prev.Next = item.Next;
        } else {
            arr[ hash ] = item.Next;
        }

        total_elements--;

        if (total_elements <= arr.Length / 4)
            TableResizing_FullRehash((uint)Math.Max(1, arr.Length / 2));
    }

    // Получить последний узел в цепочке
    private Record GetLastChainItem( Record item ) {
        while ( true ) {
            if ( item.Next == null )
                return item;

            item = item.Next;
        }
    }

    // Получить узел по ключу
    private (Record Prev, Record Curr, uint Hash) GetChainItem( TKey key ) {

        (Record Prev, Record Curr, uint Hash) not_found_res = new( null, null, 0 );

        var hash = GetHash( key );

        if ( arr[ hash ] == null )
            return not_found_res;

        Record prev = null;
        var curr = arr[ hash ];

        while ( true ) {

            if ( EqualityComparer<TKey>.Default.Equals( curr.key, key ) )
                return new ( prev, curr, hash );

            prev = curr;
            curr = curr.Next;

            if ( curr == null )
                break;
        }
            
        return not_found_res;
    }

    // Проверить ключ на существование
    private bool CheckKeyExists( TKey key ) {
        return GetChainItem( key ).Curr != null;
    }

    private void TableResizing_FullRehash(uint new_size) {
        m = new_size;
        Record[] new_arr = new Record[ m ];

        for (uint i = 0; i < arr.Length; i++)
        {
            if (arr[ i ] is null)
                continue;

            var current = arr[ i ];

            while (current is not null)
            {
                // вычислить хеш
                var hash = GetHash(current.key);

                var new_rec = new Record(current.key, current.value, null);

                // если ячейка пустая - вставить в нее
                if (new_arr[hash] is null)
                {
                    new_arr[hash] = new_rec;
                }
                else
                {
                    // если ячейка заполнена - вставить в chain
                    var lastChainItem = GetLastChainItem(new_arr[hash]);
                    lastChainItem.Next = new_rec;
                }

                current = current.Next;
            }
        }

        arr = new_arr;
    }

    private class Record(TKey key, TValue value, Record next) : Record_Base( key, value ) {
        public Record Next { get; set; } = next;
    }

    private Record[] arr;

    private uint total_elements = 0;
}
