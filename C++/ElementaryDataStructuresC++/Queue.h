#pragma once

#include<vector>
#include<array>
#include<stack>

using namespace std;

/// <summary>
/// //////////////////////////////////////// Queue via vector /////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Queue_vector {
public:

	void Enqueue( T val ) {
		ShrinkArr();
		arr.push_back( val );
		if ( head == -1 ) {
			head++;
		}
		tail++;
	};

	T Dequeue() {
		if ( head == -1 ) {
			throw runtime_error("Queue is empty");
		}
		auto tmp = arr.at( head );
		head++;
		ShrinkArr();
		return tmp;
	};

	inline T& Peek() const {
		if ( head == -1 ) {
			throw runtime_error("Queue is empty");
		}
		return arr.at( head );
	};

	Queue_vector() {
		head = -1;
		tail = -1;
	};

private:
	vector<T> arr;
	int head;
	int tail;
	void ShrinkArr() {
		if ( head == arr.size()/2 ) {

			for ( size_t i = head, j = 0; i < arr.size(); i++, j++ ) {
				arr[ j ] = arr[ i ];
			}
			arr.resize( tail - head + 1 );
			arr.shrink_to_fit();
			head = 0;
			tail = arr.size() - 1;
		}
	};
};

/// <summary>
/// ///////////////////////////////////////// Queue via an array ///////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Queue_array {
public:

	void Enqueue( T val ) {
		if ( tail + 1 == head ) {
			throw runtime_error("Exceeded queue size limit");
		}
		if ( head == -1 ) {
			head++;
		}
		if ( tail < int(arr.size() - 1) ) {
			tail++;
		} else {
			tail = 0;
			if ( tail == head ) {
				throw runtime_error("Exceeded queue size limit");
			}
		}
		
		arr[ tail ] = val;
	};

	T Dequeue() {
		if ( head == -1 ) {
			throw runtime_error("Queue is empty");
		}
		auto tmp = arr[ head ];
		if ( head == tail ) {
			head = tail = 0;
		}
		head++;
		return tmp;
	};

	inline T& Peek() const {
		if ( head == -1 ) {
			throw runtime_error("Queue is empty");
		}
		return arr[ head ];
	};

	Queue_array() {
		head = -1;
		tail = -1;
	};

private:
	static const size_t N = 10;
	array<T, N> arr;
	int head;
	int tail;
};

/// <summary>
/// //////////////////////////////////////////// Queue via a list //////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class Queue_list {
public:
	void Enqueue( T val ) {
		auto new_node = make_shared<Node>( val, nullptr );
		if ( head_root == nullptr && tail == nullptr ) {
			head_root = new_node;
			tail = new_node;
			return;
		}
		tail->next = new_node;
		tail = new_node;
	};

	T Dequeue() {
		
		if ( head_root == nullptr ) {
			throw runtime_error("Queue is empty");
		}

		auto tmp = head_root->val;

		if ( head_root == tail ) {
			head_root = nullptr;
			tail == nullptr;
		} else {
			head_root = head_root->next;
		}
		
		return tmp;
	};

	T& Peek() const {
		if ( head_root == nullptr ) {
			throw runtime_error("Queue is empty");
		}
		return head_root->val;
	};

	Queue_list() {
		head_root = nullptr;
		tail = nullptr;
	};

private:
	class Node {
	public:
		shared_ptr<Node> next;
		T val;
		Node( const T& val, shared_ptr<Node> next ) {
			this->val = val;
			this->next = next;
		};
	};

	shared_ptr<Node> head_root;
	shared_ptr<Node> tail;
};

//////////////////////////////////////////// Queue via two stacks //////////////////////////////////////////

template <typename T>
class Queue_two_stacks {
public:
	void Enqueue( const T& val ) {

		// перелить из s2 все что есть в s1
		while ( !s2.empty() ) {
			s1.push( s2.top() );
			s2.pop();
		}

		// записать новое значение
		s1.push( val );
	};

	T Dequeue() {
		// перелить из s1 все что есть в s2
		while ( !s1.empty() ) {
			s2.push( s1.top() );
			s1.pop();
		}

		// вернуть значение
		auto res = s2.top();
		s2.pop();
		return res;
	};

	T& Peek() {
		// перелить из s1 все что есть в s2
		while ( !s1.empty() ) {
			s2.push( s1.top() );
			s1.pop();
		}
		return s2.top;
	};

private:
	stack<T> s1;
	stack<T> s2;
};
