COMP 476
Assignment 1
Due February 13th 11:59 PM
Adrien Kamran, 40095393

* Write-up on assignment *

** How to run **
This is a Unity project, so this can be run like any other Unity project.
All the required files for running the project can be found in the submission.
If this does not work for any reason, please see this GitHub link for my work:
https://github.com/AdrienKamran/COMP-476/tree/main/COMP-476-Assignment-1

** File structure**
_Scenes : There is a single scene called R1R2. This contains the work for requirements R1 and R2.
_Scripts : 
- NPC : Contains ALL of the NPC behaviour code.
- Player : Contains the code required to move the player character.
- Teleporter : This is how the "toroidal" walls work.
Materials: Some basic materials to differentiate the characters on-screen.

** Code + in-game breakdown **
Please ensure that gizmos are on to view the debug lines, as they show the NPC behaviour clearly.
- RED LINES : "Stop" zone for seekers.
- CYAN LINES : "Slow" zone for seekers.
- BLUE LINES : Outer zone for seeking behaviour.
- MAGENTA LINES : Detection zone for seekers, changes based on speed.
- GREEN LINES : Shows which character an NPC is seeking.

Basic rules by default:
- Move your character (RED) with WASD.
- Seeker NPCs (BLUE) will seek their targets (follow the green lines) once they are in range.
- Fleeing NPC (MAGENTA) will flee the player character.

To my knowledge, all of the rules outlined in R2 are respected here.
Default behaviour for NPCs can be found in their FixedUpdate() method.
- NPC finds its target.
- NPC prepares its movement vector depending on seek or flee behaviour selection.
- NPC turns to face its expected movement (towards target for seek, away for flee).
- If the target is within the outer detection radius and within the vision cone, the NPC moves.

The specific behaviours for Seek and Flee are in their own methods.

The OnDrawGizmos() method has contains nearly all of the useful debug lines for use in the editor.

As you can probably tell, R3 was not completed due to a shortage of time.

Thank you for your time.