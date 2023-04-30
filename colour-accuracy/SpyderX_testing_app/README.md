
# SpyderX Elite colorimenter measurements

This repository contains three scripts that use a SpyderX Elite colorimenter to measure colour wavelenths for using in neuroimanaging experiemtns: 1) to create a gamma correction table, 2) to test a gamma table and 3) to asscertain the CIE 1931 XYZ values from arbitrary array of RGB values.

It requires three things to run:

 - MATLAB (common commercial software) or GNU Octave (a free alternative to MATLAB; for simplicity, below we will refer to MATLAB only) and Psychtoolbox.
 - Psychtoolbox (free software available at http://psychtoolbox.org)
 - PsyCalibrator (free at: https://github.com/yangzhangpsy/PsyCalibrator)

Please visit these resources for OS differences.

## Gamma table creation

create_gamma_table.m

This script is a simple wrapper for Zhicheng Lin, Qi Ma and Yang Zhang's gamma table creation code.

## Gamma table testing

test_gamma_table_correction.m

This script is a simple wrapper for Zhicheng Lin, Qi Ma and Yang Zhang's code that ensures the above gamma table works correctly whe used in Psychtoolbox.

## CIE 1931 XYZ measurements of arbitrary [R,G,B] values in an array using an SpyderX Elite Brontes colorimenter

obtain_CIEXYZ_values_of_RGB_array.m

This script accepts a) a gamma correction table and b) a list of RGB values, and generates the CIEXYZ 1931 XYZ coordinates for them (required for use in colour vision studies). Each colour has several measurements taken and the average is then given.