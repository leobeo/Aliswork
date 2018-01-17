package md58615ab26475acd82f1342ebbb3290e86;


public class BorderButtonRenderer
	extends md5270abb39e60627f0f200893b490a1ade.ButtonRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Aliswork.Droid.Renderers.BorderButtonRenderer, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BorderButtonRenderer.class, __md_methods);
	}


	public BorderButtonRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == BorderButtonRenderer.class)
			mono.android.TypeManager.Activate ("Aliswork.Droid.Renderers.BorderButtonRenderer, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public BorderButtonRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == BorderButtonRenderer.class)
			mono.android.TypeManager.Activate ("Aliswork.Droid.Renderers.BorderButtonRenderer, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public BorderButtonRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == BorderButtonRenderer.class)
			mono.android.TypeManager.Activate ("Aliswork.Droid.Renderers.BorderButtonRenderer, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
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
