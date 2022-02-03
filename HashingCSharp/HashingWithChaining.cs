using System;

/// <summary>
/// HASHING WITH CHAINING
/// </summary>
/// <typeparam name="Key">Тип ключа</typeparam>
/// <typeparam name="Value">Тип значения</typeparam>
public class DicChaining<TKey, TValue> : Hashing<TKey, TValue> {

    public DicChaining() : base( HashMethod.UNIVERSAL_HASHING ) {
        arr = new Record[ m ];
    }

    public override void Insert( TKey key, TValue value ) {

        // проверить на дублирование ключа
        if ( CheckKeyExists( key ) ) {
            throw new Exception("Key duplicating is not allowed!");
        }

        // вычислить хеш
        var hash = GetHash( key );

        var new_rec = new Record( key, value, null );

        // если ячейка пустая - вставить в нее
        if ( arr[ hash ] == null ) {
            arr[ hash ] = new_rec;
            return;
        } 

        // если ячейка заполнена - вставить в chain
        var lastChainItem = GetLastChainItem( arr[ hash ] );
        lastChainItem.Next = new_rec;
    }

    // Поиск по ключу
    public override Tuple<bool, TValue> Search( TKey key ) {
        var item = GetChainItem( key ).Item1;
        return item == null ? new Tuple<bool, TValue>( false, default ) : new Tuple<bool, TValue>( true, item.value );
    }

    // Удаление по ключу
    public override void Delete( TKey key ) {
            
        var r = GetChainItem( key );

        var prev = r.Item1;
        var item = r.Item2;
        var hash = r.Item3;

        if ( item == null ) { return; }

        if ( prev != null ) {
            prev.Next = item.Next;
        } else {
            arr[ hash ] = item.Next;
        }
    }

    // Получить последний узел в цепочке
    private Record GetLastChainItem( Record item ) {
        while ( true ) {
            if ( item.Next == null ) {
                return item;
            }
            item = item.Next;
        }
    }

    // Получить узел по ключу
    private Tuple<Record, Record, uint> GetChainItem( TKey key ) {

        var not_found_res = new Tuple<Record, Record, uint>( null, null, 0 );

        var hash = GetHash( key );
        if ( arr[ hash ] == null ) { return not_found_res; }

        Record prev = null;
        var curr = arr[ hash ];

        while ( true ) {
            if ( curr.key.Equals( key ) ) { return new Tuple<Record, Record, uint>( prev, curr, hash ); }

            prev = curr;
            curr = curr.Next;

            if ( curr == null ) { break; }
        }
            
        return not_found_res;
    }

    // Проверить ключ на существование
    private bool CheckKeyExists( TKey key ) {
        return GetChainItem( key ).Item1 != null;
    }

    private class Record : Record_Base {
        public Record Next { get; set; }
        public Record( TKey key, TValue value, Record next) :base( key, value ) {
            Next = next;
        }
    }

    private readonly Record[] arr;
}
