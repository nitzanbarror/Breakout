\# Breakout



\*The classic — with one twist: the wall fights back.\*



Final exercise for \*\*Unity 101 for CS Students\*\*. Classic Breakout, with one change:

there are no levels — the brick wall \*\*descends steadily and grows a new row every few

seconds\*\*, turning the game into an endless survival arcade. Let it reach the yellow

line and it's game over.



\*\*Full design document:\*\* \[GDD.md](GDD.md) (living doc, v0.1 → v0.6 changelog)



\## Play



\- \*\*PC (Windows):\*\* arrows / A-D to move, \*\*Space\*\* to launch, \*\*Enter\*\* after game over, \*\*Esc\*\* to quit

\- \*\*Android:\*\* drag anywhere to move, tap to launch, \*\*Back\*\* to quit



\## Tech



\- Unity \*\*6000.3.20f1\*\* (6.3 LTS), Universal 2D (URP)

\- Course patterns: `ObjectPool<T>` bricks/power-ups, coroutines, singletons,

&#x20; `GameConfig` ScriptableObject, UnityEvents observer UI, PlayerPrefs high score

\- Camera fits the play field to \*\*any screen aspect ratio\*\* (found and fixed via real-device testing)

\- \*\*Zero external assets\*\* — neon look from built-in shapes + URP Bloom; all SFX synthesized for this project

\- Builds: Windows (portrait 360×640 window) + Android APK (IL2CPP, ARM64)

