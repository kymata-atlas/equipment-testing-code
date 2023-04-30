# CIE 1931 XYZ measurements of arbitrary [R,G,B] values in an array using an Admesy Bontes colorimenter

A modified version of JETIApp which loads colour values from a file rather than generating them.

Used to create visual-domain functions which describe what participants actually saw on the screen.

 - The screen saver default for the CBU computers is 5 minutes - so make sure you are logged on to circumvent this.
 - The default output (set in the code) is CIE 1931 XYZ - so don't change this unless needed.
 - Make sure the dataset is correct in the code (If you need to recompile, press 'build')
 - In the dialogue box,
   - Select Brontes
   - Increase the speed from 20,000 to 10,000 - but no less. Any less and not enough samples will be taken.
   - Note output name (you can't change it - but make sure you know what it is)
   - Select gamma table if needed. (from the resources/folder)
