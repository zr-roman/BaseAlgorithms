using System;
using System.Collections.Generic;
using System.Linq;
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
                var dic = new Dictionary<int, T[]>();
                uint prevStart = 0;
                
                var needBreak = false;
                
                for(uint step = 1; step <= 3; step += 2 ) {
                  
                    for (uint i = 0; i < arr.Length; i += step + 1) {

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
                        
                        Merge(i, mi, en);

                        if (arr.Length == 2) {
                            return;
                        }

                        if (step == 3) {
                            if (en - i + 1 == 1)
                            {
                                dic.Add(counter++, new T[] { arr[i] });
                            }
                            else if (en - i + 1 == 2)
                            {
                                dic.Add(counter++, new T[] { arr[i], arr[i + 1] });
                            }
                            else if (en - i + 1 == 3)
                            {
                                dic.Add(counter++, new T[] { arr[i], arr[i + 1], arr[i + 2] });
                            }
                            else {
                                dic.Add(counter++, new T[] { arr[i], arr[i + 1], arr[i + 2], arr[i + 3] });
                            }
                        }
                    }
                    
                    if (needBreak)
                    {
                        break;
                    }
                }

                var listOfTasks = new List<Task>();
                int t = 2;
                int s = 1;

                while (dic.Count(x => x.Value != null) != 1) { 
                    for (int i = 0; i+s  < dic.Count && i < dic.Count; i+=t)
                    {
                        var j = i; // paradox

                        var task = Task.Factory.StartNew(() => {
                            Merge2(dic, j, j + s);
                        });
                        listOfTasks.Add(task);
                    }
                    Task.WhenAll(listOfTasks).ConfigureAwait(false).GetAwaiter().GetResult();
                    t*=2;
                    s*=2;
                }

                Task.WhenAll(listOfTasks).ContinueWith( (prev) => {
                    foreach (var key in dic.Keys) {
                        if (dic[key] != null) {
                            for (int i = 0; i < arr.Length; i++) {
                                arr[i] = dic[key][i]; // перекидывание результата в исходный массив
                            }
                            dic = null;
                            break;
                        }
                    }
                }).ConfigureAwait(false).GetAwaiter().GetResult();
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
