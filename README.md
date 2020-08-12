# Unity_8BitComputerSimulator
An 8-bit computer simulator built in Unity that shows how components of a computer actually work.

## Installation (for Unity Editor)
Clone or download the repository 

Open UnityHub, click add, and select Unity_8BitComputerSimulator-master
## Installation (for Standalone Windows Build)
Download Unity_8BitComputerSimulator/8BitSimulator_Builds.zip

Open and unzip 8BitSimulator_Builds and double-click 8BitComputerSimulator.exe

## Usage 
Show Code Button -> Pops up code text box for writing

Delay Speed Slider -> The delay per operation in seconds

Show Log Button  -> Pops up a log of all of the operations of the overall computer

Start Program Button -> Start/Reset the program

Hover over any component to see what it is via your mouse

Components colored Cyan are in use for the current operation

## Examples

```bash
LDI 3
STA 14
ADD 14
```

This does an immediate load (LDI) into the A register, stores the contents of the A register into memory address 14, and finally adds the contents of memory address 14 to the A register. The final result is 6 which is showed in the A register and O (output) register.

Note: the output register is not always utilized, typically only when the B register is needed for ALU operations.

## Opcodes
```bash
LDA - 0001 XXXX - loads the contents from XXXX into the A register, where XXXX is the memory address location
ADD - 0010 XXXX - adds the contents of XXXX to the A register, where XXXX is the memory address location
SUB - 0011 XXXX - subtracts the contents of XXXX to the A register, where XXXX is the memory address location
STA - 0100 XXXX - stores the contents of the A register into the memory address XXXX
LDI - 0101 XXXX - does an immediate load of the value XXXX to the A register
JMP - 0110 XXXX - jumps the program counter to XXXX - NOTE: do not use this, conditionals have not been setup yet
```
## Notes
I suggest having an understanding of how the individual components of a computer work. This should be used to aid that process or experiment with low level operations.
Trying out some basic programs, having a large delay time, and checking each operation via the log will seriously help your understanding. 
Also, I suggest hovering over the components to see what is what and to get familiar with the scene.

## References
Ben Eater is awesome. If you have any questions or confusions I strongly suggest watching his series on building an 8-bit computer. This is how I was able to implement the computer in Unity.

Link: https://www.youtube.com/watch?v=HyznrdDSSGM&list=PLowKtXNTBypGqImE405J2565dvjafglHU
