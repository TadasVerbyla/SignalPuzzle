Basic functionality for a signal puzzle game implemented using DOTS. 
## Currently implemented:
### Particle emitter 
Controlled by Q and E buttons to rotate and Space to emit particles. Public floats controll the amount of particles emitted and the cone in which they are sent out. 
### Bouncer 
Bounces particles that collide with them
### Absorber 
Destroys particles that in contact with the absorber
### Objective
Destroyes particles and counts down its required particles to absorb, then switches to reverse state on zero. Countdown is only shown via debug
### Victory
Is detected by checking if all objectives are in their desired state. Only indicated by debug output currently

## Known errors:
- In ParticleCollisionSystem, possible entities that particles could collide with should be found at start, but I do not yet know how to implement global variables in a DOTS system class. 
- UI elements for victory and text, as well as color-based indications for objective states are missing. As I understand, Text and SpriteRenderer components do not have proper Entity representation to access them via a system, at least from what I found. Otherwise, simple OnUpdate checks in a DOTS system could easily alter the color or text just as LocalTransform is changed in ParticleMoverSystem.
-  Generic Event System isn't utilized.
