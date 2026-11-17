#include <gtest/gtest.h>

#include <algorithm>
#include <cstdint>
#include <random>
#include <set>
#include <vector>

#include "RandomNumberGenerationC++/LinearCongruential.h"

namespace {

    double ChiSquare(const std::vector<long long>& counts, long long total) {
        const double expected = static_cast<double>(total) / counts.size();
        double chi2 = 0;
        for (auto c : counts) {
            chi2 += (c - expected) * (c - expected) / expected;
        }
        return chi2;
    }

}

// x1 = a * 42 + c mod 2^64 and so on, computed independently with exact integer arithmetic.
TEST(LinearCongruential, FirstOutputsMatchTheFormula) {
    LinearCongruential rng(42);

    EXPECT_EQ(rng(), 0x91778aed87ee5eb1u);
    EXPECT_EQ(rng(), 0x39b7f8a5c64cf56cu);
    EXPECT_EQ(rng(), 0x69afc5a5e88b394bu);
    EXPECT_EQ(rng(), 0xa161c43f85f3c87eu);
    EXPECT_EQ(rng(), 0xae1e2aac8d0e4175u);
}

TEST(LinearCongruential, ZeroSeedIsValid) {
    LinearCongruential rng(0);
    EXPECT_EQ(rng(), LinearCongruential::Increment);
    EXPECT_EQ(rng(), 0x1a08ee1184ba6d32u);
}

TEST(LinearCongruential, SameSeedGivesSameSequence) {
    LinearCongruential a(2026), b(2026);
    for (int i = 0; i < 1000; i++) {
        ASSERT_EQ(a(), b()) << "at position " << i;
    }
}

TEST(LinearCongruential, DifferentSeedsGiveDifferentSequences) {
    LinearCongruential a(1), b(2);
    int equal = 0;
    for (int i = 0; i < 1000; i++) {
        if (a() == b()) equal++;
    }
    EXPECT_EQ(equal, 0);
}

TEST(LinearCongruential, DefaultConstructedGeneratorsDiffer) {
    LinearCongruential a, b;
    EXPECT_NE(a(), b());
}

// With the full period 2^64 no state can repeat within the first million steps.
TEST(LinearCongruential, NoRepeatsInMillionOutputs) {
    LinearCongruential rng(7);
    std::vector<std::uint64_t> values(1'000'000);
    for (auto& v : values) v = rng();
    std::sort(values.begin(), values.end());
    EXPECT_EQ(std::adjacent_find(values.begin(), values.end()), values.end());
}

// The documented weakness, kept as a test: a and c are odd, so the parity of the state flips every step.
TEST(LinearCongruential, LowestBitOfRawOutputAlternates) {
    LinearCongruential rng(123);
    std::uint64_t previous = rng() & 1;
    for (int i = 0; i < 1000; i++) {
        std::uint64_t bit = rng() & 1;
        ASSERT_NE(bit, previous) << "at position " << i;
        previous = bit;
    }
}

// NextInRange uses the high bits, so it does not inherit that weakness:
// in a random sequence of 0 and 1 the value changes about every second step, not every step.
TEST(LinearCongruential, NextInRangeOfTwoValuesDoesNotAlternate) {
    LinearCongruential rng(123);
    const int n = 100'000;
    int changes = 0;
    auto previous = rng.NextInRange(0, 1);
    for (int i = 0; i < n; i++) {
        auto x = rng.NextInRange(0, 1);
        if (x != previous) changes++;
        previous = x;
    }
    EXPECT_GT(changes, n * 0.48);
    EXPECT_LT(changes, n * 0.52);
}

TEST(LinearCongruential, NextDoubleIsInHalfOpenUnitInterval) {
    LinearCongruential rng(1);
    for (int i = 0; i < 1'000'000; i++) {
        double x = rng.NextDouble();
        ASSERT_GE(x, 0.0);
        ASSERT_LT(x, 1.0);
    }
}

// Chi-square with 9 degrees of freedom exceeds 27.88 with probability 0.001.
TEST(LinearCongruential, NextDoubleIsUniform) {
    LinearCongruential rng(3);
    const long long total = 1'000'000;
    std::vector<long long> bins(10);
    for (long long i = 0; i < total; i++) {
        bins[static_cast<size_t>(rng.NextDouble() * 10)]++;
    }
    EXPECT_LT(ChiSquare(bins, total), 27.88);
}

TEST(LinearCongruential, NextInRangeIncludesBothEnds) {
    LinearCongruential rng(5);
    std::set<std::int64_t> seen;
    for (int i = 0; i < 100'000; i++) {
        auto x = rng.NextInRange(-100, 100);
        ASSERT_GE(x, -100);
        ASSERT_LE(x, 100);
        seen.insert(x);
    }
    EXPECT_EQ(seen.size(), 201u);
}

// Chi-square with 200 degrees of freedom exceeds 267.54 with probability 0.001.
TEST(LinearCongruential, NextInRangeIsUniform) {
    LinearCongruential rng(11);
    const long long total = 2'010'000;
    std::vector<long long> counts(201);
    for (long long i = 0; i < total; i++) {
        counts[static_cast<size_t>(rng.NextInRange(-100, 100) + 100)]++;
    }
    EXPECT_LT(ChiSquare(counts, total), 267.54);
}

TEST(LinearCongruential, NextInRangeOfOneValue) {
    LinearCongruential rng(9);
    for (int i = 0; i < 100; i++) {
        EXPECT_EQ(rng.NextInRange(17, 17), 17);
    }
}

TEST(LinearCongruential, NextInRangeAtTheLimitsOfInt64) {
    LinearCongruential rng(13);
    const auto lo = std::numeric_limits<std::int64_t>::min();
    const auto hi = std::numeric_limits<std::int64_t>::max();
    for (int i = 0; i < 1000; i++) {
        auto x = rng.NextInRange(lo, lo + 1);
        ASSERT_TRUE(x == lo || x == lo + 1);
        auto y = rng.NextInRange(hi - 1, hi);
        ASSERT_TRUE(y == hi - 1 || y == hi);
    }
    rng.NextInRange(lo, hi);
}

TEST(LinearCongruential, NextInRangeRejectsReversedRange) {
    LinearCongruential rng(1);
    EXPECT_THROW(rng.NextInRange(10, 9), std::invalid_argument);
}

TEST(LinearCongruential, WorksWithStandardDistributions) {
    LinearCongruential rng(21);
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
