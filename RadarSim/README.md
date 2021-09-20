# RadarSim

Simulate the Arduino base HC-04 radar(ultrasound range finder). Just to make sure the Radar App in Unity can read the udp packets from the radar.

## Output format

Example: 85, 0<br>
Means the radar has an angle of 85 degrees and no target was found.<br>

Example: 85, 1.45<br>
Means the radar has an angle of 85 degrees and a target at range 1.45 meters was found.<br>

