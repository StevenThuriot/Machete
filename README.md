Machete
====

* A quick and easy way to bundle several WCF calls to increase performance.

* TODO: 
	* Write out this readme.
	* Clean up solution a bit more
	* Add documentation
	* Cache expression trees on the server
	* Investigate security and the possible impact.


#Sample

```csharp
using (Machete.Scope)
{
	var service = Machete.CreateService<IPersonalService>();
	
	/*int result1 = */service.Call1();
	/*var result2 = */service.Call2();
	/*var result3 = */service.Call3();
	
	var results = Machete.Slash();
	Response response1 = results.First();
	
	if (response1.Succeeded)
	{
	    var result1 = (int) response1.Answer;
	    //TODO
	}
}
```
