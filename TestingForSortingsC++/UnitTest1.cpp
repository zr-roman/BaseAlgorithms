#pragma once

#include "pch.h"
#include "CppUnitTest.h"
#include <random>
#include <vector>
#include "../SortingsC++/Lib.h"
#include "../SortingsC++/Lib.cpp"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace std;

namespace SortingsUnitTests {

	int N = 20000;

	TEST_CLASS( ShellSort ) {
		
	public:

		TEST_CLASS_INITIALIZE( ClassInitialize ) {
			
		}

		TEST_METHOD( Asc ) {
			
			vector<int> arr = LibForTests::GetArbitraryArray( -N, N );

			SortingLib::ShellSort( &arr );
			for ( size_t i = 0; i + 1 < arr.size(); i++ ) {
				Assert::IsTrue( arr.at( i ) <= arr.at( i + 1 ) );
			}

			/*LibForTests::RunTest(SortingLib::Order::ASC, SortingLib::ShellSort<int>, Assert::IsTrue);*/

		};
		
		TEST_METHOD( Desc ) {

			vector<int> arr = LibForTests::GetArbitraryArray( -N, N );

			SortingLib::ShellSort( &arr, SortingLib::SortingLib::Order::DESC );
			for ( size_t i = 0; i + 1 < arr.size(); i++ ) {
				Assert::IsTrue( arr.at( i ) >= arr.at( i + 1 ) );
			}
		};
	};

	TEST_CLASS( InsertionSort ) {

		public:
			TEST_METHOD( Asc ) {

				vector<int> arr = LibForTests::GetArbitraryArray( -N, N );

				SortingLib::InsertionSort( &arr );
				for ( size_t i = 0; i + 1 < arr.size(); i++ ) {
					Assert::IsTrue( arr.at( i ) <= arr.at( i + 1 ) );
				}
			};

			TEST_METHOD( Desc ) {

				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::InsertionSort( &arr, SortingLib::SortingLib::Order::DESC );
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
				}

			};

	};

	TEST_CLASS( QuickSort ) {

		public:
			TEST_METHOD( AscPivotRandom ) {

				vector<int> arr = LibForTests::GetArbitraryArray( -N, N );

				SortingLib::QuickSort( &arr );
				for ( size_t i = 0; i + 1 < arr.size(); i++ ) {
					Assert::IsTrue( arr.at( i ) <= arr.at( i + 1 ) );
				}
			};

			TEST_METHOD( DescPivotRandom ) {

				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::QuickSort( &arr, SortingLib::Order::DESC );
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
				}

			};

			TEST_METHOD( PivotFirst ) {
				
				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::QuickSort( &arr, SortingLib::Order::ASC, SortingLib::SortingLib::Pivot::FIRST);
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					Logger::WriteMessage( (std::to_string( arr.at(i) )).c_str() );
				}

			};

			TEST_METHOD( PivotLast ) {

				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::QuickSort( &arr, SortingLib::Order::ASC, SortingLib::Pivot::LAST);
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
				}

			};

			TEST_METHOD( NullArray ) {

				auto func = [] {
					SortingLib::QuickSort( (vector<int>*)(NULL), SortingLib::Order::ASC, SortingLib::Pivot::LAST );
				};
				Assert::ExpectException<runtime_error>( func );
			};

			TEST_METHOD( EmptyArray ) {

				auto func = [] { 
					vector<int> arr(0);
					SortingLib::QuickSort( &arr, SortingLib::Order::ASC, SortingLib::Pivot::LAST );
				};
				Assert::ExpectException<runtime_error>( func );
			};
	};

	TEST_CLASS( MergeSort ) {

		public:
			TEST_METHOD( Asc ) {
				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::MergeSort( &arr, INT_MAX, INT_MIN, SortingLib::Order::ASC);
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
				}
			};

			TEST_METHOD( Desc ) {
				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::MergeSort( &arr, INT_MAX, INT_MIN, SortingLib::Order::DESC );
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
				}
			};


	};

	TEST_CLASS( HeapSort ) {

		public:
			TEST_METHOD( Asc ) {
				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::HeapSort( &arr, SortingLib::Order::ASC);
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
				}
			};

			TEST_METHOD( Desc ) {
				vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

				SortingLib::HeapSort( &arr, SortingLib::Order::DESC );
				for (size_t i = 0; i + 1 < arr.size(); i++) {
					Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
				}
			};


	};

	TEST_CLASS( BubbleSort ) {

	public:
		TEST_METHOD( Asc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::BubbleSort( &arr, SortingLib::Order::ASC);
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
			}
		};

		TEST_METHOD( Desc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::BubbleSort( &arr, SortingLib::Order::DESC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
			}
		};
	};

	TEST_CLASS( CombSort ) {

	public:
		TEST_METHOD( Asc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::CombSort( &arr, SortingLib::Order::ASC);
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
			}
		};

		TEST_METHOD( Desc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::CombSort( &arr, SortingLib::Order::DESC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
			}
		};
	};

	TEST_CLASS( BruteForceSort ) {

	public:
		TEST_METHOD( Asc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::BruteForceSort( &arr, SortingLib::Order::ASC);
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
			}
		};

		TEST_METHOD( Desc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::BruteForceSort( &arr, SortingLib::Order::DESC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
			}
		};
	};

	TEST_CLASS( SelectionSort ) {

	public:
		TEST_METHOD( Asc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::SelectionSort( &arr, SortingLib::Order::ASC);
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
			}
		};

		TEST_METHOD( Desc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::SelectionSort( &arr, SortingLib::Order::DESC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
			}
		};
	};

	TEST_CLASS( CountingSort ) {

	public:
		TEST_METHOD( Asc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::CountingSort( &arr, SortingLib::Order::ASC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
			}
		};

		TEST_METHOD( Desc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::CountingSort( &arr, SortingLib::Order::DESC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
			}
		};
	};

	TEST_CLASS( RadixSort ) {

	public:
		TEST_METHOD( Asc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::RadixSort( &arr, SortingLib::Order::ASC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) <= arr.at(i + 1));
					
			}
		};

		TEST_METHOD( Desc ) {
			vector<int> arr = LibForTests::GetArbitraryArray(-N, N);

			SortingLib::RadixSort( &arr, SortingLib::Order::DESC );
			for (size_t i = 0; i + 1 < arr.size(); i++) {
				Assert::IsTrue(arr.at(i) >= arr.at(i + 1));
					
			}
		};
	};

}
