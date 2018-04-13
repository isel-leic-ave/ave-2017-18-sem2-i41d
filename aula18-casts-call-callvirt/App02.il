
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.6.1055.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly 'App02-callvirt'
{
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 ) 

  .hash algorithm 0x00008004
  .ver 0:0:0:0
}
.module 'App02-callvirt.exe'
// MVID: {DA85CC1F-71A0-405A-9B9B-7C247FF35DE6}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x00670000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit Person
       extends [mscorlib]System.Object
{
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  } // end of method Person::.ctor

} // end of class Person

.class public auto ansi beforefieldinit Student
       extends Person
{
  .field private int32 nr
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void Person::.ctor()
    IL_0006:  nop
    IL_0007:  nop
    IL_0008:  ret
  } // end of method Student::.ctor

  .method public hidebysig instance void 
          Print() cil managed
  {
    // Code size       29 (0x1d)
    .maxstack  8
    IL_0000:  nop
    IL_0001:  ldstr      "I am a Student!!!!"
    IL_0006:  ldarg.0
    IL_0007:  ldfld      int32 Student::nr
    IL_000c:  box        [mscorlib]System.Int32
    IL_0011:  call       string [mscorlib]System.String::Concat(object,
                                                                object)
    IL_0016:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_001b:  nop
    IL_001c:  ret
  } // end of method Student::Print

} // end of class Student

.class public auto ansi beforefieldinit App
       extends [mscorlib]System.Object
{
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .entrypoint
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (class Student V_0)
    IL_0000:  nop
    IL_0001:  ldnull
    IL_0002:  stloc.0
    IL_0003:  ldloc.0
    IL_0004:  call   instance void Student::Print()
    IL_0009:  nop
    IL_000a:  ret
  } // end of method App::Main

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  } // end of method App::.ctor

} // end of class App


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\MyFolder\ISEL\pg5 ave - 2017-2018 - 2º sem\aulas\aula18-casts-call-callvirt\App02.res
