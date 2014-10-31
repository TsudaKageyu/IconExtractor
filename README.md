# IconExtractor

Icon Extractor Library for .NET

http://www.codeproject.com/Articles/26824/Extract-icons-from-EXE-or-DLL-files


Extract all the variations of an icon from .DLL/.EXE, including the ones ```ExtractIconEx()``` can't extract.

### Usage

First, add a reference to ```IconExtractor.dll``` to your .NET project. Then...

```
using System;
using System.Drawing;
using TsudaKageyu;

// -----------------------------------------------------------------------------
// Usage of IconExtractor class:

// Construct an IconExtractor object with a file.

IconExtractor ie = new IconExtractor(@"D:\sample.exe");

// Get the full name of the associated file.

string fileName = ie.FileName;

// Get the count of icons in the associated file.

int iconCount = ie.Count;

// Extract icons individually.

Icon icon0 = ie.GetIcon(0);
Icon icon1 = ie.GetIcon(1);

// Extract all the icons in one go.

Icon[] allIcons = ie.GetAllIcons();

// -----------------------------------------------------------------------------
// Usage of IconUtil class:

// Split the variations of icon0 into separate icon objects.

Icon[] splitIcons = IconUtil.SplitIcon(icon0);

// Convert an icon into bitmap. Unlike Icon.ToBitmap() it preserves the transparency.

Bitmap bitmap = IconUtil.ToBitmap(splitIcon[1]);

// Get the bit count of an icon.

int bitCount = IconUtil.GetBitCount(splitIcon[2]);
```
