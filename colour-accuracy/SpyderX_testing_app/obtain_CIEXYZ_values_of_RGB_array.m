
function output_XYZ_values = obtain_CIEXYZ_values_of_RGB_array()
% obtain_CIEXYZ_values_of_RGB_array.m
% -------
% Given a Gamma Correction table and an array of RGB values, create a table
% of CIE XYZ measurements.


% Instructions
% -----
% please install Psychtoolbox (free software available at http://psychtoolbox.org)

name_of_rgb_csv_to_measure = "data/test_file_rgb_color_array.csv";
name_of_rgb_gamma_correction_norms = "rgb_gamma_correction_norms.csv";

nMeausres = 5; % definition of the number of measurements
refreshRate = 60; % definition of the refresh rate of the screen

% load data
rgb_gamma_correction_norms = load(name_of_rgb_gamma_correction_norms);
list_of_rgb_values = load(name_of_rgb_csv_to_measure);


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
    
    fullRect = Screen('Rect', whichScreen);
    perperialRect = CenterRectOnPoint([0 0 500 500],fullRect(3)/2,fullRect(4)/2);

    
    %%%%%%%%%%%%%%%%%%%%%%%%%
    
    Stimuli.White = WhiteIndex(whichScreen);%
    Stimuli.Black = BlackIndex(whichScreen); %
    Stimuli.Gray  = [128,128,128];%(Stimuli.White+Stimuli.Black)/2;
    
    pause;
    
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

    currentRect = perperialRect(:,1)';

    ovalLength  = min(abs([currentRect(3) - currentRect(1), currentRect(4) - currentRect(2)]));

    Screen('FillRect', w,Stimuli.White,currentRect);
    Screen('FillOval', w,Stimuli.Gray, CenterRectOnPoint ([0 0 ovalLength ovalLength],(currentRect(1)+currentRect(3))/2,(currentRect(2)+currentRect(4))/2),ovalLength + 10);
    Screen('DrawLine', w,Stimuli.Black, currentRect(1), currentRect(2),  currentRect(3), currentRect(4),1);
    Screen('DrawLine', w,Stimuli.Black, currentRect(1), currentRect(4),  currentRect(3), currentRect(2),1);


    DrawFormattedText(w,'Device calibration done!\nTo start the measurement, open the lens cover and focus the photometer at the center. \n Hit any key to continue...','center',ScreenRect(4)*0.8,Stimuli.White);

    Screen('Flip',w);

    abortExp(whichScreen,gammaTableBack,EscapeKey);

    InitialStr = ['The automatic measurement will start in ',num2str(LeaveTime),' seconds!'];

    Screen('FillRect',w,[0 0 0],ScreenRect);
    DrawFormattedText(w,InitialStr,'center',ScreenRect(4)*0.8,Stimuli.White);
    KbPressWait;

    abortExp(whichScreen,gammaTableBack,EscapeKey);
    Screen('Flip',w);

    abortExp(whichScreen,gammaTableBack,EscapeKey);
    WaitSecs(LeaveTime);

    fprintf('========== Begin measurement =============\n');

    [xyY,usedRGBs] = deal(zeros(size(data,1)*nMeasures,3));

    for iRGB = 1:size(list_of_rgb_values,1)
        abortExp(whichScreen,gammaTableBack,EscapeKey);

        Screen('FillRect',w,Stimuli.Gray,ScreenRect);
        Screen('FillRect',w,data(iRGB,:),currentRect);
        Screen('Flip',w);

        for iM = 1:nMeausres

            if iM == 1
                WaitSecs(0.1);
            end

            abortExp(w,gammaTableBack,EscapeKey);


            % spyder 5 or X
            CIE_XYZ = spyderRead_APL(refreshRate, 1);
            CIE_XYZ((iRGB-1)*nMeasures + iM,:)  = mean(CIE_XYZ,1);

            usedRGBs((iRGB-1)*nMeasures + iM,:) = data(iRGB,:);
            fprintf('iRGB %3d:%4d %4d %4d: iMeasure:%4d xyY: %5.3f %5.3f %5.3f\n',iRGB,data(iRGB,1),data(iRGB,2),data(iRGB,3),iM,xyY(iRGB,1),xyY(iRGB,2),xyY(iRGB,3));

        end

        %========= measure end==============
    end % iRGB

    beep;

    if iLoc == size(perperialRect,2)
        fprintf('=============================================================\n');
        fprintf('               All test locations finished  !\n');
        fprintf('=============================================================\n');
    else
        fprintf('=============================================================\n');
        fprintf('Change to the next location, then press any key to continue...\n'  );
        pause;
    end

    output_XYZ_values = [usedRGBs,CIE_XYZ];
    
    Priority(0);
   
    Screen('CloseAll');
    Screen('LoadNormalizedGammaTable',whichScreen,gammaTableBack);
    ShowCursor;
    
    save(outputFilename,'output_XYZ_values');
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
