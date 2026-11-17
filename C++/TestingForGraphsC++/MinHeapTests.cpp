#include <gtest/gtest.h>

#include "GraphsC++/Header.h"

#include <algorithm>
#include <unordered_set>

namespace {

Person_ptr MakePerson(int id, int priority) {
    auto person = std::make_shared<Person>(std::to_string(id), std::to_string(id), id);
    person->d = priority;
    return person;
}

std::vector<Person_ptr> MakePeople(std::initializer_list<int> priorities) {
    std::vector<Person_ptr> people;
    int id = 0;
    for (int priority : priorities) {
        people.push_back(MakePerson(id++, priority));
    }
    return people;
}

} // namespace

TEST(MinHeapTests, BuildsHeapAndExtractsInAscendingPriorityOrder) {
    auto people = MakePeople({ 7, 2, 9, 1, 5, 3, 8, 4, 6 });
    MinHeap<Person_ptr> heap(people);

    const std::vector<int> expected{ 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    std::vector<int> actual;
    while (heap.size() > 0) {
        actual.push_back(heap.extract_min()->d);
    }

    EXPECT_EQ(actual, expected);
    EXPECT_EQ(heap.size(), 0u);
}

TEST(MinHeapTests, ExtractsEachItemOnceWhenOnlyTwoRemain) {
    MinHeap<Person_ptr> heap(MakePeople({ 1, 2, 3 }));

    EXPECT_EQ(heap.extract_min()->d, 1);
    EXPECT_EQ(heap.size(), 2u);
    EXPECT_EQ(heap.extract_min()->d, 2);
    EXPECT_EQ(heap.size(), 1u);
    EXPECT_EQ(heap.extract_min()->d, 3);
    EXPECT_EQ(heap.size(), 0u);
}

TEST(MinHeapTests, EmptyHeapHasSizeZero) {
    MinHeap<Person_ptr> heap({});

    EXPECT_EQ(heap.size(), 0u);
}

TEST(MinHeapTests, ExtractingSingleItemLeavesHeapEmpty) {
    auto person = MakePerson(0, 42);
    MinHeap<Person_ptr> heap({ person });

    EXPECT_EQ(heap.extract_min(), person);
    EXPECT_EQ(heap.size(), 0u);
}

TEST(MinHeapTests, DecreaseKeyMovesItemUpAndPreservesHeapOrder) {
    auto people = MakePeople({ 30, 10, 40, 20, 50, 60, 70 });
    MinHeap<Person_ptr> heap(people);

    people[6]->d = 5;
    heap.Decrease_key(people[6]);
    people[5]->d = 8;
    heap.Decrease_key(people[5]);
    people[4]->d = 3;
    heap.Decrease_key(people[4]);

    std::vector<int> expected;
    for (const auto& person : people) {
        expected.push_back(person->d);
    }
    std::sort(expected.begin(), expected.end());

    std::vector<int> actual;
    while (heap.size() > 0) {
        actual.push_back(heap.extract_min()->d);
    }

    EXPECT_EQ(actual, expected);
}

TEST(MinHeapTests, DecreaseKeyOnSingleItemHeapStillExtractsUpdatedItem) {
    auto person = MakePerson(0, 100);
    MinHeap<Person_ptr> heap({ person });

    person->d = -1;
    heap.Decrease_key(person);

    EXPECT_EQ(heap.extract_min(), person);
    EXPECT_EQ(heap.size(), 0u);
}

TEST(MinHeapTests, DecreaseKeyStillWorksAfterAnExtractionAndHeapSwaps) {
    auto people = MakePeople({ 1, 4, 3, 8, 5, 6, 7 });
    MinHeap<Person_ptr> heap(people);

    EXPECT_EQ(heap.extract_min()->d, 1);
    people[6]->d = 0;
    heap.Decrease_key(people[6]);

    EXPECT_EQ(heap.extract_min(), people[6]);
    EXPECT_EQ(heap.extract_min()->d, 3);
}

TEST(MinHeapTests, EqualPrioritiesExtractEveryDistinctItemExactlyOnce) {
    auto people = MakePeople({ 4, 4, 4, 4, 4, 4, 4 });
    MinHeap<Person_ptr> heap(people);

    std::unordered_set<const Person*> extracted;
    while (heap.size() > 0) {
        auto person = heap.extract_min();
        EXPECT_EQ(person->d, 4);
        extracted.insert(person.get());
    }

    EXPECT_EQ(extracted.size(), people.size());
}

