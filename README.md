# BaseAlgorithms

[![CI](https://github.com/zr-roman/BaseAlgorithms/actions/workflows/ci.yml/badge.svg)](https://github.com/zr-roman/BaseAlgorithms/actions/workflows/ci.yml)

Classic algorithms and data structures implemented from scratch in C++ and C#, mostly following
Cormen et al., *Introduction to Algorithms* (CLRS). The goal is to understand how each one works,
so the standard library containers and algorithms are deliberately not used for the things being implemented.

## What is inside

| Topic | C++ | C# | Tests |
|---|---|---|---|
| Sorting | `SortingsC++` | `SortingsC#` | C++ and C# |
| Hash tables | `HashingC++` | `HashingC#` | C# |
| Graphs | `GraphsC++` | `GraphsCs` | C# |
| String matching | `StringMatching C++` | `StringMatchingC#` | none yet |
| Stacks, queues, lists | `ElementaryDataStructuresC++` | `ElementaryDataStructuresC#` | none yet |
| Binary search trees | `BinarySearchTreesC++` | | Visual Studio only |
| Binary search | `BinarySearchC++` | | none yet |
| Dynamic programming | `DynamicProgrammingC++` | | none yet |
| Permutations of a string | | `RecombinationsC#` | none yet |
| Design patterns | `DesignPatternsC++` | | none yet |

Code that has no tests yet has not been verified beyond manual runs.

### Sorting

- **C++:** insertion, Shell, bubble, comb, selection, brute force, quicksort (first, last and random pivot),
  merge sort (two variants), heapsort, counting, radix and bucket sort.
- **C#:** bubble, comb, selection, insertion, Shell, quicksort, merge sort (recursive and iterative),
  heapsort, counting and radix sort.

### Hash tables (C#)

Three implementations of one abstract class, all with universal hashing:

- `ChainingHashTable` resolves collisions by chaining and grows and shrinks with the number of keys.
- `OpenAddressingHashTable` uses open addressing with linear probing, quadratic probing or double hashing,
  chosen in the constructor.
- `PerfectHashTable` is two-level perfect hashing for a number of keys known in advance.
  It requires keys with distinct hash codes.

The C++ folder has the same three tables.

### Graphs

- **C#:** BFS, DFS with topological sort, Dijkstra, Bellman-Ford, Floyd-Warshall,
  minimum spanning tree (Prim, Kruskal, Boruvka), maximum flow (Edmonds-Karp).
- **C++:** BFS, DFS, Dijkstra, Bellman-Ford, minimum spanning tree (Prim, Kruskal).

### String matching (C++ and C#)

Brute force, finite automaton, Knuth-Morris-Pratt, Boyer-Moore, Karp-Rabin.

### Other

- **Elementary data structures:** stack and queue on a vector, an array and a linked list, a queue on two stacks,
  a stack on two queues, linked lists stored in arrays (C++); a queue on two stacks and a stack on two queues (C#).
- **Binary search trees:** red-black tree with traversals.
- **Dynamic programming:** Fibonacci numbers, knapsack, longest common subsequence, matrix chain multiplication, unique paths.
- **Design patterns:** abstract factory, singleton, strategy.
- **C++ language exercises:** `MoveSemanticsC++`, `OperatorsC++`, `MultithreadingC++`, `RandomNumberGenerationC++`.
- **`SQLServer`:** a hierarchical query.

## C#: build and test

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```
dotnet build BaseAlgorithms.sln
dotnet test BaseAlgorithms.sln
```

The hash table tests include a few speed checks. To skip them or to run only them:

```
dotnet test BaseAlgorithms.sln --filter "TestCategory!=Performance"
dotnet test BaseAlgorithms.sln --filter "TestCategory=Performance"
```

Three projects are console programs and can be run directly, for example:

```
dotnet run --project "SortingsC#"
```

The others are `StringMatchingC#` and `RecombinationsC#`.

## C++: build and test

Requires CMake 3.20 or newer and a C++20 compiler. GoogleTest is downloaded during the first configuration
unless an installed copy is found.

```
cmake -S . -B build
cmake --build build
ctest --test-dir build
```

With Visual Studio generators add the configuration: `cmake --build build --config Debug`
and `ctest --test-dir build -C Debug`.

CMake currently builds only the sorting library (`SortingsC++`) and its GoogleTest tests
(`TestingForSortingsC++/UnitTest2.cpp`). The other C++ folders are source files without a build script:

- `TestingForSortingsC++/UnitTest1.cpp` and `TestingForBinarySearchTreesC++` are tests for the
  Visual Studio native unit test framework.
- `BinarySearchC++` is C++/CLI, and `RandomNumberGenerationC++` uses the Windows API.

## Repository layout

Each folder is one topic in one language; the suffix (`C++`, `C#` or `Cs`) says which.
Folders that start with `TestingFor` contain the tests for the folder they name.
