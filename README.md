# ConfigLoader
Automatic customizable ConfigNode loader based on Source Generators

## Objectives
The goal of this project is to provide a simpler API for modders to interface with in order to efficiently pull data in and out from ConfigNodes in KSP.
Because of the internals of ConfigNode, getting data in and out of them often comes at either a performance cost, or a code readability cost.

This means two major goals must be met:
1. Add compile-time metadata allowing to automatically mark fields and properties as serialization targets, with customization options as desired.
2. Implement automatic serialization and deserialization of the marked data in a way that does not required further developer input, and incurs as little performance penality as possible

For this reason, rather than with a classical Reflection approach, I've decided to experiment with .NET Source Generators.
This allows for adding metadata normally through attributes to the code, and skips the performance cost of runtime reflection by performing that step at compile time, and generating custom serialization/deserialization code.
