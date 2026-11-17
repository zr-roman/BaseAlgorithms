#pragma once

// Linear congruential generator (LCG): x(n+1) = (a * x(n) + c) mod 2^64.
// Constants from D. Knuth's MMIX: a = 6364136223846793005, c = 1442695040888963407.
//
// Full period 2^64 for any seed, by the Hull–Dobell theorem: c is odd and a - 1 is divisible by 4.
//
// The known weakness of an LCG modulo a power of two: the low bits are far from random.
// Bit k of the state repeats with period 2^(k+1); the lowest bit simply alternates 0, 1, 0, 1, ...
// So everything here takes the HIGH bits: NextDouble uses the upper 53 bits, and NextInRange
// divides instead of taking a remainder. For serious use prefer xoshiro256** or PCG.
// Not cryptographically secure.
//
// The class satisfies the UniformRandomBitGenerator requirements, like Xoshiro256StarStar.

#include <cstdint>
#include <limits>
#include <random>
#include <stdexcept>

class LinearCongruential {
public:
    using result_type = std::uint64_t;

    static constexpr std::uint64_t Multiplier = 6364136223846793005u;
    static constexpr std::uint64_t Increment = 1442695040888963407u;

    // Any seed is valid, zero included: the state is just the seed.
    explicit LinearCongruential(std::uint64_t seed) : state(seed) {}

    // A seed from the operating system's source of randomness.
    LinearCongruential() : LinearCongruential(SeedFromRandomDevice()) {}

    static constexpr result_type min() { return 0; }
    static constexpr result_type max() { return std::numeric_limits<result_type>::max(); }

    // The next state. Unsigned overflow wraps around, which is exactly "mod 2^64".
    result_type operator()() {
        state = Multiplier * state + Increment;
        return state;
    }

    // A uniformly distributed number in [0, 1): the upper 53 bits divided by 2^53.
    double NextDouble() {
        return static_cast<double>((*this)() >> 11) * 0x1.0p-53;
    }

    // A uniformly distributed integer in [start, end], both ends included.
    // The range [0, 2^64) is cut into count equal buckets and the number of the bucket is returned,
    // so the result depends on the high bits. x % count would depend on the weak low bits:
    // NextInRange(0, 1) would then alternate 0, 1, 0, 1, ...
    // Outputs beyond the last whole bucket are thrown away so that every bucket is equally likely.
    std::int64_t NextInRange(std::int64_t start, std::int64_t end) {
        if (start > end) {
            throw std::invalid_argument("start must not be greater than end");
        }

        const std::uint64_t span = static_cast<std::uint64_t>(end) - static_cast<std::uint64_t>(start);
        if (span == max()) {
            return static_cast<std::int64_t>((*this)());
        }

        const std::uint64_t count = span + 1;
        const std::uint64_t bucket = max() / count;   // count * bucket <= max(), so the last bucket fits
        std::uint64_t x;
        do {
            x = (*this)() / bucket;
        } while (x >= count);

        return static_cast<std::int64_t>(static_cast<std::uint64_t>(start) + x);
    }

private:
    std::uint64_t state;

    static std::uint64_t SeedFromRandomDevice() {
        std::random_device device;
        return (static_cast<std::uint64_t>(device()) << 32) | device();
    }
};
