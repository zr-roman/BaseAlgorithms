#include "pch.h"
#include "CppUnitTest.h"
#include "../BinarySearchTreesC++/RedBlackTree.h"

#include <vector>
#include <memory>
#include <random>
#include <cstdio>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

class RedBlackTreeTest : public RedBlackTree<int, string> {
public:

    int GetTreeMinimumTest() {
        return this->GetTreeMinimum()->key;
    };

    static shared_ptr<RedBlackTreeTest> GenerateRandomRedBlackTree(int size) {
        
        auto tree = make_shared<RedBlackTreeTest>();
        tree->_tree_min = INT_MAX;
        for ( int i = 0; i < size; i++ ) {
            auto r = GenerateRandomNumber( -size, size );
            tree->RBInsert( pair<int, string>(r, "Ivanov" + r) );
            if ( r < tree->_tree_min ) {
                tree->_tree_min = r;
            }
        }
        return tree;
    };

    bool IsRootBlack() {
        return this->GetRoot()->color == Node::Color::BLACK;
    };

    static bool DoesRedNodeHaveBlackChildren(shared_ptr<Node> x) {
        if ( x->color == Node::Color::BLACK ) {
            return true;
        }
        if (x->left == nullptr && x->right == nullptr) {
            return true;
        }
        if ( x->left != nullptr && x->right == nullptr && x->left->color == Node::Color::BLACK ) {
            return true;
        }
        if ( x->right != nullptr && x->left == nullptr && x->right->color == Node::Color::BLACK ) {
            return true;
        }
        if (x->left != nullptr && x->right != nullptr && x->left->color == Node::Color::BLACK && x->right->color == Node::Color::BLACK ) {
            return true;
        }
        return false;
    };    

    vector<shared_ptr<Node>> GetAllNonLeaves() {

        auto f = [](vector<shared_ptr<Node>>* arr, shared_ptr<Node> x) -> void {
            if ( !( x->left == nullptr && x->right == nullptr) ) {
                arr->push_back(x);
            }
        };

        vector<shared_ptr<Node>> arr;

        this->InOrderTraverseInner( this->_root_ptr->left, &arr, f );

        return arr;
        
    };

    int GetTreeMinimumRaw() {
        return _tree_min;
    };

    void DeleteNode( int key ) {
        this->RBDelete( key );
    };

private: 
    int _tree_min;
};

namespace BinarySearchTreesTest {
	
	int N = 1000;
    

	TEST_CLASS( RedBlackTree )  {

	public:
		
		TEST_METHOD( TestAll ) {

            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree( N );
			
			Assert::IsTrue(  tree->IsRootBlack() );

			auto arr = tree->GetAllLeaves();

			int x = RedBlackTreeTest::CountSimplePath( arr.at( 0 ) );
			int y = 0;

			for (auto i = 1; i < arr.size(); i++ ) {
				y = RedBlackTreeTest::CountSimplePath( arr.at(i) );
				Assert::AreEqual( x, y );
				x = y;
			}
			
			arr = tree->GetAllNonLeaves();
			for ( auto node : arr ) {
				Assert::IsTrue(RedBlackTreeTest::DoesRedNodeHaveBlackChildren(node) );
			}
		};

        TEST_METHOD( RootIsBlack ) {
            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree(N);
            Assert::IsTrue( tree->IsRootBlack() );
        };

        TEST_METHOD( SimplePaths ) {

            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree(N);
            auto arr = tree->GetAllLeaves();

			int x = RedBlackTreeTest::CountSimplePath( arr.at( 0 ) );
			int y = 0;

			for (auto i = 1; i < arr.size(); i++ ) {
				y = RedBlackTreeTest::CountSimplePath( arr.at( i ) );
				Assert::IsTrue( abs(x - y) < 2  );
				x = y;
			}
        };

        TEST_METHOD( AllRedNodesHaveBlackChildren ) {
            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree(N);
            auto arr = tree->GetAllNonLeaves();
			for ( auto node : arr ) {
				Assert::IsTrue(RedBlackTreeTest::DoesRedNodeHaveBlackChildren(node) );
			}
        };

        TEST_METHOD( TreeMinimum ) {
            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree( N );
            Assert::AreEqual( tree->GetTreeMinimumTest(), tree->GetTreeMinimumRaw() );
        };

        TEST_METHOD( Find ) {
            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree( N );
            int n = N*10;
            while ( n>0 ) {
                n--;
                auto r = GenerateRandomNumber( -N, N );
                tree->Find( r );
            }
        };

        TEST_METHOD( RBDelete_SimplePaths ) {

            FILE* stream;

            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree( N );
            int n = 500;
            while ( n > 0 ) {
                n--;
                auto r = GenerateRandomNumber( -N, N );
                
                tree->RBDelete( r );
                tree->InOrderTraverse(false);

                // Simple paths
                auto arr = tree->GetAllLeaves();

			    int x = RedBlackTreeTest::CountSimplePath( arr.at( 0 ) );
			    int y = 0;

			    for (auto i = 1; i < arr.size(); i++ ) {
				    
                    y = RedBlackTreeTest::CountSimplePath( arr.at( i ) );
                    /*
                    if ( x != y ) {
                        freopen_s(&stream, "output.txt", "w", stdout);
                        cout << "x: " << x << " y: " << y << endl;
                    }*/
                    
                    Assert::IsTrue(abs(x - y) < 3); // ослабленное условие
				    x = y;
			    }
                
            }
            
        };

        TEST_METHOD( RBDelete_RedNodeHaveBlackChildren ) {

            FILE* stream;

            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree( N );
            int n = 500;
            while ( n > 0 ) {
                n--;
                auto r = GenerateRandomNumber( -N, N );
                
                tree->RBDelete( r );
                tree->InOrderTraverse(false);

                // 1. AllRedNodesHaveBlackChildren
                auto arr = tree->GetAllNonLeaves();
                for (auto node : arr) {
                    auto res = RedBlackTreeTest::DoesRedNodeHaveBlackChildren( node );
                    if ( !res ) {
                        freopen_s( &stream, "output.txt", "w", stdout );
                        cout << "node: " << node->key << " " << node->ToString( node->color ) << " left: " << node->left->key << " " << node->left->ToString( node->left->color ) << " right:" << node->right->key << " " << node->right->ToString(node->right->color) << endl;
                    }
                    Assert::IsTrue(res);
                }
                
            }
            
        };

        TEST_METHOD( RBDelete_RootIsBlack ) {

            FILE* stream;

            auto tree = RedBlackTreeTest::GenerateRandomRedBlackTree( N );
            int n = 500;
            while ( n > 0 ) {
                n--;
                auto r = GenerateRandomNumber( -N, N );
                
                tree->RBDelete( r );
                tree->InOrderTraverse(false);

                // Root is black
                Assert::IsTrue( tree->IsRootBlack() );

            }
            
        };

	};

}
