INCLUDE common.ink


//$SETINT didbar 0

->head
==head==
$NPCWALK TestNPC 9.0 0.0 9.0

Hello there!
//comment
This is a dialogue!
I'm going to make you choose between Foo and Bar!
-> foobarbaz
=== foobarbaz ===
+	[Foo]
	You chose Foo!
+	[Bar]
	You chose Bar!
	$SETINT didbar {getInt("didbar")+1}
	This is the {getInt("didbar")} time you've chose Bar, by the way.
*	[Baz]
	What? That wasn't an option!
	Choose something else!
	-> foobarbaz


- Let's shop!
$SHOP 0

->END
	
