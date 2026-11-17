#pragma once

#include <memory>
#include <iostream>
#include <map>
#include <vector>
#include <random>

using namespace std;

template <typename K, typename V>
class RedBlackTree {

public:
	void RBInsert( pair<K, V> pair );
    void RBDelete( K key );
    void InOrderTraverse( bool print = true );
    RedBlackTree();
    
protected:

    class Node {
    public:

        enum class Color {
            RED,
            BLACK
        };

        inline const char* ToString(Color o) {
            switch (o) {
                case Color::BLACK: return "BLACK";
                case Color::RED:   return "RED";
                default:           throw runtime_error("Must not hit into here!");
            }
        };

        shared_ptr<Node> left;
        shared_ptr<Node> right;
        shared_ptr<Node> parent;
        Color color;
        K key;
        V value;
        Node( shared_ptr<Node> left, shared_ptr<Node> right, shared_ptr<Node> parent, K key, V value );
        void Print();
    };

    void RBInsertFixup( shared_ptr<Node> x );
    void RBDeleteFixup( shared_ptr<Node> x );
    void InOrderTraverseInner( shared_ptr<Node> x, vector<shared_ptr<Node>>* arr, void (*func1)(vector<shared_ptr<Node>>*, shared_ptr<Node>), bool print = true);
    
    void RightRotate( shared_ptr<Node> x );
    void LeftRotate( shared_ptr<Node> x );

    void detect_and_remove_cyclic_links( shared_ptr<Node> v );

    // RBTransplant cuts off the node u from the tree, 
    // i.e. after the procedure no one node points to node u, though node u may point to some other nodes
    void RBTransplant( shared_ptr<Node> u, shared_ptr<Node> v );
    shared_ptr<Node> _root_ptr; // _root_ptr - это не node, а указатель, который всегда указывает на Root Node, для этого используется left

public:
    
    shared_ptr<Node> GetRoot() {
        return _root_ptr->left;
    };

    shared_ptr<Node> Find( K key );
    shared_ptr<Node> GetTreeMinimum( shared_ptr<Node> x = nullptr );
    shared_ptr<Node> GetTreeMaximum( shared_ptr<Node> x = nullptr );
    vector<shared_ptr<Node>> GetAllLeaves();
    static int CountSimplePath( shared_ptr<Node> x );
};

template <typename K, typename V>
int RedBlackTree<K, V>::CountSimplePath( shared_ptr<Node> x ) {
        	
    int i = 0;

	    while ( x != nullptr ) {
		    if ( x->color == RedBlackTree<int, string>::Node::Color::BLACK ) {
			    i++;
		    }
		    x = x->parent;
	    }
    return i;
};

template <typename K, typename V>
vector<shared_ptr<typename RedBlackTree<K, V>::Node>> RedBlackTree<K, V>::GetAllLeaves( ) {

    auto f = [](vector<shared_ptr<Node>>* arr, shared_ptr<Node> x) -> void {
        if ( x->left == nullptr && x->right == nullptr ) {
            arr->push_back(x);
        }
    };

    vector<shared_ptr<Node>> arr;

    this->InOrderTraverseInner( this->_root_ptr->left, &arr, f );

    return arr;
};

template <typename K, typename V>
shared_ptr<typename RedBlackTree<K, V>::Node> RedBlackTree<K, V>::GetTreeMinimum( shared_ptr<Node> x ) {
    
    if ( x == nullptr ) {
        x = GetRoot();
    }

    while ( x->left != nullptr ) {
        x = x->left;
    }

    return x;
    
};

template <typename K, typename V>
shared_ptr<typename RedBlackTree<K, V>::Node> RedBlackTree<K, V>::GetTreeMaximum( shared_ptr<Node> x ) {
    
    if ( x == nullptr ) {
        x = GetRoot();
    }

    while ( x->right != nullptr ) {
        x = x->right;
    }

    return x;
    
};

template <typename K, typename V>
RedBlackTree<K, V>::RedBlackTree() {
    _root_ptr = make_shared<Node>( nullptr, nullptr, nullptr, -1, "pointer to root" );
};

template <typename K, typename V>
RedBlackTree<K, V>::Node::Node(shared_ptr<Node> left, shared_ptr<Node> right, shared_ptr<Node> parent, K key, V value) {
    this->left =  left ;
    this->right = right;
    this->parent = parent;
    this->key = key;
    this->value = value;
};

