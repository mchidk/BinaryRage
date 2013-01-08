# BinaryRage - the ultra fast .NET document database
BinaryRage is designed to be a lightweight fast document database for .NET. 
No configuration, no server, no setup - simply reference the dll and start using it in less than a minute.

## Show me the code
sf

## Goals
Codebase and usage must be as simple as possible (but not simpler).

## Todo
- Better queue async writes of objects (must support rewriting the same object severel thousand times - where the last in queue wins and the rest is ignored)
- Include UnitTests

# FAQ
## Is it really fast?
We have tested more than 200,000 complex objects (documents) written to disk per second on a crappy laptop :-)
All writes are performed asynchronously.

## Why are you compressing the objects before written to disk?
The less I/O - the better. A compressed object is done less I/O because of less bytes written to disk.
