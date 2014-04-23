
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.0.30319.17929
//  Copyright (c) Microsoft Corporation.  All rights reserved.


// ----- DOS Header:
// Magic:                      0x5a4d
// Bytes on last page:         0x0090
// Pages in file:              0x0003
// Relocations:                0x0000
// Size of header (paragraphs):0x0004
// Min extra paragraphs:       0x0000
// Max extra paragraphs:       0xffff
// Initial (relative) SS:      0x0000
// Initial SP:                 0x00b8
// Checksum:                   0x0000
// Initial IP:                 0x0000
// Initial (relative) CS:      0x0000
// File addr. of reloc table:  0x0040
// Overlay number:             0x0000
// OEM identifier:             0x0000
// OEM info:                   0x0000
// File addr. of COFF header:  0x0080
// ----- COFF/PE Headers:
// Signature:                  0x00004550
// ----- COFF Header:
// Machine:                    0x014c
// Number of sections:         0x0003
// Time-date stamp:            0x519d141c
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2102
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x0b
// Minor linker version:           0x00
// Size of code:                   0x00001a00
// Size of init.data:              0x00000600
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000389e
// Base of code:                   0x00002000
// Base of data:                   0x00004000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00008000
// Size of headers:                0x00000200
// Checksum:                       0x00000000
// Subsystem:                      0x0003
// DLL characteristics:            0x8540
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000384c [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000318] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00000000 [0x00000000] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003714 [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00002000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00002008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .text
//              0x000018a4 Virtual Size
//              0x00002000 Virtual Address
//              0x00001a00 Size of Raw Data
//              0x00000200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000318 Virtual Size
//              0x00004000 Virtual Address
//              0x00000400 Size of Raw Data
//              0x00001c00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00006000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00002000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000008a0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000388e Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// 

// Entry point code:
//FF 25 00 20 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000022d8 [0x0000143c] address [size] of Metadata Directory:        
// Flags:                              0x00000001
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00000000 [0x00000000] address [size] of Strong Name Signature:     
// 0x00000000 [0x00000000] address [size] of CodeManager Table:         
// 0x00000000 [0x00000000] address [size] of VTableFixups Directory:    
// 0x00000000 [0x00000000] address [size] of Export Address Table:      
// 0x00000000 [0x00000000] address [size] of Precompile Header:         

// Metadata Header
//    Storage Signature:
//              0x424a5342 Signature
//                  0x0001 Major Version
//                  0x0001 Minor Version
//              0x00000000 Extra Data Offset
//              0x0000000c Version String Length
//              'v4.0.30319' Version String
//    Storage Header:
//                    0x00 Flags
//                  0x0005 Number of Streams
//    Stream 1:
//              0x0000006c Offset
//              0x000008c8 Size
//              '#~' Name
//    Stream 2:
//              0x00000934 Offset
//              0x000008e4 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001218 Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x00001220 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001230 Offset
//              0x0000020c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000909a21757 MaskValid
//      0x000016003325fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// 

// File size            : 8704
// PE header size       : 512 (496 used)    ( 5.88%)
// PE additional info   : 991               (11.39%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.83%)
// CLR meta-data size  : 5180               (59.51%)
// CLR additional info : 0                  ( 0.00%)
// CLR method headers  : 250                ( 2.87%)
// Managed code         : 397               ( 4.56%)
// Data                 : 1536              (17.65%)
// Unaccounted          : -234              (-2.69%)

// Num.of PE sections   : 3
//   .text    - 6656
//   .rsrc    - 1024
//   .reloc   - 512

// CLR meta-data size  : 5180
//   Module        -    1 (10 bytes)
//   TypeDef       -    4 (56 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   27 (162 bytes)
//   MethodDef     -   41 (574 bytes)     0 abstract, 0 native, 41 bodies
//   FieldDef      -   19 (114 bytes)     0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -   19 (114 bytes)
//   CustomAttribute-   89 (534 bytes)
//   StandAloneSig -    9 (18 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -   19 (114 bytes)
//   MethodSemantic-   38 (228 bytes)
//   TypeSpec      -    2 (4 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   Strings       -  2275 bytes
//   Blobs         -   524 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   201 bytes

// CLR method headers : 250
//   Num.of method bodies  - 41
//   Num.of fat headers    - 19
//   Num.of tiny headers   - 22

// Managed code : 397
//   Ave method size - 9
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class TaskDto                        (public) (auto) (ansi) 
// Class TasksRequest                   (public) (auto) (ansi) 
// Class TasksResponse                  (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ ServiceStack.Interfaces
{
  .ver 3:9:33:0
}
.assembly extern /*23000003*/ System.Runtime.Serialization
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ Trex.Common
{
  .custom /*0C000004:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000017*/::.ctor(int32) /* 0A00000F */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A000004 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000006:0A000005*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000C*/::.ctor(string) /* 0A000005 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000007:0A00000D*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000014*/::.ctor(string) /* 0A00000D */ = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom /*0C000008:0A000010*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000018*/::.ctor() /* 0A000010 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C000009:0A000001*/ instance void [mscorlib/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000001 */ = ( 01 00 1A 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                     2C 56 65 72 73 69 6F 6E 3D 76 34 2E 30 01 00 54   // ,Version=v4.0..T
                                                                                                                                                                     0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C   // ..FrameworkDispl
                                                                                                                                                                     61 79 4E 61 6D 65 10 2E 4E 45 54 20 46 72 61 6D   // ayName..NET Fram
                                                                                                                                                                     65 77 6F 72 6B 20 34 )                            // ework 4
  .custom /*0C00000A:0A000002*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000009*/::.ctor(string) /* 0A000002 */ = ( 01 00 0B 54 72 65 78 2E 43 6F 6D 6D 6F 6E 00 00 ) // ...Trex.Common..
  .custom /*0C00000B:0A000003*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000A*/::.ctor(string) /* 0A000003 */ = ( 01 00 00 00 00 ) 

  // 
  //  .custom /*0C00000C:0A00000E*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000015*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000015*//DebuggingModes/*01000016*/) /* 0A00000E */ = ( 01 00 07 01 00 00 00 00 ) 

  .custom /*0C00000D:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000D*/::.ctor(string) /* 0A000006 */ = ( 01 00 0B 54 72 65 78 2E 43 6F 6D 6D 6F 6E 00 00 ) // ...Trex.Common..
  .custom /*0C00000E:0A000007*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000E*/::.ctor(string) /* 0A000007 */ = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                                                                               20 32 30 31 33 00 00 )                            //  2013..
  .custom /*0C00000F:0A000008*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTrademarkAttribute/*0100000F*/::.ctor(string) /* 0A000008 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000010:0A00000A*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000011*/::.ctor(bool) /* 0A00000A */ = ( 01 00 00 00 00 ) 
  .custom /*0C000011:0A00000B*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.GuidAttribute/*01000012*/::.ctor(string) /* 0A00000B */ = ( 01 00 24 61 66 39 63 39 36 39 66 2D 30 62 65 32   // ..$af9c969f-0be2
                                                                                                                                                               2D 34 36 62 34 2D 39 30 63 65 2D 31 63 63 39 37   // -46b4-90ce-1cc97
                                                                                                                                                               61 35 32 33 37 33 33 00 00 )                      // a523733..
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module Trex.Common.dll
// MVID: {8DEDCED3-FD9A-485E-80FC-F578C3CCC341}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x01110000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit Trex.Common.DataTransferObjects.TaskDto
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000014:0A000011*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataContractAttribute/*01000019*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ private int32 '<Id>k__BackingField'
  .custom /*0C000002:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000002*/ private valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> '<ParentTaskId>k__BackingField'
  .custom /*0C000013:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000003*/ private valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ '<Guid>k__BackingField'
  .custom /*0C000017:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ private string '<CreatedByName>k__BackingField'
  .custom /*0C00001A:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ private valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ '<CreateDate>k__BackingField'
  .custom /*0C00001D:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ private string '<Name>k__BackingField'
  .custom /*0C000020:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000007*/ private string '<Description>k__BackingField'
  .custom /*0C000023:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ private float64 '<TimeLeft>k__BackingField'
  .custom /*0C000026:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000009*/ private float64 '<TimeEstimated>k__BackingField'
  .custom /*0C000029:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000A*/ private float64 '<WorstCaseEstimate>k__BackingField'
  .custom /*0C00002C:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000B*/ private float64 '<BestCaseEstimate>k__BackingField'
  .custom /*0C00002F:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ private float64 '<RealisticEstimate>k__BackingField'
  .custom /*0C000032:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ private float64 '<RegisteredTime>k__BackingField'
  .custom /*0C000035:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000E*/ private int32 '<ProjectId>k__BackingField'
  .custom /*0C000038:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000F*/ private valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> '<ChangeDate>k__BackingField'
  .custom /*0C00003B:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000010*/ private string '<ChangedByName>k__BackingField'
  .custom /*0C00003E:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000011*/ private bool '<Inactive>k__BackingField'
  .custom /*0C000041:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname 
          instance int32  get_Id() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000001:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000001*/ init (int32 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000001       */ ldfld      int32 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Id>k__BackingField' /* 04000001 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_Id

  .method /*06000002*/ public hidebysig specialname 
          instance void  set_Id(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000012:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000001       */ stfld      int32 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Id>k__BackingField' /* 04000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_Id

  .method /*06000003*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> 
          get_ParentTaskId() cil managed
  // SIG: 20 00 15 11 11 01 08
  {
    .custom /*0C000016:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000002*/ init (valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000002       */ ldfld      valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ParentTaskId>k__BackingField' /* 04000002 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_ParentTaskId

  .method /*06000004*/ public hidebysig specialname 
          instance void  set_ParentTaskId(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> 'value') cil managed
  // SIG: 20 01 01 15 11 11 01 08
  {
    .custom /*0C000019:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000002       */ stfld      valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ParentTaskId>k__BackingField' /* 04000002 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_ParentTaskId

  .method /*06000005*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ 
          get_Guid() cil managed
  // SIG: 20 00 11 15
  {
    .custom /*0C00001C:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000003*/ init (valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000003       */ ldfld      valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Guid>k__BackingField' /* 04000003 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_Guid

  .method /*06000006*/ public hidebysig specialname 
          instance void  set_Guid(valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ 'value') cil managed
  // SIG: 20 01 01 11 15
  {
    .custom /*0C00001F:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000003       */ stfld      valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Guid>k__BackingField' /* 04000003 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_Guid

  .method /*06000007*/ public hidebysig specialname 
          instance string  get_CreatedByName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000022:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000004*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000004       */ ldfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<CreatedByName>k__BackingField' /* 04000004 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_CreatedByName

  .method /*06000008*/ public hidebysig specialname 
          instance void  set_CreatedByName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000025:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000004       */ stfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<CreatedByName>k__BackingField' /* 04000004 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_CreatedByName

  .method /*06000009*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ 
          get_CreateDate() cil managed
  // SIG: 20 00 11 19
  {
    .custom /*0C000028:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000005*/ init (valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000005       */ ldfld      valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<CreateDate>k__BackingField' /* 04000005 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_CreateDate

  .method /*0600000A*/ public hidebysig specialname 
          instance void  set_CreateDate(valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ 'value') cil managed
  // SIG: 20 01 01 11 19
  {
    .custom /*0C00002B:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000005       */ stfld      valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<CreateDate>k__BackingField' /* 04000005 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_CreateDate

  .method /*0600000B*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00002E:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000004*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000006       */ ldfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Name>k__BackingField' /* 04000006 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_Name

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000031:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000006       */ stfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Name>k__BackingField' /* 04000006 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_Name

  .method /*0600000D*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000034:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000004*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000007       */ ldfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Description>k__BackingField' /* 04000007 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_Description

  .method /*0600000E*/ public hidebysig specialname 
          instance void  set_Description(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000037:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000007       */ stfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Description>k__BackingField' /* 04000007 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_Description

  .method /*0600000F*/ public hidebysig specialname 
          instance float64  get_TimeLeft() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00003A:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000006*/ init (float64 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000008       */ ldfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<TimeLeft>k__BackingField' /* 04000008 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_TimeLeft

  .method /*06000010*/ public hidebysig specialname 
          instance void  set_TimeLeft(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00003D:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000008       */ stfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<TimeLeft>k__BackingField' /* 04000008 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_TimeLeft

  .method /*06000011*/ public hidebysig specialname 
          instance float64  get_TimeEstimated() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000040:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000006*/ init (float64 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000009       */ ldfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<TimeEstimated>k__BackingField' /* 04000009 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_TimeEstimated

  .method /*06000012*/ public hidebysig specialname 
          instance void  set_TimeEstimated(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000043:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000009       */ stfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<TimeEstimated>k__BackingField' /* 04000009 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_TimeEstimated

  .method /*06000013*/ public hidebysig specialname 
          instance float64  get_WorstCaseEstimate() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000045:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000006*/ init (float64 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000A       */ ldfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<WorstCaseEstimate>k__BackingField' /* 0400000A */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_WorstCaseEstimate

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_WorstCaseEstimate(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000047:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000A       */ stfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<WorstCaseEstimate>k__BackingField' /* 0400000A */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_WorstCaseEstimate

  .method /*06000015*/ public hidebysig specialname 
          instance float64  get_BestCaseEstimate() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000048:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000006*/ init (float64 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000B       */ ldfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<BestCaseEstimate>k__BackingField' /* 0400000B */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_BestCaseEstimate

  .method /*06000016*/ public hidebysig specialname 
          instance void  set_BestCaseEstimate(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000049:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000B       */ stfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<BestCaseEstimate>k__BackingField' /* 0400000B */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_BestCaseEstimate

  .method /*06000017*/ public hidebysig specialname 
          instance float64  get_RealisticEstimate() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00004A:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000006*/ init (float64 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000C       */ ldfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<RealisticEstimate>k__BackingField' /* 0400000C */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_RealisticEstimate

  .method /*06000018*/ public hidebysig specialname 
          instance void  set_RealisticEstimate(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00004B:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000C       */ stfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<RealisticEstimate>k__BackingField' /* 0400000C */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_RealisticEstimate

  .method /*06000019*/ public hidebysig specialname 
          instance float64  get_RegisteredTime() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00004C:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000006*/ init (float64 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000D       */ ldfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<RegisteredTime>k__BackingField' /* 0400000D */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_RegisteredTime

  .method /*0600001A*/ public hidebysig specialname 
          instance void  set_RegisteredTime(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00004D:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000D       */ stfld      float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<RegisteredTime>k__BackingField' /* 0400000D */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_RegisteredTime

  .method /*0600001B*/ public hidebysig specialname 
          instance int32  get_ProjectId() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00004E:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000001*/ init (int32 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000E       */ ldfld      int32 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ProjectId>k__BackingField' /* 0400000E */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_ProjectId

  .method /*0600001C*/ public hidebysig specialname 
          instance void  set_ProjectId(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00004F:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000E       */ stfld      int32 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ProjectId>k__BackingField' /* 0400000E */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_ProjectId

  .method /*0600001D*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> 
          get_ChangeDate() cil managed
  // SIG: 20 00 15 11 11 01 11 19
  {
    .custom /*0C000050:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000007*/ init (valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000F       */ ldfld      valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ChangeDate>k__BackingField' /* 0400000F */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_ChangeDate

  .method /*0600001E*/ public hidebysig specialname 
          instance void  set_ChangeDate(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> 'value') cil managed
  // SIG: 20 01 01 15 11 11 01 11 19
  {
    .custom /*0C000051:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000F       */ stfld      valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ChangeDate>k__BackingField' /* 0400000F */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_ChangeDate

  .method /*0600001F*/ public hidebysig specialname 
          instance string  get_ChangedByName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000052:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000004*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000010       */ ldfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ChangedByName>k__BackingField' /* 04000010 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_ChangedByName

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_ChangedByName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000053:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000010       */ stfld      string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<ChangedByName>k__BackingField' /* 04000010 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_ChangedByName

  .method /*06000021*/ public hidebysig specialname 
          instance bool  get_Inactive() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000054:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000008*/ init (bool V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000011       */ ldfld      bool Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Inactive>k__BackingField' /* 04000011 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TaskDto::get_Inactive

  .method /*06000022*/ public hidebysig specialname 
          instance void  set_Inactive(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000055:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000011       */ stfld      bool Trex.Common.DataTransferObjects.TaskDto/*02000002*/::'<Inactive>k__BackingField' /* 04000011 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TaskDto::set_Inactive

  .method /*06000023*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TaskDto::.ctor

  .property /*17000001*/ instance int32 Id()
  {
    .custom /*0C000003:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance int32 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_Id() /* 06000001 */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_Id(int32) /* 06000002 */
  } // end of property TaskDto::Id
  .property /*17000002*/ instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32>
          ParentTaskId()
  {
    .custom /*0C000015:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_ParentTaskId(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32>) /* 06000004 */
    .get instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<int32> Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_ParentTaskId() /* 06000003 */
  } // end of property TaskDto::ParentTaskId
  .property /*17000003*/ instance valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/
          Guid()
  {
    .custom /*0C000018:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_Guid(valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/) /* 06000006 */
    .get instance valuetype [mscorlib/*23000001*/]System.Guid/*01000005*/ Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_Guid() /* 06000005 */
  } // end of property TaskDto::Guid
  .property /*17000004*/ instance string CreatedByName()
  {
    .custom /*0C00001B:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_CreatedByName() /* 06000007 */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_CreatedByName(string) /* 06000008 */
  } // end of property TaskDto::CreatedByName
  .property /*17000005*/ instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/
          CreateDate()
  {
    .custom /*0C00001E:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/ Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_CreateDate() /* 06000009 */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_CreateDate(valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/) /* 0600000A */
  } // end of property TaskDto::CreateDate
  .property /*17000006*/ instance string Name()
  {
    .custom /*0C000021:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_Name() /* 0600000B */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_Name(string) /* 0600000C */
  } // end of property TaskDto::Name
  .property /*17000007*/ instance string Description()
  {
    .custom /*0C000024:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_Description(string) /* 0600000E */
    .get instance string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_Description() /* 0600000D */
  } // end of property TaskDto::Description
  .property /*17000008*/ instance float64
          TimeLeft()
  {
    .custom /*0C000027:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_TimeLeft(float64) /* 06000010 */
    .get instance float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_TimeLeft() /* 0600000F */
  } // end of property TaskDto::TimeLeft
  .property /*17000009*/ instance float64
          TimeEstimated()
  {
    .custom /*0C00002A:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_TimeEstimated() /* 06000011 */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_TimeEstimated(float64) /* 06000012 */
  } // end of property TaskDto::TimeEstimated
  .property /*1700000A*/ instance float64
          WorstCaseEstimate()
  {
    .custom /*0C00002D:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_WorstCaseEstimate() /* 06000013 */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_WorstCaseEstimate(float64) /* 06000014 */
  } // end of property TaskDto::WorstCaseEstimate
  .property /*1700000B*/ instance float64
          BestCaseEstimate()
  {
    .custom /*0C000030:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_BestCaseEstimate() /* 06000015 */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_BestCaseEstimate(float64) /* 06000016 */
  } // end of property TaskDto::BestCaseEstimate
  .property /*1700000C*/ instance float64
          RealisticEstimate()
  {
    .custom /*0C000033:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_RealisticEstimate(float64) /* 06000018 */
    .get instance float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_RealisticEstimate() /* 06000017 */
  } // end of property TaskDto::RealisticEstimate
  .property /*1700000D*/ instance float64
          RegisteredTime()
  {
    .custom /*0C000036:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_RegisteredTime(float64) /* 0600001A */
    .get instance float64 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_RegisteredTime() /* 06000019 */
  } // end of property TaskDto::RegisteredTime
  .property /*1700000E*/ instance int32 ProjectId()
  {
    .custom /*0C000039:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance int32 Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_ProjectId() /* 0600001B */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_ProjectId(int32) /* 0600001C */
  } // end of property TaskDto::ProjectId
  .property /*1700000F*/ instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/>
          ChangeDate()
  {
    .custom /*0C00003C:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_ChangeDate() /* 0600001D */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_ChangeDate(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/>) /* 0600001E */
  } // end of property TaskDto::ChangeDate
  .property /*17000010*/ instance string ChangedByName()
  {
    .custom /*0C00003F:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .get instance string Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_ChangedByName() /* 0600001F */
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_ChangedByName(string) /* 06000020 */
  } // end of property TaskDto::ChangedByName
  .property /*17000011*/ instance bool Inactive()
  {
    .custom /*0C000042:0A000014*/ instance void [System.Runtime.Serialization/*23000003*/]System.Runtime.Serialization.DataMemberAttribute/*0100001B*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .set instance void Trex.Common.DataTransferObjects.TaskDto/*02000002*/::set_Inactive(bool) /* 06000022 */
    .get instance bool Trex.Common.DataTransferObjects.TaskDto/*02000002*/::get_Inactive() /* 06000021 */
  } // end of property TaskDto::Inactive
} // end of class Trex.Common.DataTransferObjects.TaskDto

.class /*02000003*/ public auto ansi beforefieldinit Trex.Common.ServiceStack.TasksRequest
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class [ServiceStack.Interfaces/*23000002*/]ServiceStack.ServiceHost.IReturn`1/*01000002*/<class Trex.Common.ServiceStack.TasksResponse/*02000004*/>/*1B000001*/,
                  [ServiceStack.Interfaces/*23000002*/]ServiceStack.ServiceHost.IReturn/*01000003*/
{
  .field /*04000012*/ private valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> '<LastSyncDate>k__BackingField'
  .custom /*0C000044:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000024*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> 
          get_LastSyncDate() cil managed
  // SIG: 20 00 15 11 11 01 11 19
  {
    .custom /*0C000056:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000007*/ init (valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000012       */ ldfld      valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> Trex.Common.ServiceStack.TasksRequest/*02000003*/::'<LastSyncDate>k__BackingField' /* 04000012 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TasksRequest::get_LastSyncDate

  .method /*06000025*/ public hidebysig specialname 
          instance void  set_LastSyncDate(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> 'value') cil managed
  // SIG: 20 01 01 15 11 11 01 11 19
  {
    .custom /*0C000057:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000012       */ stfld      valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> Trex.Common.ServiceStack.TasksRequest/*02000003*/::'<LastSyncDate>k__BackingField' /* 04000012 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TasksRequest::set_LastSyncDate

  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TasksRequest::.ctor

  .property /*17000012*/ instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/>
          LastSyncDate()
  {
    .set instance void Trex.Common.ServiceStack.TasksRequest/*02000003*/::set_LastSyncDate(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/>) /* 06000025 */
    .get instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000004*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000006*/> Trex.Common.ServiceStack.TasksRequest/*02000003*/::get_LastSyncDate() /* 06000024 */
  } // end of property TasksRequest::LastSyncDate
} // end of class Trex.Common.ServiceStack.TasksRequest

.class /*02000004*/ public auto ansi beforefieldinit Trex.Common.ServiceStack.TasksResponse
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000013*/ private class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> '<Tasks>k__BackingField'
  .custom /*0C000046:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000013 */
    IL_0006:  /* 00   |                  */ nop
    IL_0007:  /* 00   |                  */ nop
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 73   | (0A)000015       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/>/*1B000002*/::.ctor() /* 0A000015 */
    IL_000e:  /* 28   | (06)000029       */ call       instance void Trex.Common.ServiceStack.TasksResponse/*02000004*/::set_Tasks(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/>) /* 06000029 */
    IL_0013:  /* 00   |                  */ nop
    IL_0014:  /* 00   |                  */ nop
    IL_0015:  /* 2A   |                  */ ret
  } // end of method TasksResponse::.ctor

  .method /*06000028*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> 
          get_Tasks() cil managed
  // SIG: 20 00 15 12 1D 01 12 08
  {
    .custom /*0C000058:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  1
    .locals /*11000009*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000013       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> Trex.Common.ServiceStack.TasksResponse/*02000004*/::'<Tasks>k__BackingField' /* 04000013 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method TasksResponse::get_Tasks

  .method /*06000029*/ public hidebysig specialname 
          instance void  set_Tasks(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> 'value') cil managed
  // SIG: 20 01 01 15 12 1D 01 12 08
  {
    .custom /*0C000059:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // 
    // 
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000013       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> Trex.Common.ServiceStack.TasksResponse/*02000004*/::'<Tasks>k__BackingField' /* 04000013 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TasksResponse::set_Tasks

  .property /*17000013*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/>
          Tasks()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/> Trex.Common.ServiceStack.TasksResponse/*02000004*/::get_Tasks() /* 06000028 */
    .set instance void Trex.Common.ServiceStack.TasksResponse/*02000004*/::set_Tasks(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000007*/<class Trex.Common.DataTransferObjects.TaskDto/*02000002*/>) /* 06000029 */
  } // end of property TasksResponse::Tasks
} // end of class Trex.Common.ServiceStack.TasksResponse


// =============================================================

// 
// 
