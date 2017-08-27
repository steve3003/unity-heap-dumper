# unity-heap-dumper
Utility to dump the heap of Unity projects.

The main goal of this tool is to find out what scene objects are still referenced after a change of scene.
For example if you have some static variable or DontDestroyOnLoad object that does't clear the reference of some objects of the previous scene.

Size of the objects in memory are provided for convenience but are not intended to provide the exact result.

I designed this tool to be easily extendible so that you can change it accordingly to your needs.

# Installation
Just copy this folder into your unity Assets folder.

# Usage
Set the output file and format in `UnityHeapDumper` script.

XML and JSON format are currently supported, just change the `UnityHeapDumper.dumpWriter` variable.

Select `Dump` under the `Tools` menu and a dump file will be produced.

# Credits
* https://github.com/tenpn/UnityHeapEx
* https://github.com/Zuntatos/UnityHeapDump
