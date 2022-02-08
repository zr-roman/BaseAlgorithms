#include <type_traits>
#include <vector>
#include <string>
#include <utility>
#include <memory>

using namespace std;

template <typename T>
class Iterator { // GoF Iterator
public:
	virtual void First() = 0;
	virtual void Next() = 0;
	virtual bool IsDone() const = 0;
	virtual T CurrentItem() const = 0;
protected:
	Iterator() { };
};

/// <summary>
/// ////////////////////////////////////////// LIST via Three Arrays //////////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class List_three_arrays {
public:

	void Insert( const T& val ) {

		if ( keys.size() == 0) {
			head = 0;
			tail = -1;
		}
			
		size_t index;
			
		if ( emptys.size() > 0 ) {

			// insert into first empty slot
			index = emptys[ 0 ];
			emptys.erase( emptys.begin() );
				
		} else {

			// insert into end
			index = keys.size();
			_enlargeVectorsByOne();
		}

		keys [ index ] = val;
		prevs[ index ] = tail;
		nexts[ index ] = -1;
		if ( tail != -1 ) {
			nexts[ tail ] = index;
		}
		tail = index;
	};

	string Delete( const T& val ) {
		
		auto r = _search( val );
		auto index = r.first;
		if ( index == -1 ) {
			return "Not found";
		}

		// x.prev.next = x.next
		if ( prevs[ index ] != -1 ) {
			nexts[ prevs[ index ] ] = nexts[ index ];
		}

		// x.next.prev = x.prev
		if ( nexts[ index ] != -1 ) {
			prevs[ nexts[ index ] ] = prevs[ index ];
		}

		// move head by 1 if head is the one which is deleted
		if ( prevs[ index ] == -1 ) {
			head = nexts[ index ];
		}

		if ( index == keys.size() - 1 ) {
			keys.erase( keys.cbegin() + index );
			nexts.erase( nexts.cbegin() + index );
			prevs.erase( prevs.cbegin() + index );
		} else {
			emptys.push_back( index );
		}

		return "Deleted successfully";
	};
		
	pair<string, T> Search( const T& val ) {
		auto res = _search( val );
		string f = "Found!";
		if ( res.first == -1 ) {
			f = "Not " + f;
		}
		return make_pair<string, T>( (string)f, move( res.second ) );
	};

	List_three_arrays() {
		head = -1;
		tail = -1;
	};

	class iterator : public Iterator<T> { // GoF Iterator
	public:
		iterator( const shared_ptr<List_three_arrays<T>>& list_ptr ) {
			this->list_ptr = list_ptr;
			current = -1;
		};

		virtual void First() {
			current = list_ptr->head;
		};

		virtual void Next() {
			current = list_ptr->nexts[ current ];
		};

		virtual bool IsDone() const {
			return current == -1;
		};

		virtual T CurrentItem() const {
			return list_ptr->keys[ current ];
		};
	private:
		shared_ptr<List_three_arrays<T>> list_ptr;
		int current;
	};

private:

	vector<int> prevs;
	vector<T> keys;
	vector<int> nexts;
	vector<size_t> emptys;

	size_t head;
	size_t tail;
		
	pair<int, T> _search( const T& val ) {
		if ( head == -1 ) {
			return make_pair(-1, val );
		}
		auto current_index = head;
		while ( current_index != -1 ) {
			auto target = keys[ current_index ];
			if ( target == val ) {
				return make_pair( current_index, target );
			}
			current_index = nexts[ current_index ];
		}
		return make_pair(-1, val );
	};

	void _enlargeVectorsByOne() {
		keys.push_back ( -1 );
		prevs.push_back( -1 );
		nexts.push_back( -1 );
	};
};

