#include <iostream>
#include <list>
#include <vector>
#include <memory>
#include <unordered_set>
#include "Header.h"
#include <optional>
#include "Dijkstra.h"
#include "Kruskal_MST.h"
#include "Prim_MST.h"
#include "BFS.h"
#include "DFS.h"

using namespace std;

int main() {

   // Kruskal

 //   auto p0 = make_shared<Person>("Ivanov_a", "assasdsdf", 0);
 //   auto p1 = make_shared<Person>("Ivanov_b", "assasdsdf", 1);
 //   auto p2 = make_shared<Person>("Ivanov_c", "assasdsdf", 2);
 //   auto p3 = make_shared<Person>("Ivanov_d", "assasdsdf", 3);
 //   auto p4 = make_shared<Person>("Ivanov_e", "assasdsdf", 4);
 //   auto p5 = make_shared<Person>("Ivanov_f", "assasdsdf", 5);
 //   auto p6 = make_shared<Person>("Ivanov_g", "assasdsdf", 6);
 //   auto p7 = make_shared<Person>("Ivanov_h", "assasdsdf", 7);
 //   auto p8 = make_shared<Person>("Ivanov_i", "assasdsdf", 8);    
 //   
 //   vector<Person_ptr> vertices = { p0, p1, p2, p3, p4, p5, p6, p7, p8 };

 //         // adjacency matrix // в матрице хранятся веса
 //   vector<vector<size_t>> adj_matrix = {
 //        // 0  1  2  3  4  5  6  7  8 
 //        // a  b  c  d  e  f  g  h  i
 ///* 0 a*/ { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
 ///* 1 b*/ { 4, 0, 8, 0, 0, 0, 0,11, 0 },
 ///* 2 c*/ { 0, 8, 0, 7, 0, 4, 0, 0, 2 },
 ///* 3 d*/ { 0, 0, 7, 0, 9,14, 0, 0, 0 },
 ///* 4 e*/ { 0, 0, 0, 9, 0,10, 0, 0, 0 },
 ///* 5 f*/ { 0, 0, 4,14,10, 0, 2, 0, 0 },
 ///* 6 g*/ { 0, 0, 0, 0, 0, 2, 0, 1, 6 },
 ///* 7 h*/ { 8,11, 0, 0, 0, 0, 1, 0, 7 },
 ///* 8 i*/ { 0, 0, 2, 0, 0, 0, 6, 7, 0 }
 //   };

 //   auto res = KruskalMST( adj_matrix, vertices );

 //   PrimMST( adj_matrix, vertices, p0 );

 //   cout << endl;

 //   // Dijkstra
 //   auto p0 = make_shared<Person>("Ivanov_s", "assasdsdf", 0);
 //   auto p1 = make_shared<Person>("Ivanov_t", "assasdsdf", 1);
 //   auto p2 = make_shared<Person>("Ivanov_y", "assasdsdf", 2);
 //   auto p3 = make_shared<Person>("Ivanov_x", "assasdsdf", 3);
 //   auto p4 = make_shared<Person>("Ivanov_z", "assasdsdf", 4);
 //   
 //   vector<Person_ptr> vertices = { p0, p1, p2, p3, p4 };

 //   int M = INT_MAX;

 //     // adjacency matrix // в матрице хранятся веса
 //   vector<vector<int>> adj_matrix = {
 //        // 0  1  2  3  4
 //        // s  t  y  x  z
 ///* 0 s*/ { M, 10,5, M, M },
 ///* 1 t*/ { M, M, 2, 1, M },
 ///* 2 y*/ { M, 3, M, 9, 2 },
 ///* 3 x*/ { M, M, M, M, 4 },
 ///* 4 z*/ { 7, M, M, 6, M },
 //   };

 //   Dijkstra( adj_matrix, vertices, p0 );

 //    cout << endl;

 //   // BellmanFord
 //   auto p0 = make_shared<Person>("Ivanov_s", "assasdsdf", 0);
 //   auto p1 = make_shared<Person>("Ivanov_t", "assasdsdf", 1);
 //   auto p2 = make_shared<Person>("Ivanov_y", "assasdsdf", 2);
 //   auto p3 = make_shared<Person>("Ivanov_x", "assasdsdf", 3);
 //   auto p4 = make_shared<Person>("Ivanov_z", "assasdsdf", 4);
 //   
 //   vector<Person_ptr> vertices = { p0, p1, p2, p3, p4 };

 //   int M = INT_MAX;

 //     // adjacency matrix // в матрице хранятся веса
 //   vector<vector<int>> adj_matrix = {
 //        // 0  1  2  3  4
 //        // s  t  y  x  z
 ///* 0 s*/ { M, 6, 7, M, M },
 ///* 1 t*/ { M, M, 8, 5,-4 },
 ///* 2 y*/ { M, M, M,-3, 9 },
 ///* 3 x*/ { M,-2, M, M, M },
 ///* 4 z*/ { 2, M, M, 7, M },
 //   };

 //   
 //   cout << BellmanFord(adj_matrix, vertices, p0) << endl;
 //   for ( auto& v : vertices ) {
 //       cout << v->Name << " " << v->d << " " << (v->pi != nullptr ? v->pi->Name : "NIL") << endl;
 //   }
 //   cout << N;

     //Directed unweighted graph
    auto p0 = make_shared<Person>("Ivanov0", "assasdsdf", 0);
    auto p1 = make_shared<Person>("Ivanov1", "assasdsdf", 1);
    auto p2 = make_shared<Person>("Ivanov2", "assasdsdf", 2);
    auto p3 = make_shared<Person>("Ivanov3", "assasdsdf", 3);
    auto p4 = make_shared<Person>("Ivanov4", "assasdsdf", 4);
    auto p5 = make_shared<Person>("Ivanov5", "assasdsdf", 5);
    auto p6 = make_shared<Person>("Ivanov6", "assasdsdf", 6);
    auto p7 = make_shared<Person>("Ivanov7", "assasdsdf", 7);
    auto p8 = make_shared<Person>("Ivanov8", "assasdsdf", 8);

    vector<Person_ptr> vertices = { p0, p1, p2, p3, p4, p5, p6, p7, p8 };

    // adjacency lists
    vector<list<shared_ptr<Person>>> adj_lists = {
        /* 0 */ { },
        /* 1 */ { p2, p4 },
        /* 2 */ { p5 },
        /* 3 */ { p5, p6 },
        /* 4 */ { p2 },
        /* 5 */ { p4 },
        /* 6 */ { p6 },
        /* 7 */ { p6, p8 },
        /* 8 */ { p6 }
    };

   // adjacency matrix
    vector<vector<bool>> adj_matrix = {
          //0  1  2  3  4  5  6  7  8
  /* 0 */ { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
  /* 1 */ { 0, 0, 1, 0, 1, 0, 0, 0, 0 },
  /* 2 */ { 0, 0, 0, 0, 0, 1, 0, 0, 0 },
  /* 3 */ { 0, 0, 0, 0, 0, 1, 1, 0, 0 },
  /* 4 */ { 0, 0, 1, 0, 0, 0, 0, 0, 0 },
  /* 5 */ { 0, 0, 0, 0, 1, 0, 0, 0, 0 },
  /* 6 */ { 0, 0, 0, 0, 0, 0, 1, 0, 0 },
  /* 7 */ { 0, 0, 0, 0, 0, 0, 1, 0, 1 },
  /* 8 */ { 0, 0, 0, 0, 0, 0, 1, 0, 0 }
    };

    auto p = p3;

    cout << "BFS adj list" << endl;

    BFS( adj_lists, p );

    cout << endl << endl << "BFS adj matrix" << endl;

    BFS( adj_matrix, vertices, p );
    
    cout << endl << endl << "DFS adj list" << endl;

    DFS( adj_lists, p );

    cout << endl << endl << "DFS adj matrix" << endl;

    DFS( adj_matrix, vertices, p );

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Undirected graph
 //   auto p0 = make_shared<Person>( "Ivanov0", "assasdsdf", 0 );
 //   auto p1 = make_shared<Person>("Ivanov1", "assasdsdf", 1);
 //   auto p2 = make_shared<Person>("Ivanov2", "assasdsdf", 2);
 //   auto p3 = make_shared<Person>("Ivanov3", "assasdsdf", 3);
 //   auto p4 = make_shared<Person>("Ivanov4", "assasdsdf", 4);
 //   auto p5 = make_shared<Person>("Ivanov5", "assasdsdf", 5);
 //   auto p6 = make_shared<Person>("Ivanov6", "assasdsdf", 6);
 //   auto p7 = make_shared<Person>("Ivanov7", "assasdsdf", 7);

 //   vector<Person_ptr> vertices = { p0, p1, p2, p3, p4, p5, p6, p7 };

 //   // adjacency lists
 //   vector<list<shared_ptr<Person>>> adj_lists = { 
 ///* 0 */ { p3, p6 },
 ///* 1 */ { p3, p4 },
 ///* 2 */ { p4, p5 },
 ///* 3 */ { p0, p1, p4 },
 ///* 4 */ { p1, p2, p3, p5, p6, p7 },
 ///* 5 */ { p2, p4, p7 },
 ///* 6 */ { p0, p4 },
 ///* 7 */ { p4, p5 }
 //   };

 //   // adjacency matrix
 //   vector<vector<bool>> adj_matrix = {
 //         //0  1  2  3  4  5  6  7
 // /* 0 */ { 0, 0, 0, 1, 0, 0, 1, 0 },
 // /* 1 */ { 0, 0, 0, 1, 1, 0, 0, 0 },
 // /* 2 */ { 0, 0, 0, 0, 1, 1, 0, 0 },
 // /* 3 */ { 1, 1, 0, 0, 1, 0, 0, 0 },
 // /* 4 */ { 0, 1, 1, 1, 0, 1, 1, 1 },
 // /* 5 */ { 0, 0, 1, 0, 1, 0, 0, 1 },
 // /* 6 */ { 1, 0, 0, 0, 1, 0, 0, 0 },
 // /* 7 */ { 0, 0, 0, 0, 1, 1, 0, 0 },
 //   };
 
    //BFS( adj_lists, vertices, p );
    //cout << endl << endl;
    //BFS( adj_matrix, vertices, p );

};
