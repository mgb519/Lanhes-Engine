INCLUDE common.ink


//$SETINT didbar 0

->head
==head==

$NPCWALK TestNPC 20.0 0.0 13.4
$NPCWALK TestNPC 20.0 0.0 9.0
$NPCWALK TestNPC 9.0 0.0 9.0
$NPCWALK Player -3.0 0.0 9.0
$NPCWALK Player 7.5 0.0 9.0
$WAIT
$NPCWALK Player 7.5 0.0 7.0
$NPCWALK Player 7.5 0.0 9.0
$WAIT
TESTDIALOGUE_01
//comment
TESTDIALOGUE_02
TESTDIALOGUE_03

-> foobarbaz
=== foobarbaz ===
$PROMPT Foo, or Bar?
+	[TESTDIALOGUE_R_01]
	You chose Foo!
+	[TESTDIALOGUE_R_02]
	You chose Bar!
	$SETINT didbar {getInt("didbar")+1}
	TESTDIALOGUE_07
*	[TESTDIALOGUE_R_03]
	What? That wasn't an option!
	Choose something else!
	-> foobarbaz


- Let's shop!
$SHOP 0

->END
	
