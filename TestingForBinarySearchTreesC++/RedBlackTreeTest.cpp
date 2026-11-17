#include <gtest/gtest.h>

#include <climits>
#include <memory>
#include <random>
#include <set>
#include <string>
#include <type_traits>

#include "BinarySearchTreesC++/RedBlackTree.h"

namespace {

    using Tree = RedBlackTree<int, std::string>;
    using NodePtr = decltype(std::declval<Tree&>().GetRoot());
    using Node = std::remove_reference_t<decltype(*std::declval<NodePtr>())>;

    bool IsBlack(const NodePtr& x) { return x == nullptr || x->color == Node::Color::BLACK; }   // nil is black

    // The parent link as a plain pointer, whatever its type is: shared_ptr now,
    // weak_ptr or a raw pointer after the memory leak is fixed.
    const Node* Raw(const std::shared_ptr<Node>& p) { return p.get(); }
    const Node* Raw(const std::weak_ptr<Node>& p) { return p.lock().get(); }
    const Node* Raw(const Node* p) { return p; }

    // Checks every property of a red-black tree in the subtree of x (CLRS 13.1) and the BST order.
    // Returns the black height; the first violation found goes to error.
    int CheckSubtree(const NodePtr& x, const Node* parent, long long lo, long long hi, std::string& error) {
        if (x == nullptr) return 1;

        auto note = [&](const std::string& what) {
            if (error.empty()) error = what + " at key " + std::to_string(x->key);
        };
        if (Raw(x->parent) != parent) note("wrong parent link");
        if (x->key < lo || x->key > hi) note("BST order broken");
        if (!IsBlack(x) && (!IsBlack(x->left) || !IsBlack(x->right))) note("red node with a red child");

        int left = CheckSubtree(x->left, x.get(), lo, x->key, error);
        int right = CheckSubtree(x->right, x.get(), x->key, hi, error);
        if (left != right) note("different black heights of the left and right subtrees");

        return left + (IsBlack(x) ? 1 : 0);
    }

    // Empty string = the tree is a valid red-black tree.
    std::string CheckTree(Tree& t) {
        std::string error;
        auto root = t.GetRoot();
        if (!IsBlack(root)) error = "red root";
        CheckSubtree(root, nullptr, LLONG_MIN, LLONG_MAX, error);
        return error;
    }

    void CollectKeys(const NodePtr& x, std::multiset<int>& keys) {
        if (x == nullptr) return;
        CollectKeys(x->left, keys);
        keys.insert(x->key);
        CollectKeys(x->right, keys);
    }

    std::multiset<int> Keys(Tree& t) {
        std::multiset<int> keys;
        CollectKeys(t.GetRoot(), keys);
        return keys;
    }

}

TEST(RedBlackTree, EmptyTreeHasNoRoot) {
    Tree t;
    EXPECT_EQ(t.GetRoot(), nullptr);
    EXPECT_EQ(t.Find(1), nullptr);
}

