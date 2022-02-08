using System;
using static System.Console;
using static System.DateTime;

namespace Sortings
{
    class Program
    {
        static void Main(string[] args)
        {
            var nt = (Name1:5, Name2:4, Name3:("a", "b"));

            var rnd = new Random();
            var start = 0;
            var end = 100;
            var arr = new int[] {
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end ),
                    rnd.Next( start, end )
            };

            //Lib.QuickSort( arr, Order.DESC, Pivot.LAST );
            //Lib.MergeSort( arr, Order.DESC, int.MinValue, int.MaxValue );
            //Lib.InsertionSort( arr, Order.DESC );
            //Lib.Heapsort( arr, Order.DESC );
            Lib.ShellSort( arr, Order.ASC );

            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }
    }
}
