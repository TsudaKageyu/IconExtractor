/*
 *  IconExtractor/IconUtil for .NET
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
using System.Drawing;
using System.IO;

namespace TsudaKageyu
{
    public static class IconUtil
    {
        /// <summary>
        /// Split an Icon consists of multiple icons into an array of Icon each 
        /// consists of single icons.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon to be split.</param>
        /// <returns>An array of System.Drawing.Icon.</returns>
        public static Icon[] Split(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Create an .ico file in memory, then split it into separate icons.

            var data = GetIconData(icon);

            var splitIcons = new List<Icon>();
            {
                int count = BitConverter.ToUInt16(data, 4);

                for (int i = 0; i < count; i++)
                {
                    using (var writer = new BinaryWriter(new MemoryStream()))
                    {
                        // Copy ICONDIR and set idCount to 1.

                        writer.Write(data, 0, 4);
                        writer.Write((short)1);

                        // Copy ICONDIRENTRY and set dwImageOffset to 22.

                        writer.Write(data, 6 + 16 * i, 12);
                        writer.Write(22);

                        // Copy a picture.

                        int size = BitConverter.ToInt32(data, 6 + 16 * i + 8);
                        int offset = BitConverter.ToInt32(data, 6 + 16 * i + 12);
                        writer.Write(data, offset, size);

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
        /// <param name="icon">An System.Drawing.Icon to be converted.</param>
        /// <returns>A System.Drawing.Bitmap Object.</returns>
        public static Bitmap ToBitmap(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Quick workaround: Create an .ico file in memory, then load it as a Bitmap.

            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return (Bitmap)Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Gets the bit depth of an Icon.
        /// </summary>
        /// <param name="icon">An System.Drawing.Icon object.</param>
        /// <returns>The biggest bit depth of the icons.</returns>
        public static int GetBitDepth(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            var data = GetIconData(icon);

            int count = BitConverter.ToInt16(data, 4);
            int bitDepth = 0;
            for (int i =0; i < count; ++i)
            {
                int depth = BitConverter.ToUInt16(data, 6 + 16 * i + 6);
                if (depth > bitDepth)
                    bitDepth = depth;
            }

            return bitDepth;
        }

        private static byte[] GetIconData(Icon icon)
        {
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
