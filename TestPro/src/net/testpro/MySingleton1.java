package net.testpro;

public class MySingleton1 {

	private static MySingleton1 _instance = new MySingleton1();

	private MySingleton1() {
		// with lazy initialization
		System.out.println(this.toString());
	}

	public static MySingleton1 getInstance() {
		return _instance;
	}
}
