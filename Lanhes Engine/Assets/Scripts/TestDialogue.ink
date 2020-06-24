INCLUDE common.ink

->head
==head==


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
*	[Baz]
	What? That wasn't an option!
	Choose something else!
	-> foobarbaz

- $SHOP 0

->END
	