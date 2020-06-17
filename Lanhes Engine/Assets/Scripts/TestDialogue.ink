INCLUDE common.ink


Hello there!
//comment
This is a dialogue!
I'm going to make you choose between Foo and Bar!
-> foobarbaz
=== foobarbaz ===
CHOOSE!
+	[Foo]
	You chose Foo!
+	[Bar]
	You chose Bar!
+	[Baz]
	What? That wasn't an option!
	Choose something else!
	-> foobarbaz
-Lets shop!
$SHOP 0

->END
	