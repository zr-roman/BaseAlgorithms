#include "pch.h"

using namespace System;

int BinarySearch( int* arr, int key ) {
    
    int start = 0;
    int end = (*(&arr + 1) - arr) - 1;
    
    while ( start <= end ) {

        int mid = ( start + end ) / 2;

        if ( arr[ mid ] == key ) {
            return mid;
        }

        if ( key < arr[ mid ] ) {
            end = mid - 1;
        }

        if ( key > arr[ mid ] ) {
            start = mid + 1;
        }
    }
    return -1;

}

int BinarySearchRecursive( int* arr, int key, int start, int end ) {

    if ( start > end ) {
        return -1;
    }
    
    int mid = (start + end) / 2;

    if ( key == arr[ mid ] ) {
        return mid;
    }

    if ( key < arr[ mid ] ) {
        return BinarySearchRecursive( arr, key, start, mid - 1 );
    }

    if ( key > arr[ mid ] ) {
        return BinarySearchRecursive( arr, key, mid + 1, end );
    }
}

int main(array<System::String ^> ^args) {
                         //0 1 2 3 4 5 6 7 8 9  10 11 12 13 14 15 16 17 18
    int arr[] = /*new int[] */ { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }; // input - sorted array
    int arrSize = sizeof(arr) / sizeof(arr[0]);

    int res1 = BinarySearch( arr, -5 );

    int res2 = BinarySearchRecursive( arr, 22, 0, *(&arr + 1) - arr - 1 );

    /*
    delete[] arr;
    arr = nullptr;*/

    return 0;
}
