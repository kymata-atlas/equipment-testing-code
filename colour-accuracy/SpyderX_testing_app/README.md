
# SpyderX Elite Colorimeter Measurements

This repository contains three scripts that use a SpyderX Elite colorimeter to measure colour wavelengths for using in neuroimanging experiments: 1) to create a gamma correction table, 2) to test a gamma table and 3) to ascertain the CIE 1931 XYZ values from arbitrary array of RGB values.

It requires three things to run:

 - MATLAB (common commercial software) or GNU Octave (a free alternative to MATLAB; for simplicity, below we will refer to MATLAB only) and Psychtoolbox.
 - Psychtoolbox (free software available at http://psychtoolbox.org)
 - PsyCalibrator (free at: https://github.com/yangzhangpsy/PsyCalibrator)

Please visit these resources for OS differences.

## Gamma table creation using an SpyderX Elite colorimeter

`create_gamma_table.m`

This script is a wrapper for [PsyCalibrator](https://github.com/yangzhangpsy/PsyCalibrator))'s gamma table creation for use in the EMEG Lab.

It also gives uses [PsyCalibrator](https://github.com/yangzhangpsy/PsyCalibrator)) to verify the gamma correction table works correctly when used in Psychtoolbox.

## CIE 1931 XYZ measurements of arbitrary [R,G,B] values in an array using an SpyderX Elite colorimeter

`obtain_CIEXYZ_values_of_RGB_array.m`

This script accepts a) a gamma correction table and b) a list of RGB values, and generates the CIEXYZ 1931 XYZ coordinates for them (required for use in colour vision studies). Each colour has several measurements taken and the average is then given.