TEST(RedBlackTree, InsertKeepsAllProperties) {
    std::mt19937 rng(1);
    Tree t;
    for (int i = 0; i < 2000; i++) {
        t.RBInsert({ static_cast<int>(rng() % 100'000), "v" });
        ASSERT_EQ(CheckTree(t), "") << "after insert #" << i;
    }
}

TEST(RedBlackTree, InsertOfSortedKeysKeepsTreeBalanced) {
    Tree t;
    for (int k = 0; k < 1000; k++) t.RBInsert({ k, "v" });
    ASSERT_EQ(CheckTree(t), "");
    // the height of a red-black tree with n nodes is at most 2 log2(n + 1), about 20 for 1000
    int height = 0;
    for (const Node* x = t.GetTreeMinimum().get(); x != nullptr; x = Raw(x->parent)) height++;
    EXPECT_LE(height, 20);
}

TEST(RedBlackTree, FindReturnsInsertedValue) {
    Tree t;
    for (int k = 0; k < 100; k++) t.RBInsert({ k, "v" + std::to_string(k) });
    for (int k = 0; k < 100; k++) {
        auto x = t.Find(k);
        ASSERT_NE(x, nullptr) << "key " << k;
        EXPECT_EQ(x->value, "v" + std::to_string(k));
    }
    EXPECT_EQ(t.Find(100), nullptr);
}

TEST(RedBlackTree, MinimumAndMaximum) {
    Tree t;
    for (int k : { 50, 20, 80, 10, 90, 30 }) t.RBInsert({ k, "v" });
    EXPECT_EQ(t.GetTreeMinimum()->key, 10);
    EXPECT_EQ(t.GetTreeMaximum()->key, 90);
}

// Removing a black leaf takes one black node off its paths: RBDeleteFixup must restore the black height.
//      10B              10B
//     /   \     ->      /        <- invalid: the right path has 1 black node, the left one 2
//    5B   20B          5B
//   /                 /
//  1R                1R
TEST(RedBlackTree, DeleteOfBlackLeafKeepsAllProperties) {
    Tree t;
    for (int k : { 10, 5, 20, 1 }) t.RBInsert({ k, "v" });
    ASSERT_EQ(CheckTree(t), "");

    t.RBDelete(20);

    EXPECT_EQ(CheckTree(t), "");
    EXPECT_EQ(Keys(t), (std::multiset<int>{ 1, 5, 10 }));
}

TEST(RedBlackTree, DeleteOfEveryKeyKeepsAllProperties) {
    Tree t;
    for (int k = 0; k < 200; k++) t.RBInsert({ k, "v" });
    for (int k = 0; k < 200; k++) {
        t.RBDelete(k);
        ASSERT_EQ(CheckTree(t), "") << "after deleting key " << k;
    }
    EXPECT_EQ(t.GetRoot(), nullptr);
}

TEST(RedBlackTree, DeleteOfRootKeepsAllProperties) {
    Tree t;
    for (int k = 0; k < 100; k++) t.RBInsert({ k, "v" });
    for (int i = 0; i < 50; i++) {
        t.RBDelete(t.GetRoot()->key);
        ASSERT_EQ(CheckTree(t), "") << "after deleting root #" << i;
    }
    EXPECT_EQ(Keys(t).size(), 50u);
}

TEST(RedBlackTree, DeleteOfAbsentKeyChangesNothing) {
    Tree t;
    for (int k = 0; k < 10; k++) t.RBInsert({ k, "v" });
    t.RBDelete(100);
    EXPECT_EQ(Keys(t).size(), 10u);
    EXPECT_EQ(CheckTree(t), "");
}

// Random inserts and deletes replayed against std::set: same keys and a valid tree after every step.
TEST(RedBlackTree, RandomOperationsKeepAllProperties) {
    std::mt19937 rng(2026);
    for (int run = 0; run < 50; run++) {
        Tree t;
        std::multiset<int> reference;
        for (int op = 0; op < 400; op++) {
            int k = static_cast<int>(rng() % 60);
            if (rng() % 2 == 0) {
                if (reference.count(k) == 0) {
                    t.RBInsert({ k, "v" });
                    reference.insert(k);
                }
            } else {
                t.RBDelete(k);
                reference.erase(k);
            }
            ASSERT_EQ(Keys(t), reference) << "run " << run << ", operation #" << op;
            ASSERT_EQ(CheckTree(t), "") << "run " << run << ", operation #" << op;
        }
    }
}

// Nodes point to each other with shared_ptr both ways (children and parent), so the counters never
// reach zero and nothing is freed. After the tree is destroyed its nodes must be destroyed too.
TEST(RedBlackTree, NodesAreFreedWhenTreeIsDestroyed) {
    std::weak_ptr<Node> root, leaf;
    {
        Tree t;
        for (int k = 0; k < 10; k++) t.RBInsert({ k, "v" });
        root = t.GetRoot();
        leaf = t.GetTreeMinimum();
    }
    EXPECT_TRUE(root.expired()) << "the root node is still alive";
    EXPECT_TRUE(leaf.expired()) << "a leaf is still alive";
}

TEST(RedBlackTree, DeletedNodeIsFreed) {
    Tree t;
    for (int k = 0; k < 10; k++) t.RBInsert({ k, "v" });
    std::weak_ptr<Node> deleted = t.Find(5);

    t.RBDelete(5);

    EXPECT_TRUE(deleted.expired()) << "the deleted node is still alive";
}
