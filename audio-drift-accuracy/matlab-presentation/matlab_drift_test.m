

wavfilename = 'drift_test_48k.wav'; % can't find this file? It is not
                                    % uploaded to github, as github only takes files <100mb
                                    % AT has this file - ask him for it.
[wavedata, freq] = audioread(wavfilename);

channels = 2;
reqlatencyclass=4;
sugglatency=[]; % not changing anything this.
InitializePsychSound(1);
buffersize = 48;
devices = PsychPortAudio('GetDevices');
deviceid = 6; 
MySoundHandle = PsychPortAudio('Open', deviceid, [], reqlatencyclass, freq, channels, buffersize, sugglatency);


PsychPortAudio('FillBuffer', MySoundHandle, wavedata');

    
% Perform one warmup trial, to get the sound hardware fully up and running,
% performing whatever lazy initialization only happens at real first use.
% This "useless" warmup will allow for lower latency for start of playback
% during actual use of the audio driver in the real trials:

PsychPortAudio('Start', MySoundHandle, 1, 0, 0, 0.001);
PsychPortAudio('Stop', MySoundHandle, 1);
    
PsychPortAudio('Start', MySoundHandle, 1, 0, 0);

Waitsecs (410);