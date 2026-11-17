#include <gtest/gtest.h>

#include <algorithm>
#include <cstdint>
#include <random>
#include <set>
#include <vector>

#include "RandomNumberGenerationC++/Xoshiro256StarStar.h"

namespace {

    // Pearson's chi-square statistic: how far the observed counts are from equal ones.
    double ChiSquare(const std::vector<long long>& counts, long long total) {
        const double expected = static_cast<double>(total) / counts.size();
        double chi2 = 0;
        for (auto c : counts) {
            chi2 += (c - expected) * (c - expected) / expected;
        }
        return chi2;
    }

}

// The values were computed by an independent implementation of the published algorithm.
// SplitMix64 from seed 42 gives the state (0xbdd732262feb6e95, 0x28efe333b266f103, ...).
TEST(Xoshiro256StarStar, FirstOutputsMatchTheReferenceAlgorithm) {
    Xoshiro256StarStar rng(42);

    EXPECT_EQ(rng(), 0x15780b2e0c2ec716u);
    EXPECT_EQ(rng(), 0x6104d9866d113a7eu);
    EXPECT_EQ(rng(), 0xae17533239e499a1u);
    EXPECT_EQ(rng(), 0xecb8ad4703b360a1u);
    EXPECT_EQ(rng(), 0xfde6dc7fe2ec5e64u);
}

TEST(Xoshiro256StarStar, SameSeedGivesSameSequence) {
    Xoshiro256StarStar a(2026), b(2026);
    for (int i = 0; i < 1000; i++) {
        ASSERT_EQ(a(), b()) << "at position " << i;
    }
}

TEST(Xoshiro256StarStar, DifferentSeedsGiveDifferentSequences) {
    Xoshiro256StarStar a(1), b(2);
    int equal = 0;
    for (int i = 0; i < 1000; i++) {
        if (a() == b()) equal++;
    }
    EXPECT_EQ(equal, 0);
}

TEST(Xoshiro256StarStar, DefaultConstructedGeneratorsDiffer) {
    Xoshiro256StarStar a, b;
    EXPECT_NE(a(), b());
}

TEST(Xoshiro256StarStar, NoRepeatsInMillionOutputs) {
    Xoshiro256StarStar rng(7);
    std::vector<std::uint64_t> values(1'000'000);
    for (auto& v : values) v = rng();
    std::sort(values.begin(), values.end());
    EXPECT_EQ(std::adjacent_find(values.begin(), values.end()), values.end());
}

TEST(Xoshiro256StarStar, NextDoubleIsInHalfOpenUnitInterval) {
    Xoshiro256StarStar rng(1);
    for (int i = 0; i < 1'000'000; i++) {
        double x = rng.NextDouble();
        ASSERT_GE(x, 0.0);
        ASSERT_LT(x, 1.0);
    }
}

// 1 000 000 numbers in 10 bins. Chi-square with 9 degrees of freedom exceeds 27.88 with probability 0.001.
// The seed is fixed, so the test is deterministic.
TEST(Xoshiro256StarStar, NextDoubleIsUniform) {
    Xoshiro256StarStar rng(3);
    const long long total = 1'000'000;
    std::vector<long long> bins(10);
    for (long long i = 0; i < total; i++) {
        bins[static_cast<size_t>(rng.NextDouble() * 10)]++;
    }
    EXPECT_LT(ChiSquare(bins, total), 27.88);
}

TEST(Xoshiro256StarStar, NextInRangeIncludesBothEnds) {
    Xoshiro256StarStar rng(5);
    std::set<std::int64_t> seen;
    for (int i = 0; i < 100'000; i++) {
        auto x = rng.NextInRange(-100, 100);
        ASSERT_GE(x, -100);
        ASSERT_LE(x, 100);
        seen.insert(x);
    }
    EXPECT_EQ(seen.size(), 201u) << "every value from -100 to 100 must occur";
}

// 201 values; chi-square with 200 degrees of freedom exceeds 267.54 with probability 0.001.
// In particular 0 must not come twice as often as its neighbours.
TEST(Xoshiro256StarStar, NextInRangeIsUniform) {
    Xoshiro256StarStar rng(11);
    const long long total = 2'010'000;
    std::vector<long long> counts(201);
    for (long long i = 0; i < total; i++) {
        counts[static_cast<size_t>(rng.NextInRange(-100, 100) + 100)]++;
    }
    EXPECT_LT(ChiSquare(counts, total), 267.54);
}

TEST(Xoshiro256StarStar, NextInRangeOfOneValue) {
    Xoshiro256StarStar rng(9);
    for (int i = 0; i < 100; i++) {
        EXPECT_EQ(rng.NextInRange(17, 17), 17);
    }
}

TEST(Xoshiro256StarStar, NextInRangeAtTheLimitsOfInt64) {
    Xoshiro256StarStar rng(13);
    const auto lo = std::numeric_limits<std::int64_t>::min();
    const auto hi = std::numeric_limits<std::int64_t>::max();
    for (int i = 0; i < 1000; i++) {
        auto x = rng.NextInRange(lo, lo + 1);
        ASSERT_TRUE(x == lo || x == lo + 1);
        auto y = rng.NextInRange(hi - 1, hi);
        ASSERT_TRUE(y == hi - 1 || y == hi);
    }
    rng.NextInRange(lo, hi);   // the whole range must not overflow
}

TEST(Xoshiro256StarStar, NextInRangeRejectsReversedRange) {
    Xoshiro256StarStar rng(1);
    EXPECT_THROW(rng.NextInRange(10, 9), std::invalid_argument);
}

// The class is a UniformRandomBitGenerator, so it plugs into the standard library.
TEST(Xoshiro256StarStar, WorksWithStandardDistributions) {
    Xoshiro256StarStar rng(21);
    std::uniform_int_distribution<int> dice(1, 6);
    for (int i = 0; i < 1000; i++) {
        int x = dice(rng);
        ASSERT_GE(x, 1);
        ASSERT_LE(x, 6);
    }

    std::vector<int> v{ 1, 2, 3, 4, 5 };
    std::shuffle(v.begin(), v.end(), rng);
    std::sort(v.begin(), v.end());
    EXPECT_EQ(v, (std::vector<int>{ 1, 2, 3, 4, 5 }));
}
