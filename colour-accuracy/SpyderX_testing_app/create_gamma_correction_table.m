

% create_gamma_table.m
% -------
% Wrapper for PsyCalibrator's makeCorrectedGammaTab_APL.
% Will create  a file that contains the measurement results will be generated in MATLAB within the current working path, named Gamma.mat 


% Instructions
% -----
% Install Psychtoolbox (free software available at http://psychtoolbox.org)
% Download PsyCalibrator and place it in a suitable location (free at: https://github.com/yangzhangpsy/PsyCalibrator)
% If you are on a Mac you will need to install libusb (i.e through brew) >> brew install libusb
% 
% Make equipment-testing-code/colour-accuracy/SpyderX_testing_app/ your
% working directory

psycalibrator_location = 'matlab_libraries'; % this assumes the xxx, and you are in the working directoy
deviceType = 2; %2 for SpyderX, 1 for Spyder5

addpath(genpath(psycalibrator_location)); savepath;

%Run the following: (By default it will choose the highest screen to do the test on.)

gammaMeasure_APL(deviceType,[],[],[],[],[],[],2); 

% This creates a Gamma table (gamma.mat) which sets out the colours at
% different RGB intensities, specific to your screen. Now you want a *gamma
% correction* table that linearises these readings.

Gamma = makeCorrectedGammaTab_APL('Gamma.mat');

% Save it:

csvwrite("rgb_gamma_correction_norms.csv",Gamma.gammaTable)

% Now verify that it worked.

gammaMeasure_APL(deviceType,[],[],[], Gamma.gammaTable,[],[],2);

% --------------------

% Now to use them! To switch between using the corrections, and not using
% them in Psychtoolbox use:

load('rgb_gamma_correction_norms.csv');
Screen('LoadNormalizedGammaTable', whichScreen, rgb_gamma_correction_norms);

% (where whichScreen is the screen you want to change.)

%to switch it on, and
Screen('LoadNormalizedGammaTable',whichScreen,linspace(0,1,256)'*[1 1 1]);

%to switch back.