/// <summary>
/// //////////////////////////////////////////// LIST via One Array, raw pointers ///////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class List_one_array {
public:
	
	void Insert( const T& val ) {
		
		if ( arr.size() == 0 ) {
			head = 0;
			tail = -1;
		}

		size_t index;

		if ( emptys.size() > 0 ) {
			index = emptys[ 0 ];
			emptys.erase( emptys.cbegin() );
		} else {
			index = arr.size();
			_enlargeArr();
		}

		arr[ index ] = new T( val );
		arr[ index + 1 ] = nullptr;
		arr[ index + 2 ] = tail == -1 ? nullptr : new size_t( tail );
		if ( tail != -1 ) {
			if ( arr[ tail + 1 ] ) {
				delete arr[ tail + 1 ];
			}
			arr[  tail + 1 ] = new size_t( index );
		}

		tail = index;
	};

	string Delete( const T& val ) {

		auto r = _search( val );
		int index = r.first;
		if ( index == -1 ) {
			return "Not found";
		}

		// x.prev.next = x.next
		if ( arr[ index + 2 ] != nullptr ) {
			auto i = *((size_t*)(arr[ index + 2 ])) + 1;
			if ( arr[ i ] ) {
				delete arr[ i ];
			}
			arr[ i ] = index == tail ? nullptr : new size_t( *((size_t*)arr[ index + 1 ]) );
		}

		// x.next.prev = x.prev
		if ( arr[ index + 1 ] != nullptr ) {
			auto i = *((size_t*)(arr[ index + 1 ])) + 2;
			if ( arr[ i ] ) {
				delete arr[ i ];
			}
			arr[ i ] = index == head ? arr[ i ] = nullptr : new size_t( *((size_t*)arr[ index + 2 ]) );
		}

		// move head by 1 ahead if head is the one which is deleted
		if ( arr [ index + 2 ] == nullptr ) {
			head = *((size_t*)arr[ index + 1 ]);
			arr [ head + 2 ] = nullptr;
		}

		_deallocateMemoryForDeletedElement( index );

		if ( index == arr.size() - 3 ) {
			for ( size_t i = 0; i < 3; i++ ) {
				arr.erase( arr.cend() - 1 );
			}
		} else {
			emptys.push_back( index );
		}
		
		return "Deleted successfully";
	};

	pair<string, T> Search( const T& val ) {
		auto res = _search( val );
		string f = "Found!";
		if ( res.first == -1 ) {
			f = "Not " + f;
		}
		return make_pair<string, T>( (string)f, move( res.second ) );
	};

	List_one_array() {
		head = -1;
		tail = -1;
	};

	~List_one_array() {
		for ( size_t i = 0; i < arr.size(); i++ ) {
			if ( arr[ i ] ) {
				delete arr[ i ];
				arr[ i ] = nullptr;
			}
		}
		arr.clear();
		arr.resize(0);
		arr.shrink_to_fit();
	};

	class iterator : public Iterator<T> { // GoF Iterator
	public:
		iterator( const List_one_array<T>& list ) : list(list) {
			current = nullptr;
		};

		virtual void First() {
			current = (int*)&(list.head);
		};

		virtual void Next() {
			auto next_ind = *((size_t*)current) + 1;
			current = (int*)list.arr[ next_ind ];
		};

		virtual bool IsDone() const {
			return current == nullptr;
		};

		virtual T CurrentItem() const {
			return *((T*)(list.arr[ *((size_t*)current) ]));
		};
	private:
		const List_one_array<T>& list;
		int* current;
	};

private:
	vector<void*> arr;
	int head;
	int tail;
	vector<size_t> emptys;

	void _enlargeArr() {
		arr.push_back( nullptr );
		arr.push_back( nullptr );
		arr.push_back( nullptr );
	};

	void _deallocateMemoryForDeletedElement( size_t i ) {
		if ( arr[ i ] ) {
			delete arr[ i ];
			arr[ i ] = nullptr;
		}
		if ( arr[ i + 1 ] ) {
			delete arr[ i + 1 ];
			arr[ i + 1 ] = nullptr;
		}
		if ( arr[ i + 2 ] ) {
			delete arr[ i + 2 ];
			arr[ i + 2 ] = nullptr;
		}
	}

	pair<int, T> _search( const T& val ) {
		if ( head == -1 ) {
			return make_pair( -1, val );
		}
		auto current_index = head;
		while ( true ) {
			auto target = *((T*)(arr[ current_index ]));
			if ( target == val ) {
				return make_pair( current_index, target );
			}
			if ( arr[ current_index + 1 ] == nullptr ) {
				break;
			}
			current_index = *((size_t*)(arr[ current_index + 1 ]));
		}
		return make_pair( -1, val );
	};
	
};

