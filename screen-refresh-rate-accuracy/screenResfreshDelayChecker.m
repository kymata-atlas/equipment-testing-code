function screenResfreshDelayChecker

AssertOpenGL;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%
%  Screen refresh delay-checker for matlab/pystoolbox
%  visual stimulus.
%   
%  This will test accurracy in two ways: using tic, and toc, and sending
%  triggers to the MEG machine. Using tic/toc appears to add 2 ms.
%
%  Instuctions for use in MEG CBU:
%
%  1) Plug Photodiode into the Left Yellow button box port – this uses channel 10
%  (i.e., it comes through on STI010, or equates to value 512 if you re
%  reading from the combined channel [i.e., STI101]). The box where I plug this into is
%  where all the cables for each of the buttons comprising the button boxes are
%  plugged in – I just unplug cable for the Left Yellow button and plug in the
%  photodiode into its spot. Then I turn the photodiode on (it is rarely plugged
%  in and on by default).
%
%  2) Run this script.
%
%  AT 04/04/2023
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

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

    Screen('FillRect', window, white, [0 0 W H]);

    vbl=Screen('Flip', window, vbl + (0.5)*ifi); % sending white to projector
    
    % Whether the position of the trigger is in front or behind the
    % flip makes a big differnce - make sure this is the same as your actual expermental code.
    
    tic % Using tic/toc appears to add 2 ms compared with the triggers.
    MEG.SendTrigger(1);

    
    MEG.WaitForButtonPress(); % as the diode is plugged into the Left Hand yellow box
    times(i) = toc; % when projector goes white/participant gets white
    

    MEG.SendTrigger(0); % set back as zero

end

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