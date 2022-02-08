#pragma once

#include <iostream>
#include <map>

using namespace std;

int FibNaive(int n) {
	if ( n == 1 ) { return 0; }
	if ( n == 2 ) { return 1; }
	return FibNaive( n - 1 ) + FibNaive( n-2 );
};

int FibMemoizedRecursive(int n, shared_ptr<map<int, int>> dic = nullptr) {
	
	if ( dic == nullptr ) {
		dic = make_shared<map<int, int>>();
		dic->insert({ 1,0 });
		dic->insert({2, 1});
	} 

	if ( dic->count(n) ) { 
		return dic->at( n ); 
	}

	dic->insert( { n, FibMemoizedRecursive(n - 1, dic) + FibMemoizedRecursive(n - 2, dic) } );
	return dic->at(n);	
};

int FibMemoizedIterative(int n) {
	map<int, int> dic = {};
	dic.insert( { 1, 0 } );
	dic.insert( { 2,1 } );
	for ( size_t i = 3; i <= n; i++ ) {
		dic.insert( {i, dic[ i - 1 ] + dic[ i - 2 ] } );
	}
	return dic[ n ];
};