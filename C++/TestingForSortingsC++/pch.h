// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here

#pragma once
#include <vector>
#include "../SortingsC++/Lib.h"
#include "CppUnitTestAssert.h"

using namespace std;

class LibForTests {

	public:
		
		static vector<int> GetArbitraryArray( int start, int end ) {

			vector<int> arr;

			auto n = abs( RandomizationLib::GenerateRandomNumber( start, end )) + 10;
			for (size_t i = 0; i < n; i++) {
				arr.push_back( RandomizationLib::GenerateRandomNumber( start, end ) );
			}
			return arr;
		};

		/* compiles, but not working
		template <typename F>
		static void RunTest(Order order, void (*sorting_function)(F, Order), void (*assert_function)(bool, const wchar_t*, const Microsoft::VisualStudio::CppUnitTestFramework::__LineInfo*)) {

			vector<int> arr = LibForTests::GetArbitraryArray( -100, 100 );

			sorting_function( F(arr), order );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				assert_function( arr.at(i) <= arr.at(i + 1),  NULL, NULL);
			}

		};*/
};


#endif //PCH_H
