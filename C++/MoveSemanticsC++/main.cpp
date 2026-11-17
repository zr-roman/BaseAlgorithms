
#include <iostream>
#include "Header.h"
#include <crtdbg.h>

void main_program()
{
    //cout << sizeof( registry ) << endl;

    auto i4 = new Person( "Nikolaev", nullptr );
    auto i3 = new Person( "Ivanov", i4 );
    auto i2 = new Person( "Petrov", i3 );
    auto i1 = new Person( "Sidorov", i2 );

    // вызывается move .ctor 
    // Move constructor вызывается в случае, когда объект, из которого создается копия, вот-вот будет уничтожен
    // Чтобы явно показать, что временный объект мы больше использовать не будем, предусмотрен std:move
    Person A = move_if_noexcept( *(new Person( "Voronov", nullptr )) );
    //delete i1;
    
    Person B;
    // вызывается move assignment operator
    // аналогично move .ctor
    B = move( *i2 );
    //delete i2;

    // вызывается copy .ctor
    Person* C = new Person( *i3 );

    // вызывается copy assignment operator
    Person D;
    D = *i4;
    
    Person Q;
    Person W;
    Person S;

    // std::forward Perfect forwarding
    //Person E = Q + W + S; // does not compile!

    /*
    i1->ChangeName("S1");
    i2->ChangeName("P1");*/
    i3->ChangeName("I1");
    
    std::cout << "Hello World!\n";
            
    delete i4;
    delete i3;
    delete i2;
    delete i1;
    delete C;

    Person::ClearRegistry();
}

int main() {

    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);

    int n = 10;

    while ( n-- > 0 ) {
        main_program();
    }
    
    _CrtDumpMemoryLeaks();
}
