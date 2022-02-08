#pragma once

#include<vector>
#include<array>
#include<queue>

using namespace std;

/// <summary>
/// //////////////////////////////////////// Stack via a vector //////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Stack_vector {
public:
	void Push( const T& val ) {
		top++;
		arr.push_back( val );
	};
	
	T Pop() {
		if ( top == -1 ) {
			throw new runtime_error("Stack underflow");
		}
		return arr[ top-- ];
	};
	
	T Peek() const {
		if ( top == -1 ) {
			throw new runtime_error("Stack underflow");
		}
		return arr[ top ];
	};

	bool IsStackEmpty() const {
		return top == -1 ? true : false;
	};

	Stack_vector()  {
		top = -1;
	};

private:
	vector<T> arr;
	int top;
};

/// <summary>
/// ////////////////////////////////////////// Stack via an array /////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Stack_array {
public:
	void Push( const T& val ) {
		if ( top == arr.size() - 1 ) {
			throw runtime_error("Stack overflow");
		}
		arr[ ++top ] = val;
	};

	T Pop() {
		if ( top == -1 ) {
			throw runtime_error("Stack underflow");
		}
		return arr[ top-- ];
	};

	T Peek() const {
		if ( top == -1 ) {
			throw runtime_error("Stack underflow");
		}
		return arr[ top ];
	};

	bool IsStackEmpty() const {
		return top == -1 ? true : false;
	};

	Stack_array() {
		for ( size_t i = 0; i < N; i++ ) {
			arr[ i ] = 0;
		}
		top = -1;
	};

private:
	static const size_t N = 10;
	array<T, N> arr;
	int top = -1;
	
};

/// <summary>
/// //////////////////////////////////////// Stack via a list ////////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Stack_list {
public:
	inline void Push( T val ) {
		if ( root == nullptr ) {
			root = make_shared<Node>( val, nullptr );
			return;
		}
		auto new_node = make_shared<Node>(val, nullptr);
		auto tmp = root;
		new_node->next = root;
		root = new_node;
	};

	T Pop() {
		if (root == nullptr) {
			throw runtime_error("Stack underflow");
		}
		auto tmp = root->val;
		root = root->next;
		return tmp;
	};

	T& Peek() const {
		if ( root == nullptr ) {
			throw runtime_error("Stack underflow");
		}
		return root->val;
	};

	inline bool IsStackEmpty() const {
		return root == nullptr ? true : false;
	};

	Stack_list() {
		root = nullptr;
	};

private:
	class Node {
	public:
		
		explicit Node(const T& val, shared_ptr<Node> next) {
			this->val = val;
			this->next = next;
		};
		~Node() {
			cout << "Destroy: " << val << endl;
		};
	
		shared_ptr<Node> next;
		T val;
	};

	shared_ptr<Node> root = nullptr;
};

/// <summary>
/// /////////////////////////////////// Stack via two queues ///////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Stack_two_queues {
public:

	void Push( const T& val ) {

		shared_ptr<queue<T>> q_from;
		shared_ptr<queue<T>> q_to;

		if ( q1->size() < q2->size() ) {
			q_from = q1;
			q_to = q2;
		} else if( q2->size() < q1->size() ) {
			q_from = q2;
			q_to = q1;
		} else if( q1->empty() && q2->empty() ) {
			q1->push( val );
			return;
		} else {
			throw runtime_error("Must not hit into here!");	
		}

		while ( q_from->size() > 0 ) {
			q_to->push( q_from->front() );
			q_from->pop();
		}

		q_to->push( val );

	};

	T Pop() {

		if ( q1->size() == 1 ) {
			auto tmp = q1->front();
			q1->pop();
			return tmp;
		}
		if ( q2->size() == 1 ) {
			auto tmp = q2->front();
			q2->pop();
			return tmp;
		}

		shared_ptr<queue<T>> q_from;
		shared_ptr<queue<T>> q_to;
		if ( q1->size() < q2->size() ) {
			q_from = q2;
			q_to = q1;
		} else if( q2->size() < q1->size() ) {
			q_from = q1;
			q_to = q2;
		} else if( q1->empty() && q2->empty() ) {
			throw runtime_error("Stack underflow");
		} else {
			throw runtime_error("Must not hit into here!");	
		}

		while ( q_from->size() > 1 ) {
			q_to->push( q_from->front() );
			q_from->pop();
		}
		
		auto res = q_from->front();
		q_from->pop();
		return res;
	};

	T& Peek() {

		if ( q1->empty() && q2->empty() ) {
			throw runtime_error("Stack underflow");
		}

		if ( q1->size() == 1 ) {
			return q1->front();
		}
		if ( q2->size() == 1 ) {
			return q2->front();
		}

		shared_ptr<queue<T>> q_from;
		shared_ptr<queue<T>> q_to;
		if ( q1->size() < q2->size() ) {
			q_from = q2;
			q_to = q1;
		} else if( q2->size() < q1->size() ) {
			q_from = q1;
			q_to = q2;
		} else {
			throw runtime_error("Must not hit into here!");
		}

		while ( q_from->size() > 1 ) {
			q_to->push( q_from->front() );
			q_from->pop();
		}
		return q_from->front();
	};

	Stack_two_queues() {
		q1 = make_shared<queue<T>>();
		q2 = make_shared<queue<T>>();
	};

private:
	shared_ptr<queue<T>> q1;
	shared_ptr<queue<T>> q2;
};
