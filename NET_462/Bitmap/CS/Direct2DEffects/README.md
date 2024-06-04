## Direct2DEffects
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_462/Bitmap/CS/Direct2DEffects)
____
#### Demonstrates applying Direct2D effects to the image in C1Bitmap
____
This sample loads an image in C1Bitmap, converts it to Direct2D
bitmap, applies various effects, then converts back to C1Bitmap
and to a WriteableBitmap that can be set as the Source for the
Image element.

When the user clicks the Export button the image is converted to
Direct2D bitmap, then used as the source for a Grayscale effect.
The result is imported into another instance of C1Bitmap, then
stored to a file.
