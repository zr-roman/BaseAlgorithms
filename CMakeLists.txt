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

include(GoogleTest)
gtest_discover_tests(sorting_tests)
