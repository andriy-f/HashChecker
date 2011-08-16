package net.testpro;

public class TestPro {
	/** {Inline} */
	static int P;
	public static int dod(int k)
	{
		if(k==0)return 0;
		else if (k==1) return P;
		else return (k%2)*P+2*dod(k/2);
	}
	
	public static int mno(int k)
	{
		if(k==0)return 1;
		else if (k==1) return P;
		else {
			int t=mno(k/2);
			return (k%2==1?P:1)*t*t;			
		}
	}
	
	/**
	 * @param args Helps to die
	 * {@link net.testpro.TestPro#P label}
	 */
	public static void main(String[] args) {
		//P=2;
		//System.out.println(mno(6));
		//System.setProperty("bc.test.data.home", "test/data");
		//org.bouncycastle.crypto.test.AllTests.main(new String[]{});
		
	}

}
