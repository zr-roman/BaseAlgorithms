// Every sorting algorithm is compared with std::sort / std::stable_sort on random inputs.
#include <gtest/gtest.h>

#include <algorithm>
#include <climits>
#include <functional>
#include <random>
#include <stdexcept>
#include <string>
#include <vector>

#include "SortingsC++/Lib.h"

namespace {

    using Order = SortingLib::Order;
    using SortFn = std::function<void(std::vector<int>*, Order)>;

    std::vector<int> RandomVector(size_t n, int lo, int hi, std::mt19937& rng) {
        std::uniform_int_distribution<int> dist(lo, hi);
        std::vector<int> v(n);
        for (auto& x : v) x = dist(rng);
        return v;
    }

    struct Algorithm {
        const char* name;
        SortFn sort;
        bool huge_key_range = true;  // CountingSort allocates (max - min + 1) counters
        bool sentinel_free = true;   // MergeSort_Cormen fails when INT_MAX / INT_MIN occur in the data
        friend void PrintTo(const Algorithm& a, std::ostream* os) { *os << a.name; }
    };

    class SortingTest : public ::testing::TestWithParam<Algorithm> {};

    void ExpectSortsLikeStd(const SortFn& sort, int lo, int hi) {
        std::mt19937 rng(20261108);
        for (size_t n : {1, 2, 3, 7, 10, 100, 1000}) {
            for (Order order : {Order::ASC, Order::DESC}) {
                for (int rep = 0; rep < 5; ++rep) {
                    auto v = RandomVector(n, lo, hi, rng);
                    auto expected = v;
                    if (order == Order::ASC) std::sort(expected.begin(), expected.end());
                    else std::sort(expected.begin(), expected.end(), std::greater<>());
                    sort(&v, order);
                    ASSERT_EQ(v, expected) << "n=" << n << (order == Order::ASC ? " ASC" : " DESC");
                }
            }}
    }

    TEST_P(SortingTest, MatchesStdSortOnRandomInput) { ExpectSortsLikeStd(GetParam().sort, -1000, 1000); }

    TEST_P(SortingTest, ManyDuplicates) { ExpectSortsLikeStd(GetParam().sort, 0, 3); }

    TEST_P(SortingTest, AlreadySortedInputStaysSorted) {
        for (Order order : {Order::ASC, Order::DESC}) {
            std::vector<int> v{1, 2, 3, 4, 5, 6, 7, 8};
            if (order == Order::DESC) std::reverse(v.begin(), v.end());
            const auto expected = v;
            GetParam().sort(&v, order);
            EXPECT_EQ(v, expected);
        }
    }

    TEST_P(SortingTest, ReverseSortedInput) {
        std::vector<int> v(500);
        for (int i = 0; i < 500; ++i) v[i] = 500 - i;
        GetParam().sort(&v, Order::ASC);
        EXPECT_TRUE(std::is_sorted(v.begin(), v.end()));
    }

    TEST_P(SortingTest, ExtremeValues) {
        if (!GetParam().huge_key_range) GTEST_SKIP() << "needs max-min+1 counters";
        if (!GetParam().sentinel_free) GTEST_SKIP() << "INT_MAX/INT_MIN are used as sentinels";
        std::vector<int> v{INT_MAX, 0, INT_MIN, -1, INT_MAX, 1, INT_MIN};
        auto expected = v;
        std::sort(expected.begin(), expected.end());
        GetParam().sort(&v, Order::ASC);
        EXPECT_EQ(v, expected);
    }

    TEST_P(SortingTest, EmptyInputIsNoOp) {
        std::vector<int> v;
        EXPECT_NO_THROW(GetParam().sort(&v, Order::ASC));
        EXPECT_TRUE(v.empty());
    }

    TEST_P(SortingTest, NullInputThrows) {
        EXPECT_THROW(GetParam().sort(nullptr, Order::ASC), std::invalid_argument);
    }

