package md5a495699ddd81d03ef4d3456377e718ca;


public class MyHandler
	extends android.content.AsyncQueryHandler
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Plugin.Badge.MyHandler, Plugin.Badge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyHandler.class, __md_methods);
	}


	public MyHandler (android.content.ContentResolver p0)
	{
		super (p0);
		if (getClass () == MyHandler.class)
			mono.android.TypeManager.Activate ("Plugin.Badge.MyHandler, Plugin.Badge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.ContentResolver, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
