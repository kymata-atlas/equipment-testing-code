function screenResfreshDelayChecker

AssertOpenGL;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%
%  Screen refresh delay-checker for matlab/pystoolbox
%  visual stimulus.
%
%  AT 24/06/14
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

% set up parallel ports

task.ppaddr = hex2dec('378');  % LPT1
% initialize at beginning
task.ioObj = io32;
io32status = io32(task.ioObj);
task.BB.address = hex2dec('379');  % LPT1

%set up variables

times = zeros(100,1);

% run

Screen('FillRect', window, black, [0 0 W H]);
vbl=Screen('Flip', window);
wait(4000);

%photodiodeCurrentArray = [];

for i = 1:length(times)

     Screen('FillRect', window, black, [0 0 W H]);
     Screen('Flip', window);
    
     wait(500);
     
     photodiodeBlackvalue = io32(task.ioObj, task.BB.address);
     
     Screen('FillRect', window, white, [0 0 W H]);
     Screen('Flip', window); % sending white to projector
     tic; % starting/trigger
     
     
     photodiode = 0;    

     while(photodiode == 0)
        photodiodeCurrent = io32(task.ioObj, task.BB.address);
        %photodiodeCurrentArray = [photodiodeCurrentArray photodiodeCurrent];
        if (photodiodeCurrent ~= photodiodeBlackvalue)
            times(i) = toc; % when projector goes white/participant gets white
            photodiode = 1;  
        end        
     end
     
end


Priority(0);
ShowCursor
Screen('CloseAll');

for i = 1:length(times)
   disp([ 'photodiodeCurrent:' i '=' num2str(times(i))]); 
end

%for i = 1:length(photodiodeCurrentArray)
%   disp([ 'photodiodeCurrentArray:' i '=' num2str(photodiodeCurrentArray(i))]); 
%end

% display answer
disp(['mean:' num2str(mean(times)) 'ms']);
disp(['highest:' num2str(max(times)) 'ms']);
disp(['lowest:' num2str(min(times)) 'ms']);

hist(times);

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