template <typename K, typename V>
void RedBlackTree<K, V>::RBInsert( pair<K, V> pair ) {

    // новый узел, вставляемый в дерево
    shared_ptr<Node> z = make_shared<Node>( nullptr, nullptr, nullptr, pair.first, pair.second );    

    // Начиная от root, доходим до самого низа и вставляем узел под последним листом
    shared_ptr<Node> x = GetRoot();
    shared_ptr<Node> y = nullptr;
    while ( x != nullptr ) {
        y = x;
        if ( z->key < x->key ) {
            x = x->left;
        } else {
            x = x->right;
        }
    }
    z->parent = y;
    //

    // если дерево пустое, то узел становится root
    if ( y == nullptr ) {
        _root_ptr->left = z; 
    // если дерево не пустое, тогда определяем, каким child он станет: left или right
    } else if ( z->key < y->key ) {
        y->left = z;
    } else {
        y->right = z;
    }
    //

    z->left = nullptr; // каждый новый узел вставляется в низ дерева и не имеет потомков
    z->right = nullptr; // каждый новый узел вставляется в низ дерева и не имеет потомков
    z->color = Node::Color::RED; // каждый вставляемый узел становится красным

    RBInsertFixup( z );
};

template <typename K, typename V>
void RedBlackTree<K, V>::RBInsertFixup(shared_ptr<Node> x ) {
    
    if ( x->parent == nullptr ) {
        return;
    }
    
    while ( x->parent != nullptr && x->parent->color == Node::Color::RED ) {

        if ( x->parent->parent == nullptr ) {
            break;
        }
        
        if ( x->parent == x->parent->parent->left ) {

            auto y = x->parent->parent->right;
                        
            if ( y != nullptr && y->color == Node::Color::RED ) {
                x->parent->color = Node::Color::BLACK;       // случай 1 Дядя Y узла Z - красный
                y->color = Node::Color::BLACK;               // случай 1
                x->parent->parent->color = Node::Color::RED; // случай 1
                x = x->parent->parent;                       // случай 1
            } else {
                if (x == x->parent->right) {
                    x = x->parent;                           // случай 2 Дядя Y узла Z - черный, и Z - правый потомок
                    LeftRotate( x );                         // случай 2
                }
                x->parent->color = Node::Color::BLACK;       // случай 3 Дядя Y узла Z - черный, и Z - левый потомок
                x->parent->parent->color = Node::Color::RED; // случай 3
                RightRotate( x->parent->parent );            // случай 3
            }
            
        } else { // то же самое что и в if, но с заменой "right" на "left" и наоборот

            auto y = x->parent->parent->left;
                        
            if ( y != nullptr && y->color == Node::Color::RED ) {
                x->parent->color = Node::Color::BLACK;       // случай 1
                y->color = Node::Color::BLACK;               // случай 1
                x->parent->parent->color = Node::Color::RED; // случай 1
                x = x->parent->parent;                       // случай 1
            } else {
                if (x == x->parent->left) {
                    x = x->parent;                           // случай 2
                    RightRotate( x );                        // случай 2
                }
                x->parent->color = Node::Color::BLACK;       // случай 3
                x->parent->parent->color = Node::Color::RED; // случай 3
                LeftRotate( x->parent->parent );             // случай 3
            }
        }
    }
    GetRoot()->color = Node::Color::BLACK;
};

template <typename K, typename V>
void RedBlackTree<K,V>::InOrderTraverse(bool print) {
    InOrderTraverseInner( GetRoot(), nullptr, nullptr, print );
};

template <typename K, typename V>
void RedBlackTree<K,V>::InOrderTraverseInner(shared_ptr<Node> x, vector<shared_ptr<Node>>* arr, void (*func1)( vector<shared_ptr<Node>>*, shared_ptr<Node>), bool print) {
    
    if ( x == nullptr ) {
        return;
    }
    
    InOrderTraverseInner( x->left, arr, func1 );

    if ( print ) {
        //x->Print();
    }

    if ( func1 != nullptr ) {
        func1(arr, x);
    }

    InOrderTraverseInner( x->right, arr, func1 );
};

