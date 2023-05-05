using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sortings {

    public static partial class Lib {
        
        public static void IterativeMergeSort<T>( T[] arr, Order order ) where T: IComparable<T> {
            DoPerformSort( new IterativeMergeSortInner<T>( arr, order ) );
        }

        private class IterativeMergeSortInner<T> : SortAbstract<T> where T: IComparable<T> {
                        
            public IterativeMergeSortInner( T[] arr, Order order ) : base( arr, order) {

            }

            protected override void DoSort( uint start, uint end ) {

                int counter = 0;
                var dic = new Dictionary<int, T[]>(); // хеш-таблица отсортированных четверок
                uint prevStart = 0;
                
                var needBreak = false;

                int step = 1;
                
                while(step <= 3) { 
                  
                    for (uint i = 0; i < arr.Length; i += (uint)step + 1) {

                        var en = i + step;
                        
                        if (en > end) {
                            en = end;
                        }

                        var mi = (i + en) / 2;

                        if (step > end) {
                            i = start;
                            mi = prevStart - 1;
                            en = end;
                            needBreak = true;
                        }
                        prevStart = i;
                        
                        Merge(i, (uint)mi, (uint)en);

                        if (arr.Length == 2) {
                            return;
                        }

                        FillInDictionary(dic, i, step, en, ref counter);
                    }

                    if (needBreak)
                    {
                        break;
                    }

                    step += 2;
                }

                var listOfTasks = new List<Task>();

                MergeAllParts(listOfTasks, dic).GetAwaiter().GetResult();

                FillResultingArray(listOfTasks, dic).GetAwaiter().GetResult();

                dic = null;
            }

            private void FillInDictionary(Dictionary<int, T[]> dic, uint i, int step, long en, ref int counter) {
                
                if (step == 3) {

                    var count = en - i + 1;

                    if (count == 1) {
                        dic.Add(counter++, new T[] { arr[i] });
                    } else if (count == 2) {
                        dic.Add(counter++, new T[] { arr[i], arr[i + 1] });
                    } else if (count == 3) {
                        dic.Add(counter++, new T[] { arr[i], arr[i + 1], arr[i + 2] });
                    } else if (count == 4) {
                        dic.Add(counter++, new T[] { arr[i], arr[i + 1], arr[i + 2], arr[i + 3] });
                    }
                }
            }

            private async Task MergeAllParts(List<Task> listOfTasks, Dictionary<int, T[]> dic) { 
                 
                int step = 2;

                while (dic[0].Length != arr.Length ) { 
                    for (int i = 0; i < dic.Count && i + step / 2 < dic.Count; i+= step) {
                        
                        var j = i; 
                        
                        listOfTasks.Add( Task.Factory.StartNew(() => { Merge2(dic, j, j + step / 2); }) );
                    }
                    await Task.WhenAll(listOfTasks).ConfigureAwait(false);
                    step *= 2;
                }
            }

            private async Task FillResultingArray(List<Task> listOfTasks, Dictionary<int, T[]> dic) {

                await Task.WhenAll(listOfTasks).ContinueWith((prev) => {

                    foreach (var key in dic.Keys) {

                        if (dic[key] != null) {

                            for (int i = 0; i < arr.Length; i++) {
                                arr[i] = dic[key][i]; // перекидывание результата в исходный массив
                            }
                            
                            break;
                        }
                    }
                }).ConfigureAwait(false);
            }

            private void Merge( uint start, uint mid, uint end ) {
                
                var n1 = mid - start + 1;
                var n2 = end - mid;

                var arrayB = new T[ n1 ];
                var arrayC = new T[ n2 ];
                
                var bi = 0;
                var ci = 0;

                for ( uint i = start; i <= mid; i++ ) {
                    arrayB[ bi++ ] = arr[ i ];
                }
                                
                for ( uint i = mid + 1; i <= end; i++ ) {
                    arrayC[ ci++ ] = arr[ i ];
                }
                
                bi = ci = 0;

                for ( uint i = start; i <= end; i++ ) {

                    if ( bi >= arrayB.Length ) {
                        arr[ i ] = arrayC[ ci++ ];
                        continue;
                    }

                    if ( ci >= arrayC.Length ) {
                        arr[ i ] = arrayB[ bi++ ];
                        continue;
                    }

                    var compareResult = arrayB[ bi ].CompareTo( arrayC[ ci ] );

                    if ( ( order.Equals( Order.ASC ) && compareResult <= 0 ) || ( order.Equals( Order.DESC ) && compareResult > 0 ) ) {
                        arr[ i ] = arrayB[ bi++ ];
                    } else {
                        arr[ i ] = arrayC[ ci++ ];
                    }
                }
            }

            private void Merge2(Dictionary<int, T[]> dic, int k, int j  ) {

                var arrayB = dic[ k ];
                var arrayC = dic[ j ];

                var bi = 0;
                var ci = 0;

                var totalLength = arrayB.Length + arrayC.Length;

                var resArr = new T[totalLength];

                for ( uint i = 0; i < totalLength; i++ ) {

                    if ( bi >= arrayB.Length ) {
                        resArr[ i ] = arrayC[ ci++ ];
                        continue;
                    }

                    if ( ci >= arrayC.Length ) {
                        resArr[ i ] = arrayB[ bi++ ];
                        continue;
                    }

                    var compareResult = arrayB[ bi ].CompareTo( arrayC[ ci ] );

                    if ( ( order.Equals( Order.ASC ) && compareResult <= 0 ) || ( order.Equals( Order.DESC ) && compareResult > 0 ) ) {
                        resArr[ i ] = arrayB[ bi++ ];
                    } else {
                        resArr[ i ] = arrayC[ ci++ ];
                    }
                }

                dic[k] = resArr;
                dic[j] = null;
            }
        }
    }
}
