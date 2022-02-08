
#include <iostream>
#include "FibonacciNumbers.h"
#include "MatrixChainMultiplication.h"
#include "LongestCommonSubsequence.h"
#include "Knapsack.h"

using namespace std;

int main()
{

	int n = 8;
	
	cout << FibMemoizedRecursive(n) << endl;
	cout << FibMemoizedIterative(n) << endl;	
	cout << FibNaive(n) << endl;

	vector<size_t> p = { 30, 35, 15, 5, 10, 20, 25 };

	auto res = MatrixChainOrder( p );

	auto s = res->arr_s;

	PrintMatrixParenthesis( s, 0, s.size() - 1 );

	cout << endl;
	
	cout << LongestCommonSubsequence("HIEROGLYPHOLOGY", "MICHAELANGELO"); //GTCGTCGGAAGCCGGCCGAA

	vector<Item> k = { {2,3}, {2,1}, {4,3}, {5,4}, {3,2} };

	auto r = Knapsack(k, 7);

	cout << endl;
}
