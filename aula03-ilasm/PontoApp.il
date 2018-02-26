
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.6.1055.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern Ponto
{
  .ver 0:0:0:0
}
.assembly PontoApp
{
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 ) 

  .hash algorithm 0x00008004
  .ver 0:0:0:0
}
.module PontoApp.exe
// MVID: {C55C4564-8253-4ACD-AD9A-2D428B28D927}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x00950000


// =============== CLASS MEMBERS DECLARATION ===================

.class private auto ansi beforefieldinit PontoApp
       extends [mscorlib]System.Object
{
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .entrypoint
    // Code size       71 (0x47)
    .maxstack  2
    .locals init (class [Ponto]Ponto V_0)
    IL_0000:  nop
    IL_0001:  ldc.i4.5
    IL_0002:  ldc.i4.7
    IL_0003:  newobj     instance void [Ponto]Ponto::.ctor(int32,
                                                           int32)
    IL_0008:  stloc.0
    IL_0009:  ldloc.0
    IL_000a:  callvirt   instance void [Ponto]Ponto::print()
    IL_000f:  nop
    IL_0010:  ldstr      "p._x = {0}\n"
    IL_0015:  ldloc.0
    IL_0016:  ldfld      int32 [Ponto]Ponto::_x
    IL_001b:  box        [mscorlib]System.Int32
    IL_0020:  call       string [mscorlib]System.String::Format(string,
                                                                object)
    IL_0025:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_002a:  nop
    IL_002b:  ldstr      "p._y = {0}\n"
    IL_0030:  ldloc.0
    IL_0031:  ldfld      int32 [Ponto]Ponto::_z
    IL_0036:  box        [mscorlib]System.Int32
    IL_003b:  call       string [mscorlib]System.String::Format(string,
                                                                object)
    IL_0040:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_0045:  nop
    IL_0046:  ret
  } // end of method PontoApp::Main

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  } // end of method PontoApp::.ctor

} // end of class PontoApp


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\MyFolder\ISEL\pg5 ave - 2017-2018 - 2º sem\aulas\aula03-ilasm\PontoApp.res
