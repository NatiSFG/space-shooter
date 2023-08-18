# 🔍 Microtasks
- New Enemy Type: The spinner enemy will shoot ice beams that can damage the player and freeze the player for 2 seconds. The enemy shoots randomly between every 3 to 7 seconds as they rotate continuously and move vertically down the screen. They can shoot in any direction, depending on where they are in their rotation.

### Code:
- Change CurrentWave script to use an action instead of update
- Break UIManager into separate scripts
- Make enemies:
    - Only allow horizontal movement once each time they get across the screen
    - (MAYBE) Prevent enemies from hovering real close to the edge of the bottom of the screen with horizontal movement
- Learn new input system

### Misc:
- Review the Parameter Modifiers article in detail
- What does the math mean in Camera Shake? Random.value * magnitude * 2 - magnitude. Update CameraShake article
- Any way to make a file private that is on a public GitHub repo?
