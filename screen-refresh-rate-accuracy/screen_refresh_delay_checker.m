function screen_refresh_delay_checker

    AssertOpenGL;

    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    %
    %  Screen refresh delay-checker for matlab/pystoolbox
    %  visual stimulus.
    %   
    %  This will test accurracy in two ways: a) recording the  the response from the
    %  photodiode as mesured by the MEGsynchclass as a button trigger, and b) sending
    %  triggers to the MEG machine, so that both triggers can be compared.
    %  Note that approach a) adds about 0.5 ms to the response time, so b) is a better
    %  estimate. 
    %
    %  Instuctions for use in MEG CBU:
    %
    %  1) Plug the Photodiode into the Left Yellow button box port – this
    %  uses channel 10 (i.e., it comes through on STI010, or equates to
    %  value 512 if you're reading from the combined channel
    %  [i.e., STI101])*. The box where we plug this into is where all the
    %  cables for each of the buttons comprising the button boxes are
    %  plugged in – one can just unplug the cable for the Left Yellow
    %  button and plug in the photodiode into its spot. Then I turn the
    %  photodiode on. Remember to undo all of this when you clean up after
    %  the testing.
    %
    %  *Stim channels for the button box are STI008-STI016, and are binary
    %  0-1. These are combined into the STIM101 as a decimal. MEGSYNC is
    %  able to convert these into "LY" (left hand button box press) for
    %  example.
    %
    %  2) Run this script.
    %
    %  AT 04/04/2023
    %
    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


    flip_request_position = "after"; % 'after' of 'before' After is thought to avoid more variability. 


    % set up screen

    white = 225; % pixel value for white
    black = 0; % pixel value for black
    screens=Screen('Screens');
    screenNumber=max(screens);
    Screen('Preference', 'SkipSyncTests', 0); %maximum accuracy
    [window, rect] = Screen(screenNumber, 'OpenWindow');
    [center(1), center(2)] = RectCenter(rect);
    W=rect(RectRight); % screen width
    H=rect(RectBottom); % screen height
    priorityLevel=MaxPriority(window);
    Priority(priorityLevel);
    HideCursor;
    Screen('BlendFunction', window, GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
    fps=Screen('FrameRate',window);      % frames per second
    ifi=Screen('GetFlipInterval', window);
    if fps==0
        fps=1/ifi;
    end;
        
    fps
    ifi

    MEG=MEGSynchClass; %%initialise CBU MEG
    MEG.SendTrigger(0); %%set to 0

    %set up variables

    times = zeros(10,1);

    % run

    Screen('FillRect', window, black, [0 0 W H]);
    vbl=Screen('Flip', window);
    WaitSecs(4);
    MEG.SendTrigger(0); % set as zero


    % do test of flips
    for i = 1:length(times)
        
        Screen('FillRect', window, black, [0 0 W H]);
        Screen('Flip', window);

        WaitSecs(5);

        Screen('FillRect', window, black, [0 0 W H])
        vbl=Screen('Flip', window);
        

        if strcmp(flip_request_position, "before")
            Mtic = MEG.Clock;
            MEG.SendTrigger(1);  
        end

        Screen('FillRect', window, white, [0 0 W H]);
        vbl=Screen('Flip', window, vbl + (0.5)*ifi); % sending white to projector
        
        if strcmp(flip_request_position,"after")

            % Whether the position of the trigger is in front or behind the
            % flip makes a big differnce - make sure this is the same as your
            % actual expermental code! For the CBU, if before the flip the delay is 33.33ms,
            % if after it is 16.66ms. "After" is likely to have less variability.       
        
            Mtic = MEG.Clock;
            MEG.SendTrigger(1);  
        end
        
         MEG.WaitForButtonPress(); % as the diode is plugged into the Left Hand yellow box
         lag = MEG.TimeOfLastButtonPress - Mtic; % this is more accurate than tic toc. Compared with
                                                 % the MEG triggers, it only adds 0.5 ms
         times(i) = lag; % when projector goes white/participant gets white
         
         WaitSecs(0.005)
 
         MEG.SendTrigger(0); % set back as zero
 
     end

    disp('Timings estimated using MEGsynchclass timing estimates')

    for i = 1:length(times)
        disp([ 'photodiodeCurrent:' i '=' num2str(times(i))]);
    end

    % display answer
    disp(['mean:' num2str(mean(times)) 'ms']);
    disp(['highest:' num2str(max(times)) 'ms']);
    disp(['lowest:' num2str(min(times)) 'ms']);

    hist(times);

    Priority(0);
    ShowCursor
    Screen('CloseAll');


end



function key_pressed = getkey(task)

% This function listens for key presses

    buttonpressed = 0;
    
    % Loop until a button is pressed
    while (buttonpressed == 0)
            
        % Listen for Esc from computer to see if the paradigm wants to be
        % aborted
        [keyIsDown, secs, keyCode] = KbCheck(); % Listen for key press
        if keyCode(27) == 1
            disp('Esc pressed')
            KbName(keyCode)
            Screen('CloseAll');
        end
        
    end

end