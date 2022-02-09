#pragma once
#include <iostream>
#include <vector>


using namespace std;

class Shape {
public:
	virtual void GetInfo() = 0;
	virtual ~Shape() {}
};

// Abstract product A
class Circle : public Shape {
public:
	Circle(int radius) {
		this->radius = radius;
	};

private:
	int radius;
};

// Abstract product B
class Rectangle : public Shape {
public:

	Rectangle(int side1, int side2) {
		this->side1 = side1;
		this->side2 = side2;
	};

private:
	int side1;
	int side2;
};

// Abstract product C
class Triangle : public Shape {
public:
	
	Triangle(int side1, int side2, int side3) {
		this->side1 = side1;
		this->side2 = side2;
		this->side3 = side3;
	}

private:
	int side1;
	int side2;
	int side3;
};

// Corcrete products
class JacksCircle : public Circle {

public:

	JacksCircle(int radius) : Circle( radius ) {

	}

	void GetInfo() override {
		cout << "JacksCircle is blue and has 3 black dots inside" << endl;
	}

};

class TomsCircle : public Circle {

public:

	TomsCircle(int radius) : Circle(radius) {

	}

	void GetInfo() override {
		cout << "TomsCircle is black and has 5 red lines inside" << endl;
	}

};

class MarysCircle : public Circle {

public:

	MarysCircle(int radius) : Circle(radius) {

	}

	void GetInfo() override {
		cout << "MarysCircle is yellow and has 7 purple ticks inside" << endl;
	}

};

class JacksRectangle : public Rectangle {
public:
	JacksRectangle(int side1, int side2) : Rectangle(side1, side2) {

	}

	void GetInfo() override {
		cout << "JacksRectangle is green and has 2 roses inside" << endl;
	}

};

class JacksTriangle : public Triangle {
public:
	JacksTriangle(int side1, int side2, int side3) : Triangle(side1, side2, side3) {

	}

	void GetInfo() override {
		cout << "JacksTriangle is brown and has several stars inside" << endl;
	}
};

class TomsTriangle : public Triangle {
public:
	TomsTriangle(int side1, int side2, int side3) : Triangle(side1, side2, side3) {

	}

	void GetInfo() override {
		cout << "TomsTriangle is orange and has 10 foxes inside" << endl;
	}
};

class TomsRectangle : public Rectangle {
public:
	TomsRectangle(int side1, int side2) : Rectangle(side1, side2) {

	}

	void GetInfo() override {
		cout << "TomsRectangle is golden and has 9 trees inside" << endl;
	}

};

class MarysRectangle : public Rectangle {
public:
	MarysRectangle(int side1, int side2) : Rectangle(side1, side2) {

	}

	void GetInfo() override {
		cout << "MarysRectangle is silver and has 22 birds inside" << endl;
	}

};

class MarysTriangle : public Triangle {
public:
	MarysTriangle(int side1, int side2, int side3) : Triangle(side1, side2, side3) {

	}

	void GetInfo() override {
		cout << "MarysTriangle is white and has 5 boxes inside" << endl;
	}
};

// Abstract factory for creating shapes
class ShapesFactory {
public:
	virtual shared_ptr<Circle> CreateCircle(int) = 0;
	virtual shared_ptr<Rectangle> CreateRectangle(int, int) = 0;
	virtual shared_ptr<Triangle> CreateTriangle(int, int, int) = 0;
};

// Concrete factory of Jack
class JacksFactory : public ShapesFactory {
public:
	
	shared_ptr<Circle> CreateCircle(int radius) {
		return make_shared<JacksCircle>( radius );
	}

	shared_ptr<Rectangle> CreateRectangle(int side1, int side2) {
		return make_shared<JacksRectangle>( side1, side2 );
	}

	shared_ptr<Triangle> CreateTriangle(int side1, int side2, int side3) {
		return make_shared<JacksTriangle>( side1, side2, side3 );
	}

};

// Concrete factory of Tom
class TomsFactory : public ShapesFactory {
public:

	shared_ptr<Circle> CreateCircle( int radius ) {
		return make_shared<TomsCircle>(radius);
	}

	shared_ptr<Rectangle> CreateRectangle(int side1, int side2) {
		return make_shared<TomsRectangle>(side1, side2);
	}

	shared_ptr<Triangle> CreateTriangle(int side1, int side2, int side3) {
		return make_shared<TomsTriangle>(side1, side2, side3);
	}

};

// Concrete factory of Mary
class MarysFactory : public ShapesFactory {
public:
	shared_ptr<Circle> CreateCircle(int radius) {
		return make_shared<MarysCircle>(radius);
	}

	shared_ptr<Rectangle> CreateRectangle(int side1, int side2) {
		return make_shared<MarysRectangle>(side1, side2);
	}

	shared_ptr<Triangle> CreateTriangle(int side1, int side2, int side3) {
		return make_shared<MarysTriangle>(side1, side2, side3);
	}
};

// Class containing all the shapes of one person
class ShapesBag {
public:

	void GetInfo() {
		for ( auto& c : circles )    { c->GetInfo(); }
		for ( auto& r : rectangles ) { r->GetInfo(); }
		for ( auto& t : triangles )  { t->GetInfo(); }
	}

	vector<shared_ptr<Circle>> circles;
	vector<shared_ptr<Rectangle>> rectangles;
	vector<shared_ptr<Triangle>> triangles;
};

// Class creating the set of shapes of one person
class Game {
public:

	shared_ptr<ShapesBag> CreateBag( ShapesFactory& factory ) {
		auto p = make_shared<ShapesBag>();
		p->circles.push_back( factory.CreateCircle( 6 ) );
		p->rectangles.push_back( factory.CreateRectangle( 3, 12 ) );
		p->triangles.push_back( factory.CreateTriangle( 1, 2, 3 ) );

		return p;
	}

};
