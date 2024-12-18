# gamedev ca2 3d platformer report

Finn Nankivell & Sylvie McKendry
25 Oct 2024

## Introduction:

The task for this CA was to use Unity's game development features to create a short 3D platformer of at least two levels in length, with some basic mechanics such as item pickups and win states. For this project, we decided to create a fast paced 3D platformer about double jumping set on the rooftops of skyscrapers with a focus on speedrunning.


## Premise:

The game is called "Skyline Sprint" and is about a character named Hope, who races the skyscraper rooftops of her city using her parkour skills and trusty gravity boots. The primary objective of the game is to beat the three levels (a tutorial and two other levels) by reaching the tractor beam at the end of the level. Secondary objectives include optimizing your path through the level for a better time, and hunting for all the item pickups. Initially, Hope's mobility starts off limited, as she only has a regular jump and a ground pound charged jump ability which can be used to gain some extra height. However upon collecting a pickup (in the form of a battery), she gains the ability to double jump, and every subsequent battery collected increases the amount of double jumps she can perform. The player should be aiming to find as many batteries as they can in order to improve their mobility and reach the end of the level. As an additional hidden mechanic, double jumping in the air increases the speed at which she moves in the air, so players aiming for better times are encouraged to balance spending time finding batteries as it greatly improves their speed with moving towards the end of the level.


## Division of labour:

Early on in the pre-production phase, we made a rough plan of how the work and roles would be divided for this project. While there were some tasks we shared and this division of labour was not entirely rigid, we mostly stuck to these roles.

Finn:

- Game Design
- Concept Art
- Character Design
- 3D Modelling & Rigging
- Animation
- Misc Programming
- Environmental Art


Sylvie:

- Game Design
- Game Systems Programming
- Level Design
- Testing
- UI Programming
- Environmental Art


## Design:

Our initial design process began with ideation relating to game mechanics. We knew that our game was going to be gameplay driven from the start, and we wanted it to have a focus on speedrunning, so we decided to develop ideas around that. We iterated through a few concepts before settling on our final one, which was inspired by games like A Short Hike with its feather mechanic allowing the player to upgrade their character's vertical mobility by collecting feathers. At this point we began looking at sources of visual inspiration and character concepts. Initial concepts focused on the idea of a winged human character who collects feathers in a homage to A Short Hike, but we decided that doing realistic wing animation would be difficult to achieve, and went for a more traditional humanoid character design, with gravity boots to explain the double jump mechanic.

At this point we also began thinking about the world design. The inclusion of gravity boots in the character design opened up a lot of possibilities for world design and setting. Initially we though about setting it on a space station, but we felt that there would be a confusion over how gravity works in such a setting, so we opted for the rooftop setting, which was inspired by some of our visual inspirations such as High Hell and Tokyo 42. We would aim for blocky grey buildings with some environmental decoration scatter throughout such as AC units.


## Development:

Once design had finished we began with development. Sylvie began early production on the game's systems while Finn worked on the character model, using a combined workflow of Blocbench for the modelling and painting, and Blender for the rigging and animation. They opted to go for a the more pixelated look that is facilitated by Blockbench. Sylvie worked on the character controller, using Cinemachine for the camera movement, and a placeholder capsule for the model.

Once the character model was complete, integrating it into the game was the next task. We used AnimationControllers to achieve this, using a flowchart with conditional statements to determine which animation would be played. Initially there were separate states for every possible animation, including the three states of jumping, so Sylvie created a BlendTree approach for the jump, resulting in more realistic animation. We also created a distinct jump animation for the double jump, and added particles on activation. A particle manager was created to handle the spawning and clearing of particle systems in the scene. It was also at this point that the battery pickup mechanic was added, along with the necessary dynamic UI for it, which was achieved using a script that dynamically adds elements to the canvas UI based on the player's air jump properties.

Once character movement was nailed down, level design was the next task. We had built a greybox tutorial level during testing of the character's movement abilities, and decided to leave it mostly as is as we felt it was a good introduction to the game's mechanics. The next step was to plan our next two levels. We made two rough sketches of our plans for the two levels (named Four Corners and Climb). Four corners is based around going to the four corners of the level and completing small challenges to get batteries so you can make the final jump to the end of the level, and Climb is about climbing in a spiral around a tall central tower and collecting batteries along the way. Both have a variety of valid speedrun routes, however a casual player can still enjoy the game without worrying about getting the fastest time.

The finishing touches of development involved adding some necessary usability features, such as a main menu and pause button. We also added some important stylistic features, such as sound design on certain actions, an animation at the end of the level, and some further environmental decoration to give the levels a little more life.


## Testing:

We ran several tests on other players during the development process. The initial unofficial test was done with friends online on 11/12/24, where we watched them play the game and after letting them beat it once, we challenged them to beat our best speedrun times on the levels. It was surprising how quickly they were able to learn the game and start getting better times (currently the best time on the Four Corners level is held by one of our friends). We were able to use this playtest to greatly improve the levels to be more interesting and fun to speedrun.

On 13/12/24 we ran a playtest with the rest of the game development module where we all played each other's games. We were able to get some feedback here from more casual players who didn't want to commit to speedrunning the game. From this playtest we learned that we'd have to make some slight adjustments to the levels, such as using environmental art to subtly highlight the way forward, and we made some very slight adjustments to how the camera works in response to feedback, so that the angle always remains consistent when respawning. Finally, we knew from this test that we needed to add some basic usability features, such as a main menu and pause menu with an explanation of the controls.


## Conclusion:

In conclusion, we ate on this one.
