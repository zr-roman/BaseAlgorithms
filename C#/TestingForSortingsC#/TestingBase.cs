using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    public abstract class TestingBase {
        
        protected TestingBase() {
            rnd = new Random();
            arr_Size = rnd.Next(1001, 2001);
            GenerateNewArray();
        }

        protected int[] arr;
        protected int arr_Size;
        protected Random rnd;
      
        protected void Assertion(Order order) {
            if (order == Order.ASC) {
                for ( int i = 0; i < arr.Length - 1; i++ ) {
                    Assert.IsTrue( arr[ i ] <= arr[ i + 1] );
                }
            }
            if (order == Order.DESC) {
                for (int i = 0; i < arr.Length - 1; i++) {
                    Assert.IsTrue(arr[i] >= arr[i + 1]);
                }
            }
        }

        protected void GenerateNewArray() {
            var min = int.MinValue + 1;
            var list = new List<int>();
            for (int i = 0; i < arr_Size; i++)
            {
                list.Add( rnd.Next(min, int.MaxValue) );
            }
            arr =  list.ToArray();
        }
    }
}
