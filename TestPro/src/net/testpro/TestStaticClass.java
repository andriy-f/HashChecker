package net.testpro;

public class TestStaticClass {

	public static int g=10;
	public static final int CONST=1;
	
	static {
		int i=0;
		g=i;
		//CONST=5; //invalid
		System.out.println("static block "+g);
	}
	
	public static void printf(String... str)
	{
		System.out.println("printf");
		for (String str1 : str) {
			System.out.println(str1);
		}
		
	}
	
	public static void inner()
	{
		TestStaticClass clin=new TestStaticClass();
	}
	
	static void meth()
	{
		final int k=0;
		int r=10;
	}
	
	private TestStaticClass()
	{
		System.out.println("private con"+g);
	}
}
