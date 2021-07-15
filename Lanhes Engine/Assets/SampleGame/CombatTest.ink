INCLUDE ../Scripts/common.ink


->head
==head==
-It's time to d-d-d-d-duel!

$BATTLE 0

{ getBattleResult():
- 1: -> won_battle
- 2: -> lost_battle
}

==won_battle==
You are victorious!
-> end_of_dialogue

==lost_battle==
\| \|\| \n\|\| \|\_
And now, you see that Evil will always triumph over Good, becuase Good is dumb.

-> end_of_dialogue

==end_of_dialogue==
->END
