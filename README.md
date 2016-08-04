# StepUp
2D multiplayer fight game

Step Up is a 2D fight game inspired by game such as Doodle Jump or Smash Bros.
In Step Up, up to 4 players fights to kick opponents out of the Arena, and be the last survivor. By stunning other players, fighting them, or using items, any means are good to win.
For each player is required a controller such as an XBox controller.

The rules of Step Up are simple.
- If a player is no more visible by the Camera, he dies and respawn up to 3 times; then he is eliminated.
- The camera follow the highest player on the screen, and it can only move to the top.
- Players can jump through platforms, fight, or use items.
- The more a player is hit, the more he will be stunned/pushed away when he will be hit again.

This kind of game has to be nervous, dynamic, so we designed both items and players mechanics in this ways.

Items help distanced players to get back in the course, while leading players have items to climb quickly.

Since players are already climbing, the have not so mush time to fight, so there are only two attacks:
- a simple kick useable while jumping, and which push back an opponent in the direction of the kick.
- a storng and canalized attack which push and stun everyone in its radius. The more the attack is canalized, the more the radius is.

The simple attack is design to counter the strong or a jumping player.
The strong attack is a gamble : either it affact other players and give a big advange, or nobody is touch and other players used the canalization time to get higher.

Finally there is a dash, allowing to avoid both negative items or attack for a very short duration, but it has a cooldown.

The technical part has got a lot of attention, because the game must be very reactive. The controller is extremely responsive, and both the UI and the game gives clear but discrete feedback to all action so that anyone can easily understand what is going on.

All of this result in a short and dynamic game, easy to play.
