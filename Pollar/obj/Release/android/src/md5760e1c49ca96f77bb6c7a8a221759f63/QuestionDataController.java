package md5760e1c49ca96f77bb6c7a8a221759f63;


public class QuestionDataController
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Pollar.QuestionDataController, Pollar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", QuestionDataController.class, __md_methods);
	}


	public QuestionDataController () throws java.lang.Throwable
	{
		super ();
		if (getClass () == QuestionDataController.class)
			mono.android.TypeManager.Activate ("Pollar.QuestionDataController, Pollar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
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
