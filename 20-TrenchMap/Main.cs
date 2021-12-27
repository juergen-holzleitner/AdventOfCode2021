using _20_TrenchMap;

var input = TestInput.ReadInput(@"input.txt");
var currImage = input.InputImage;
var infinitePixel = '.';
for (int n = 0; n < 2; ++n)
{
  currImage = TestPixelEnhancement.EnhanceImage(currImage, input.EnhancmentAlgorithm, infinitePixel);
  infinitePixel = TestInfinitePixel.GetNextInfinitePixel(infinitePixel, input.EnhancmentAlgorithm);
}

int numLitPixels = TestCounting.GetNumLitPixels(currImage);
System.Console.WriteLine(numLitPixels);
