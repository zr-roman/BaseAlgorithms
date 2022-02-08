

using System;
using System.Numerics;
using System.Text;

[assembly: CLSCompliant(true)]

class Program {

    static void Main( string[] args ) {

        string s = "evse evseevseevpaababacae osdababacajfevseevseevapsos devseevjababacafl sdkjflsevseevseev";
        string pattern = "evseev";
        //string pattern = "ababaca";
        //string pattern = "datadata";
        
        var ptr5 = new BoyerMoore();
        var res5 = ptr5.GetAllOccurences( s, pattern );

        var ptr3 = new KMP();
        var res3 = ptr3.GetAllOccurences(s, pattern);

        var ptr = new KarpRabin( KarpRabinHash.STRONGER );
        var res1 = ptr.GetAllOccurences( s, pattern );

        
        var p = new FiniteAutomaton();
        var res2 = p.GetAllOccurences( s, pattern );

        var p4 = new BruteForce();
        var res4 = p4.GetAllOccurences( s, pattern );

        Console.WriteLine("\aHello World!\n");
    }
}