    const Algorithm kAlgorithms[] = {
        {"ShellSort", [](auto* a, Order o) { SortingLib::ShellSort(a, o); }},
        {"InsertionSort", [](auto* a, Order o) { SortingLib::InsertionSort(a, o); }},
        {"QuickSortRandomPivot", [](auto* a, Order o) { SortingLib::QuickSort(a, o, SortingLib::Pivot::RANDOM); }},
        {"QuickSortFirstPivot", [](auto* a, Order o) { SortingLib::QuickSort(a, o, SortingLib::Pivot::FIRST); }},
        {"QuickSortLastPivot", [](auto* a, Order o) { SortingLib::QuickSort(a, o, SortingLib::Pivot::LAST); }},
        {"HeapSort", [](auto* a, Order o) { SortingLib::HeapSort(a, o); }},
        {"MergeSortCormen", [](auto* a, Order o) { SortingLib::MergeSort_Cormen(a, INT_MAX, INT_MIN, o); }, true, false},
        {"MergeSortGeneral", [](auto* a, Order o) { SortingLib::MergeSort_General(a, o); }},
        {"BubbleSort", [](auto* a, Order o) { SortingLib::BubbleSort(a, o); }},
        {"SelectionSort", [](auto* a, Order o) { SortingLib::SelectionSort(a, o); }},
        {"CombSort", [](auto* a, Order o) { SortingLib::CombSort(a, o); }},
        {"BruteForceSort", [](auto* a, Order o) { SortingLib::BruteForceSort(a, o); }},
        {"CountingSort", [](auto* a, Order o) { SortingLib::CountingSort(a, o); }, false},
        {"RadixSort", [](auto* a, Order o) { SortingLib::RadixSort(a, o); }},
        {"BucketSort", [](auto* a, Order o) { SortingLib::BucketSort(a, o); }},
    };

    INSTANTIATE_TEST_SUITE_P(AllAlgorithms, SortingTest, ::testing::ValuesIn(kAlgorithms),
                            [](const auto& info) { return std::string(info.param.name); });

    struct Item {
        int key;
        int id;
        bool operator<(const Item& o) const { return key < o.key; }
        bool operator>(const Item& o) const { return key > o.key; }
        bool operator<=(const Item& o) const { return key <= o.key; }  // MergeSort_Cormen compares with <= and >=
        bool operator>=(const Item& o) const { return key >= o.key; }
    };

    TEST(MergeSort, IsStable) {
        std::mt19937 rng(1);
        std::uniform_int_distribution<int> key(0, 5);
        for (Order order : {Order::ASC, Order::DESC}) {
            std::vector<Item> v;
            for (int i = 0; i < 300; ++i) v.push_back({key(rng), i});
            auto expected = v;
            if (order == Order::ASC) std::stable_sort(expected.begin(), expected.end());
            else std::stable_sort(expected.begin(), expected.end(), std::greater<>());
            auto cormen = v, general = v;
            SortingLib::MergeSort_Cormen(&cormen, Item{INT_MAX, -1}, Item{INT_MIN, -1}, order);
            SortingLib::MergeSort_General(&general, order);
            for (size_t i = 0; i < v.size(); ++i) {
                ASSERT_EQ(cormen[i].id, expected[i].id) << "MergeSort_Cormen, position " << i;
                ASSERT_EQ(general[i].id, expected[i].id) << "MergeSort_General, position " << i;
            }
        }
    }

    TEST(Sorting, WorksWithStrings) {
        std::vector<std::string> v{"pear", "apple", "fig", "banana", "apple"};
        auto expected = v;
        std::sort(expected.begin(), expected.end());
        auto a = v, b = v, c = v;
        SortingLib::QuickSort(&a);
        SortingLib::MergeSort_General(&b);
        SortingLib::HeapSort(&c);
        EXPECT_EQ(a, expected);
        EXPECT_EQ(b, expected);
        EXPECT_EQ(c, expected);
    }

    TEST(Sorting, WorksWithDoubles) {
        std::vector<double> v{0.5, 0.2, 0.9, -1.5, 0.25};
        auto expected = v;
        std::sort(expected.begin(), expected.end());
        SortingLib::ShellSort(&v);
        EXPECT_EQ(v, expected);
    }

}  // namespace