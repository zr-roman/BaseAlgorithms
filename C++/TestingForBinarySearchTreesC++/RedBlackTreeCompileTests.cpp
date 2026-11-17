// Checks that the tree compiles for key and value types other than int and string,
// and that RedBlackTree.h can be included in more than one .cpp file of a program
// (this file and RedBlackTreeTest.cpp are two translation units of one executable).
//
// Built only with the CMake option RBT_COMPILE_CHECKS=ON:
// with the current RedBlackTree.h it does not compile or link, which would break every other test.

#include <gtest/gtest.h>

#include <string>

#include "BinarySearchTreesC++/RedBlackTree.h"

TEST(RedBlackTreeTypes, StringKeys) {
    RedBlackTree<std::string, int> t;
    for (const char* k : { "m", "c", "x", "a", "e" }) t.RBInsert({ k, 1 });
    t.RBDelete("c");
    EXPECT_EQ(t.Find("c"), nullptr);
    ASSERT_NE(t.Find("e"), nullptr);
    EXPECT_EQ(t.GetTreeMinimum()->key, "a");
}

TEST(RedBlackTreeTypes, DoubleKeysAndIntValues) {
    RedBlackTree<double, int> t;
    t.RBInsert({ 1.5, 15 });
    t.RBInsert({ -2.5, -25 });
    ASSERT_NE(t.Find(1.5), nullptr);
    EXPECT_EQ(t.Find(1.5)->value, 15);
}
