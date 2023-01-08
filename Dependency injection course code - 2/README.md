# Notes

## Composition vs Inheritance

### What Is the Difference Between Inheritance and Composition?
Inheritance is all about the specialization of a general concept. The derived class is a specialized version of the base class and promotes code reuse. It implicitly inherits all non-private members of its base classes, whether direct or indirect. It can also hide or override the inherited members.

On the other hand, the composition is about the association of objects of different classes. It enables code reuse by adding a reference to another object instead of inheriting the complete implementation.

### Composition Over Inheritance
The main problem with inheritance is that it may lead to a deep hierarchy of classes. This hierarchy is fragile, and the implementation of derived classes can break or be forced to change with any change at the top of the hierarchy.
