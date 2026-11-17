#include <iostream>
#include <memory>
#include "Stack.h"
#include "Queue.h"
#include "Lists.h"

using namespace std;

int MainProgram() {    

    // Stack via two queues
    auto s = make_unique<Stack_two_queues<int>>();
    s->Push(1);
    s->Push(2);
    s->Push(3);
    s->Push(4);
    s->Push(5);

    cout << s->Pop() << endl;
    cout << s->Pop() << endl;

    cout << s->Peek() << endl;
    cout << s->Peek() << endl;
    cout << s->Peek() << endl;

    cout << s->Pop() << endl;
    cout << s->Pop() << endl;

    s->Push(6);
    s->Push(7);
    s->Push(8);
    s->Push(9);
    s->Push(10);

    cout << s->Pop() << endl;
    cout << s->Pop() << endl;
    cout << s->Pop() << endl;

    // Queue via two stacks
    /*auto q = make_unique<Queue_two_stacks<int>>();
    q->Enqueue(1);
    q->Enqueue(2);
    q->Enqueue(3);
    q->Enqueue(4);
    q->Enqueue(5);

    cout << q->Dequeue() << endl;
    q->Enqueue(6);
    cout << q->Dequeue() << endl;
    q->Enqueue(7);
    cout << q->Dequeue() << endl;
    q->Enqueue(8);
    cout << q->Dequeue() << endl;
    cout << q->Dequeue() << endl;
    cout << q->Dequeue() << endl;
    cout << q->Dequeue() << endl;
    cout << q->Dequeue() << endl;*/

    // List via one array, shared ptr
   //auto list = make_shared<List_one_array_sp<string>>();

   // cout << "121: " << list->Delete("a") << endl;

   // list->Insert("a");
   // list->Insert("b");
   // list->Insert("c");
   // list->Insert("d");
   // list->Insert("e");

   // //list->Delete("c");

   // list->Insert("f");
   // list->Insert("g");
   // list->Insert("h");
   // list->Insert("i");
   // list->Insert("j");

   // auto it = make_unique<List_one_array_sp<string>::iterator>( *(list.get()) );
   // 
   // int n = 4;
   // while ( n-- > 0 ) {
   //     
   //     for ( it->First(); !it->IsDone(); it->Next() ) {
   //         cout << it->CurrentItem() << endl;
   //     }

   //     if (n == 3) {
   //         cout << "d: " << list->Delete("d") << endl;
   //         cout << "f: " << list->Delete("f") << endl;
   //     }
   //     
   //     if ( n == 2 ) {
   //         list->Insert("aa");
   //         list->Insert("bb");
   //         list->Insert("cc");
   //         list->Insert("dd");
   //         list->Insert("ee");
   //     }

   //     if ( n == 1 ) {
   //         cout << "f: " << list->Delete("a") << endl;
   //         cout << "ee: " << list->Delete("ee") << endl;
   //         cout << "h: "  << list->Delete("h") << endl;
   //         cout << "bb: " << list->Delete("bb") << endl;
   //     }
   //     
   //     cout << endl;
   // }

    // List via one array, raw pointers
    //auto list = make_shared<List_one_array<int>>();

    //cout << "121: " << list->Delete(121) << endl;

    //list->Insert(1);
    //list->Insert(2);
    //list->Insert(3);
    //list->Insert(4);
    //list->Insert(5);
    //list->Insert(6);
    //list->Insert(7);
    //list->Insert(8);
    //list->Insert(9);
    //list->Insert(10);
    //
    //auto it = make_unique<List_one_array<int>::iterator>( *(list.get()) );
    //
    //int n = 4;
    //while ( n-- > 0 ) {
    //    
    //    for ( it->First(); !it->IsDone(); it->Next() ) {
    //        cout << it->CurrentItem() << endl;
    //    }

    //    if (n == 3) {
    //        cout << "4: " << list->Delete(4) << endl;
    //        cout << "6: " << list->Delete(6) << endl;
    //    }
    //    
    //    if ( n == 2 ) {
    //        list->Insert(11);
    //        list->Insert(12);
    //        list->Insert(13);
    //        list->Insert(14);
    //        list->Insert(15);
    //    }

    //    if ( n == 1 ) {
    //        cout << "1: " << list->Delete(1) << endl;
    //        cout << "15: " << list->Delete(15) << endl;
    //        cout << "8: "  << list->Delete(8) << endl;
    //        cout << "12: " << list->Delete(12) << endl;
    //    }
    //    
    //    cout << endl;
    //}

    //List via three arrays

    /*auto list = make_shared<List_three_arrays<int>>();
    list->Insert(1);
    list->Insert(2);
    list->Insert(3);
    list->Insert(4);
    list->Insert(5);
    list->Insert(6);
    list->Insert(7);
    list->Insert(8);
    list->Insert(9);
    list->Insert(10);

    int n = 4;
    while ( n-- > 0 ) {
        auto it = make_unique<List_three_arrays<int>::iterator>( list );
        for ( it->First(); !it->IsDone(); it->Next() ) {
            cout << it->CurrentItem() << endl;
        }

        if (n == 3) {
            list->Delete(4);
            cout << "Deleted 4" << endl;
            list->Delete(6);
            cout << "Deleted 6" << endl;
        }
        
        if ( n == 2 ) {
            list->Insert(11);
            list->Insert(12);
            list->Insert(13);
            list->Insert(14);
            list->Insert(15);
        }

        if ( n == 1 ) {
            list->Delete(1);
            cout << "Deleted 1" << endl;
            list->Delete(8);
            cout << "Deleted 8" << endl;
            list->Delete(12);
            cout << "Deleted 12" << endl;
        }
        
        cout << endl;
    }

    cout << "18: " << list->Search(18).first << endl;
    cout << "8: " << list->Search(8).first << endl;
    cout << "14: " << list->Search(14).first << endl;*/
    

    // Queue via list
    /*auto q3 = make_shared<Queue_list<int>>();
    q3->Enqueue(10);
    q3->Enqueue(9);
    q3->Enqueue(8);
    q3->Enqueue(7);
    q3->Enqueue(6);
    q3->Enqueue(5);
    q3->Enqueue(4);
    q3->Enqueue(3);
    q3->Enqueue(2);
    q3->Enqueue(1);

    cout << q3->Dequeue() << endl;
    cout << q3->Dequeue() << endl;
    cout << q3->Dequeue() << endl;
    cout << q3->Dequeue() << endl;*/

    //Queue via array
    //auto q2 = make_shared<Queue_array<int>>();

    //q2->Enqueue(10);
    //q2->Enqueue(9);
    //q2->Enqueue(8);
    //q2->Enqueue(7);
    //q2->Enqueue(6);

    //cout << q2->Dequeue() << endl;
    //cout << q2->Dequeue() << endl;
    //cout << q2->Dequeue() << endl;
    //cout << q2->Dequeue() << endl;

    //q2->Enqueue(5);
    //q2->Enqueue(4);
    //q2->Enqueue(3);
    //q2->Enqueue(2);
    //q2->Enqueue(1);
    //q2->Enqueue(0);
    //q2->Enqueue(-1);
    //q2->Enqueue(-2);

    // Queue via vector
    //auto q1 = make_shared<Queue_vector<int>>();

    //q1->Enqueue(10);
    //q1->Enqueue(9);
    //q1->Enqueue(8);
    //q1->Enqueue(7);
    //q1->Enqueue(6);
    //q1->Enqueue(5);
    //q1->Enqueue(4);
    //q1->Enqueue(3);
    //q1->Enqueue(2);
    //q1->Enqueue(1);

    //cout << q1->Dequeue() << endl;
    //cout << q1->Dequeue() << endl;
    //cout << q1->Dequeue() << endl;
    //cout << q1->Dequeue() << endl;
    //cout << q1->Dequeue() << endl;
    //cout << q1->Dequeue() << endl;

    //q1->Enqueue(20);
    //q1->Enqueue(30);
    //q1->Enqueue(40);
    //q1->Enqueue(50);
    //q1->Enqueue(60);

    // Stack
    //auto s1 = make_shared<Stack_vector<int>>();

    //auto s2 = make_shared<Stack_array<int>>();

    //s1->Push(1);
    //s1->Push(2);
    //s1->Push(3);
    //s1->Push(4);

    //cout << s1->Peek() << endl;
    //cout << s1->Peek() << endl;
    //cout << s1->Pop() << endl;
    //cout << s1->Peek() << endl;

    //s2->Push(0);
    //s2->Push(1);
    //s2->Push(2);
    //s2->Push(3);
    //s2->Push(4);
    //s2->Push(5);
    //s2->Push(6);
    //s2->Push(7);
    //s2->Push(8);
    //s2->Push(9);
 
    //cout << s2->Peek() << endl;
    //cout << s2->Peek() << endl;
    //cout << s2->Pop() << endl;
    //cout << s2->Peek() << endl;
    //
    //auto s3 = make_shared<Stack_list<int>>();

    //s3->Push(0);
    //s3->Push(1);
    //s3->Push(2);
    //s3->Push(3);
    //s3->Push(4);
    //s3->Push(5);
    //s3->Push(6);
    //s3->Push(7);
    //s3->Push(8);
    //s3->Push(9);

    //cout << endl;
    //cout << s3->Peek() << endl;
    //cout << s3->Pop() << endl;
    //cout << s3->Pop() << endl;
    //cout << s3->Pop() << endl;
    //cout << s3->Pop() << endl;

    //s3->Push(10);
    //s3->Push(11);
    //s3->Push(12);
    //s3->Push(13);

    //cout << s3->Pop() << endl;
    //cout << s3->Pop() << endl;
    //cout << s3->Pop() << endl;


    return 0;
}

int main() {
        
    _CrtSetDbgFlag( _CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF );

    MainProgram();

    _CrtDumpMemoryLeaks();
}
