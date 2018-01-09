package md5d6d70644ce83bd2ca7c13170975b7fca;


public class GcmService
	extends md5d6d70644ce83bd2ca7c13170975b7fca.GcmServiceBase
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Gcm.Client.GcmService, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GcmService.class, __md_methods);
	}


	public GcmService (java.lang.String p0)
	{
		super (p0);
		if (getClass () == GcmService.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmService, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public GcmService ()
	{
		super ();
		if (getClass () == GcmService.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmService, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GcmService (java.lang.String[] p0)
	{
		super ();
		if (getClass () == GcmService.class)
			mono.android.TypeManager.Activate ("Gcm.Client.GcmService, Aliswork.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String[], mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
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