template <typename K, typename V>
void RedBlackTree<K, V>::RightRotate(shared_ptr<Node> x ) {

    if (x == nullptr) {
        return;
    }

    auto y = x->left;

    if (y == nullptr) {
        return;
    }
    
    if ( y->right != nullptr ) {
        y->right->parent = x;
    }
    x->left = y->right;
    y->right = x;
    y->parent = x->parent;

    if ( x->parent != nullptr ) {
        if ( x->parent->left == x ) {
            x->parent->left = y;
        } else if ( x->parent->right == x ) {
            x->parent->right = y;
        }
    }

    x->parent = y;

    if ( y->parent == nullptr ) {
        _root_ptr->left = y;
    }
    
};

template <typename K, typename V>
void RedBlackTree<K, V>::LeftRotate(shared_ptr<Node> x ) {

    if ( x == nullptr ) {
        return;
    }

    auto y = x->right;

    if ( y == nullptr ) {
        return;
    }

    if ( y->left != nullptr ) {
        y->left->parent = x;
    }
    x->right = y->left;
    y->left = x;
    y->parent = x->parent;

    if ( x->parent != nullptr ) {
        if ( x->parent->left == x ) {
            x->parent->left = y;
        } else if ( x->parent->right == x ) {
            x->parent->right = y;
        }
    }

    x->parent = y;

    if (y->parent == nullptr) {
        _root_ptr->left = y;
    }
};

template <typename K, typename V>
void RedBlackTree<K,V>::Node::Print() {
    
    cout << endl;
    if (this->parent == nullptr) {
        cout << "root ";
    }
    else {
        cout << "     ";
    }
    cout << this->key << " " << ToString(this->color);
    if (this->left != nullptr) {
        cout << " left:" << this->left->key << " " << ToString(this->left->color) << "     ";
    }
    if (this->right != nullptr) {
        cout << " right:" << this->right->key << " " << ToString(this->right->color) << "     ";;
    }
};

template <typename K, typename V>
shared_ptr<typename RedBlackTree<K, V>::Node> RedBlackTree<K, V>::Find( K key ) {

    shared_ptr<Node> x = GetRoot();
    shared_ptr<Node> y = nullptr;
    while ( x != nullptr ) {
        y = x;
        if ( key == x->key ) {
            return x;
        } else  if ( key < x->key ) {
            x = x->left;
        } else {
            x = x->right;
        }
    }
    return nullptr;
};

template <typename K, typename V>
void RedBlackTree<K, V>::RBTransplant( shared_ptr<RedBlackTree<K, V>::Node> u, shared_ptr<RedBlackTree<K, V>::Node> v ) {

    if ( v == nullptr || u == nullptr ) {
        return;
    }
    
    if ( u->parent == nullptr ) {
        _root_ptr->left = v;
        
    } else if ( u == u->parent->right ) {
        u->parent->right = v;
    } else {
        u->parent->left = v;
    }

    v->parent = u->parent;

};

template <typename K, typename V>
void RedBlackTree<K, V>::detect_and_remove_cyclic_links( shared_ptr<Node> v ) {
    
    // найти и удалить ссылку left или right, которая ссылается на V
    vector<shared_ptr<RedBlackTree<K, V>::Node>> arr = { v->right, v->left };
    for (size_t i = 0; i < arr.size(); i++) {

        auto tmp = arr.at(i);
        while (tmp != nullptr) {
            if (tmp->left == v) {
                tmp->left = nullptr;
                break;
            }
            if ( tmp->right == v ) {
                tmp->right = nullptr;
                break;
            }
            if (v->key < tmp->key) {
                tmp = tmp->left;
            } else {
                tmp = tmp->right;
            }
        }
    }
}

