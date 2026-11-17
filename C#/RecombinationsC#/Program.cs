using System;
using System.Collections.Generic;

namespace Recombinations {

    class Program {
        
        static void Main(string[] args) {
            
            var str = "abcde";
            
            var result = GetAllRecombinations( str );
            var counter = 0;
            foreach ( var item in result ) {
                counter++;
                Console.WriteLine( counter + ": " + item );
            }
           
           Console.ReadLine();
        }

        static HashSet<string> GetAllRecombinations( string s, int index = 1, HashSet<string> set = null ) {

            // защита от дурака
            if (set == null || ( set != null && set.Count == 0 )) {
                set = new HashSet<string> { s[ 0 ].ToString() };
            }
            if ( string.IsNullOrEmpty( s ) ) {
                throw new Exception("Входящая строка не может быть пустой!");
            }

            // инициализации
            var newChar = s[ index ].ToString();
            var newSet = new HashSet<string>();
            
            // в цикле произвести действие для каждого элемента списка
            foreach (var item in set) {
                // для каждой позиции текущего элемента произвести действие: вставить в эту позицию символ
                for ( int j = 0; j <= item.Length; j++ ) { // здесь "<=" потому что вставка символа производится также и в конец строки
                    var val = item.Insert( j, newChar ); // вставка символа
                    newSet.Add( val );
                }
            }
            // рекурсивно вызвать функцию, в случае если индекс не вышел за пределы строки
            if ( s.Length - 1 > index ) {
               return GetAllRecombinations( s, index + 1, newSet );
            } else {
                return newSet;
            }
        }
    }
}
