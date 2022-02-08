using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testing {

    public abstract class TestingBase {
        
        protected TestingBase() {
            rnd = new Random();
            N = (byte)rnd.Next( 10, 51 );
            GenerateNewArray();
        }

        protected int[] arr;
        protected byte N;
        protected Random rnd;

        protected void AssertionAsc() {
            for ( int i = 0; i < arr.Length - 1; i++ ) {
                Assert.IsTrue( arr[ i ] <= arr[ i + 1] );
            }
        }

        protected void AssertionDesc() {
            for ( int i = 0; i < arr.Length - 1; i++ ) {
                Assert.IsTrue( arr[ i ] >= arr[ i + 1 ] );
            }
        }

        protected void GenerateNewArray() {
            var min = int.MinValue + 1;
            arr = new int[] {
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue ),
                    rnd.Next( min, int.MaxValue )
            };
        }
    }
}
