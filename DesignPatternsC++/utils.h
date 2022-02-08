#pragma once
#include <stdexcept>

using namespace std;

#pragma region вспомогательные функции
enum class Order {
	ASC,
	DESC
};

inline const char* ToString(Order o) {
	switch (o) {
	case Order::ASC:   return "ASC";
	case Order::DESC:   return "DESC";
	default:      throw runtime_error("Must not hit into here!");
	}
}
#pragma endregion