/// <summary>
/// //////////////////////////////////////// LIST via one array, shared_ptr //////////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class List_one_array_sp {
public:

	void Insert( const T& val ) {
		
		if ( arr.size() == 0 ) {
			head = 0;
			tail = -1;
		}

		size_t index;

		if ( emptys.size() > 0 ) {
			index = emptys[ 0 ];
			emptys.erase( emptys.cbegin() );
		} else {
			index = arr.size();
			_enlargeArr();
		}

		arr[ index ] = make_shared<T>( val );
		arr[ index + 1 ] = nullptr;
		arr[ index + 2 ] = tail == -1 ? nullptr : make_shared<size_t>( tail );
		if ( tail != -1 ) {
			arr[  tail + 1 ] = make_shared<size_t>( index );
		}

		tail = index;
	};

	string Delete( const T& val ) {

		auto r = _search( val );
		int index = r.first;
		if ( index == -1 ) {
			return "Not found";
		}

		// x.prev.next = x.next
		if ( arr[ index + 2 ] != nullptr ) {
			auto i = *( (size_t*)( arr[ index + 2 ].get() ) ) + 1;
			arr[ i ] = index == tail ? nullptr : make_shared<size_t>( *((size_t*)( arr[ index + 1 ].get()) ) );
		}
		
		// x.next.prev = x.prev
		if ( arr[ index + 1 ] != nullptr ) {
			auto i = *( (size_t*)( arr[ index + 1 ].get() ) ) + 2;
			arr[ i ] = index == head ? arr[ i ] = nullptr : make_shared<size_t>( *((size_t*)(arr[ index + 2 ].get()) ) );
		}
		
		// move head by 1 to next if head is the one which is deleted
		if ( arr [ index + 2 ] == nullptr ) {
			head = *( (size_t*)( arr[ index + 1 ].get() ) );
			arr [ head + 2 ] = nullptr;
		}
		
		if ( index == arr.size() - 3 ) {
			for ( size_t i = 0; i < 3; i++ ) {
				arr.erase( arr.cend() - 1 );
			}
		} else {
			emptys.push_back( index );
		}
		
		return "Deleted successfully";
	};

	List_one_array_sp() {
		head = -1;
		tail = -1;
	};

	class iterator : public Iterator<T> { // GoF Iterator
	public:
		iterator( const List_one_array_sp<T>& list ) : list(list) {
			current = -1;
		};

		virtual void First() {
			current = list.head;
		};

		virtual void Next() {
			auto next_ind = current + 1;
			if ( list.arr[ next_ind ] == nullptr ) {
				current = -1;
			} else {
				current = *( (size_t*)( list.arr[ next_ind ].get() ) );
			}
		};

		virtual bool IsDone() const {
			return current == -1;
		};

		virtual T CurrentItem() const {
			return *( (T*)( list.arr[ current ].get() ) );
		};

	private:
		const List_one_array_sp<T>& list;
		int current;
	};

private:
	vector<shared_ptr<void>> arr;
	int head;
	int tail;
	vector<size_t> emptys;

	void _enlargeArr() {
		arr.push_back( nullptr );
		arr.push_back( nullptr );
		arr.push_back( nullptr );
	};

	pair<int, T> _search( const T& val ) {
		if ( head == -1 ) {
			return make_pair( -1, val );
		}
		auto current_index = head;
		while ( true ) {
			auto target = *((T*)((arr[ current_index ]).get())) ;
			if ( target == val ) {
				return make_pair( current_index, target );
			}
			if ( arr[ current_index + 1 ] == nullptr ) {
				break;
			}
			current_index =  *((size_t*)(arr[ current_index + 1 ].get())) ;
		}
		return make_pair( -1, val );
	};
};
