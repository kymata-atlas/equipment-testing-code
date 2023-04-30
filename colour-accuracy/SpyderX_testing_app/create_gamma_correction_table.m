

% create_gamma_table.m
% -------
% Wrapper for xxxx.
% Will create  a file that contains the measurement results will be generated in MATLAB within the current working path, named Gamma.mat 


% Instruactions
% -----
% please install Psychtoolbox (free software available at http://psychtoolbox.org)
% Download PsyCalibrator and place it in a suitable location (free at: https://github.com/yangzhangpsy/PsyCalibrator)


psycalibrator_location = 'xxxxx' % this assumes the xxx, and you are in the working directoy
deviceType = 2 %2 for SpyderX, 1 for Spyder5

addpath(genpath(Path)); savepath;


please close cover wait for replay

spyderCalibration_APL;


now place it against the xxx please close cover wait for replay

gammaMeasure_APL(deviceType,[],[],[],[],[],[],2);
Gamma = makeCorrectedGammaTab_APL('Gamma.mat');