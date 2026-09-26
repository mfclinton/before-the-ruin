# Before the Ruin

You play Charlotte, an adventurer visiting a quiet town with a strange tower nearby. You explore the town, talk to people, and fight turn based battles.

- Play: [itch.io](https://unitedfailures.itch.io/before-the-ruin)
- Made: September 2024 for Brackeys Game Jam 2024.2
- Team: [@mfclinton](https://github.com/mfclinton) (programming), [Kayla](https://kaylachu23.itch.io) (art and UI), [Marekuma](https://marekuma.itch.io) (story and dialogue), [@MrAozora](https://github.com/MrAozora) (music and sound effects)
- Engine: Unity, C#

This is an export of a private repo with only the code we wrote. Art, audio, the Unity project files, and third party plugins aren't included. The history is squashed into one commit.

## What I built

- Turn based battles run by a state machine. Both turns play out as the same short platforming minigame with a different set of projectiles. Each projectile carries its own effect on your health and the enemy's.
- Walking around town, talking to people, using doors, and the dialogue system. One character controller handles both the top down town and the side on interiors, and swaps footstep sounds to match.
- Keeping track of progress between scenes, like which bosses are beaten and which conversations are done, and placing the player at the right door when a scene loads.
