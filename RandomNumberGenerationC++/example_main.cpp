// Example: the generators in place of RandNum / RandNumRange.
#include <iostream>
#include <map>
#include <string>

#include "LinearCongruential.h"
#include "Xoshiro256StarStar.h"

// The same checks for any generator with NextDouble and NextInRange.
template <typename Generator>
void Demo(const std::string& name, Generator& rng) {
    std::cout << "=== " << name << " ===\n";

    int counts[10] = {};
    for (int i = 0; i < 1'000'000; i++) {
        counts[static_cast<int>(rng.NextDouble() * 10)]++;
    }
    for (int i = 0; i < 10; i++) {
        std::cout << "[" << i / 10.0 << ", " << (i + 1) / 10.0 << "): " << counts[i] << '\n';
    }

    std::map<long long, int> range;
    for (int i = 0; i < 201'000; i++) {
        range[rng.NextInRange(-100, 100)]++;
    }
    std::cout << "\ndistinct values in [-100, 100]: " << range.size()
              << ", count of 0: " << range[0] << ", of 1: " << range[1] << "\n\n";
}

int main() {
    // A new seed on every run; Xoshiro256StarStar xoshiro(42) or LinearCongruential lcg(42) repeats the run.
    Xoshiro256StarStar xoshiro;
    LinearCongruential lcg;

    Demo("xoshiro256**", xoshiro);
    Demo("LCG (Knuth's MMIX constants)", lcg);

    // The weakness of the LCG: the lowest bit of the raw output just alternates.
    // NextInRange takes the high bits and does not have this problem.
    std::cout << "LCG, lowest bit of the raw output: ";
    for (int i = 0; i < 20; i++) std::cout << (lcg() & 1);
    std::cout << "\nLCG, NextInRange(0, 1):            ";
    for (int i = 0; i < 20; i++) std::cout << lcg.NextInRange(0, 1);
    std::cout << "\nxoshiro256**, lowest bit:          ";
    for (int i = 0; i < 20; i++) std::cout << (xoshiro() & 1);
    std::cout << '\n';
}
