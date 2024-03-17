
function output_XYZ_values = obtain_CIEXYZ_values_of_RGB_array()
% obtain_CIEXYZ_values_of_RGB_array.m
% -------
% Given a Gamma Correction table and an array of RGB values, create a table
% of CIE XYZ measurements.


% Instructions
% -----
% Install Psychtoolbox (free software available at http://psychtoolbox.org)
% Download PsyCalibrator and place it in a suitable location (free at: https://github.com/yangzhangpsy/PsyCalibrator)
% 
% If you are on a Mac you will need to install libusb (i.e through brew) >> brew install libusb
% You will need to copy the file spyderRead_APL_xyz.m into
% PsyCalibrator-main/PsyCalibrator
%
% Make equipment-testing-code/colour-accuracy/SpyderX_testing_app/ your
% working directory. Then run this script.

% to record the xyY values instead of CIE1931 XYZ values, replace
% >>   multiple_CIE_XYZ_spyder_readings = spyderRead_APL_xyz(refreshRate, nMeasures);
% with
% >>   multiple_CIE_XYZ_spyder_readings = spyderRead_APL(refreshRate, nMeasures);

psycalibrator_location = 'matlab_libraries'; % this is the location of PsyCalibrator-main, reletive to your working directory
name_of_rgb_csv_to_measure = "data/test_file_rgb_color_array.csv";
name_of_rgb_gamma_correction_norms = "data/Kymata_test_screen_gamma_correction_tables/2023_rgb_gamma_correction_norms.csv";
nMeasures = 3; % definition of the number of measurements

% load data
rgb_gamma_correction_norms = load(name_of_rgb_gamma_correction_norms);
list_of_rgb_values = load(name_of_rgb_csv_to_measure);

addpath(genpath(psycalibrator_location)); savepath;

%%%%%%%%%%%%%
% begin
%%%%%%%%%%%%%
KbName('UnifyKeyNames');
try
    commandwindow;
    %%%%%%%%%%%%%%%%%%%%%%%%%
    Screen('Preference', 'SkipSyncTests', 1);
    Screen('Preference', 'VisualDebugLevel', 0);
    Screen('Preference', 'Verbosity', 0);

    whichScreen = max(Screen('Screens'));    
    fullRect = Screen('Rect', whichScreen);
    perperialRect = CenterRectOnPoint([0 0 500 500],fullRect(3)/2,fullRect(4)/2);

    
    %%%%%%%%%%%%%%%%%%%%%%%%%
    
    Stimuli.White = WhiteIndex(whichScreen);%
    Stimuli.Black = BlackIndex(whichScreen); %
    Stimuli.Gray  = [128,128,128];%(Stimuli.White+Stimuli.Black)/2;
    
    gammaTableBack = Screen('ReadNormalizedGammaTable', whichScreen);
    
    [w,ScreenRect] = Screen('OpenWindow',whichScreen,Stimuli.Gray,[],32);
    
    Screen('LoadNormalizedGammaTable',whichScreen,rgb_gamma_correction_norms);
    
    %%%% start %%%%%
    HideCursor;
    ifi         = Screen('GetFlipInterval', w);
    refreshRate = 1/ifi;
    %%%%%%%%%%%%%%%%
    
    Screen('TextSize',w,30);
    Screen('TextStyle',w,0);%0=normal,1=bold,2=italic,4=underline,8=outline,32=condense,64=extend
    Screen(w,'TextFont','Verdana');
    
    EscapeKey = KbName('ESCAPE');
    
    Priority(MaxPriority(whichScreen));
    
    DrawFormattedText(w,'We need to calibrate the device first by establishing the black level.\n Now make sure the lens cover of the photometer is fully closed. \n Then hit any key to proceed.','center','center',Stimuli.White);
    Screen('Flip',w);
    abortExp(whichScreen,gammaTableBack,EscapeKey);
    KbPressWait;
        
    % ---- calibrate the device ----/

    % do measure or calibration(if necessary) once
    spyderCalibration_APL(0);
               
    Screen('FillRect',w,Stimuli.Gray,ScreenRect);

    currentRect = perperialRect;

    ovalLength  = min(abs([currentRect(3) - currentRect(1), currentRect(4) - currentRect(2)]));

    Screen('FillRect', w,Stimuli.White,currentRect);
    Screen('FillOval', w,Stimuli.Gray, CenterRectOnPoint ([0 0 ovalLength ovalLength],(currentRect(1)+currentRect(3))/2,(currentRect(2)+currentRect(4))/2),ovalLength + 10);
    Screen('DrawLine', w,Stimuli.Black, currentRect(1), currentRect(2),  currentRect(3), currentRect(4),1);
    Screen('DrawLine', w,Stimuli.Black, currentRect(1), currentRect(4),  currentRect(3), currentRect(2),1);


    DrawFormattedText(w,'Device calibration done!\nTo start the measurement, open the lens cover and focus the photometer at the center. \n Hit any key to continue...','center',ScreenRect(4)*0.8,Stimuli.White);

    Screen('Flip',w);

    abortExp(whichScreen,gammaTableBack,EscapeKey);

    InitialStr = 'The automatic measurement will start in 10 seconds!';

    Screen('FillRect',w,[0 0 0],ScreenRect);
    DrawFormattedText(w,InitialStr,'center',ScreenRect(4)*0.8,Stimuli.White);
    KbPressWait;

    abortExp(whichScreen,gammaTableBack,EscapeKey);
    Screen('Flip',w);

    abortExp(whichScreen,gammaTableBack,EscapeKey);
    WaitSecs(10);

    fprintf('========== Begin measurement =============\n');

    [CIE_XYZ,usedRGBs] = deal(zeros(size(list_of_rgb_values,1),3));

    for iRGB = 1:size(list_of_rgb_values,1)
        %abortExp(whichScreen,gammaTableBack,EscapeKey);

        Screen('FillRect',w,Stimuli.Gray,ScreenRect);
        Screen('FillRect',w,list_of_rgb_values(iRGB,:),currentRect);
        Screen('Flip',w);

        WaitSecs(0.1);
        
        abortExp(w,gammaTableBack,EscapeKey);


        % spyder 5 or X
        multiple_CIE_XYZ_spyder_readings = spyderRead_APL_xyz(refreshRate, nMeasures);

        CIE_XYZ(iRGB,:)  = mean(multiple_CIE_XYZ_spyder_readings,1);
        usedRGBs(iRGB,:) = list_of_rgb_values(iRGB,:);
        fprintf('iRGB %3d:%4d %4d %4d: CIE_XYZ: %5.3f %5.3f %5.3f\n',iRGB,list_of_rgb_values(iRGB,1),list_of_rgb_values(iRGB,2),list_of_rgb_values(iRGB,3),CIE_XYZ(iRGB,1),CIE_XYZ(iRGB,2),CIE_XYZ(iRGB,3));



        %========= measure end==============
    end % iRGB

    beep;


    fprintf('=============================================================\n');
    fprintf('               All test locations finished  !\n');
    fprintf('=============================================================\n');

    output_XYZ_values = [usedRGBs,CIE_XYZ];
    
    Priority(0);
   
    Screen('CloseAll');
    Screen('LoadNormalizedGammaTable',whichScreen,gammaTableBack);
    ShowCursor;
    
    csvwrite("output_XYZ_values.csv",output_XYZ_values)
    fprintf('----------------------------\n');
    fprintf('Data have been saved!\n');
    fprintf('----------------------------\n');
    
   
catch obtain_CIEXYZ_values_of_RGB_array_error
    
    sca;
    Screen('LoadNormalizedGammaTable',whichScreen,linspace(0,1,256)'*[1 1 1]);
    ShowCursor;
    Priority(0);
    rethrow(obtain_CIEXYZ_values_of_RGB_array_error);
end % try
end % end  for function

function abortExp(whichScreen,gammaTableBack,EscapeKey)
[~,~,keyCode] = KbCheck;

    if keyCode(EscapeKey)
        sca;
        ShowCursor;
        Priority(0);
        Screen('LoadNormalizedGammaTable',whichScreen,gammaTableBack);
        error('Test aborted by the experimenter!');
    end

end
