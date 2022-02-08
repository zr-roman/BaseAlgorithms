
using System;

namespace Sortings {

    public enum Order {
        ASC,
        DESC
    }

    public enum Pivot {
        FIRST   =   0b1,
        LAST    =  0b10,
        RANDOM  = 0b100
    }

    public static partial class Lib {
        
        private static T GetGreatestKeyValue<T>( T[] arr ) where T: IComparable<T> {

            if ( arr.Length == 0 ) {
                throw new Exception("Empty or null array is not allowed!");
            }

            // вычисление максимального значения ключа
            T greatest = arr[ 0 ];

            for ( var i = 0; i < arr.Length; i++ ) {
                
                if ( arr[ i ].CompareTo( greatest ) > 0 ) {
                    greatest = arr[ i ];
                }
            }

            return greatest;
        }

        private abstract class SortAbstract<T> where T: IComparable<T> {

            public void Sort( uint start, uint end ) {
                if ( !already_sorted ) {
                    DoSort( start, end );
                }
            }

            public uint GetArraySize() { return (uint)(arr == null ? 0 : arr.Length ); }

            protected abstract void DoSort( uint start, uint end );
            protected SortAbstract( T[] arr, Order order ) {

                this.order = order;

                if ( arr == null || arr.Length == 0 ) {
                    throw new Exception("Error: null or emply array not allowed!");
                }
            
                already_sorted = true;

                if ( order == Order.ASC ) {
                    for ( uint i = 0; i + 1 < arr.Length; i++ ) {
                        if ( arr[ i ].CompareTo( arr[ i + 1 ] ) > 0 ) {
                            already_sorted = false;
                            break;
                        }
                    }
                }
                if ( order == Order.DESC ) {
                    for ( uint i = 0; i + 1 < arr.Length; i++ ) {
                        if ( arr[ i ].CompareTo( arr[ i + 1 ] ) < 0 ) {
                            already_sorted = false;
                            break;
                        }
                    }
                }

                this.arr = arr;
            }

            protected void Swap( uint i, uint j ) {
                var tmp  = arr[ i ];
                arr[ i ] = arr[ j ];
                arr[ j ] = tmp;
            }

            protected T[] arr;
            protected readonly Order order;
            protected bool already_sorted;
        }
        
        private static void DoPerformSort<T>( SortAbstract<T> ptr ) where T: IComparable<T> {
            ptr.Sort( 0, ptr.GetArraySize() - 1 );
        }

    }
}
