using System;

namespace Sortings {

    public static partial class Lib {

        public static void CountingSort( int[] arr, Order order = Order.ASC) {
            DoPerformSort( new CountingSortInner(arr, order) );
        }

        private class CountingSortInner : LinearTimeSort {
        
            public CountingSortInner( int[] arr, Order order ) : base( arr, order ) { }
            public override void SpecificSort( int[] arr, uint radix_param = 0 ) { // GoF template method

                int func( int key ) {
                    return (int)(radix_param != 0 ? key / radix_param % 10 : key);
                }

                if ( arr == null || arr.Length == 0 ) {
                    return;
                }

                // вычисление максимального значения ключа в случае counting sort 
                // или установка максимального значения = 9 в случае если radix sort
                int greatest = 9;
    
                if ( radix_param == 0 ) {
                    greatest = arr[ 0 ];
    
                    for ( uint i = 0; i < arr.Length; i++ ) {
                        if ( arr[ i ] < 0 ) {
                            throw new Exception("Negative array keys are not allowed!");
                        }
                        if ( arr[ i ] > greatest ) {
                            greatest = arr[ i ];
                        }
                    }
                }
    
                // заполнение counting массива
                uint[] arrayB = new uint[ greatest + 1 ];
                for ( uint i = 0; i < arr.Length; i++ ) {
                    arrayB[ func( arr[ i ] ) ]++;
                }

                // инкрементирование counting массива 
                for ( uint i = 0; i + 1 < arrayB.Length; i++ ) {
                    arrayB[ i + 1 ] += arrayB[ i ];
                }

                uint[] arrayRes = new uint[ arr.Length ];

                // заполнение результирующего массива
                // для сохранения свойства stable цикл идет в обратном порядке
                for ( int i = arr.Length - 1; i >= 0 ; i-- ) {
                    var ind = func( arr[ i ] );
                    arrayB[ ind ]--;
                    var resInd = arrayB[ ind ];
                    arrayRes[ resInd ] = (uint)arr[ i ];
                }

                // переброска результирующего массива в исходный массив
                for ( uint i = 0; i < arr.Length; i++ ) {
                    arr[ i ] = (int)arrayRes[ i ];
                }
            }
        }
    }
}