template <typename K, typename V>
void RedBlackTree<K, V>::RBDelete( K key ) {
    
    auto z = Find( key );
    if ( z == nullptr ) {
        return;
    }

    // случай, когда в дереве один Node и это root, и он же удаляется
    if ( _root_ptr->left == z && z->left == nullptr && z->right == nullptr ) {
        _root_ptr->left = nullptr;
        return;
    }

    // случай, когда удаляется лист (у Кормена не присутствует)
    if (z->left == nullptr && z->right == nullptr) {
        if ( z == z->parent->left ) {
            z->parent->left = nullptr;
        } else {
            z->parent->right = nullptr;
        }
        return;
    }

    auto y = z;
    auto y_original_color = y->color;
    shared_ptr<Node> x;
    if ( z->left == nullptr ) {
        x = z->right;
        RBTransplant( z, z->right );
    } else if ( z->right == nullptr ) {
        x = z->left;
        RBTransplant( z, z->left );
    } else {
        y = GetTreeMinimum( z->right );
        y_original_color = y->color;
        x = y->right;
        if ( y->parent == z ) {
            if ( x != nullptr ) { x->parent = y; }
        } else {
            RBTransplant( y, y->right );
            if ( y != nullptr && z != nullptr ) { y->right = z->right; detect_and_remove_cyclic_links(y); } // здесь образуется циклическая ссылка
            if ( y->right != nullptr ) { y->right->parent = y; }
        }
        RBTransplant( z, y );
        if ( y != nullptr && z != nullptr ) { y->left = z->left; }
        if ( y != nullptr && y->left != nullptr ) { y->left->parent = y; }
        if ( y != nullptr && z != nullptr ) { y->color = z->color; }
    }
    if ( y_original_color == Node::Color::BLACK ) {
        RBDeleteFixup( x );
    }
};

template <typename K, typename V>
void RedBlackTree<K,V>::RBDeleteFixup( shared_ptr<Node> x ) {

    shared_ptr<Node> w = nullptr;
    bool stop = false;
    while ( x != nullptr && !stop && x->color == Node::Color::BLACK ) {
        
        if ( x->parent != nullptr && x == x->parent->left ) {
            
            w = x->parent->right;
            
            if ( w != nullptr && w->color == Node::Color::RED ) {
                w->color = Node::Color::BLACK;
                x->parent->color = Node::Color::RED;
                LeftRotate( x->parent );
                w = x->parent->right;
            }

            if ( w != nullptr && w->left != nullptr && w->left->color == Node::Color::BLACK && w->right != nullptr && w->right->color == Node::Color::BLACK ) {
                
                w->color = Node::Color::RED;
                x = x->parent;

            } else {
                if ( w != nullptr && w->right != nullptr && w->right->color == Node::Color::BLACK ) {
                    if ( w->left != nullptr ) { w->left->color = Node::Color::BLACK; }
                    w->color = Node::Color::RED;
                    RightRotate( w );
                    if ( x->parent != nullptr ) { w = x->parent->right; }
                }
                if ( w != nullptr && x != nullptr && x->parent != nullptr ) { w->color = x->parent->color; }
                if ( x != nullptr && x->parent != nullptr ) { x->parent->color = Node::Color::BLACK; }
                if ( w != nullptr && w->right != nullptr ) { w->right->color = Node::Color::BLACK; }
                LeftRotate( x->parent );
                stop = true;
            }

        } else {
            
            if ( x->parent != nullptr ) { w = x->parent->left; }
            
            if ( w != nullptr && w->color == Node::Color::RED && x->parent != nullptr ) {
                w->color = Node::Color::BLACK;
                x->parent->color = Node::Color::RED;
                RightRotate( x->parent );
                w = x->parent->left;
            }

            if ( w != nullptr && w->right != nullptr && w->right->color == Node::Color::BLACK && w->left != nullptr && w->left->color == Node::Color::BLACK ) {
                
                w->color = Node::Color::RED;
                if ( x != nullptr ) { x = x->parent; }

            } else {

                if ( w != nullptr && w->left != nullptr && w->left->color == Node::Color::BLACK ) {
                    if ( w->right != nullptr ) { w->right->color = Node::Color::BLACK; }
                    w->color = Node::Color::RED;
                    LeftRotate( w );
                    if ( x->parent != nullptr ) { w = x->parent->left; }
                }
                if ( w != nullptr && x->parent != nullptr ) { w->color = x->parent->color; }
                if ( x->parent != nullptr ) { x->parent->color = Node::Color::BLACK; }
                if ( w != nullptr && w->right != nullptr ) { w->right->color = Node::Color::BLACK; }
                RightRotate( x->parent );
                stop = true;
            }
        }
    }
    if ( x != nullptr ) { x->color = Node::Color::BLACK; }
};

/// <summary>
/// Generates a random number
/// </summary>
/// <param name="start">: inclusive lower bound</param>
/// <param name="end">: inclusive upper bound</param>
/// <returns>A random number within a given range</returns>
int GenerateRandomNumber( const int& start, const int& end ) {
	random_device dev;
	mt19937 rng( dev() );
	uniform_int_distribution<int> dist( start, end );
	auto res = dist( rng );
	return res;
};
