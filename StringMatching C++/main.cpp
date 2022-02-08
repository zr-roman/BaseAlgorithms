
#include <iostream>
#include <memory>
#include "FiniteAutomaton.h"
#include "KarpRabin.h"
#include "KnuthMorrisPratt.h"
#include "BoyerMoore.h"
#include "BruteForce.h"
int main()
{
    auto v = make_pair<int, int>(4, 3);

    string s = "evse evseevseevpaababacae osdababacajfevseevseevapsos devseevjababacafl sdkjflsevseevseev";
    string pattern = "evseev";
    //string pattern = "ababaca";
    //string pattern = "datadata";

    auto ptr5 = make_unique<BoyerMoore>();
    auto res5 = ptr5->GetAllOccurences(s, pattern);

    auto ptr3 = make_unique<KMP>();
    auto res3 = ptr3->GetAllOccurences(s, pattern);

    auto ptr = make_unique<KarpRabin>( KarpRabinHash::STRONGER );
    auto res1 = ptr->GetAllOccurences(s, pattern);

    auto p = make_unique<FiniteAutomaton>();
    auto res2 = p->GetAllOccurences(s, pattern);

    auto p4 = make_unique<BruteForce>();
    auto res4 = p4->GetAllOccurences(s, pattern);

    std::cout << "Hello World!\n";
}
