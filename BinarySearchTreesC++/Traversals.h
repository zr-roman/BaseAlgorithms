#pragma once
#include <memory>

using namespace std;

class Node {
public:
    int value;
    shared_ptr<Node> left;
    shared_ptr<Node> right;

    Node(shared_ptr<Node> l, shared_ptr<Node> r, int v) {
        left = l;
        right = r;
        value = v;
    }
    Node(int v) {
        left = nullptr;
        right = nullptr;
        value = v;
    }
};

void InOrderTraverse(shared_ptr<Node> node) {
    if (node == nullptr) {
        return;
    }
    InOrderTraverse(node.get()->left);
    cout << node.get()->value << "\r\n";
    InOrderTraverse(node.get()->right);
}

void PreOrderTraverse(shared_ptr<Node> node) {
    if (node == nullptr) {
        return;
    }
    cout << node.get()->value << "\r\n";
    PreOrderTraverse(node.get()->left);
    PreOrderTraverse(node.get()->right);
}

void PostOrderTraverse(shared_ptr<Node> node) {
    if (node == nullptr) {
        return;
    }
    PostOrderTraverse(node.get()->left);
    PostOrderTraverse(node.get()->right);
    cout << node.get()->value << "\r\n";
}
