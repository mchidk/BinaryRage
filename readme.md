# BinaryRage - the ultra fast .NET key/value store
+ BinaryRage is designed to be a lightweight ultra fast key/value store for .NET with no dependencies
+ It's production-ready - already in several large production-environments
+ Supports complex objects out of the box (and lists of objects)
+ 100% FREE and open (no paid pro version or anything lame like that)
+ No configuration, no strange driver/connector, no server, no setup - simply reference the dll and start using it in less than a minute.
+ Created it because I think there is a huge need for a very simple key/value store
+ If you hate writing boilerplate code - you will love BinaryRage

## Created by Michael Christensen (shameless facts)
+ I'm Chief Technical Officer, Technical Lead in a mid-sized web-agency in Denmark using .NET
+ Doing a bit of consulting on special occasions
+ Love startups and performance
+ Twitter: @mchidk

## Show me the code
Simple class - simply include an [Serializable] attribute.

	[Serializable]
	public class Product
	{
		public string Title { get; set; }
		public string ThumbUrl { get; set; }
		public string Description { get; set; }
		public float Price { get; set; }
	}

Insert-syntax (same for create and update)

	BinaryRage.DB.Insert("mykey", myProduct, @"C:\testpath");

... or with a list

	BinaryRage.DB.Insert("mykey", listOfProducts, @"C:\testpath");

Get the saved data

	var myProduct = BinaryRage.DB.Get<Product>("mykey", @"C:\testpath");
	
... or with a list

	var listOfProducts = BinaryRage.DB.Get<List<Product>>("mykey", @"C:\testpath");

... get JSON 

	var myJSON = BinaryRage.DB.GetJSON<List<Product>>("mykey", @"C:\testpath");


Query objects directly with LINQ

	var bestsellers = BinaryRage.DB.Get<List<Category>>("bestsellers", @"C:\products\").Where(p => !string.IsNullOrEmpty(p.Name));

Check if all writes are done

	BinaryRage.DB.WaitForCompletion();



That's it - can it be any simpler?

## Key API

Generate a unique key

	BinaryRage.Key.GenerateUniqueKey();

Calculate checksum on a string

	BinaryRage.Key.CalculateChecksum(string inputString);

Fast MD5Hash generating

	BinaryRage.Key.GenerateMD5Hash(string input);

## Goals
Codebase and usage must be as simple as possible (but not simpler).

## Todo
- Always check out open issues if you want to help out

# FAQ
## Is it really fast?
We have tested more than 200,000+ complex objects written to disk per second on a crappy laptop :-)

All writes are performed asynchronously. Reads are instantly available (also if writes are not completed) - by design.

## Why are you compressing the objects before written to disk?
The less I/O - the better. A compressed object is doing less I/O because of fewer bytes written to disk - simple as that.

## Why do I have to provide a path when I save an object?
I have decided that I want to provide sharding directly from the start. You can easily wrap the insert and get methods and "hardcode" the location if you want.
