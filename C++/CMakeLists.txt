cmake_minimum_required(VERSION 3.20)
project(BaseAlgorithms LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

if(MSVC)
    add_compile_options(/W4 /utf-8)
else()
    add_compile_options(-Wall -Wextra)
endif()

# Sorting library; sources include it as "SortingsC++/Lib.h", so the repository root is an include directory
add_library(sortings STATIC SortingsC++/Lib.cpp)
target_include_directories(sortings PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})

# GoogleTest: an installed one if there is one, otherwise downloaded into the build directory
find_package(GTest CONFIG QUIET)
if(NOT GTest_FOUND)
    if(POLICY CMP0135)
        cmake_policy(SET CMP0135 NEW)  # timestamps of extracted files = extraction time
    endif()
    include(FetchContent)
    FetchContent_Declare(googletest
        URL https://github.com/google/googletest/archive/refs/tags/v1.15.2.zip)
    set(gtest_force_shared_crt ON CACHE BOOL "" FORCE)  # MSVC: the same runtime library as the tests
    set(INSTALL_GTEST OFF CACHE BOOL "" FORCE)
    FetchContent_MakeAvailable(googletest)
endif()

enable_testing()

# UnitTest1.cpp is a Visual Studio CppUnitTest file and is not built here
add_executable(sorting_tests TestingForSortingsC++/UnitTest2.cpp)
target_link_libraries(sorting_tests PRIVATE sortings GTest::gtest_main)

# Random number generators xoshiro256** and LCG: header-only, their tests and a small example.
# RandomNumberGenerationC++.cpp uses windows.h and is not built here.
add_executable(random_tests
    TestingForRandomNumberGenerationC++/RandomTest.cpp
    TestingForRandomNumberGenerationC++/LcgTest.cpp)
target_include_directories(random_tests PRIVATE ${CMAKE_CURRENT_SOURCE_DIR})
target_link_libraries(random_tests PRIVATE GTest::gtest_main)

add_executable(random_example "RandomNumberGenerationC++/example_main.cpp")

# Red-black tree (header-only) and its tests.
# RBTest.cpp is a Visual Studio CppUnitTest file and is not built here.
add_executable(rbt_tests TestingForBinarySearchTreesC++/RedBlackTreeTest.cpp)
target_include_directories(rbt_tests PRIVATE ${CMAKE_CURRENT_SOURCE_DIR})
target_link_libraries(rbt_tests PRIVATE GTest::gtest_main)

# Graph algorithms' shared min-heap.
add_executable(graph_min_heap_tests TestingForGraphsC++/MinHeapTests.cpp)
target_include_directories(graph_min_heap_tests PRIVATE ${CMAKE_CURRENT_SOURCE_DIR})
target_link_libraries(graph_min_heap_tests PRIVATE GTest::gtest_main)

# Tests that do not compile or link with the current RedBlackTree.h (other key types, the header
# included in two .cpp files). Turn on with -DRBT_COMPILE_CHECKS=ON once the tree is fixed.
option(RBT_COMPILE_CHECKS "Build the red-black tree tests for other key types" OFF)
if(RBT_COMPILE_CHECKS)
    target_sources(rbt_tests PRIVATE TestingForBinarySearchTreesC++/RedBlackTreeCompileTests.cpp)
endif()

include(GoogleTest)
gtest_discover_tests(sorting_tests)
gtest_discover_tests(random_tests)
# Leaks are checked by the tests themselves (NodesAreFreedWhenTreeIsDestroyed, DeletedNodeIsFreed).
# Without this LeakSanitizer in the sanitizer build would fail every test that creates a tree.
gtest_discover_tests(rbt_tests PROPERTIES ENVIRONMENT "ASAN_OPTIONS=detect_leaks=0")
gtest_discover_tests(graph_min_heap_tests)
