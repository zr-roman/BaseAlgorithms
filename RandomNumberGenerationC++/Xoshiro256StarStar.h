#pragma once

// Pseudo-random number generator xoshiro256** (D. Blackman, S. Vigna, 2018).
// Reference implementation and the description of the algorithm: https://prng.di.unimi.it/
//
// State: four 64-bit words. Period: 2^256 - 1. Passes the BigCrush and PractRand test suites.
// Not cryptographically secure: do not use it for keys, tokens or passwords.
//
// The class satisfies the UniformRandomBitGenerator requirements, so it also works with
// std::uniform_int_distribution, std::shuffle and the other algorithms of <random>.

#include <cstdint>
#include <limits>
#include <random>
#include <stdexcept>

class Xoshiro256StarStar {
public:
    using result_type = std::uint64_t;

    // The same seed gives the same sequence: useful for tests and for reproducing a run.
    explicit Xoshiro256StarStar(std::uint64_t seed) {
        // The authors recommend filling the state with SplitMix64: it never gives the all-zero state,
        // which is the only state xoshiro cannot leave.
        for (auto& word : s) {
            word = SplitMix64(seed);
        }
    }

    // A seed from the operating system's source of randomness: a different sequence on every run.
    Xoshiro256StarStar() : Xoshiro256StarStar(SeedFromRandomDevice()) {}

    static constexpr result_type min() { return 0; }
    static constexpr result_type max() { return std::numeric_limits<result_type>::max(); }

    // The next 64 random bits.
    result_type operator()() {
        const std::uint64_t result = Rotl(s[1] * 5, 7) * 9;
        const std::uint64_t t = s[1] << 17;

        s[2] ^= s[0];
        s[3] ^= s[1];
        s[1] ^= s[2];
        s[0] ^= s[3];
        s[2] ^= t;
        s[3] = Rotl(s[3], 45);

        return result;
    }

    // A uniformly distributed number in [0, 1).
    // A double has 53 significant bits: the upper 53 bits of the output divided by 2^53.
    double NextDouble() {
        return static_cast<double>((*this)() >> 11) * 0x1.0p-53;
    }

    // A uniformly distributed integer in [start, end], both ends included.
    // Rejection sampling: outputs from the incomplete last "block" of the range are thrown away,
    // otherwise small numbers would come a little more often (modulo bias).
    std::int64_t NextInRange(std::int64_t start, std::int64_t end) {
        if (start > end) {
            throw std::invalid_argument("start must not be greater than end");
        }

        // The number of values minus one; computed in unsigned arithmetic, so it cannot overflow.
        const std::uint64_t span = static_cast<std::uint64_t>(end) - static_cast<std::uint64_t>(start);
        if (span == max()) {
            return static_cast<std::int64_t>((*this)());
        }

        const std::uint64_t count = span + 1;
        const std::uint64_t limit = max() - max() % count;   // the largest multiple of count that fits
        std::uint64_t x;
        do {
            x = (*this)();
        } while (x >= limit);

        return static_cast<std::int64_t>(static_cast<std::uint64_t>(start) + x % count);
    }

private:
    std::uint64_t s[4];

    static std::uint64_t Rotl(std::uint64_t x, int k) {
        return (x << k) | (x >> (64 - k));
    }

    static std::uint64_t SplitMix64(std::uint64_t& x) {
        std::uint64_t z = (x += 0x9e3779b97f4a7c15);
        z = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        z = (z ^ (z >> 27)) * 0x94d049bb133111eb;
        return z ^ (z >> 31);
    }

    static std::uint64_t SeedFromRandomDevice() {
        std::random_device device;
        return (static_cast<std::uint64_t>(device()) << 32) | device();
    }
};
