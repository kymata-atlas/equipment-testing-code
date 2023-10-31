wavfilename = '1000_Stereo_unramped.wav';
[wavedata, freq] = audioread(wavfilename);

channels = 2;
reqlatencyclass=4;
sugglatency=[]; % not changing anything this.
InitializePsychSound(1);
devices = PsychPortAudio('GetDevices');
deviceid = 5; % use Windows WASAPI
MySoundHandle = PsychPortAudio('Open', deviceid, [], reqlatencyclass, freq, channels, 48, sugglatency);

PsychPortAudio('FillBuffer', MySoundHandle, wavedata');

    
% Perform one warmup trial, to get the sound hardware fully up and running,
% performing whatever lazy initialization only happens at real first use.
% This "useless" warmup will allow for lower latency for start of playback
% during actual use of the audio driver in the real trials
     
PsychPortAudio('Start', MySoundHandle, 1, 0, 0, 0.001);
PsychPortAudio('Stop', MySoundHandle, 1);
    

MEG = MEGSynchClass;
MEG.SendTrigger(0);
WaitSecs(0.5);

for i=1:10
    WaitSecs(0.5);
    tic

    MEG.SendTrigger(1);
    PsychPortAudio('Start', MySoundHandle, 1, 0, 0);
    
    toc
    MEG.SendTrigger(0);
    WaitSecs(0.5);
end
