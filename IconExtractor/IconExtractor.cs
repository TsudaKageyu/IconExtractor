/*
 *  IconExtractor.cs
 *  Copyright (C) 2014 Tsuda Kageyu. All rights reserved.
 *
 *  Redistribution and use in source and binary forms, with or without
 *  modification, are permitted provided that the following conditions
 *  are met:
 *
 *   1. Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *   2. Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 *  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
 *  TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 *  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER
 *  OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 *  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 *  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 *  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 *  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace TsudaKageyu
{
    public class IconExtractor
    {
        private static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

            [DllImport("kernel32.dll", SetLastError = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr FreeLibrary(IntPtr hModule);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [SuppressUnmanagedCodeSecurity]
            public static extern bool EnumResourceNames(IntPtr hModule, IntPtr lpszType, ENUMRESNAMEPROC lpEnumFunc, IntPtr lParam);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

            [DllImport("kernel32.dll", SetLastError = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

            [DllImport("kernel32.dll", SetLastError = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr LockResource(IntPtr hResData);

            [DllImport("kernel32.dll", SetLastError = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        [SuppressUnmanagedCodeSecurity]
        private delegate bool ENUMRESNAMEPROC(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);

        ////////////////////////////////////////////////////////////////////////
        // Constants

        // Flags for LoadLibraryEx().

        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

        // Resource types for EnumResourceNames().

        private readonly static IntPtr RT_ICON = (IntPtr)3;
        private readonly static IntPtr RT_GROUP_ICON = (IntPtr)14;

        ////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// Resource data of the type RT_GROUP_ICON, or icon headers.
        /// </summary>
        private List<byte[]> iconDirs = new List<byte[]>();

        /// <summary>
        /// Resource data of the type RT_ICON, or icon pictures.
        /// </summary>
        private Dictionary<ushort, byte[]> iconPics = new Dictionary<ushort, byte[]>();

        ////////////////////////////////////////////////////////////////////////
        // Public properties

        /// <summary>
        /// Count of the icons in the file.
        /// </summary>
        public int Count
        {
            get { return iconDirs.Count; }
        }

        /// <summary>
        /// Initializes a new instance of the IconExtractor class from the specified file name.
        /// </summary>
        /// <param name="fileName">The file to extract icons from.</param>
        public IconExtractor(string fileName)
        {
            Initialize(fileName);
        }

        /// <summary>
        /// Extracts an icon from the file.
        /// </summary>
        /// <param name="index">Zero based index of the icon to be extracted.</param>
        /// <returns>A System.Drawing.Icon object.</returns>
        /// <remarks>Always returns new copy of the Icon. It should be disposed by the user.</remarks>
        public Icon GetIcon(int index)
        {
            if (index < 0 || Count <= index)
                throw new ArgumentOutOfRangeException("index");

            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                var dir = iconDirs[index];

                // Copy GRPICONDIR to ICONDIR.

                // #pragma pack( push )
                // #pragma pack( 2 )
                // typedef struct
                // {
                //    WORD            idReserved;   // Reserved (must be 0)
                //    WORD            idType;       // Resource type (1 for icons)
                //    WORD            idCount;      // How many images?
                // } GRPICONDIR, *LPGRPICONDIR;
                // #pragma pack( pop )

                // typedef struct
                // {
                //     WORD           idReserved;   // Reserved (must be 0)
                //     WORD           idType;       // Resource Type (1 for icons)
                //     WORD           idCount;      // How many images?
                // } ICONDIR, *LPICONDIR;

                writer.Write(dir, 0, 6);    // Just copy as they are identical.

                int count = BitConverter.ToUInt16(dir, 4);  // GRPICONDIR.idCount
                int offset = 6 + 16 * count;                // sizeof(ICONDIR) + sizeof(ICONDIRENTRY) * count
                byte[][] pics = new byte[count][];

                for (int i = 0; i < count; ++i)
                {
                    // Copy GRPICONDIRENTRY to ICONDIRENTRY.

                    // #pragma pack( push )
                    // #pragma pack( 2 )
                    // typedef struct
                    // {
                    //    BYTE   bWidth;           // Width, in pixels, of the image
                    //    BYTE   bHeight;          // Height, in pixels, of the image
                    //    BYTE   bColorCount;      // Number of colors in image (0 if >=8bpp)
                    //    BYTE   bReserved;        // Reserved
                    //    WORD   wPlanes;          // Color Planes
                    //    WORD   wBitCount;        // Bits per pixel
                    //    DWORD  dwBytesInRes;     // how many bytes in this resource?
                    //    WORD   nID;              // the ID
                    // } GRPICONDIRENTRY, *LPGRPICONDIRENTRY;
                    // #pragma pack( pop )

                    // typedef struct
                    // {
                    //     BYTE   bWidth;          // Width, in pixels, of the image
                    //     BYTE   bHeight;         // Height, in pixels, of the image
                    //     BYTE   bColorCount;     // Number of colors in image (0 if >=8bpp)
                    //     BYTE   bReserved;       // Reserved ( must be 0)
                    //     WORD   wPlanes;         // Color Planes
                    //     WORD   wBitCount;       // Bits per pixel
                    //     DWORD  dwBytesInRes;    // How many bytes in this resource?
                    //     DWORD  dwImageOffset;   // Where in the file is this image?
                    // } ICONDIRENTRY, *LPICONDIRENTRY;

                    writer.Write(dir, 6 + 14 * i, 12);  // First 12bytes are identical.
                    writer.Write(offset);               // Write offset instead of ID.

                    ushort id = BitConverter.ToUInt16(dir, 6 + 14 * i + 12);    // GRPICONDIRENTRY.nID
                    pics[i] = iconPics[id];

                    offset += pics[i].Length;
                }

                // Copy pictures.

                for (int i = 0; i < count; ++i)
                    writer.Write(pics[i], 0, pics[i].Length);

                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                return new Icon(writer.BaseStream);
            }
        }

        /// <summary>
        /// Extracts all the icons from the file.
        /// </summary>
        /// <returns>An array of System.Drawing.Icon objects.</returns>
        /// <remarks>Always returns new copies of the Icons. They should be disposed by the user.</remarks>
        public Icon[] GetAllIcons()
        {
            var icons = new List<Icon>();
            for (int i = 0; i < Count; ++i)
                icons.Add(GetIcon(i));

            return icons.ToArray();
        }

        /// <summary>
        /// Split an Icon consists of multiple icons into an array of Icon each 
        /// consists of single icons.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon to be split.</param>
        /// <returns>An array of System.Drawing.Icon.</returns>
        public static Icon[] SplitIcon(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Create an .ico file in memory, then split it into separate icons.

            byte[] src = null;
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                src = ms.ToArray();
            }

            var splitIcons = new List<Icon>();
            {
                int count = BitConverter.ToUInt16(src, 4);

                for (int i = 0; i < count; i++)
                {
                    using (var writer = new BinaryWriter(new MemoryStream()))
                    {
                        // Copy ICONDIR and set idCount to 1.

                        writer.Write(src, 0, 4);
                        writer.Write((short)1);

                        // Copy ICONDIRENTRY and set dwImageOffset to 22.

                        writer.Write(src, 6 + 16 * i, 12);
                        writer.Write(22);

                        // Copy a picture.

                        int size = BitConverter.ToInt32(src, 6 + 16 * i + 8);
                        int offset = BitConverter.ToInt32(src, 6 + 16 * i + 12);
                        writer.Write(src, offset, size);

                        // Create an icon from the in-memory file.

                        writer.BaseStream.Seek(0, SeekOrigin.Begin);
                        splitIcons.Add(new Icon(writer.BaseStream));
                    }
                }
            }

            return splitIcons.ToArray();
        }

        /// <summary>
        /// Converts an Icon to a GDI+ Bitmap preserving the transparent area.
        /// </summary>
        /// <param name="icon">An System.Drawing.Icon to be coverted.</param>
        /// <returns>A System.Drawing.Bitmap Object.</returns>
        public static Bitmap IconToBitmap(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Quick workaround: Create an .ico file in memory, then load it as a Bitmap.

            Bitmap bmp;
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                bmp = (Bitmap)Image.FromStream(ms);
            }

            return bmp;
        }

        private void Initialize(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            IntPtr hModule = IntPtr.Zero;
            try
            {
                hModule = NativeMethods.LoadLibraryEx(fileName, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
                if (hModule == IntPtr.Zero)
                    throw new Win32Exception();

                ENUMRESNAMEPROC callback = (hMod, type, name, lparam) =>
                {
                    if (type == RT_GROUP_ICON)
                        iconDirs.Add(GetDataFromResource(hMod, type, name));
                    else if (type == RT_ICON)
                        iconPics.Add((ushort)name, GetDataFromResource(hMod, type, name));

                    return true;
                };
                NativeMethods.EnumResourceNames(hModule, RT_GROUP_ICON, callback, IntPtr.Zero);
                NativeMethods.EnumResourceNames(hModule, RT_ICON, callback, IntPtr.Zero);
            }
            finally
            {
                if (hModule != IntPtr.Zero)
                    NativeMethods.FreeLibrary(hModule);
            }
        }

        private byte[] GetDataFromResource(IntPtr hModule, IntPtr type, IntPtr name)
        {
            IntPtr hResInfo = NativeMethods.FindResource(hModule, name, type);
            if (hResInfo == IntPtr.Zero)
                throw new Win32Exception();

            IntPtr hResData = NativeMethods.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
                throw new Win32Exception();

            IntPtr pResData = NativeMethods.LockResource(hResData);
            if (pResData == IntPtr.Zero)
                throw new Win32Exception();

            uint size = NativeMethods.SizeofResource(hModule, hResInfo);
            if (size == 0)
                throw new Win32Exception();

            byte[] buf = new byte[size];
            Marshal.Copy(pResData, buf, 0, buf.Length);

            return buf;
        }
    }
}
