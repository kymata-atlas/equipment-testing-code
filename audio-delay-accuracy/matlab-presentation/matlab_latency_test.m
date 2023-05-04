wavfilename = '1000_Stereo_unramped.wav';
[wavedata, freq] = audioread(wavfilename);

channels = 2;
reqlatencyclass=2;
sugglatency=[]; % not changing anything this.
InitializePsychSound(1);
devices = PsychPortAudio('GetDevices');
deviceid = 9; %windows direct sound              184, 24, 22, 23, 22, 23, 22 (!) % Does use accept two sound handels.
%deviceid = 4; %MME                                DO NOT USE - INTRODUCES JITTER
%deviceid = 10; %ASIO (default)                   190,65, 93,93,93,93...... (to within a ms!)
%deviceid = [];
MySoundHandle = PsychPortAudio('Open', deviceid, [], reqlatencyclass, freq, channels, [], sugglatency);

%Delay asio  first pulse 15ms, and then everyone from the point is 83 ms from that trigger

%with direct sound the first pulse is 127 ms and then 83 thereafter

%MME all over the place and jittery.

%osccioscpoel trigering from the NI card and then measuing the soundcard output and comparing the distance betert the two for the delay.

%And its consistant.

PsychPortAudio('FillBuffer', MySoundHandle, wavedata');

    
    % Perform one warmup trial, to get the sound hardware fully up and running,
    % performing whatever lazy initialization only happens at real first use.
    % This "useless" warmup will allow for lower latency for start of playback
    % during actual use of the audio driver in the real trials:
    %PsychPortAudio('Start', MySoundHandleA, 1, 0, 1);
    %PsychPortAudio('Stop', MySoundHandleA, 1);
    



MEG = MEGSynchClass;
MEG.SendTrigger(0);
WaitSecs(0.5);

PsychPortAudio('Start', MySoundHandle, 1, inf, 0);

for i=1:10
    WaitSecs(0.5);
    tic

    MEG.SendTrigger(1);
    PsychPortAudio('RescheduleStart', MySoundHandle, 1, 0, 1);
    
    toc
    MEG.SendTrigger(0);
    WaitSecs(0.5);
end
